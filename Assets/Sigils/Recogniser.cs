using UnityEngine;
using System.Collections.Generic;

namespace MagicDuel.Sigils
{
    /** @var enum Feature Features that describe a sigil path */
    public enum Feature { Up, Down, Left, Right, Corner };

    /**
     * Sigil recogniser based off handwriting recognition
     * https://jackschaedler.github.io/handwriting-recognition/
     * (based off https://www.rand.org/content/dam/rand/pubs/research_memoranda/2007/RM5016.pdf)
     */
    public class Recogniser
    {
        /** @param float smoothing The smoothing amount, from 0 to 1 */
        private float smoothing;

        /** @param float thinning The thinning distance - minimum distance between points */
        private float thinning;

        /** @param float cornerAngle The angle in degrees that is recognised as a corner */
        private float cornerAngle;

        /** @param int nonCardnialDirections The number of directions used to recognise lines before and after a corner */
        private int nonCardinalDirections = 4; // 16 in the original

        /**
         * Constructor
         *
         * @param float smoothing The smoothing amount from 0 to 1
         * @param float thinning Th thinning distance - minimum distance between points
         * @param float cornerAngle The angle in degrees that is recognised as a corner
         */
        public Recogniser(float smoothing = 0.25f, float thinning = .05f, float cornerAngle = 65f)
        {
            this.smoothing = smoothing;
            this.thinning = thinning;
            this.cornerAngle = cornerAngle;
        }

        /**
         * Generate a list of features based on a set of points
         *
         * @param Vector2[] points A list of points
         *
         * @return string The feature hash of the points
         */
        public string run(Vector2[] points)
        {
            //DebugUtil.Points(points);
            var smoothed = Smooth(points);
            //DebugUtil.Points(smoothed, Color.green);
            var thinned = Thin(smoothed);
            //DebugUtil.Points(thinned, Color.blue);
            var features = GetFeatures(thinned);
            //DebugUtil.Features(features);
            var featuresHash = GetFeaturesHash(features);

            return featuresHash;
        }

        /**
         * Smooth a list of points
         *
         * @param Vector2[] points The list of points to smooth
         *
         * @return Vector2[] The smoothed points
         */
        private Vector2[] Smooth(Vector2[] points)
        {
            var inverseSmoothing = 1 - smoothing;
            var output = new Vector2[points.Length];
            var lastPoint = points[0];
            output[0] = lastPoint;

            for (var i = 1; i < points.Length; ++i)
            {
                output[i] = (smoothing * output[i - 1]) + (inverseSmoothing * points[i]);
            }

            return output;
        }

        /**
         * Thin a list of points - remove points that are too close together
         *
         * @param Vector2[] points The points to thin
         *
         * @return Vector2[] The thinned points
         */
        private Vector2[] Thin(Vector2[] points)
        {
            var sqrThinning = thinning * thinning;
            var output = new List<Vector2>();
            var lastPoint = points[0];
            output.Add(lastPoint);

            for (var i = 1; i < points.Length; ++i)
            {
                // Only kep the point if it's further from the last point than the thinning distance
                if ((points[i] - lastPoint).sqrMagnitude > sqrThinning)
                {
                    lastPoint = points[i];
                    output.Add(lastPoint);
                }
            }

            return output.ToArray();
        }

        /**
         * Get a list of line features from a list of points
         *
         * @param Vector2[] points The points
         *
         * @return Feature[] The features
         */
        private Feature[] GetFeatures(Vector2[] points)
        {
            var features = new List<Feature>();
            var directions = new int[points.Length - 1];
            Feature? lastCardinal = null;

            // Get the non-cardinal direction of each point from the last point
            for (var i = 1; i < points.Length; ++i)
            {
                directions[i - 1] = GetDirection(points[i - 1], points[i]);
            }

            for (var i = 1; i < points.Length; ++i)
            {
                var lastPoint = points[i - 1];
                var point = points[i];
                Feature cardinal;

                // Add a corner if the last two points are in the same non-cardinal direction,
                // the next two points are in the same non-cardinal direction,
                // and the angle between them is greater than the corner angle
                if ((i >= 2) && (i < directions.Length - 2))
                {
                    var last = directions[i - 2];
                    var current = directions[i - 1];
                    var next = directions[i];
                    var nextNext = directions[i + 1];

                    if ((last == current) && (next == nextNext) && (next != current))
                    {
                        var angle = Vector2.Angle(points[i] - points[i - 1], points[i + 1] - points[i]);

                        if (angle > cornerAngle)
                        {
                            features.Add(Feature.Corner);
                        }
                    }
                }

                if (Mathf.Abs(point.x - lastPoint.x) >= Mathf.Abs(point.y - lastPoint.y))
                {
                    cardinal = point.x - lastPoint.x >= 0 ? Feature.Right : Feature.Left;
                }
                else
                {
                    cardinal = point.y - lastPoint.y >= 0 ? Feature.Up : Feature.Down;
                }

                // Add the cardinal direction if it's different to the last one
                if (cardinal != lastCardinal)
                {
                    features.Add(cardinal);
                    lastCardinal = cardinal;
                }
            }

            return features.ToArray();
        }

        /**
         * Get the non-cardnial direction between two vectors
         *
         * @param Vector2 a The first vector
         * @param Vector2 b The second vector
         *
         * @return int The non-cardinal direction index
         */
        private int GetDirection(Vector2 a, Vector2 b)
        {
            var delta = b - a;
            var sign = (b.y < a.y) ? -1.0f : 1.0f;
            var angle = Vector2.Angle(Vector2.right, delta) * sign;
            angle = (angle + 360) % 360;

            // Convert from 0-360 to 0-(nonCardinalDirections-1)
            return Mathf.FloorToInt((float)nonCardinalDirections * (angle / 360f));
        }

        /**
         * Get a hash from a list of features
         *
         * @param Feature[] features The features
         *
         * @return string The features hash
         */
        private string GetFeaturesHash(Feature[] features)
        {
            var str = "";

            foreach (var feature in features)
            {
                str += (str == "" ? "" : ", ")
                    + feature.ToString();
            }

            return str;
        }
    }
}

using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    /**
     * The interface for a single segment in an SVG path
     */
    public abstract class PathSegment
    {
        public float length;

        /**
         * Get the position at a point along the path
         * 
         * @param startPosition The position to start at
         * @param t The position through the path, 0 = start, 1 = end
         * 
         * @return Returns the position of the path at t
         */
        public abstract Vector3 GetPosition(Vector3 startPosition, float t);

        /**
         * Get the tangent at a point along the path
         * 
         * @param startPosition The position to start at
         * @param t The position through the path, 0 = start, 1 = end
         * 
         * @return Returns the tangent of the path at t
         */
        public abstract Vector3 GetDirection(Vector3 startPosition, float t);

        /**
         * Get an estimate of the path length
         */
        public virtual float CalculateLength()
        {
            length = 0;
            var cursor = new Vector3(0, 0, 0);
            const int numPoints = 30;

            for (var i = 0; i <= numPoints; ++i)
            {
                var endPoint = GetPosition(Vector3.zero, 1.0f / numPoints * i);
                length += (endPoint - cursor).magnitude;
                cursor = endPoint;
            }

            return length;
        }

        /**
         * Get the bounding box of the shape
         * 
         * @return Returns an array of two vectors: the minimum (x, y) and the maximum (x, y)
         */
        public Vector2[] GetExtents(Vector3 cursor)
        {
            Vector2[] extents = null;
            const int numPoints = 30;

            for (var i = 0; i <= numPoints; ++i)
            {
                var endPoint = GetPosition(cursor, 1.0f / numPoints * i);

                if (extents == null)
                {
                    extents = new Vector2[2];
                    extents[0] = endPoint;
                    extents[1] = endPoint;
                }
                else
                {
                    extents[0] = Vector2.Min(extents[0], endPoint);
                    extents[1] = Vector2.Max(extents[1], endPoint);
                }
            }

            return extents;
        }

        public abstract void Normalize(Vector3 startPosition, float scale);
    }
}

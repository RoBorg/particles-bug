using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Assertions;
using System;

namespace MagicDuel.Svg
{
    public class SvgParser
    {
        /** @var paths The path segments to draw the sigil */
        protected List<List<PathSegments.PathSegment>> paths;

        /** @var inputData The SVG data, used while parsing */
        protected string inputData;

        /** @var inputPosition The SVG data current position, used while parsing */
        protected int inputPosition;

        protected Vector3 subPathStart = new Vector3();
        protected Vector3 lastEndpoint = new Vector3();

        /**
         * Draw the SVG
         * 
         * A "point" object is created for every point along the path
         * 
         * @param parent The object to parent the points to
         * @param point The object to create at each point on the path
         */
        public void Draw(GameObject parent, GameObject point)
        {
            var pointPositions = GetPoints();

            foreach (var position in pointPositions)
            {
                var newPoint = GameObject.Instantiate<GameObject>(point);
                newPoint.transform.SetParent(parent.transform);
                newPoint.transform.localPosition = position;
            }
        }

        /**
         * Get a list of points along the entire path
         * @param distancePerStep How far apart to space the points
         */
        public Vector2[] GetPoints(float distancePerStep = 0.01f)
        {
            var points = new List<Vector2>();
            var cursor = new Vector2();
            var totalLength = 0f;

            foreach (var path in paths)
            {
                foreach (var pathSegment in path)
                {
                    totalLength += pathSegment.length;
                }
            }

            var steps = Mathf.Round(totalLength / distancePerStep);
            distancePerStep = totalLength / steps;

            float currentDistance = 0;
            float lengthSoFar = 0f;

            var startPosition = paths[0][0].GetPosition(cursor, 0);

            foreach (var path in paths)
            {
                foreach (var pathSegment in path)
                {
                    // The next point is to be placed at currentDistance
                    // The current segment runs from lengthSoFar to lengthSoFar + pathSegment.length
                    // Therefore the position along the segment is
                    // (currentDistance - lengthSoFar) / pathSegment.length
                    while (currentDistance < lengthSoFar + pathSegment.length)
                    {
                        float t = (currentDistance - lengthSoFar) / pathSegment.length;
                        var position = pathSegment.GetPosition(cursor, t);

                        if (currentDistance == 0 || (position - startPosition).magnitude > 0.01)
                        {
                            points.Add(position);
                        }

                        currentDistance += distancePerStep;
                    }

                    cursor = pathSegment.GetPosition(cursor, 1);

                    lengthSoFar += pathSegment.length;
                }
            }

            return points.ToArray();
        }

        /**
         * Load the paths from the SVG data
         * 
         * @param svg The SVG file containing the sigil graphic
         */
        public void Load(TextAsset svg)
        {
            Assert.IsNotNull(svg, "Failed to load SVG");

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(svg.text);

            // Create the 'svg' namespace
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("svg", "http://www.w3.org/2000/svg");

            // Select all 'path' nodes
            var pathNodes = xmlDocument.SelectNodes("svg:svg/svg:g/svg:path", namespaceManager);
            paths = new List<List<PathSegments.PathSegment>>();

            // Add the paths
            foreach (XmlNode pathNode in pathNodes)
            {
                var path = LoadPath(pathNode.Attributes["d"].Value);
                paths.Add(path);
            }

            Normalize();
            CalculatePathLengths();
        }

        /**
         * Resize so all coordinates are in the range 0..1
         */
        protected void Normalize()
        {
            var extents = GetExtents();
            var scale = 0f;

            // See if it's bigger horizontally or vertically
            var horizontalSize = extents[1].x - extents[0].x;
            var verticalSize = extents[1].y - extents[0].y;

            if (horizontalSize > verticalSize)
                scale = 1 / horizontalSize;
            else
                scale = 1 / verticalSize;

            foreach (var path in paths)
            {
                foreach (var pathSegment in path)
                {
                    var offset = -extents[0];
                    offset.x -= horizontalSize / 2;
                    offset.y -= verticalSize / 2;

                    pathSegment.Normalize(offset, scale);
                }
            }
        }

        /**
         * Get the bounding box of the sigil
         * 
         * @return Returns an array of two vectors, one for the bottom left and one for the upper right bound
         */
        protected Vector2[] GetExtents()
        {
            Vector2[] extents = null;
            var cursor = new Vector3();

            foreach (var path in paths)
            {
                foreach (var pathSegment in path)
                {
                    var e = pathSegment.GetExtents(cursor);
                    if (extents == null)
                    {
                        extents = new Vector2[2];
                        extents[0] = e[0];
                        extents[1] = e[1];
                    }
                    else
                    {
                        extents[0] = Vector2.Min(extents[0], e[0]);
                        extents[1] = Vector2.Max(extents[1], e[1]);
                    }
                    cursor = pathSegment.GetPosition(cursor, 1);
                }
            }

            return extents;
        }

        /**
         * Calculate the length of every segment in the path
         */
        protected void CalculatePathLengths()
        {
            foreach (var path in paths)
            {
                foreach (var pathSegment in path)
                {
                    pathSegment.CalculateLength();
                }
            }
        }

        /**
         * Load an SVG path from a string
         *
         * @param svgString The "d" attribute of the "path" node
         */
        protected List<PathSegments.PathSegment> LoadPath(string svgString)
        {
            inputData = svgString;
            inputPosition = 0;
            var path = new List<PathSegments.PathSegment>();

            ConsumeWhitespace();
            while (!AtEnd())
            {
                var pathSegment = GetPathSegment();
                path.AddRange(pathSegment);
            }

            return path;
        }

        /**
         * Skip whitespace
         * The input position is incremented until it points to a
         * non-whitespace charcter
         */
        protected void ConsumeWhitespace()
        {
            while (!AtEnd())
            {
                var c = inputData[inputPosition];
                if (!(c == ' ' || c == '\r' || c == '\n' || c == '\t' || c == ','))
                    break;

                inputPosition++;
            }
        }

        /**
         * Find if the input position is at the end of the input string
         * 
         * @return Returns true if the input position is at the end of the input string, false otherwise
         */
        protected bool AtEnd()
        {
            return inputPosition == inputData.Length;
        }

        /**
         * Load a single path segment in a path string
         */
        protected List<PathSegments.PathSegment> GetPathSegment()
        {
            var type = inputData[inputPosition];
            inputPosition++;
            ConsumeWhitespace();

            var pathSegments = new List<PathSegments.PathSegment>();

            switch (type)
            {
                // Relative move
                case 'm':
                    {
                        // If this is the first segment in the path, treat it as absolute
                        if (pathSegments.Count == 0)
                        {
                            var segment = new PathSegments.AbsoluteMove();
                            segment.moveTo.x = GetFloat();
                            segment.moveTo.y = -GetFloat();
                            segment.moveTo.z = 0;
                            pathSegments.Add(segment);

                            subPathStart.x = segment.moveTo.x;
                            subPathStart.y = segment.moveTo.y;
                            subPathStart.z = segment.moveTo.z;
                        }
                        else
                        {
                            var segment = new PathSegments.RelativeMove();
                            segment.offset.x = GetFloat();
                            segment.offset.y = -GetFloat();
                            segment.offset.z = 0;
                            pathSegments.Add(segment);

                            subPathStart = lastEndpoint + segment.offset;
                        }

                        // Following number are relative line segments
                        while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                        {
                            var lineSegment = new PathSegments.RelativeLine();
                            lineSegment.offset.x = GetFloat();
                            lineSegment.offset.y = -GetFloat();

                            pathSegments.Add(lineSegment);

                            lastEndpoint += lineSegment.offset;
                        }

                        break;
                    }

                // Absolute move
                case 'M':
                    {
                        var segment = new PathSegments.AbsoluteMove();
                        segment.moveTo.x = GetFloat();
                        segment.moveTo.y = -GetFloat();
                        segment.moveTo.z = 0;

                        pathSegments.Add(segment);
                        subPathStart = segment.moveTo * 1;
                        lastEndpoint = segment.moveTo * 1;

                        // Following number are absolute line segments
                        while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                        {
                            var lineSegment = new PathSegments.AbsoluteLine();
                            lineSegment.destination.x = GetFloat();
                            lineSegment.destination.y = -GetFloat();

                            pathSegments.Add(lineSegment);
                            lastEndpoint = segment.moveTo * 1;
                        }

                        break;
                    }

                //  Close path
                case 'z':
                case 'Z':
                    {
                        //var segment = new PathSegments.ClosePath();
                        var segment = new PathSegments.AbsoluteLine();
                        segment.destination = subPathStart;
                        pathSegments.Add(segment);
                        lastEndpoint = subPathStart * 1;
                        break;
                    }

                // Relative line
                case 'l':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.RelativeLine();

                        segment.offset.x = GetFloat();
                        segment.offset.y = -GetFloat();
                        segment.offset.z = 0;
                        pathSegments.Add(segment);
                        lastEndpoint += segment.offset;
                    }
                    break;

                // Absolute line
                case 'L':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.AbsoluteLine();

                        segment.destination.x = GetFloat();
                        segment.destination.y = -GetFloat();
                        segment.destination.z = 0;
                        pathSegments.Add(segment);
                        lastEndpoint = segment.destination * 1;
                    }
                    break;

                // Relative horizontal line
                case 'h':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.RelativeHorizontalLine();

                        segment.offset = GetFloat();
                        pathSegments.Add(segment);
                        lastEndpoint.x += segment.offset;
                    }
                    break;

                // Absolute horizontal line
                case 'H':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.AbsoluteHorizontalLine();

                        segment.position = GetFloat();
                        pathSegments.Add(segment);
                        lastEndpoint.x = segment.position;
                    }
                    break;

                // Relative vertical line
                case 'v':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.RelativeVerticalLine();

                        segment.offset = -GetFloat();
                        pathSegments.Add(segment);
                        lastEndpoint.y += segment.offset;
                    }
                    break;

                // Absolute vertical line
                case 'V':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.AbsoluteVerticalLine();

                        segment.position = -GetFloat();
                        pathSegments.Add(segment);
                        lastEndpoint.y = segment.position;
                    }
                    break;

                // Relative cubic bezier curve
                case 'c':

                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.RelativeCubicBezierCurve();

                        segment.controlPoints = new Vector3[2];
                        segment.controlPoints[0].x = GetFloat();
                        segment.controlPoints[0].y = -GetFloat();
                        segment.controlPoints[1].x = GetFloat();
                        segment.controlPoints[1].y = -GetFloat();

                        segment.endPosition = new Vector3();
                        segment.endPosition.x = GetFloat();
                        segment.endPosition.y = -GetFloat();

                        pathSegments.Add(segment);
                        lastEndpoint += segment.endPosition;
                    }

                    break;


                // Absolute cubic bezier curve
                case 'C':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.AbsoluteCubicBezierCurve();

                        segment.controlPoints = new Vector3[2];
                        segment.controlPoints[0].x = GetFloat();
                        segment.controlPoints[0].y = -GetFloat();
                        segment.controlPoints[1].x = GetFloat();
                        segment.controlPoints[1].y = -GetFloat();

                        segment.endPosition = new Vector3();
                        segment.endPosition.x = GetFloat();
                        segment.endPosition.y = -GetFloat();

                        pathSegments.Add(segment);
                        lastEndpoint = segment.endPosition * 1;
                    }

                    break;

                case 'A':
                    while (!AtEnd() && !Char.IsLetter(inputData[inputPosition]))
                    {
                        var segment = new PathSegments.AbsoluteEllipticalArc();

                        segment.radiusX = GetFloat();
                        segment.radiusY = GetFloat();
                        segment.xAxisRotation = GetFloat();
                        segment.largeArc = GetFloat() != 0;
                        segment.sweep = GetFloat() == 0;

                        segment.destination.x = GetFloat();
                        segment.destination.y = -GetFloat();

                        pathSegments.Add(segment);
                        lastEndpoint = segment.destination * 1;
                    }

                    break;

                default:
                    throw new System.FormatException("Unknown path segment type " + type);
            }

            return pathSegments;
        }

        /**
         * Get a floating point number from the input data
         *
         * @return Returns the floating point number
         */
        protected float GetFloat()
        {
            var startPosition = inputPosition;

            // Get the integer part
            if (!AtEnd() && inputData[inputPosition] == '-')
            {
                inputPosition++;
                if (AtEnd() || inputData[inputPosition] < '0' || inputData[inputPosition] > '9')
                    throw new System.FormatException("Expected a number");
            }

            // Increment the input position until we find a non-digit or EOF
            while (!AtEnd())
            {
                if (inputData[inputPosition] < '0' || inputData[inputPosition] > '9')
                    break;

                inputPosition++;
            }

            if (inputPosition == startPosition)
                throw new System.FormatException("Expected an integer");

            if (!AtEnd() && inputData[inputPosition] == '.')
            {
                inputPosition++;
                // Get the fractional part
                // Increment the input position until we find a non-digit or EOF
                while (!AtEnd())
                {
                    if (inputData[inputPosition] < '0' || inputData[inputPosition] > '9')
                        break;

                    inputPosition++;
                }

                if (inputPosition == startPosition)
                    throw new System.FormatException("Expected an fractional part");
            }

            var f = (float)System.Double.Parse(inputData.Substring(startPosition, inputPosition - startPosition));

            ConsumeWhitespace();

            return f;
        }

        /**
         * Get an integer from the input data
         * 
         * @return Returns the integer
         */
        protected int GetInt()
        {
            int startPosition = inputPosition;

            // Increment the input position until we find a non-digit or EOF
            while (!AtEnd())
            {
                if (inputData[inputPosition] < '0' || inputData[inputPosition] > '9')
                    break;

                inputPosition++;
            }

            int endPosition = inputPosition - 1;

            if (startPosition == endPosition)
                throw new System.FormatException("Expected an integer");

            return Int32.Parse(inputData.Substring(startPosition, endPosition - startPosition));
        }
    }
}

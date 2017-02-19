using UnityEngine;
using System;

namespace MagicDuel.Svg.PathSegments
{
    public class AbsoluteEllipticalArc : PathSegment
    {
        public float radiusX;
        public float radiusY;
        public float xAxisRotation;
        public bool largeArc;
        public bool sweep;
        public Vector3 destination = new Vector3();

        // http://www.w3.org/TR/SVG/implnote.html#ArcSyntax
        // To double check our implementation against another source, you can use: 
        //      https://java.net/projects/svgsalamander/sources/svn/content/trunk/svg-core/src/main/java/com/kitfox/svg/pathcmd/Arc.java
        //      http://fridrich.blogspot.com/2011/06/bounding-box-of-svg-elliptical-arc.html
        // t: [0, 1]
        public override Vector3 GetPosition(Vector3 cursor, float t)
        {

            // In accordance to: http://www.w3.org/TR/SVG/implnote.html#ArcOutOfRangeParameters
            radiusX = Mathf.Abs(radiusX);
            radiusY = Mathf.Abs(radiusY);
            xAxisRotation = Mathf.Abs(xAxisRotation % 360);
            var xAxisRotationRadians = Mathf.Deg2Rad * xAxisRotation;
            
            // If the endpoints are identical, then this is equivalent to omitting the elliptical arc segment entirely.
            if (cursor.x == destination.x && cursor.y == destination.y)
            {
                return cursor;
            }

            // If rx = 0 or ry = 0 then this arc is treated as a straight line segment joining the endpoints.    
            if (radiusX == 0 || radiusY == 0)
            {
                //return pointOnLine(cursor, destination, t);
                throw new Exception("todo");
            }

            // Following "Conversion from endpoint to center parameterization"
            // http://www.w3.org/TR/SVG/implnote.html#ArcConversionEndpointToCenter

            // Step #1: Compute transformedPoint
            var dx = (cursor.x - destination.x) / 2;
            var dy = (cursor.y - destination.y) / 2;
            var transformedPoint = new Vector2(
                Mathf.Cos(xAxisRotationRadians) * dx + Mathf.Sin(xAxisRotationRadians) * dy,
                -Mathf.Sin(xAxisRotationRadians) * dx + Mathf.Cos(xAxisRotationRadians) * dy);

            // Ensure radii are large enough
            var radiiCheck = Mathf.Pow(transformedPoint.x, 2) / Mathf.Pow(radiusX, 2) + Mathf.Pow(transformedPoint.y, 2) / Mathf.Pow(radiusY, 2);
            if (radiiCheck > 1)
            {
                radiusX = Mathf.Sqrt(radiiCheck) * radiusX;
                radiusY = Mathf.Sqrt(radiiCheck) * radiusY;
            }

            // Step #2: Compute transformedCenter
            var cSquareNumerator = Mathf.Pow(radiusX, 2) * Mathf.Pow(radiusY, 2) - Mathf.Pow(radiusX, 2) * Mathf.Pow(transformedPoint.y, 2) - Mathf.Pow(radiusY, 2) * Mathf.Pow(transformedPoint.x, 2);
            var cSquareRootDenom = Mathf.Pow(radiusX, 2) * Mathf.Pow(transformedPoint.y, 2) + Mathf.Pow(radiusY, 2) * Mathf.Pow(transformedPoint.x, 2);
            var cRadicand = cSquareNumerator / cSquareRootDenom;
            // Make sure this never drops below zero because of precision
            cRadicand = cRadicand < 0 ? 0 : cRadicand;
            var cCoef = (largeArc != sweep ? 1 : -1) * Mathf.Sqrt(cRadicand);
            var transformedCenter = new Vector2(
                cCoef * ((radiusX * transformedPoint.y) / radiusY),
                cCoef * (-(radiusY * transformedPoint.x) / radiusX));


            // Step #3: Compute center
            var center = new Vector2(
                Mathf.Cos(xAxisRotationRadians) * transformedCenter.x - Mathf.Sin(xAxisRotationRadians) * transformedCenter.y + ((cursor.x + destination.x) / 2),
                Mathf.Sin(xAxisRotationRadians) * transformedCenter.x + Mathf.Cos(xAxisRotationRadians) * transformedCenter.y + ((cursor.y + destination.y) / 2));

            // Step #4: Compute start/sweep angles
            // Start angle of the elliptical arc prior to the stretch and rotate operations.
            // Difference between the start and end angles
            var startVector = new Vector2(
                (transformedPoint.x - transformedCenter.x) / radiusX,
                (transformedPoint.y - transformedCenter.y) / radiusY);

            var startAngle = Vector2.Angle(Vector2.right, startVector) * Mathf.Deg2Rad;
            var sign = startVector.y < 0 ? -1 : 1;
            startAngle *= sign;

            var endVector = new Vector2(
                (-transformedPoint.x - transformedCenter.x) / radiusX,
                (-transformedPoint.y - transformedCenter.y) / radiusY);

            var sweepAngle = Vector2.Angle(startVector, endVector) * Mathf.Deg2Rad;
            sign = startVector.x * endVector.y - startVector.y * endVector.x < 0 ? -1 : 1;
            sweepAngle *= sign;

            if (!sweep && sweepAngle > 0)
            {
                sweepAngle -= 2 * Mathf.PI;
            }
            else if (sweep && sweepAngle < 0)
            {
                sweepAngle += 2 * Mathf.PI;
            }

            // We use % instead of `mod(..)` because we want it to be -360deg to 360deg (but actually in radians)
            sweepAngle %= 2 * Mathf.PI;

            // From http://www.w3.org/TR/SVG/implnote.html#ArcParameterizationAlternatives
            var angle = startAngle + (sweepAngle * t);
            var ellipseComponentX = radiusX * Mathf.Cos(angle);
            var ellipseComponentY = radiusY * Mathf.Sin(angle);

            var point = new Vector2(
                Mathf.Cos(xAxisRotationRadians) * ellipseComponentX - Mathf.Sin(xAxisRotationRadians) * ellipseComponentY + center.x,
                Mathf.Sin(xAxisRotationRadians) * ellipseComponentX + Mathf.Cos(xAxisRotationRadians) * ellipseComponentY + center.y);

            /*
            // Attach some extra info to use
            point.ellipticalArcStartAngle = startAngle;
            point.ellipticalArcEndAngle = startAngle + sweepAngle;
            point.ellipticalArcAngle = angle;

            point.ellipticalArcCenter = center;
            point.resultantRx = radiusX;
            point.resultantRy = radiusY;
            */

            return point;
        }

        public override void Normalize(Vector3 startPosition, float scale)
        {
            destination = (destination + startPosition) * scale;
            radiusX *= scale;
            radiusY *= scale;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            throw new NotImplementedException();
        }
    }
}
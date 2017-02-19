using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel
{
    public class PointsToPlane
    {
        // http://www.ilikebigbits.com/blog/2015/3/2/plane-from-points
        // Constructs a plane from a collection of points
        // so that the summed squared distance to all points is minimzized
        public Plane GetPlane(Vector3[] points)
        {
            Assert.IsTrue(points.Length >= 3, "At least three points required");

            var centroid = GetCentroid(points);

            // Calc full 3x3 covariance matrix, excluding symmetries:
            var xx = 0f;
            var xy = 0f;
            var xz = 0f;
            var yy = 0f;
            var yz = 0f;
            var zz = 0f;

            foreach (var point in points)
            {
                var r = point - centroid;

                xx += r.x * r.x;
                xy += r.x * r.y;
                xz += r.x * r.z;
                yy += r.y * r.y;
                yz += r.y * r.z;
                zz += r.z * r.z;
            }

            var detX = yy * zz - yz * yz;
            var detY = xx * zz - xz * xz;
            var detZ = xx * yy - xy * xy;

            var detMax = Mathf.Max(detX, detY, detZ);
            Assert.IsTrue(detMax > 0.0, "The points don't span a plane");

            // Pick path with best conditioning:
            var dir = Vector3.zero;
            if (detMax == detX)
            {
                var a = (xz * yz - xy * zz) / detX;
                var b = (xy * yz - xz * yy) / detX;
                dir = new Vector3(1f, a, b);
            }
            else if (detMax == detY)
            {
                var a = (yz * xz - xy * zz) / detY;
                var b = (xy * xz - yz * xx) / detY;
                dir = new Vector3(a, 1f, b);
            }
            else
            {
                var a = (yz * xy - xz * yy) / detZ;
                var b = (xz * xy - yz * xx) / detZ;
                dir = new Vector3(a, b, 1f);
            };

            dir.Normalize();

            return new Plane(dir, centroid);
        }

        public Vector2[] GetPointsOnPlane(Vector3[] points, Plane plane)
        {
            var pointsOnPlane = new Vector3[points.Length];
            var output = new Vector2[points.Length];

            // Project onto plane, then undo the rotation of the plane
            for (var i = 0; i < points.Length; i++)
            {
                var rot = -Quaternion.LookRotation(plane.normal).eulerAngles;

                pointsOnPlane[i] = Vector3.ProjectOnPlane(points[i], plane.normal);
                pointsOnPlane[i] = VectorUtils.RotatePointAroundPivot(pointsOnPlane[i], Vector3.zero, new Vector3(0, 0, rot.z));
                pointsOnPlane[i] = VectorUtils.RotatePointAroundPivot(pointsOnPlane[i], Vector3.zero, new Vector3(0, rot.y, 0));
                pointsOnPlane[i] = VectorUtils.RotatePointAroundPivot(pointsOnPlane[i], Vector3.zero, new Vector3(rot.x, 0, 0));
            }

            // Recenter
            var centroid = GetCentroid(pointsOnPlane);

            for (var i = 0; i < pointsOnPlane.Length; i++)
            {
                pointsOnPlane[i] -= centroid;
                output[i] = new Vector2(pointsOnPlane[i].x, pointsOnPlane[i].y);
            }

            return output;
        }

        private Vector3 GetCentroid(Vector3[] points)
        {
            var centroid = Vector3.zero;

            for (var i = 0; i < points.Length; i++)
            {
                centroid += points[i];
            }

            centroid /= (float)points.Length;

            return centroid;
        }
    }
}

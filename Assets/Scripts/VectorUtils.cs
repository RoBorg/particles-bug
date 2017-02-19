using UnityEngine;

namespace MagicDuel
{
    public static class VectorUtils
    {
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            var dir = point - pivot; // Get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // Rotate it

            return dir + pivot;
        }

        public static Bounds GetBounds(Vector3[] points)
        {
            float minX = Mathf.Infinity;
            float maxX = -Mathf.Infinity;
            float minY = Mathf.Infinity;
            float maxY = -Mathf.Infinity;
            float minZ = Mathf.Infinity;
            float maxZ = -Mathf.Infinity;

            foreach (var point in points)
            {
                if (point.x < minX)
                    minX = point.x;

                if (point.x > maxX)
                    maxX = point.x;

                if (point.y < minY)
                    minY = point.y;

                if (point.y > maxY)
                    maxY = point.y;

                if (point.z < minZ)
                    minZ = point.z;

                if (point.z > maxZ)
                    maxZ = point.z;
            }

            var width = maxX - minX;
            var height = maxY - minY;
            var depth = maxZ - minZ;

            var center = new Vector3(minX + (width / 2), minY + (height / 2), minZ + (depth / 2));
            var size = new Vector3(width, height, depth);

            return new Bounds(center, size);
        }

        public static Bounds GetBounds(Vector2[] points)
        {
            var points3d = new Vector3[points.Length];

            for (var i = 0; i < points.Length; ++i)
            {
                points3d[i] = new Vector3(points[i].x, points[i].y, 0);
            }

            return GetBounds(points3d);
        }
    }
}

using UnityEngine;
using System.Collections;

namespace MagicDuel
{
    public class DebugUtil : MonoBehaviour
    {
        public static void Points(Vector2[] positions)
        {
            Points(positions, Color.red);
        }

        public static void Points(Vector2[] positions, Color color)
        {
            var points = new Vector3[positions.Length];

            for (var i = 0; i < positions.Length; ++i)
            {
                points[i] = new Vector3(positions[i].x, 0, positions[i].y);
            }

            Points(points, color);
        }

        public static void Points(Vector3[] positions)
        {
            Points(positions, Color.red);
        }

        public static void Points(Vector3[] positions, Color color)
        {
            var parent = new GameObject("Debug points");

            for (var i = 0; i < positions.Length; i++)
            {
                Point(positions[i], color, parent.transform);
            }
        }

        public static GameObject Point(Vector3 position)
        {
            return Point(position, Color.red);
        }

        public static GameObject Point(Vector3 position, Color color, Transform parent = null)
        {
            var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(point.GetComponent<Collider>());

            if (parent != null)
            {
                point.transform.parent = parent;
            }

            point.transform.position = position;
            point.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            point.GetComponent<Renderer>().material.color = color;

            return point;
        }

        public static GameObject Plane(Plane plane)
        {
            var o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.transform.localScale = new Vector3(1f, 1f, 0.01f);
            o.transform.position = Vector3.zero;
            o.transform.rotation = Quaternion.LookRotation(plane.normal);
            o.transform.Translate(o.transform.forward * -plane.distance);

            return o;
        }

        public static void TemporaryPoint(Vector3 position)
        {
            TemporaryPoint(position, Color.black);
        }

        public static void TemporaryPoint(Vector3 position, Color color)
        {
            var point = Point(position, color);
            var debug = point.AddComponent<DebugUtil>();
            debug.DestroyNextUpdate();
        }

        public void DestroyNextUpdate()
        {
            StartCoroutine(DoDestroyNextUpdate());
        }

        private IEnumerator DoDestroyNextUpdate()
        {
            yield return null;

            Destroy(gameObject);
        }
    }
}

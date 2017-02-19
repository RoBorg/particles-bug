using UnityEngine;

namespace MagicDuel.Extensions
{
    public static class GameObjectExtensions
    {
        public static Bounds GetBounds(this GameObject gameObject)
        {
            var bounds = new Bounds(gameObject.transform.position, Vector3.zero);

            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
            {
                bounds.Encapsulate(collider.bounds);
            }

            return bounds;
        }
    }
}

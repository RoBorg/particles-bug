using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace MagicDuel.Spells.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public delegate void CollisionAction(Projectile projectile, Collision collision, bool hasCollidedPreviously);
        public delegate void EndOfLifetimeAction(Projectile projectile);

        public event CollisionAction CollisionEvent;
        public event EndOfLifetimeAction EndOfLifetimeEvent;

        protected new Collider collider;
        protected new Rigidbody rigidbody;
        protected bool hasCollidedPreviously = false;

        protected virtual void Start()
        {
            collider = GetComponent<Collider>();
            Assert.IsNotNull(collider, "No collider on projectile " + name);

            rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(rigidbody, "No rigid body on projectile " + name);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (CollisionEvent != null)
            {
                CollisionEvent(this, collision, hasCollidedPreviously);
            }

            hasCollidedPreviously = true;
        }

        public void DisableCollider()
        {
            collider.enabled = false;
        }

        public void StartLifetime(float lifetime)
        {
            StartCoroutine(Live(lifetime));
        }

        protected IEnumerator Live(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);

            if (EndOfLifetimeEvent != null)
            {
                EndOfLifetimeEvent(this);
            }
        }
    }
}

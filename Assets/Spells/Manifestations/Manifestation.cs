using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace MagicDuel.Spells.Manifestations
{
    public class Manifestation : MonoBehaviour
    {
        public Spell spell;

        public delegate void CollisionAction(Manifestation manifestation, Collision collision);
        public delegate void EffectAction(Manifestation manifestation);

        public static event CollisionAction OnCollision;
        public static event EffectAction OnStartEffect;
        public static event EffectAction OnEndEffect;

        public new Collider collider { get; protected set; }
        public new Rigidbody rigidbody { get; protected set; }

        public delegate void DelayDelegate();

        protected virtual void Start()
        {
            collider = GetComponent<Collider>();
            // Assert.IsNotNull(collider, "No collider on manifestation " + name);

            rigidbody = GetComponent<Rigidbody>();
            // Assert.IsNotNull(rigidbody, "No rigid body on manifestation " + name);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (OnCollision != null)
            {
                OnCollision(this, collision);
            }
        }

        protected void StartEffect()
        {
            if (OnStartEffect != null)
            {
                OnStartEffect(this);
            }
        }

        protected void EndEffect()
        {
            if (OnEndEffect != null)
            {
                OnEndEffect(this);
            }
        }
    }
}

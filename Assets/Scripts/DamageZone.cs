using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace MagicDuel
{
    /// <summary>
    /// DamageZone represents an area that can be hit and take damage
    /// 
    /// An object wishing to have multiple hit boxes should have a DamageZone
    /// for each one.  The hit box also needs a Collider on it for the physical
    /// bounds.
    /// 
    /// The parent object then subscribes to the DamageZone's TakeDamageEvent
    /// When a damaging event happens (e.g. a grenade exploding), the Damger
    /// notifies the global Damage class with a DamageEvent
    /// The DamageZone is subscribe to this, and interrogates the Damager
    /// to find out how much damage has been done. It then passes this to the
    /// parent object by raising the TakeDamageEvent
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DamageZone : MonoBehaviour, Damageable
    {
        public delegate void TakeDamageAction(Character damager, DamageAmounts damage);

        /// <summary>
        /// Incoming damage is multiplied by this amount
        /// </summary>
        public float damageMultiplier = 1;

        /// <summary>
        /// The Collider that can be hit to cause damage
        /// </summary>
        public new Collider collider { get; protected set; }

        /// <summary>
        /// The owner of the DamageZone (character) should subscribe to this event so they are notified
        /// when the DamageZone takes damage
        /// </summary>
        public event TakeDamageAction TakeDamageEvent;

        private void OnEnable()
        {
            // Subscribe to the global damage event for notification about grenade explosions etc
            Damage.DamageEvent += OnDamage;
        }

        private void OnDisable()
        {
            // Unsubscrube from events
            Damage.DamageEvent -= OnDamage;
        }

        private void Start()
        {
            collider = GetComponent<Collider>();
            Assert.IsNotNull(collider, "Collider not found");
        }

        /// <summary>
        /// Called to deal damage to the DamageZone
        /// </summary>
        /// <param name="damager">The object doing the damage</param>
        /// <param name="damageSourcePosition">The position of the damager object</param>
        private void OnDamage(Damager damager, Vector3 damageSourcePosition)
        {
            var damage = damager.GetBaseDamage(damageSourcePosition, this);

            // DamageZone doesn't actually take damage, instead it notifies the owner that it has been damaged
            RaiseTakeDamage(damager.GetCaster(), damage);
        }

        /// <summary>
        /// Part of the Damageable interface
        /// </summary>
        /// <returns>Return's the DamageZone's collider</returns>
        public Collider GetCollider()
        {
            return collider;
        }

        /// <summary>
        /// Get the actual damage, after armour/multipliers
        /// </summary>
        /// <param name="baseDamage"></param>
        /// <returns></returns>
        public DamageAmounts GetDamage(DamageAmounts baseDamage)
        {
            return baseDamage.GetMultiplied(damageMultiplier);
        }

        /// <summary>
        /// Tell listeners that the DamageZone has taken damage
        /// </summary>
        /// <param name="damage"></param>
        public void RaiseTakeDamage(Character characterDoingDamage, DamageAmounts damage)
        {
            if (TakeDamageEvent != null)
            {
                TakeDamageEvent(characterDoingDamage, damage);
            }
        }
    }
}

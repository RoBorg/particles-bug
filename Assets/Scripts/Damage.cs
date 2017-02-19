using UnityEngine;

namespace MagicDuel
{
    /// <summary>
    /// This is the main event router for damage
    /// 
    /// Any object wishing to take or deal damage should go through this class
    /// 
    /// Dealing Damage
    ///   Implement the Damager interface
    ///   Call Damage.RaiseDamage
    /// 
    /// Taking Damage
    ///   Implement the Damageable interface
    ///   Subscribe to the DamageEvent
    ///   When you receive a DamageEvent
    ///     Call damager.GetBaseDamage() to find out how much damage has been dealt.  This adjusts for distance.
    ///     Adjust for armour and multipliers
    ///     Subtract health
    ///     Call Damage.RaiseDamaged() to notify other objects that damage has been taken
    ///     Call Damage.RaiseKilled() if the character has been killed
    /// </summary>
    public static class Damage
    {
        public delegate void DamageAction(Damager damager, Vector3 damageSourcePosition);
        public delegate void DamagedAction(Character damaged, Character damager, float damageTaken);
        public delegate void KilledAction(Character killed, Character killer);

        /// <summary>
        /// Subscribe to this event to be notified when any object does damage, e.g. a grenade explodes
        /// </summary>
        public static event DamageAction DamageEvent;

        /// <summary>
        /// Subscribe to this event to be notified when any object takes damage
        /// </summary>
        public static event DamagedAction DamagedEvent;

        /// <summary>
        /// Subscribe to this event to be notified when a character is killed
        /// </summary>
        public static event KilledAction KilledEvent;

        /// <summary>
        /// Raise an event to say that something is trying to do damage
        /// Things like grenades should call this function
        /// </summary>
        /// <param name="damager"></param>
        /// <param name="damageSourcePosition"></param>
        public static void RaiseDamage(Damager damager, Vector3 damageSourcePosition)
        {
            if (DamageEvent != null)
            {
                DamageEvent(damager, damageSourcePosition);
            }
        }

        /// <summary>
        /// Raise an event to say that someone has taken damage
        /// </summary>
        /// <param name="damaged">The character that took damage</param>
        /// <param name="damager">The character that did the damage</param>
        /// <param name="damageTaken">The damage taken</param>
        public static void RaiseDamaged(Character damaged, Character damager, float damageTaken)
        {
            if (DamagedEvent != null)
            {
                DamagedEvent(damaged, damager, damageTaken);
            }
        }

        /// <summary>
        /// Raise an event to say that a character has been killed
        /// </summary>
        /// <param name="killed">The character that was killed</param>
        /// <param name="killer">The character that killed them</param>
        public static void RaiseKilled(Character killed, Character killer)
        {
            if (KilledEvent != null)
            {
                KilledEvent(killed, killer);
            }
        }

        /// <summary>
        /// Get the distance between a point and a collider
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetDistance(Vector3 startPosition, Collider collider)
        {
            // If the point is inside the collider, return a distance of zero
            if (collider.bounds.Contains(startPosition))
            {
                return 0;
            }

            var closestPoint = collider.ClosestPointOnBounds(startPosition);

            return Vector3.Distance(startPosition, closestPoint);
        }


    }
}

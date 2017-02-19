using UnityEngine;

namespace MagicDuel
{
    /// <summary>
    /// Any object that does damage should implement this interface
    /// See the Damage class
    /// </summary>
    public interface Damager
    {
        /// <summary>
        /// Get the amount of damage that the damager will do to the damageable before armour
        /// </summary>
        /// <param name="damageSourcePosition">The location of the damager</param>
        /// <param name="damageable">The object being damaged, so the damager can calculate falloff due to distance</param>
        /// <returns></returns>
        DamageAmounts GetBaseDamage(Vector3 damageSourcePosition, Damageable damageable);

        /// <summary>
        /// Get the character that ultimately cause the damage, e.g. the person who fired a grenade
        /// </summary>
        /// <returns></returns>
        Character GetCaster();
    }
}

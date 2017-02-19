using UnityEngine;

namespace MagicDuel
{
    /// <summary>
    /// Anything that can take damage should implement this interface
    /// 
    /// The damage system works as follows:
    /// 
    /// Anything that can take damage must implement the Damageable interface
    /// </summary>
    public interface Damageable
    {
        /// <summary>
        /// Get the collider that must be hit in order for direct damage to be taken
        /// </summary>
        /// <returns>Returns the collider that can be hit to cause damage</returns>
        Collider GetCollider();

        /// <summary>
        /// Given the base damage of a weapon, return the actual damage that will be taken
        /// after armour mitigation etc has been applied
        /// </summary>
        /// <param name="baseDamage">The amount of damage that the weapon deals</param>
        /// <returns>Returns the amount of damage after any mitigation/multipliers</returns>
        DamageAmounts GetDamage(DamageAmounts baseDamage);
    }
}

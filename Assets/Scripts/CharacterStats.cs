using UnityEngine;
using System.Collections.Generic;

namespace MagicDuel
{
    /// <summary>
    /// This class holds all the character's parameters: max health, max mana, and other properties
    /// 
    /// There are two ways of using this class:
    /// 
    /// 1) As a ScriptableObject asset
    ///   Create an asset and configure the values as desired
    ///   Use the asset to initialize base values for characters
    /// 
    /// 2) For actual character stats
    ///   Create a copy of the character's base stats: var stats = new CharacterStats(baseStats)
    ///   Apply modifiers: stats.ApplyModifiers()
    ///   Use these values for actual run-time calculations
    /// </summary>
    [CreateAssetMenu]
    public class CharacterStats : ScriptableObject
    {
        public float maximumHealth = 100;
        public float startHealth = 100;
        public float healthRegeneration = 1;
        public float maximumMana = 100;
        public float startMana = 100;
        public float manaRegeneration = 1;

        // Modifiers

        public float criticalHitDamageModifier = 1;
        public float criticalHitChanceModifier = 1;
        public float shieldStrengthModifier = 1;
        public float shieldDurationModifier = 1;
        public float projectileSpeedModifier = 1;
        public float projectileDamageModifier = 1;

        /**
         * Clone the object
         */
        public CharacterStats Clone()
        {
            var other = ScriptableObject.CreateInstance<CharacterStats>();

            other.maximumHealth = maximumHealth;
            other.healthRegeneration = healthRegeneration;
            other.maximumMana = maximumMana;
            other.manaRegeneration = manaRegeneration;
            other.criticalHitDamageModifier = criticalHitDamageModifier;
            other.criticalHitChanceModifier = criticalHitChanceModifier;
            other.shieldStrengthModifier = shieldStrengthModifier;
            other.shieldDurationModifier = shieldDurationModifier;
            other.projectileSpeedModifier = projectileSpeedModifier;
            other.projectileDamageModifier = projectileDamageModifier;

            return other;
        }
    }
}

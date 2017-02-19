using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace MagicDuel
{
    public class Character : MonoBehaviour
    {
        /// <summary>
        /// ???
        /// </summary>
        public delegate void UpdateEvent();

        /** @var baseStats The character's base stats, before procs */
        public CharacterStats baseStats;

        /** stats The character's current stats, after procs */
        protected CharacterStats stats;

        /// <summary>
        /// The character's current health
        /// </summary>
        public float currentHealth { get; protected set; }

        /// <summary>
        /// The character's current mana
        /// </summary>
        public float currentMana { get; protected set; }

        /// <summary>
        /// Called when the object is created
        /// </summary>
        protected virtual void Awake()
        {
            currentHealth = 99999;
            currentMana = 99999;
        }

        /// <summary>
        /// Callback for taking damage
        /// </summary>
        /// <param name="damage">The damage to be taken, before armour mitigation</param>
        protected virtual void OnTakeDamage(Character damager, DamageAmounts damage)
        {
            var healthDamage = damage.health + damage.fire + damage.ice + damage.water + damage.lightning;
            currentHealth -= healthDamage;

            currentMana -= damage.mana;

            currentHealth = Mathf.Clamp(currentHealth, 0, stats.maximumHealth);
            currentMana = Mathf.Clamp(currentMana, 0, stats.maximumMana);

            // Broadcast that the character has taken damage
            Damage.RaiseDamaged(this, damager, healthDamage);

            if (currentHealth == 0)
            {
                // ToDo: proper death, end level

                // Broadcast that the character has been killed
                Damage.RaiseKilled(this, damager);
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Increase the character's health by the given amount
        /// </summary>
        /// <param name="amount"></param>
        public void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, stats.maximumHealth);
        }
    }
}

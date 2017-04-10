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
    }
}

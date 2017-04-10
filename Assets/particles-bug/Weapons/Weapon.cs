using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel
{
    /// <summary>
    /// Base class for all weapons (wands)
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// States the weapon can be in
        /// </summary>
        protected enum State { Pointer, Idle, Drawing, Charged, Throwing, Firing };

        /// <summary>
        /// The tip of the weapon, for attaching spells to
        /// </summary>
        public GameObject weaponTip;

        /// <summary>
        /// The tip of the weapon, for grabbing with VRTK
        /// </summary>
        public Rigidbody weaponTipAttachmentPoint;

        /// <summary>
        /// The GameObject that gets placed in a trail while a sigil is being drawn
        /// </summary>
        public GameObject trailObject;

        /// <summary>
        /// Whether we're on an AI character or not
        /// </summary>
        public bool onAi = false;

        /// <summary>
        /// The character that's holding the weapon
        /// </summary>
        public Character holder { get; protected set; }

        /// <summary>
        /// The current state
        /// </summary>
        protected State state = State.Idle;

        /// <summary>
        /// The spell currently being fired
        /// </summary>
        protected Spells.Spell currentSpell;

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnEnable()
        {
            Assert.IsNotNull(weaponTip, "Weapon tip is null");
            Assert.IsNotNull(weaponTipAttachmentPoint, "Weapon tip attachment point is null");

            holder = GetComponentInParent<Character>();
            Assert.IsNotNull(holder, "Character not found in parents for " + name);
        }

        /// <summary>
        /// Fire the current spell
        /// </summary>
        protected virtual void FireSpell()
        {
            currentSpell.Fire(this, holder);
            state = State.Firing;
        }
    }
}

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace MagicDuel.Spells
{
    public abstract class Spell
    {
        public delegate void EffectAction(Spell spell);
        
        public enum FiringMethod { FIRE, THROW, FLAME, NONE }

        /// <summary>
        /// The spell's internal name.  This is the name of the directory under Resources
        /// </summary>
        public string name { get; protected set; }

        /// <summary>
        /// The spell's name displayed to the player
        /// </summary>
        public string title { get; protected set; }

        /// <summary>
        /// A description of the spell for the spellbook
        /// </summary>
        public string description { get; protected set; }

        /// <summary>
        /// The minimum rank at which the player may unlock the spell
        /// </summary>
        public int rank { get; protected set; }

        /// <summary>
        /// The number of study points to unlock the spell
        /// </summary>
        public int unlockCost { get; protected set; }

        /// <summary>
        /// The name of the parent spell
        /// </summary>
        public string parentName { get; protected set; }

        /// <summary>
        /// The amount of mana spent in casting the spell
        /// </summary>
        public int manaCost { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public FiringMethod firingMethod { get; protected set; }
        public float projectileSpeed { get; protected set; }
        public GameObject chargedObject { get; protected set; }
        public Spell parent { get; set; }
        public List<Spell> children { get; set; }
        public List<Spells.Projectiles.Projectile> projectiles { get; protected set; }

        protected GameObject chargedObjectInstance;

        /// <summary>
        /// The character that cast the spell
        /// </summary>
        protected Character caster;

        /**
         * Create the new spell
         * 
         * @param name The spell's standard properties
         */
        public Spell(StandardSpellProperties standardSpellProperties)
        {
            name = standardSpellProperties.name;
            firingMethod = standardSpellProperties.firingMethod;
            title = standardSpellProperties.title;
            description = standardSpellProperties.description;
            rank = standardSpellProperties.rank;
            unlockCost = standardSpellProperties.unlockCost;
            manaCost = standardSpellProperties.manaCost;
            parentName = standardSpellProperties.parentName;
            projectileSpeed = standardSpellProperties.projectileSpeed;
            chargedObject = standardSpellProperties.chargedObject;

            children = new List<Spell>();
            projectiles = new List<Spells.Projectiles.Projectile>();
        }

        /// <summary>
        /// Create the charge object and attach it to the tip of the spell.
        /// This effect indicates that the wand has recognized the sigil and
        /// is ready to fire
        /// </summary>
        /// <param name="weapon">The wand to which the charge will be attached</param>
        public virtual void Charge(Weapon weapon)
        {
            // ToDo: add AddCharge(GameObject charge) to Weapon
            chargedObjectInstance = (GameObject)GameObject.Instantiate(chargedObject, weapon.weaponTip.transform);
            chargedObjectInstance.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Begin firing the spell.  This should be called when the trigger is pressed
        /// </summary>
        /// <param name="weapon">The wand firing the spell</param>
        public virtual void Fire(Weapon weapon, Character caster)
        {
            // Store who is casting the spell, so that damage can be attributed later
            this.caster = caster;

            // ToDo: add RemoveCharge to Weapon
            // Destroy the charge object
            GameObject.Destroy(chargedObjectInstance);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCasting()
        {
            return false;
        }

        /// <summary>
        /// Calculate the force needed to throw a projectile to the given target position
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        public virtual Vector3 GetFiringForce(Vector3 start, Vector3 target, float speed, bool direct = true)
        {
            return Vector3.forward;
        }
    }
}

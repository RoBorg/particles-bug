using UnityEngine;
using UnityEngine.Assertions;
using MagicDuel.Sigils;
using System.Collections.Generic;

namespace MagicDuel.Spells
{
    public abstract class Spell : Damager
    {
        public delegate void EffectAction(Spell spell);

        public static event EffectAction StartEffectEvent;
        public static event EffectAction EndEffectEvent;

        public event EffectAction FireStartEvent;
        public event EffectAction FireEndEvent;

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
        /// The sigil drawn to cast the spell
        /// </summary>
        public Sigil sigil { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public FiringMethod firingMethod { get; protected set; }
        public float projectileSpeed { get; protected set; }
        public GameObject chargedObject { get; protected set; }
        public DamageAmounts damage { get; protected set; }
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
            sigil = standardSpellProperties.sigil;
            firingMethod = standardSpellProperties.firingMethod;
            title = standardSpellProperties.title;
            description = standardSpellProperties.description;
            rank = standardSpellProperties.rank;
            unlockCost = standardSpellProperties.unlockCost;
            manaCost = standardSpellProperties.manaCost;
            parentName = standardSpellProperties.parentName;
            projectileSpeed = standardSpellProperties.projectileSpeed;
            chargedObject = standardSpellProperties.chargedObject;
            damage = new DamageAmounts(standardSpellProperties.damageHealth, standardSpellProperties.damageMana,
                standardSpellProperties.damageFire, standardSpellProperties.damageIce, standardSpellProperties.damageWater,
                standardSpellProperties.damageLightning);

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

            // Raise the event to say that firing has started
            RaiseFireStart();

            // If it's not a continuous-fire spell (flamethrower, grenade), stop firing
            if ((firingMethod != FiringMethod.FLAME) && (firingMethod != FiringMethod.THROW))
            {
                RaiseFireEnd();
            }

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
        /// Call when the trigger is released - stop firing the flamethrower
        /// or release the grenade
        /// </summary>
        public void Release()
        {
            RaiseFireEnd();
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
            var force = Ballistics.GetForce(start, target, speed, direct);

            if (force == null)
            {
                // Fire at 45 degrees
                var direction = target - start;
                direction.y = 0;
                direction.Normalize();
                direction.y = 1;
                direction.Normalize();

                return direction * speed;
            }

            return (Vector3)force;
        }

        /// <summary>
        /// Raise the event to indicate that 
        /// </summary>
        protected void RaiseStartEffect()
        {
            if (StartEffectEvent != null)
            {
                StartEffectEvent(this);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        protected void RaiseEndEffect()
        {
            if (EndEffectEvent != null)
            {
                EndEffectEvent(this);
            }
        }

        /// <summary>
        /// Raise the event to say that firing has started, i.e. the trigger has been
        /// pressed
        /// </summary>
        protected void RaiseFireStart()
        {
            if (FireStartEvent != null)
            {
                FireStartEvent(this);
            }
        }

        /// <summary>
        /// Raise the event to say that firing has stopped, i.e. the trigger has been
        /// released
        /// </summary>
        protected void RaiseFireEnd()
        {
            if (FireEndEvent != null)
            {
                FireEndEvent(this);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="damageSourcePosition">The point in world space from which damage should be applied</param>
        public void DoDamage(Vector3 damageSourcePosition)
        {
            Damage.RaiseDamage(this, damageSourcePosition);
        }

        /// <summary>
        /// From the Damager interface
        /// Get the character causing the damager
        /// </summary>
        /// <returns>Returns the character that cast the spell</returns>
        public Character GetCaster()
        {
            return caster;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="damageSourcePosition"></param>
        /// <param name="damageable"></param>
        /// <returns></returns>
        public DamageAmounts GetBaseDamage(Vector3 damageSourcePosition, Damageable damageable)
        {
            var splashRadius = 1; // TODO
            var distance = Damage.GetDistance(damageSourcePosition, damageable.GetCollider());
            var distanceMultiplier = 0f;
            var normalizedDistance = distance / splashRadius;
            var baseDamage = new DamageAmounts();

            if (distance > splashRadius)
            {
                return baseDamage;
            }

            foreach (var damageType in (DamageTypes[])System.Enum.GetValues(typeof(DamageTypes)))
            {

                if (damageType == DamageTypes.Mana)
                {
                    distanceMultiplier = (splashRadius - distance) / splashRadius;
                }
                else
                {
                    distanceMultiplier = 3 - (2 * Mathf.Exp((normalizedDistance * normalizedDistance) / Mathf.Sqrt(2 * Mathf.PI)));
                }

                baseDamage.Set(damageType, damage.Get(damageType) * distanceMultiplier);
            }

            return baseDamage;
        }
    }
}

using UnityEngine;

namespace MagicDuel.Spells
{
    public class StandardSpellProperties
    {
        public string name { get; protected set; }
        public Sigils.Sigil sigil { get; protected set; }
        public Spell.FiringMethod firingMethod { get; protected set; }
        public string title { get; protected set; }
        public string description { get; protected set; }
        public int rank { get; protected set; }
        public int unlockCost { get; protected set; }
        public int manaCost { get; protected set; }
        public float projectileSpeed { get; protected set; }
        public string parentName { get; protected set; }
        public GameObject chargedObject { get; protected set; }
        public int damageHealth { get; protected set; }
        public int damageMana { get; protected set; }
        public int damageFire { get; protected set; }
        public int damageIce { get; protected set; }
        public int damageWater { get; protected set; }
        public int damageLightning { get; protected set; }

        /// <summary>
        /// Create the new StandardSpellProperties
        /// </summary>
        /// <param name="name">The internal name</param>
        /// <param name="sigil">The sigil</param>
        /// <param name="title">The external (user visible) name</param>
        /// <param name="description">A textual description of the spell</param>
        /// <param name="rank">The rank at which a player may unlock the spell</param>
        /// <param name="locked">Whether the spell is locked or not</param>
        /// <param name="unlockCost">The number of study points required to unlock the spell</param>
        /// <param name="manaCost">The amount of mana used when casting the spell</param>
        /// <param name="parentName">The name of the parent spell, or empty string for none</param>
        public StandardSpellProperties(string name, Sigils.Sigil sigil, Spell.FiringMethod firingMethod, string title, string description, int rank,
            bool locked, int unlockCost, int manaCost, float projectileSpeed, string parentName, GameObject chargedObject, int damageHealth,
            int damageMana, int damageFire, int damageIce, int damageWater, int damageLightning)
        {
            this.name = name;
            this.sigil = sigil;
            this.firingMethod = firingMethod;
            this.title = title;
            this.description = description;
            this.rank = rank;
            this.unlockCost = unlockCost;
            this.manaCost = manaCost;
            this.projectileSpeed = projectileSpeed;
            this.parentName = parentName;
            this.chargedObject = chargedObject;
            this.damageHealth = damageHealth;
            this.damageMana = damageMana;
            this.damageFire = damageFire;
            this.damageIce = damageIce;
            this.damageWater = damageWater;
            this.damageLightning = damageLightning;
        }
    }
}

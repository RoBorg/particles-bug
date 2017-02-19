using UnityEngine;
using UnityEngine.Assertions;
using System.Xml;
using System.Collections.Generic;

namespace MagicDuel.Spells
{
    public class SpellFactory
    {
        private abstract class SpellInstanceFactory
        {
            protected StandardSpellProperties standardSpellProperties;

            public abstract Spell GetSpell();

            protected T LoadObject<T>(string name)
                where T : MonoBehaviour
            {
                var o = Resources.Load<T>("Spells/" + standardSpellProperties.name + "/" + name);
                Assert.IsNotNull(o, "Resource Spells/" + standardSpellProperties.name + "/" + name + " not loaded as " + typeof(T));

                return o;
            }
        }

        private class FlamethrowerFactory : SpellInstanceFactory
        {
            private Projectiles.Projectile flameObject;
            private Projectiles.Projectile flameDamageProbeObject;
            private float spreadAngle;
            private float damagePerSecond;
            private float duration;

            public FlamethrowerFactory(StandardSpellProperties standardSpellProperties, float spreadAngle, float damagePerSecond, float duration)
            {
                this.standardSpellProperties = standardSpellProperties;
                this.spreadAngle = spreadAngle;
                this.damagePerSecond = damagePerSecond;
                this.duration = duration;

                flameObject = LoadObject<Projectiles.Projectile>("Flame");
                flameDamageProbeObject = LoadObject<Projectiles.Projectile>("FlameDamageProbe");
            }

            public override Spell GetSpell()
            {
                return new Flamethrower(standardSpellProperties, flameObject, flameDamageProbeObject, spreadAngle, damagePerSecond, duration);
            }
        }

        private static Dictionary<string, SpellInstanceFactory> spellFactories = new Dictionary<string, SpellInstanceFactory>();

        public static Spell Create(XmlNode spellNode)
        {
            var name = spellNode.SelectSingleNode("name").InnerText;
            var firingMethod = (Spell.FiringMethod)System.Enum.Parse(typeof(Spell.FiringMethod), spellNode.SelectSingleNode("firingMethod").InnerText);
            var title = spellNode.SelectSingleNode("title").InnerText;
            var description = spellNode.SelectSingleNode("description").InnerText;
            var rank = int.Parse(spellNode.SelectSingleNode("rank").InnerText);
            var locked = bool.Parse(spellNode.SelectSingleNode("locked").InnerText);
            var unlockCost = int.Parse(spellNode.SelectSingleNode("unlockCost").InnerText);
            var manaCost = int.Parse(spellNode.SelectSingleNode("manaCost").InnerText);
            var projectileSpeed = float.Parse(spellNode.SelectSingleNode("projectileSpeed").InnerText);
            var parentName = spellNode.SelectSingleNode("parentName").InnerText;
            var chargedObject = Resources.Load<GameObject>("Spells/" + name + "/Charged");
            var damageHealth = GetDamageValue(spellNode, "health");
            var damageMana = GetDamageValue(spellNode, "mana");
            var damageFire = GetDamageValue(spellNode, "fire");
            var damageIce = GetDamageValue(spellNode, "ice");
            var damageWater = GetDamageValue(spellNode, "water");
            var damageLightning = GetDamageValue(spellNode, "lightning");

            var standardSpellProperties = new StandardSpellProperties(name, firingMethod, title, description,
                rank, locked, unlockCost, manaCost, projectileSpeed, parentName, chargedObject, damageHealth,
                damageMana, damageFire, damageIce, damageWater, damageLightning);

            SpellInstanceFactory spellInstanceFactory = null;

            string[] types = { "flamethrower" };
            var typesXpath = string.Join("|", types);


            var node = spellNode.SelectSingleNode(typesXpath);
            Assert.IsNotNull(node, "XML error - no spell type node found");

            switch (node.Name)
            {
                case "flamethrower":
                    {
                        var spreadAngle = float.Parse(node.SelectSingleNode("spreadAngle").InnerText);
                        var damagePerSecond = float.Parse(node.SelectSingleNode("damagePerSecond").InnerText);
                        var duration = float.Parse(node.SelectSingleNode("duration").InnerText);

                        spellInstanceFactory = new FlamethrowerFactory(standardSpellProperties, spreadAngle, damagePerSecond, duration);
                        break;
                    }

                default:
                    {
                        throw new System.ArgumentException("No creation code for " + node.Name);
                    }
            }

            spellFactories[standardSpellProperties.name] = spellInstanceFactory;

            return spellInstanceFactory.GetSpell();
        }

        /// <summary>
        /// Get the number under spell/damage/[health/mana/...]
        /// </summary>
        /// <param name="spellNode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static int GetDamageValue(XmlNode spellNode, string name)
        {
            var damageNode = spellNode.SelectSingleNode("damage");

            if (damageNode == null)
            {
                return 0;
            }

            var amountNode = damageNode.SelectSingleNode(name);

            if (amountNode == null)
            {
                return 0;
            }

            return int.Parse(amountNode.InnerText);
        }

        public static Spell GetSpell(string name)
        {
            return spellFactories[name].GetSpell();
        }
    }
}

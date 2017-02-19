using UnityEngine;
using System.Collections.Generic;
using System.Xml;

namespace MagicDuel.Spells
{
    public class Library
    {
        public delegate void UpdateEvent();
        public event UpdateEvent OnUpdate;

        public List<Spell> spells = new List<Spell>();
        protected SpellFactory spellFactory = new SpellFactory();

        /**
         * Load all the sigils using the Library.txt XML file
         */
        public void Load()
        {
            // Load the document listing all the sigils
            var libraryXmlSource = Resources.Load<TextAsset>("Spells").text;
            var libraryXml = new XmlDocument();
            libraryXml.LoadXml(libraryXmlSource);
            var spellsNode = libraryXml.SelectSingleNode("spells");
            var spellNodes = spellsNode.SelectNodes("spell");
            spells.Clear();

            foreach (XmlNode spellNode in spellNodes)
            {
                LoadSpell(spellNode);
            }
        }

        /**
         * Get a spell by name
         *
         * @param string name The spell name
         *
         * @return Spell The spell
         */
        public Spell Get(string name)
        {
            foreach (var spell in spells)
            {
                if (spell.name == name)
                    return spell;
            }

            throw new System.ArgumentException("Unknown spell name '" + name + '"');
        }

        /**
         * Get a spell by feature list
         *
         * @param string featureHash THe features hash
         *
         * @return Spell The spell or null if none match
         */
        public Spell GetByHash(string featuresHash)
        {
            foreach (var spell in spells)
            {
                if (spell.sigil.featuresHash == featuresHash)
                {
                    return spell;
                }
            }

            return null;
        }

        /**
         * Load a single sigil from the XML document
         * 
         * @param sigilNode The XML document <sigil> node to load the data from
         */
        protected void LoadSpell(XmlNode spellNode)
        {
            spells.Add(SpellFactory.Create(spellNode));
        }

        /**
         * Get a list of the spells that are available to be unlocked
         * 
         * @return Returns a list of spells that can be unlocked
         */
        public List<Spell> GetUnlockableSpells()
        {
            return new List<Spell>();
        }

        /**
         * Get a list of the spells that are available to be unlocked
         * 
         * @param rank The player's rank
         * @param studyPoints The number of study points available to spend
         * 
         * @return Returns a list of spells that can be unlocked
         */
        public List<Spell> GetUnlockableSpells(int rank, int studyPoints)
        {
            var unlockableSpells = new List<Spell>();

            return unlockableSpells;
        }

        /**
         * Unlock a spell
         * 
         * @param spell The spell to unlock
         */
        public void Unlock(Spells.Spell spell)
        {
            if (OnUpdate != null)
                OnUpdate();

        }
    }
}

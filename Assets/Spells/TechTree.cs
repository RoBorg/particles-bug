using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace MagicDuel.Spells
{
    public class TechTree
    {
        /** @var spells The root level spells */
        protected List<Spell> spells = new List<Spell>();

        /** @var depth The number of levels in the tree */
        public int depth { get; protected set; }

        /** @var width The maximum width of any level */
        public int width { get; protected set; }

        /** @var ranks The maximum rank */
        public int ranks { get; protected set; }

        protected Library spellsLibrary;

        /**
         * Build the tree representation of a library of spells
         */
        public void Build(Library spellsLibrary)
        {
            this.spellsLibrary = spellsLibrary;
            foreach (var spell in spellsLibrary.spells)
            {
                if (spell.parentName == "")
                {
                    spells.Add(spell);
                }
                else
                {
                    spell.parent = spellsLibrary.Get(spell.parentName);
                    Assert.IsNotNull(spell.parentName, "Invalid parent spell name: " + spell.parentName);
                    spell.parent.children.Add(spell);
                }
            }

            CalculateDepth();
            CalculateWidth();
            CalculateRanks();
        }

        /**
         * Calculate the value of the depth property
         */
        protected void CalculateDepth()
        {
            depth = 0;

            // Find the child with the largest depth
            foreach (var spell in spellsLibrary.spells)
                depth = Mathf.Max(depth, CalculateDepth(spell));
        }

        /**
         * Calculate the value of the depth property
         * 
         * @param spell The spell to look at
         * 
         * @return Returns the maximum depth below the given spell
         */
        protected int CalculateDepth(Spell spell)
        {
            var newDepth = 0;

            // Find the child with the largest depth
            foreach (var child in spell.children)
                newDepth = Mathf.Max(newDepth, CalculateDepth(child));

            return newDepth + 1;
        }

        /**
         * Calculate the number of ranks
         */
        protected void CalculateRanks()
        {
            ranks = 0;

            // Find the child with the largest depth
            foreach (var spell in spellsLibrary.spells)
                ranks = Mathf.Max(ranks, spell.rank);
        }

        /**
         * Get all the spells, grouped by rank
         */
        public List<Spell>[] GetRankedSpells()
        {
            int maxRank = 0;
            foreach (var spell in spellsLibrary.spells)
                maxRank = Mathf.Max(maxRank, spell.rank);

            var rankedSpells = new List<Spell>[maxRank];

            for (var i = 0; i < maxRank; ++i)
                rankedSpells[i] = new List<Spell>();

            foreach (var spell in spellsLibrary.spells)
                rankedSpells[spell.rank - 1].Add(spell);

            return rankedSpells;
        }

        /**
         * Calculate the value of the width property
         */
        public void CalculateWidth()
        {
            int maxRank = 0;
            foreach (var spell in spellsLibrary.spells)
                maxRank = Mathf.Max(maxRank, spell.rank);

            var rankTotal = new int[maxRank];

            foreach (var spell in spellsLibrary.spells)
                rankTotal[spell.rank - 1]++;

            width = 0;
            for (var i = 0; i < maxRank; ++i)
                width = Mathf.Max(width, rankTotal[i]);
        }


        /**
         * Get all the spells at the given mastery rank
         * 
         * @param rank The mastery rank to search for
         * 
         * @return Returns a list of all spells at that rank
         */
        public List<Spell> GetByRank(int rank)
        {
            var rankSpells = new List<Spell>();

            foreach (var spell in spellsLibrary.spells)
            {
                if (spell.rank == rank)
                    rankSpells.Add(spell);
            }

            return rankSpells;
        }

        protected List<Spell> GetByLevel(int desiredLevel, int currentLevel, Spell spell)
        {
            var levelSpells = new List<Spell>();

            if (currentLevel == desiredLevel)
            {
                levelSpells.Add(spell);
                return levelSpells;
            }

            foreach (var child in spell.children)
                levelSpells.AddRange(GetByLevel(desiredLevel, currentLevel + 1, spell));

            return levelSpells;
        }

        public List<Spell> GetSpells()
        {
            return spells;
        }
    }
}
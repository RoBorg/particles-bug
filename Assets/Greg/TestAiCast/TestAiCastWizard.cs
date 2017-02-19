using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicDuel.Wizards
{
    public class TestAiCastWizard : Wizard
    {
        public string spellName;
        public Wizard characterToFight;

        protected override Character GetCharacterToFight()
        {
            return characterToFight;
        }

        public override Spells.Spell GetSpellToCast()
        {
            var spellsLibrary = new Spells.Library();
            spellsLibrary.Load();

            return Spells.SpellFactory.GetSpell(spellName);
        }
    }
}

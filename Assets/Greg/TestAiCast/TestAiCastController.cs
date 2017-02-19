using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicDuel.Spells;

namespace MagicDuel.TestAiCast
{
    public class TestAiCastController : MonoBehaviour
    {
        public Wizards.TestAiCastWizard wizardPrefab;
        public Wizards.TargetWizard targetPrefab;

        private void Start()
        {
            var spellsLibrary = new Spells.Library();
            spellsLibrary.Load();
            var offset = -spellsLibrary.spells.Count / 2;

            foreach (var spell in spellsLibrary.spells)
            {
                Debug.Log(spell.name);
                var wizard = Instantiate(wizardPrefab);
                wizard.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(new Vector3(offset * 2, 0, -3));
                wizard.name = spell.name + " Wizard";
                wizard.spellName = spell.name;

                var target = Instantiate(targetPrefab);
                target.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(new Vector3(offset * 2, 0, 3));
                target.name = spell.name + " Target";

                wizard.characterToFight = target;

                offset++;
            }
        }
    }
}

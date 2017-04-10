using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using RootMotion.FinalIK;

namespace MagicDuel.Wizards.WizardStateMachine.States
{

    public class WizardStateCast : WizardState
    {
        public enum ArmState { DRAWING, THROWING, POINTING, SHIELDING, NONE }

        private Animator animator;
        private IKSolverLimb rightHand;
        private IKSolverLookAt lookAt;
        private Vector3 weaponTarget;

        protected override void OnEnable()
        {
            base.OnEnable();

            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator not found");

            var biped = GetComponent<BipedIK>();
            Assert.IsNotNull(biped, "Biped IK not found");

            rightHand = biped.solvers.rightHand;
            Assert.IsNotNull(rightHand, "Right Hand not found");
            Assert.IsNotNull(rightHand.target, "Right Hand target not set");

            lookAt = biped.solvers.lookAt;
            Assert.IsNotNull(lookAt, "Look At not found");
            Assert.IsNotNull(lookAt.target, "Look At target not set");

            var spell = wizard.GetSpellToCast();

            StartCoroutine(Cast(spell));
        }

        /*
        private void Update()
        {
            if (rightArmState == ArmState.POINTING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        */

        /*
        private void LateUpdate()
        {
            if (rightArmState == ArmState.POINTING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        */

        /*
        private void OnAnimatorIK()
        {
            Debug.Log("IIK");
            if (rightArmState == ArmState.POINTING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        */

        private IEnumerator Cast(Spells.Spell spell)
        {
            var enemyHeight = 1.8f;

            spell.Fire(wizard.weapon, wizard);
            
            // Keep the wand pointed at the enemy
            while (true)
            {
                yield return new WaitForEndOfFrame();
                //yield return null;

                var enemyFeet = wizard.enemy.transform.position;
                var enemyChest = enemyFeet + (Vector3.up * enemyHeight * 0.8f);

                var target = (enemyFeet - wizard.weapon.transform.position).normalized + wizard.weapon.transform.position;
                rightHand.target.position = target;
                weaponTarget = enemyChest;
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
    }
}

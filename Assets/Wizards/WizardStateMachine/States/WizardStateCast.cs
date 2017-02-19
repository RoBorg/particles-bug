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
        private IKSolverLimb leftHand;
        private IKSolverLimb rightHand;
        private IKSolverLookAt lookAt;
        private Vector3 weaponTarget;
        private Transform shoulder;

        protected override void OnEnable()
        {
            base.OnEnable();

            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator not found");

            shoulder = transform.Find("Reference/Hips/Spine/Chest/RightShoulder/RightArm");
            Assert.IsNotNull(shoulder, "Right Arm not found");

            var biped = GetComponent<BipedIK>();
            Assert.IsNotNull(biped, "Biped IK not found");

            leftHand = biped.solvers.leftHand;
            Assert.IsNotNull(leftHand, "Left Hand not found");
            Assert.IsNotNull(leftHand.target, "Left Hand target not set");

            rightHand = biped.solvers.rightHand;
            Assert.IsNotNull(rightHand, "Right Hand not found");
            Assert.IsNotNull(rightHand.target, "Right Hand target not set");

            lookAt = biped.solvers.lookAt;
            Assert.IsNotNull(lookAt, "Look At not found");
            Assert.IsNotNull(lookAt.target, "Look At target not set");

            var spell = wizard.GetSpellToCast();

            StartCoroutine(Cast(spell, () => stateMachine.animator.SetTrigger("FinishedCast")));
        }

        /*
        private void Update()
        {
            if (rightArmState == ArmState.THROWING)
            {

            }
            else if (rightArmState == ArmState.DRAWING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
            else if (rightArmState == ArmState.POINTING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        */

        /*
        private void LateUpdate()
        {
            if (rightArmState == ArmState.THROWING)
            {

            }
            else if (rightArmState == ArmState.DRAWING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
            else if (rightArmState == ArmState.POINTING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        */

        /*
        private void OnAnimatorIK()
        {
            Debug.Log("IIK");
            if (rightArmState == ArmState.THROWING)
            {

            }
            else if (rightArmState == ArmState.DRAWING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
            else if (rightArmState == ArmState.POINTING)
            {
                wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        */

        private IEnumerator Cast(Spells.Spell spell, System.Action callback)
        {
            var force = Vector3.zero;
            var wandTip = wizard.weapon.weaponTip.transform.position;
            // ToDo: get this properly
            var enemyHeight = 1.8f;
            var enemyFeet = wizard.enemy.transform.position;
            var enemyChest = enemyFeet + (Vector3.up * enemyHeight * 0.8f);

            {
                var target = enemyChest;
                force = spell.GetFiringForce(wandTip, target, 100);
            }

            // ToDo: do this with IK
            //wizard.weapon.transform.LookAt(enemyChest);
            wizard.weapon.transform.LookAt(wizard.weapon.weaponTip.transform.position + force);
            spell.Fire(wizard.weapon, wizard);

            var startTarget = rightHand.target.position;
            var time = 0f;

            // Keep the wand pointed at the enemy
            while (spell.IsCasting())
            {
                yield return new WaitForEndOfFrame();
                //yield return null;

                time += Time.deltaTime;

                enemyFeet = wizard.enemy.transform.position;
                enemyChest = enemyFeet + (Vector3.up * enemyHeight * 0.8f);

                var target = (enemyFeet - wizard.weapon.transform.position).normalized + wizard.weapon.transform.position;
                rightHand.target.position = Vector3.Lerp(startTarget, target, time / 0.5f);
                weaponTarget = enemyChest;
                //lookAt.target.position = enemyHead;
                wizard.weapon.transform.LookAt(weaponTarget);
            }

            rightHand.IKPositionWeight = 1;
            lookAt.IKPositionWeight = 1;

            // Put the wand back to its resting position
            var startPosition = wizard.weapon.transform.forward - wizard.weapon.transform.position;
            var endPosition = wizard.weapon.transform.parent.forward - wizard.weapon.transform.parent.position;

            while (weaponTarget != endPosition)
            {
                weaponTarget = Vector3.Lerp(startPosition, endPosition, Time.time * 0.1f);
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            Debug.Log("Finished cast");
            callback();
        }
    }
}

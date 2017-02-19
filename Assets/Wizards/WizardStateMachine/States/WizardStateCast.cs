using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using RootMotion.FinalIK;

namespace MagicDuel.Wizards.WizardStateMachine.States
{

    public class WizardStateCast : WizardState
    {
        public enum ArmState { DRAWING, THROWING, POINTING, SHIELDING, NONE }
        public GameObject trail;

        private GameObject sigilContainer;
        private Animator animator;
        private float drawTime = 1f;
        private float aimingTime = 0.5f;
        private IKSolverLimb leftHand;
        private IKSolverLimb rightHand;
        private IKSolverLookAt lookAt;
        private ArmState leftArmState = ArmState.NONE;
        private ArmState rightArmState = ArmState.NONE;
        private Coroutine leftHandTransition;
        private Coroutine rightHandTransition;
        private Coroutine lookAtTransition;
        private Vector3 weaponTarget;
        private Transform shoulder;

        protected override void OnEnable()
        {
            base.OnEnable();

            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator not found");

            shoulder = transform.Find("Reference/Hips/Spine/Chest/RightShoulder/RightArm");
            Assert.IsNotNull(shoulder, "Right Arm not found");

            Assert.IsNotNull(trail, "Trail not set");

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

            StartCoroutine(DrawSigil(spell.sigil, () =>
            {
                StartCoroutine(Cast(spell, () => stateMachine.animator.SetTrigger("FinishedCast")));
            }));
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            //ToDo: cancel casting if in progress
        }


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
                //wizard.weapon.transform.LookAt(weaponTarget);
            }
        }
        

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
        private IEnumerator DrawSigil(Sigils.Sigil sigil, System.Action callback)
        {
            
            var points = sigil.GetPoints(0.02f);

            if (rightHandTransition != null)
            {
                StopCoroutine(rightHandTransition);
            }

            //rightHandTransition = StartCoroutine(Transition(() => { return rightHand.IKPositionWeight; }, value => { rightHand.IKPositionWeight = value; }, 0, 1, 4));
            /*
            rightArmState = ArmState.DRAWING;

            sigilContainer = new GameObject();
            sigilContainer.transform.position = transform.position + (transform.forward * 0.5f) + (Vector3.up * 1.5f);
            sigilContainer.transform.LookAt(transform.position + (Vector3.up * 1.4f));
            sigilContainer.transform.localScale = Vector3.one * 0.5f;

            foreach (var point in points)
            {
                var trailPoint = (GameObject)Instantiate(trail, sigilContainer.transform);

                trailPoint.transform.localPosition = point;
                trailPoint.transform.localRotation = Quaternion.identity;

                // Put the hand at half way between the shoulder and the sigil point
                rightHand.target.position = shoulder.position + ((trailPoint.transform.position - shoulder.position) / 2);
                weaponTarget = trailPoint.transform.position;

                yield return new WaitForSeconds(drawTime / points.Length);
            }

            StartCoroutine(DestroySigil(sigilContainer, 1f));
            */
            yield return new WaitForSeconds(aimingTime);
            callback();
        }

        private IEnumerator DestroySigil(GameObject container, float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(container, 2f);

            foreach (Transform child in container.transform)
            {
                var rb = child.gameObject.AddComponent<Rigidbody>();
                rb.mass = Random.Range(0.01f, 0.05f);
                rb.useGravity = false;

                rb.AddExplosionForce(0.5f, container.transform.position, 20f);

                // Disable the particle emission
                // This has to be written in a roundabout way like this to work
                var particleSystem = child.gameObject.GetComponentInChildren<ParticleSystem>();

                if (particleSystem != null)
                {
                    var emission = particleSystem.emission;
                    emission.rateOverTime = 0;
                }
            }
        }

        private IEnumerator Cast(Spells.Spell spell, System.Action callback)
        {
            var force = Vector3.zero;
            var wandTip = wizard.weapon.weaponTip.transform.position;
            // ToDo: get this properly
            var enemyHeight = 1.8f;
            var enemyFeet = wizard.enemy.transform.position;
            var enemyHead = enemyFeet + (Vector3.up * enemyHeight * 0.95f);
            var enemyChest = enemyFeet + (Vector3.up * enemyHeight * 0.8f);

            switch (spell.firingMethod)
            {
                case Spells.Spell.FiringMethod.FIRE:
                    {
                        {
                            var speed = spell.projectileSpeed;
                            var target = wizard.enemy.transform.position;

                            // Aim fast projectiles for the chest instead of the feet
                            if (speed >= 30)
                            {
                                target += Vector3.up * 1.5f;
                            }

                            force = spell.GetFiringForce(wandTip, target, 0);
                        }

                        break;
                    }

                case Spells.Spell.FiringMethod.THROW:
                    {
                        // ToDo: throwing speed
                        force = spell.GetFiringForce(wandTip, enemyFeet, 10f, false);

                        break;
                    }

                case Spells.Spell.FiringMethod.FLAME:
                    {
                        // ToDo: get enemy height
                        var target = enemyChest;

                        force = spell.GetFiringForce(wandTip, target, 100);

                        break;
                    }

                case Spells.Spell.FiringMethod.NONE:
                    {
                        // Do nothing
                        break;
                    }

                default:
                    {
                        throw new System.ArgumentException("Unknown firing method " + spell.firingMethod);
                    }
            }

            // ToDo: do this with IK
            //wizard.weapon.transform.LookAt(enemyChest);
            wizard.weapon.transform.LookAt(wizard.weapon.weaponTip.transform.position + force);
            spell.Fire(wizard.weapon, wizard);

            if (spell.firingMethod == Spells.Spell.FiringMethod.THROW)
            {
                // ToDo: Throwing animation
                foreach (var projectile in spell.projectiles)
                {
                    projectile.transform.parent = null;
                    projectile.GetComponent<Rigidbody>().isKinematic = false;
                    projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                }
            }
            else if (spell.firingMethod == Spells.Spell.FiringMethod.FLAME)
            {
                var startTarget = rightHand.target.position;
                var time = 0f;
                rightArmState = ArmState.POINTING;

                if (lookAtTransition != null)
                {
                    StopCoroutine(lookAtTransition);
                }

                //lookAtTransition = StartCoroutine(Transition(() => { return lookAt.IKPositionWeight; }, value => { lookAt.IKPositionWeight = value; }, 0, 1, 4));
                
                // Keep the wand pointed at the enemy
                while (spell.IsCasting())
                {
                    yield return new WaitForEndOfFrame();
                    //yield return null;

                    time += Time.deltaTime;

                    enemyFeet = wizard.enemy.transform.position;
                    enemyHead = enemyFeet + (Vector3.up * enemyHeight * 0.95f);
                    enemyChest = enemyFeet + (Vector3.up * enemyHeight * 0.8f);

                    var target = (enemyFeet - wizard.weapon.transform.position).normalized + wizard.weapon.transform.position;
                    rightHand.target.position = Vector3.Lerp(startTarget, target, time / 0.5f);
                    weaponTarget = enemyChest;
                    //lookAt.target.position = enemyHead;
                    wizard.weapon.transform.LookAt(weaponTarget);
                }
                
            }

            if (rightHandTransition != null)
            {
                StopCoroutine(rightHandTransition);
            }

            rightHandTransition = StartCoroutine(Transition(() => { return rightHand.IKPositionWeight; }, value => { rightHand.IKPositionWeight = value; }, 0, 1, -4));

            if (lookAtTransition != null)
            {
                StopCoroutine(lookAtTransition);
            }

            lookAtTransition = StartCoroutine(Transition(() => { return lookAt.IKPositionWeight; }, value => { lookAt.IKPositionWeight = value; }, 0, 1, -4));

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

        private IEnumerator Transition(System.Func<float> GetValue, System.Action<float> SetValue, float min, float max, float speed)
        {
            var value = 0f;

            while (true)
            {
                value = GetValue();
                value += speed * Time.deltaTime;

                if ((speed < 0) && (value <= min))
                {
                    break;
                }
                else if ((speed > 0) && (value >= max))
                {
                    break;
                }

                SetValue(value);

                yield return new WaitForEndOfFrame();
            }

            SetValue(Mathf.Clamp(value, min, max));
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace MagicDuel.Wizards
{
    [RequireComponent(typeof(UnityEngine.AI.NavMesh))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WizardStateMachine.States.WizardStateIdle))]
    [RequireComponent(typeof(WizardStateMachine.States.WizardStateMove))]
    [RequireComponent(typeof(WizardStateMachine.States.WizardStateCast))]
    public class Wizard : Character
    {
        public Transform movementTarget;
        public RuntimeAnimatorController stateMachineController;
        public WizardStateMachine.WizardStateMachine stateMachine { get; protected set; }
        public Transform wandAttachPoint;

        public System.Action ReachDestinationEvent;

        protected Vector3 lastMovementTarget = Vector3.zero;
        protected float turnSpeed = 10;
        protected Bounds navMeshBounds = new Bounds();

        private UnityEngine.AI.NavMeshAgent agent;
        public Character enemy { get; protected set; }
        public Weapon weapon { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            Assert.IsNotNull(agent, "NavMeshAgent component not found");

            var triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();

            foreach (var point in triangulation.vertices)
            {
                navMeshBounds.Encapsulate(point);
            }

            var animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator not found");

            agent.updatePosition = false;

            stateMachine = CreateStateMachine();

            weapon = wandAttachPoint.GetComponentInChildren<Weapon>();
            Assert.IsNotNull(weapon, "Weapon not found");
        }

        protected virtual void Start()
        {
            enemy = GetCharacterToFight();
            Assert.IsNotNull(enemy, "Nobody found to fight!");

            if (movementTarget == null)
            {
                var g = new GameObject("Target (" + name + ")");
                g.transform.position = transform.position;
                movementTarget = g.transform;
            }
        }

        private WizardStateMachine.WizardStateMachine CreateStateMachine()
        {
            var animatorObject = new GameObject("State Machine");
            animatorObject.transform.parent = transform;
            animatorObject.transform.localPosition = Vector3.zero;

            var stateMachineAnimator = animatorObject.AddComponent<Animator>();
            stateMachineAnimator.runtimeAnimatorController = stateMachineController;

            return animatorObject.AddComponent<WizardStateMachine.WizardStateMachine>();
        }

        /// <summary>
        /// Set the target position to move to
        /// </summary>
        /// <param name="target">The transform of the target position</param>
        public void SetTarget(Vector3 target)
        {
            target.y = 0;
            movementTarget.position = target;
        }

        /// <summary>
        /// Set the target position to move to
        /// </summary>
        /// <param name="gameObject">An object at the target position</param>
        public void SetTarget(GameObject gameObject)
        {
            SetTarget(gameObject.transform.position);
        }

        protected virtual Character GetCharacterToFight()
        {
            var characters = new List<Character>(FindObjectsOfType<Character>());
            characters.Remove(this);

            if (characters.Count == 0)
            {
                return null;
            }

            return characters[Random.Range(0, characters.Count - 1)];
        }

        public virtual Spells.Spell GetSpellToCast()
        {
            return null;
        }
    }
}

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
        private Cover[] covers;
        private Locomotion locomotion = null;
        public Character enemy { get; protected set; }
        public Weapon weapon { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            Assert.IsNotNull(agent, "NavMeshAgent component not found");

            covers = FindObjectsOfType<Cover>();

            var triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();

            foreach (var point in triangulation.vertices)
            {
                navMeshBounds.Encapsulate(point);
            }

            var animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator not found");

            locomotion = new Locomotion(animator);
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

        protected void OnEnable()
        {
            Spells.Spell.StartEffectEvent += OnSpellStartEffect;
            Spells.Spell.EndEffectEvent += OnSpellEndEffect;
        }

        protected void OnDisable()
        {
            Spells.Spell.StartEffectEvent -= OnSpellStartEffect;
            Spells.Spell.EndEffectEvent -= OnSpellEndEffect;
        }

        protected virtual void Update()
        {
        }

        /// <summary>
        /// Set the target position to move to
        /// </summary>
        /// <param name="target">The transform of the target position</param>
        public void SetTarget(Vector3 target)
        {
            target.y = 0;
            this.movementTarget.position = target;
        }

        /// <summary>
        /// Set the target position to move to
        /// </summary>
        /// <param name="gameObject">An object at the target position</param>
        public void SetTarget(GameObject gameObject)
        {
            SetTarget(gameObject.transform.position);
        }

        /// <summary>
        /// Set the target position so we're in cover from a position
        /// </summary>
        /// <param name="avoidTransform">The position to take cover from</param>
        public void TakeCoverFrom(Transform avoidTransform)
        {
            List<Vector3> coverPoints = new List<Vector3>();
            var avoid = avoidTransform.position;
            avoid.y = 0;

            if (IsInCover(avoid))
            {
                Debug.Log("Already in cover");
                return;
            }

            foreach (var cover in covers)
            {
                // Find a point on the other side of the cover from what we're hiding from
                var start = cover.transform.position - avoid;
                start.y = 0;

                // Raycast back from way past it so we can account for the depth of the cover
                RaycastHit hit;
                var rayStart = avoid + (start * 10f);
                var ray = new Ray(rayStart, avoid - rayStart);

                if (cover.collider.Raycast(ray, out hit, 9999))
                {
                    coverPoints.Add(hit.point);
                }
                else
                {
                    Assert.IsTrue(false, "No hit!");
                }
            }

            // Find the closest point to hide
            var minDist = Mathf.Infinity;
            Vector3? bestCoverPoint = null;

            // ToDo: don't always pick the closest one?
            foreach (var coverPoint in coverPoints)
            {
                var path = new UnityEngine.AI.NavMeshPath();
                if (UnityEngine.AI.NavMesh.CalculatePath(transform.position, coverPoint, UnityEngine.AI.NavMesh.AllAreas, path))
                {
                    var pathLength = GetPathLenth(path);

                    if (pathLength < minDist)
                    {
                        minDist = pathLength;
                        bestCoverPoint = coverPoint;
                    }
                }
            }

            if (bestCoverPoint != null)
            {
                movementTarget.position = (Vector3)bestCoverPoint;
            }
            else
            {
                Debug.Log("No suitable cover");
            }
        }

        /// <summary>
        /// Get the length of a path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The path length</returns>
        private float GetPathLenth(UnityEngine.AI.NavMeshPath path)
        {
            var length = 0f;

            for (var i = 1; i < path.corners.Length; i++)
            {
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            return length;
        }

        /// <summary>
        /// Check if we're in cover from a point
        /// </summary>
        /// <param name="from">The point to check if we're covered from</param>
        /// <returns>Whether we're in cover or not</returns>
        private bool IsInCover(Vector3 from)
        {
            RaycastHit hit;
            var direction = transform.position - from;
            var ray = new Ray(from, direction);
            var maxDistance = direction.magnitude;

            foreach (var cover in covers)
            {
                if (cover.collider.Raycast(ray, out hit, maxDistance))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get a random point on the NavMesh a set distance away
        /// </summary>
        /// <param name="distance">The distance to look at</param>
        /// <returns>A point on the NavMesh, or null if none was found</returns>
        public Vector3? GetRandomPoint(float distance)
        {
            var hit = new UnityEngine.AI.NavMeshHit();
            var point = new Vector3();
            var triesLeft = 3;
            var found = false;

            while (!found && (triesLeft > 0))
            {
                point = transform.position + (Random.onUnitSphere * distance);
                point.y = 0;
                triesLeft--;
                found = UnityEngine.AI.NavMesh.SamplePosition(point, out hit, 1, UnityEngine.AI.NavMesh.AllAreas);
            }

            if (!found)
            {
                return null;
            }

            return hit.position;
        }

        /// <summary>
        /// Get a random point on the NavMesh
        /// </summary>
        /// <returns>A random point on the NavMesh, or null if none was found</returns>
        public Vector3? GetRandomPoint()
        {

            var tryPoint = new Vector3();
            var triesLeft = 10;
            Vector3? point = null;
            var min = navMeshBounds.min;
            var max = navMeshBounds.max;

            while ((point == null) && (triesLeft > 0))
            {
                tryPoint.x = Random.Range(min.x, max.x);
                tryPoint.z = Random.Range(min.z, max.z);
                triesLeft--;
                point = GetNavigablePoint(tryPoint);
            }

            return point;
        }

        /// <summary>
        /// Get a navigable point within maxDistance of the input point, or null if there isn't one
        /// </summary>
        /// <param name="point">The position to test around</param>
        /// <param name="maxDistance">The max distance to test in</param>
        /// <returns>A point that we can get to, or null</returns>
        protected Vector3? GetNavigablePoint(Vector3 point, float maxDistance = 0.5f)
        {
            var hit = new UnityEngine.AI.NavMeshHit();
            var isValid = UnityEngine.AI.NavMesh.SamplePosition(point, out hit, maxDistance, UnityEngine.AI.NavMesh.AllAreas);

            if (!isValid)
            {
                return null;
            }

            // Make sure the point is reachable
            var path = new UnityEngine.AI.NavMeshPath();

            agent.CalculatePath(point, path);

            return path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete ? (Vector3?)hit.position : null;
        }

        protected virtual void OnCast(Spells.Spell spell)
        {
            // Do nothing
        }

        protected virtual void OnSpellStartEffect(Spells.Spell spell)
        {
            // Do nothing
        }

        protected virtual void OnSpellEndEffect(Spells.Spell spell)
        {
            // Do nothing
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

        protected void TriggerReachDestination()
        {
            if (ReachDestinationEvent != null)
            {
                ReachDestinationEvent();
            }
        }

        public virtual Spells.Spell GetSpellToCast()
        {
            //var spell = Spells.SpellFactory.GetSpell(Random.value < 0.5 ? "Q" : "Square");
            var spell = Spells.SpellFactory.GetSpell("Circle"); // Missile
            //var spell = Spells.SpellFactory.GetSpell("Square"); // Grenade
            //var spell = Spells.SpellFactory.GetSpell("Z"); // Flamethrower
            //var spell = Spells.SpellFactory.GetSpell("Shield"); // Shield
            //var spell = Spells.SpellFactory.GetSpell("Wall"); // Barrier
            //var spell = Spells.SpellFactory.GetSpell("Heal"); // Heal
            //var spell = Spells.SpellFactory.GetSpell("Drain"); // Drain
            //var spell = Spells.SpellFactory.GetSpell("Blind"); // Blind

            return spell;
        }
    }
}

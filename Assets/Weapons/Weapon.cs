using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

namespace MagicDuel
{
    /// <summary>
    /// Base class for all weapons (wands)
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// States the weapon can be in
        /// </summary>
        protected enum State { Pointer, Idle, Drawing, Charged, Throwing, Firing };

        /// <summary>
        /// Called when a spell starts being fired
        /// </summary>
        public event System.Action<Spells.Spell> CastStartEvent;

        /// <summary>
        /// Called when a spell stop being fired
        /// </summary>
        public event System.Action<Spells.Spell> CastEndEvent;

        /// <summary>
        /// The tip of the weapon, for attaching spells to
        /// </summary>
        public GameObject weaponTip;

        /// <summary>
        /// The tip of the weapon, for grabbing with VRTK
        /// </summary>
        public Rigidbody weaponTipAttachmentPoint;

        /// <summary>
        /// The GameObject that gets placed in a trail while a sigil is being drawn
        /// </summary>
        public GameObject trailObject;

        /// <summary>
        /// Whether we're on an AI character or not
        /// </summary>
        public bool onAi = false;

        /// <summary>
        /// The controller that's holding the weapon
        /// </summary>
        public GameObject controller { get; protected set; }

        /// <summary>
        /// The character that's holding the weapon
        /// </summary>
        public Character holder { get; protected set; }

        /// <summary>
        /// The current state
        /// </summary>
        protected State state = State.Idle;

        /// <summary>
        /// The spell currently being fired
        /// </summary>
        protected Spells.Spell currentSpell;

        /// <summary>
        /// The points that have been drawn so far in the sigil
        /// </summary>
        private List<Vector3> sigilPoints = new List<Vector3>();

        /// <summary>
        /// The directions the controller was pointing for each point in sigilPoints
        /// </summary>
        private List<Vector3> sigilPointDirections = new List<Vector3>();

        /// <summary>
        /// The position of the last sigil point to be drawn
        /// </summary>
        private Vector3 lastSigilPoint = Vector3.zero;

        /// <summary>
        /// The minimum spacing between sigil points
        /// </summary>
        private float sigilMovementThreshold = 0.01f;

        /// <summary>
        /// The trail GameObjects for the sigilPoints
        /// </summary>
        private List<GameObject> trail = new List<GameObject>();

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnEnable()
        {
            Assert.IsNotNull(weaponTip, "Weapon tip is null");
            Assert.IsNotNull(weaponTipAttachmentPoint, "Weapon tip attachment point is null");

            if (onAi)
            {
                holder = GetComponentInParent<Character>();
            }

            Assert.IsNotNull(holder, "Character not found in parents for " + name);
        }

        /// <summary>
        /// Callback for when the controller trigger is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnTriggerPressed(object sender)
        {
            switch (state)
            {
                case State.Idle:
                    state = State.Drawing;
                    break;

                case State.Charged:
                    FireSpell();
                    break;
            }
        }

        /// <summary>
        /// Callback for when the controller trigger is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnTriggerReleased(object sender)
        {
            if (state == State.Drawing)
            {
                RecogniseSigil();
            }
            else if (state == State.Throwing)
            {
                currentSpell.Release();

                state = State.Idle;
            }
        }

        /// <summary>
        /// When a sigil has been drawn, figure out which spell it represents and fire/make ready that spell
        /// </summary>
        protected void RecogniseSigil()
        {
            // The minimum number of points a sigil must have to count as being drawn
            var minSigilPoints = 10;

            if (sigilPoints.Count >= minSigilPoints)
            {
                // Map the points onto a plane by taking the average direction the controller was pointing in
                // while the sigil was being drawn
                var pointsToPlane = new PointsToPlane();
                var averageDirection = new Vector3();

                foreach (var pointDirection in sigilPointDirections)
                    averageDirection += pointDirection;

                averageDirection.Normalize();

                var plane = new Plane(averageDirection, 0);
                var pointsOnPlane = pointsToPlane.GetPointsOnPlane(sigilPoints.ToArray(), plane);
                var size = VectorUtils.GetBounds(pointsOnPlane).size;

                // Scale the trimming distance based on the size of the drawn sigil
                var recogniser = new Sigils.Recogniser(0.25f, Mathf.Max(size.x, size.y, size.z) / 5f);
                var featuresHash = recogniser.run(pointsOnPlane);
                var spellsLibrary = new Spells.Library();
                spellsLibrary.Load();
                var spell = spellsLibrary.GetByHash(featuresHash);

                // ToDo: put this back!!!
                // if ((spell != null) && !spell.locked)
                if (spell != null)
                {
                    state = State.Charged;
                    currentSpell = Spells.SpellFactory.GetSpell(spell.name);
                    currentSpell.Charge(this);
                    currentSpell.FireStartEvent += RaiseFireStart;
                    currentSpell.FireEndEvent += RaiseFireEnd;

                    // Replace the sigil with a nicer looking one
                    DrawSigil(sigilPoints.ToArray(), pointsOnPlane, averageDirection);
                }
                else
                {
                    state = State.Idle;
                }
            }
            else
            {
                state = State.Idle;
            }

            ClearSigilPoints();
            ClearTrail();
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        protected virtual void Update()
        {
            if (state == State.Drawing)
            {
                var position = weaponTip.transform.position;
                var deltaPosition = lastSigilPoint - position;

                if (deltaPosition.magnitude > sigilMovementThreshold)
                {
                    if (sigilPoints.Count > 0)
                    {
                        var trailSegment = Instantiate(trailObject);
                        trailSegment.transform.position = lastSigilPoint + (deltaPosition / 2f);
                        trailSegment.transform.rotation.SetLookRotation(weaponTip.transform.position - trailSegment.transform.position);
                        trailSegment.transform.Rotate(Vector3.forward, -90f, Space.Self);
                        trailSegment.transform.Rotate(Vector3.up, -90f, Space.Self);

                        trail.Add(trailSegment);
                    }

                    // ToDo: check this is correct
                    if (!onAi)
                    {
                        sigilPoints.Add(position);
                        sigilPointDirections.Add(controller.transform.forward);
                        lastSigilPoint = position;
                    }
                }
            }
        }

        /// <summary>
        /// Fire the current spell
        /// </summary>
        protected virtual void FireSpell()
        {
            currentSpell.Fire(this, holder);

            if (currentSpell.firingMethod == Spells.Spell.FiringMethod.THROW)
            {
                state = State.Throwing;
            }
            else if (currentSpell.firingMethod == Spells.Spell.FiringMethod.FLAME)
            {
                state = State.Firing;
            }
            else
            {
                state = State.Idle;
            }
        }

        /// <summary>
        /// Raise the FireStart event
        /// </summary>
        /// <param name="spell">The spell being fired</param>
        protected void RaiseFireStart(Spells.Spell spell)
        {
            if (CastStartEvent != null)
            {
                CastStartEvent(spell);
            }
        }

        /// <summary>
        /// Raise the FireEnd event
        /// </summary>
        /// <param name="spell">The spell that has finished firing</param>
        protected void RaiseFireEnd(Spells.Spell spell)
        {
            if (CastEndEvent != null)
            {
                CastEndEvent(spell);
            }
        }

        /// <summary>
        /// Remove the sigil trail
        /// </summary>
        protected void ClearTrail()
        {
            foreach (var trailObject in trail)
            {
                Destroy(trailObject);
            }

            trail.Clear();
        }

        /// <summary>
        /// Clear the recogniser history of points
        /// </summary>
        protected void ClearSigilPoints()
        {
            sigilPoints.Clear();
            sigilPointDirections.Clear();
            lastSigilPoint = Vector3.zero;
        }

        /// <summary>
        /// Replace the hand-drawn sigil with a nicer one
        /// </summary>
        /// <param name="points">The drawn sigil points</param>
        /// <param name="pointsOnPlane">The drawn sigil points mapped to a plane</param>
        /// <param name="normal">The normal to the mapped plane</param>
        protected void DrawSigil(Vector3[] points, Vector2[] pointsOnPlane, Vector3 normal)
        {
            var sigil = currentSpell.sigil;
            var center = VectorUtils.GetBounds(points).center;
            var size = VectorUtils.GetBounds(pointsOnPlane).size;
            var scale = Mathf.Max(size.x, size.y);

            var container = new GameObject("Replaced Sigil");
            var rotation = new Quaternion();
            rotation.SetLookRotation(normal);
            container.transform.position = center;
            container.transform.rotation = rotation;

            foreach (var point in sigil.GetPoints())
            {
                var sigilPoint = Instantiate(trailObject, container.transform);
                sigilPoint.transform.localRotation = Quaternion.identity;
                sigilPoint.transform.localPosition = new Vector3(point.x, point.y, 0) * scale;
            }

            StartCoroutine(DestroySigil(container, 1f));
        }

        /// <summary>
        /// Destroy the replaced sigil after a given amount of time
        /// </summary>
        /// <param name="container">The sigil container</param>
        /// <param name="timeout">The time to wait</param>
        /// <returns></returns>
        protected IEnumerator DestroySigil(GameObject container, float timeout)
        {
            yield return new WaitForSeconds(timeout);

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

        /// <summary>
        /// Change the mode to work as a pointer (for UI interactions) instead of a weapon
        /// </summary>
        public void SetAsPointer()
        {
            state = State.Pointer;
        }
    }
}

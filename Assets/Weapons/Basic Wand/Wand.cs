using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel
{
    public class Wand : Weapon
    {
        private ParticleSystem particles;

        protected override void OnTriggerPressed(object sender)
        {
            base.OnTriggerPressed(sender);

            if ((state == State.Pointer) || (state == State.Idle) || (state == State.Drawing))
            {
                var emission = particles.emission;
                emission.enabled = true;
            }
        }

        protected override void OnTriggerReleased(object sender)
        {
            base.OnTriggerReleased(sender);

            var emission = particles.emission;
            emission.enabled = false;
        }

        void Start()
        {
            particles = GetComponentInChildren<ParticleSystem>();
            Assert.IsNotNull(particles, "ParticleSystem not found");

            var emission = particles.emission;
            emission.enabled = false;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
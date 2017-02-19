using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel
{
    public class Wand : Weapon
    {
        private ParticleSystem particles;

        private void Start()
        {
            particles = GetComponentInChildren<ParticleSystem>();
            Assert.IsNotNull(particles, "ParticleSystem not found");

            var emission = particles.emission;
            emission.enabled = false;
        }
    }
}
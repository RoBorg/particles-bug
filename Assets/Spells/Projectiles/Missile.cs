using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel.Spells.Projectiles
{
    public class Missile : Projectile
    {
        protected bool exploded = false;

        protected void Update()
        {
            transform.forward = rigidbody.velocity;
        }
    }
}

using UnityEngine;
using System.Collections;

namespace MagicDuel.Spells
{
    public class Flamethrower : Spell
    {
        private Projectiles.Projectile flameObject;
        private Projectiles.Projectile flameDamageProbeObject;
        public float spreadAngle { get; protected set; }
        public float damagePerSecond { get; protected set; }
        public float duration { get; protected set; }

        public bool isCasting { get; private set; }
        private Projectiles.Projectile flame;

        public Flamethrower(StandardSpellProperties standardSpellProperties, Projectiles.Projectile flameObject,
            Projectiles.Projectile flameDamageProbeObject, float spreadAngle, float damagePerSecond,
            float duration)
            : base(standardSpellProperties)
        {
            this.flameObject = flameObject;
            this.flameDamageProbeObject = flameDamageProbeObject;
            this.spreadAngle = spreadAngle;
            this.damagePerSecond = damagePerSecond;
            this.duration = duration;

            isCasting = false;
        }

        public override void Fire(Weapon weapon, Character caster)
        {
            base.Fire(weapon, caster);

            flame = Object.Instantiate(flameObject);

            flame.transform.parent = weapon.weaponTip.transform;
            flame.transform.localPosition = Vector3.forward;
            flame.transform.localRotation = Quaternion.identity;

            isCasting = true;
            flame.StartCoroutine(EmitDamageProbes(weapon, EndFiring));
        }

        public override bool IsCasting()
        {
            return isCasting;
        }

        private IEnumerator EmitDamageProbes(Weapon weapon, System.Action callback)
        {
            var startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                var probe = Object.Instantiate(flameDamageProbeObject);
                probe.transform.position = weapon.weaponTip.transform.position;
                probe.transform.rotation = weapon.transform.rotation;

                // todo: right speed
                probe.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * 10, ForceMode.VelocityChange);

                // ToDo: right lifetime
                probe.StartLifetime(2f);

                probe.CollisionEvent += OnBurn;
                probe.EndOfLifetimeEvent += OnEndOfProbeLifetime;

                // ToDo: right delay
                yield return new WaitForSeconds(0.1f);

                // Make sure we run after LateUpdate or the IK will break stuff!
                yield return new WaitForEndOfFrame();
            }

            callback();
        }

        private void EndFiring()
        {
            Object.Destroy(flame.gameObject);
            isCasting = false;
            RaiseFireEnd();
        }

        private void OnBurn(Projectiles.Projectile projectile, Collision collision, bool hasCollidedBefore)
        {
            DoDamage(projectile.transform.position);
            Object.Destroy(projectile.gameObject);
        }

        private void OnEndOfProbeLifetime(Projectiles.Projectile projectile)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
}

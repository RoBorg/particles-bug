using UnityEngine;
using System.Collections;

namespace MagicDuel.Wizards
{
    public class CleverWizard : Wizard
    {
        protected override void Update()
        {
            base.Update();
            movementTarget.position = Vector3.zero;
            /*
            if (Random.value < .003)
            {
                var newPosition = GetRandomPoint();

                if (newPosition != null)
                {
                    target = (Vector3)newPosition;
                    DebugUtil.Point((Vector3)newPosition);
                }
            }
            ?*/
        }

        protected override void OnCast(Spells.Spell spell)
        {
            
        }

        protected override void OnSpellStartEffect(Spells.Spell spell)
        {
            // Do nothing
        }

        protected override void OnSpellEndEffect(Spells.Spell spell)
        {
            // TODO Debug.Log("Stop being afraid of " + projectile.name);
        }

        private IEnumerator Dodge(Spells.Projectiles.Projectile avoid)
        {
            yield return new WaitForSeconds(1.5f);

            if (avoid != null)
            {
                TakeCoverFrom(avoid.transform);
            }
        }
    }
}

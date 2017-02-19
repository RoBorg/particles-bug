using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel.Spells.Manifestations
{
    public class Barrier : Manifestation
    {
        public event System.Action DieEvent;

        protected override void Start()
        {
            base.Start();

            Assert.IsNotNull(collider, "Collider not found on " + name);
        }

        protected void RaiseDie()
        {
            if (DieEvent != null)
            {
                DieEvent();
            }
        }

        public virtual void Collapse()
        {
            RaiseDie();
        }
    }
}

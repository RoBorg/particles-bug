using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel.Wizards.WizardStateMachine.States
{
    public class WizardStateMove : WizardState
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            wizard.ReachDestinationEvent += OnReachDestination;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            wizard.ReachDestinationEvent -= OnReachDestination;
        }

        private void OnReachDestination()
        {
            stateMachine.animator.SetTrigger("Idle");
        }
    }
}

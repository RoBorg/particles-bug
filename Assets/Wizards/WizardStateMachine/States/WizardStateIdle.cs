using UnityEngine;

namespace MagicDuel.Wizards.WizardStateMachine.States
{
    public class WizardStateIdle : WizardState
    {
        protected virtual void Update()
        {
            stateMachine.animator.SetTrigger("Cast");
        }
    }
}

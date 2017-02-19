using UnityEngine;

namespace MagicDuel.Wizards.WizardStateMachine.States
{
    public class WizardStateIdle : WizardState
    {
        protected virtual void Update()
        {
            if (Random.value < 0.003f)
            {
                stateMachine.animator.SetTrigger("Cast");
            }
            else if (Random.value < 0.001f)
            {
                var point = wizard.GetRandomPoint();

                if (point != null)
                {
                    wizard.SetTarget((Vector3)point);
                    stateMachine.animator.SetTrigger("Move");
                }
            }
        }
    }
}

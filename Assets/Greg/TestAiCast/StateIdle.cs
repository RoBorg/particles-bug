using UnityEngine;

namespace MagicDuel.TestAi
{
    public class StateIdle : Wizards.WizardStateMachine.States.WizardStateIdle
    {
        private static int counter = 0;

        protected override void Update()
        {
            counter++;

            if (counter % 200 == 0)
            {
                stateMachine.animator.SetTrigger("Cast");
            }
        }
    }
}

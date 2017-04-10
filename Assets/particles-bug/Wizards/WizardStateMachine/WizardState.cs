using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel.Wizards.WizardStateMachine
{
    public class WizardState : MonoBehaviour
    {
        protected Wizard wizard;
        protected WizardStateMachine stateMachine;

        protected virtual void OnEnable()
        {
            wizard = GetComponent<Wizard>();
            Assert.IsNotNull(wizard, "Wizard not found");

            stateMachine = wizard.stateMachine;
            Assert.IsNotNull(stateMachine, "Wizard State Machine not found");
        }

        protected virtual void OnDisable()
        {
        }
    }
}

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace MagicDuel.Wizards.WizardStateMachine
{
    public class WizardStateMachine : MonoBehaviour
    {
        public Animator animator { get; protected set; }

        private Wizard wizard;
        private Dictionary<string, MonoBehaviour> components = new Dictionary<string, MonoBehaviour>();
        private MonoBehaviour currentComponent;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator not set");

            wizard = GetComponentInParent<Wizard>();
            Assert.IsNotNull(wizard, "Wizard not found in parent");

            foreach (var component in wizard.GetComponents<WizardState>())
            {
                Assert.IsFalse(component.enabled, "WizardState components must not be enabled: Found " + component.GetType() + " on " + wizard.name);
            }
        }

        private void Start()
        {
            GetStateComponent("Idle").enabled = true;
        }

        public void OnAnimatorStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, string state)
        {
            if (currentComponent != null)
            {
                currentComponent.enabled = false;
            }

            currentComponent = GetStateComponent(state);
            Assert.IsNotNull(currentComponent, "No component for state '" + state + "' on " + name);

            currentComponent.enabled = true;
        }

        public void OnAnimatorStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, string state)
        {
            // Do nothing
        }

        private MonoBehaviour GetStateComponent(string name)
        {
            if (!components.ContainsKey(name))
            {
                components[name] = (MonoBehaviour)wizard.GetComponent("WizardState" + name);
            }

            return components[name];
        }
    }
}

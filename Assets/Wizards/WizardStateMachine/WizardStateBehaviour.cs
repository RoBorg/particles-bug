using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel.Wizards.WizardStateMachine
{
    public class WizardStateBehaviour : StateMachineBehaviour
    {
        public string state = "";

        private WizardStateMachine wizardStateMachine;

        /// <summary>
        /// Called when a transition starts and the state machine starts to evaluate this state
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Assert.AreNotEqual("", state, "State name not set");
            GetStateMachine(animator).OnAnimatorStateEnter(animator, stateInfo, layerIndex, state);
        }

        /// <summary>
        /// Called when a transition ends and the state machine finishes evaluating this state
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Assert.AreNotEqual("", state, "State name not set");
            GetStateMachine(animator).OnAnimatorStateExit(animator, stateInfo, layerIndex, state);
        }

        private WizardStateMachine GetStateMachine(Animator animator)
        {
            if (wizardStateMachine != null)
            {
                return wizardStateMachine;
            }

            wizardStateMachine = animator.gameObject.GetComponent<WizardStateMachine>();
            Assert.IsNotNull(wizardStateMachine, "Wizard State Machine not found");

            return wizardStateMachine;
        }
    }
}

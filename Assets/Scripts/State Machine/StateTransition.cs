using UnityEngine;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    public class StateTransition 
    {
        [SerializeField] StateCondition condition;
        [SerializeField] State trueState;

        public State GetTrueState()
        {
            return trueState;
        }

        public void SetTrueState(State trueState)
        {
            this.trueState = trueState;
        }
        
        public bool Check()
        {
            return condition.Check();
        }

        public void Bind(StateController controller, State caller)
        {
            condition.Bind(controller, caller);
        }

        public StateTransition Clone()
        {
            StateTransition clone = new StateTransition();
            clone.condition = condition.Clone();
            clone.trueState = trueState;
            return clone;
        }
    }
}

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
        
        public bool Check(StateController controller, State caller)
        {
            return condition.Check(controller, caller);
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

using UnityEngine;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    public class StateTransition
    {
        [SerializeField] Condition condition;
        [SerializeField] State trueState;

        public StateTransition Clone()
        {
            StateTransition clone = new StateTransition();
            clone.condition = condition.Clone();
            clone.trueState = trueState;
            return clone;
        }
        
        public bool Check(StateController controller, State state)
        {
            return condition.Check(controller, state);
        }

        public State GetTrueState()
        {
            return trueState;
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    class TriggerTransition
    {
        [SerializeField] StateTrigger stateTrigger;
        [SerializeField] State trueState;

        public TriggerTransition Clone()
        {
            TriggerTransition clone = new TriggerTransition();
            clone.stateTrigger = stateTrigger.Clone();
            clone.trueState = trueState;
            return clone;
        }

        public UnityEvent GetEventTrigger(StateController controller)
        {
            return stateTrigger.EventTrigger(controller);
        }

        public State GetTrueState()
        {
            return trueState;
        }
    }
}

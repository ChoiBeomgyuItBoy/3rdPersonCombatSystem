using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    public class TriggerTransition
    {
        [SerializeField] StateTrigger trigger;
        [SerializeField] State trueState;

        public UnityEventBase GetEventTrigger()
        {
            return trigger.EventTrigger();
        }

        public State GetTrueState()
        {
            return trueState;
        }

        public void Bind(StateController controller)
        {
            trigger.Bind(controller);
        }

        public TriggerTransition Clone()
        {
            TriggerTransition clone = new TriggerTransition();
            clone.trigger = trigger.Clone();
            clone.trueState = trueState;
            return clone;
        }
    }
}

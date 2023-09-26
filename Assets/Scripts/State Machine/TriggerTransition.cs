using System;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    class TriggerTransition
    {
        [SerializeField] StateTrigger trigger;
        [SerializeField] State trueState;

        public TriggerTransition Clone()
        {
            TriggerTransition clone = new TriggerTransition();
            clone.trigger = trigger.Clone();
            clone.trueState = trueState;
            return clone;
        }

        public UnityEventBase GetEventTrigger(StateController controller)
        {
            return trigger.EventTrigger(controller);
        }

        public State GetTrueState()
        {
            return trueState;
        }
    }
}

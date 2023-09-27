using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine
{
    public abstract class StateTrigger : ScriptableObject
    {
        protected StateController controller;

        public UnityEventBase EventTrigger()
        {
            return GetEventTrigger();
        }

        public void Bind(StateController controller)
        {
            this.controller = controller;
        }

        public StateTrigger Clone()
        {
            return Instantiate(this);
        }

        protected abstract UnityEventBase GetEventTrigger();
    }   
}

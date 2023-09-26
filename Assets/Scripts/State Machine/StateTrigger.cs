using System;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine
{
    public abstract class StateTrigger : ScriptableObject
    {
        protected StateController controller;

        public StateTrigger Clone()
        {
            return Instantiate(this);
        }

        public UnityEventBase EventTrigger(StateController controller)
        {
            if(this.controller == null)
            {
                this.controller = controller;
            }
            
            return GetEventTrigger();
        }

        protected abstract UnityEventBase GetEventTrigger();
    }   
}

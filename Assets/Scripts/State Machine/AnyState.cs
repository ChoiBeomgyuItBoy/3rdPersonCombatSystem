using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/New Any State")]
    public class AnyState : ScriptableObject
    {
        [SerializeField] List<TriggerTransition> transitions;

        public AnyState Clone()
        {
            AnyState clone = Instantiate(this);
            clone.transitions = transitions.ConvertAll((transition) => transition.Clone());
            return clone;
        }

        public List<TriggerTransition> GetTransitions()
        {
            return transitions;
        }

        public void Subscribe(StateController controller)
        {
            transitions.ForEach((transition) => 
            {
                State trueState = transition.GetTrueState();
                UnityEventBase triggerEvent = transition.GetEventTrigger(controller);
                
                if(triggerEvent is UnityEvent)
                {
                    UnityEvent trigger = triggerEvent as UnityEvent;
                    trigger.AddListener(() => controller.SwitchState(trueState));
                }

                if(triggerEvent is UnityEvent<float>)
                {
                    UnityEvent<float> trigger = triggerEvent as UnityEvent<float>;
                    trigger.AddListener((value) => controller.SwitchState(trueState));
                }
            });
        }
    }
}
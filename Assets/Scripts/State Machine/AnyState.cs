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

        public void Subscribe(StateController controller)
        {
            transitions.ForEach((transition) => 
            {
                State trueState = transition.GetTrueState();
                UnityEvent triggerEvent = transition.GetEventTrigger(controller);
                triggerEvent.AddListener(() => controller.SwitchState(trueState));
            });
        }
    }
}
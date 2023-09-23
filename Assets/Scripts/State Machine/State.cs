using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/New State")]
    public class State : ScriptableObject
    {
        [SerializeField] List<StateAction> actions = new List<StateAction>();
        [SerializeField] List<StateTransition> transitions = new List<StateTransition>();
        public event Action onExit;

        public State Clone()
        {
            State clone = Instantiate(this);

            clone.actions = actions.ConvertAll((action) => action.Clone());
            clone.transitions = transitions.ConvertAll((transition) => transition.Clone());

            return clone;
        }

        public void Tick(StateController controller)
        {
            DoActions(controller);
            CheckConditions(controller);
        }

        private void DoActions(StateController controller)
        {
            actions.ForEach((action) =>
            {
                action.Tick(controller, this);
            });
        } 

        private void CheckConditions(StateController controller)
        {
            transitions.ForEach((transition) =>
            {
                if(transition.Check(controller, this))
                {
                    onExit?.Invoke();
                    controller.SwitchState(transition.GetTrueState());
                    return;
                }
            });
        }
    }
}
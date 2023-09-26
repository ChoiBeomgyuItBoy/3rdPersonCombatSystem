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
        public event Action<State> onExit;

        public List<StateTransition> GetTransitions()
        {
            return transitions;
        }

        public void Tick(StateController controller)
        {
            DoActions(controller);
            CheckConditions(controller);
        }

        public void Exit()
        {
            onExit?.Invoke(this);
        }

        public State Clone()
        {
            State clone = Instantiate(this);
            clone.actions = actions.ConvertAll((action) => action.Clone());
            clone.transitions = transitions.ConvertAll((transition) => transition.Clone());
            return clone;
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
                    controller.SwitchState(transition.GetTrueState());
                    return;
                }
            });
        }
    }
}
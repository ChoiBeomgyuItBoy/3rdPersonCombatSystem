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
        StateController controller;

        public List<StateTransition> GetTransitions()
        {
            return transitions;
        }

        public void Tick()
        {
            DoActions();
            CheckConditions();
        }

        public void Exit()
        {
            onExit?.Invoke(this);
        }

        public void Bind(StateController controller)
        {
            this.controller = controller;
            actions.ForEach((action) => action.Bind(controller, this));
            transitions.ForEach((transition) => transition.Bind(controller, this));
        }

        public State Clone()
        {
            State clone = Instantiate(this);
            clone.actions = actions.ConvertAll((action) => action.Clone());
            clone.transitions = transitions.ConvertAll((transition) => transition.Clone());
            return clone;
        }

        private void DoActions()
        {
            actions.ForEach((action) =>
            {
                action.Tick();
            });
        } 

        private void CheckConditions()
        {
            transitions.ForEach((transition) =>
            {
                if(transition.Check())
                {
                    controller.SwitchState(transition.GetTrueState());
                    return;
                }
            });
        }
    }
}
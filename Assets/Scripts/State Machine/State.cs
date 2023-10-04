using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/New State")]
    public class State : ScriptableObject
    {
        public string stateName = "New State";
        [SerializeField] Vector2 position = Vector2.zero;
        [SerializeField] List<StateAction> actions = new List<StateAction>();
        [SerializeField] List<StateTransition> transitions = new List<StateTransition>();
        StateController controller;
        public event Action onExit;

        public Vector2 GetPosition()
        {
            return position;
        }

        public List<StateAction> GetActions()
        {
            return actions;
        }

        public List<StateTransition> GetTransitions()
        {
            return transitions;
        }

        public StateTransition GetTransition(State trueState)
        {
            return transitions.SingleOrDefault(transition => transition.GetTrueState() == trueState);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Tick()
        {
            DoActions();
            CheckConditions();
        }

        public void Exit()
        {
            onExit?.Invoke();
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
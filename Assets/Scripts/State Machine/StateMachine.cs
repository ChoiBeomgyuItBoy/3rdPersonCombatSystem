using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace CombatSystem.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/New State Machine")]
    public class StateMachine : ScriptableObject
    {
        [SerializeField] State initialState;
        [SerializeField] State currentState;
        [SerializeField] AnyState anyState;
        [SerializeField] List<State> states = new List<State>();
        Dictionary<State, State> cloneLookup = new Dictionary<State, State>();

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = cloneLookup[newState];
        }

        public void Enter(StateController controller)
        {
            if(anyState != null)
            {
                anyState.Subscribe(controller);
            }

            SwitchState(initialState);
        }

        public void Tick(StateController controller)
        {
            currentState?.Tick(controller);
        }

        public StateMachine Clone()
        {
            StateMachine clone = Instantiate(this);

            if(anyState != null)
            {
                clone.anyState = anyState.Clone();
            }
 
            clone.states = new List<State>();

            Traverse((state) => 
            {
                clone.cloneLookup[state] = state.Clone();
                clone.states.Add(clone.cloneLookup[state]);
            });

            return clone;
        }  

        private void Traverse(Action<State> visiter)
        {
            Queue<State> frontier = new Queue<State>();
            Dictionary<State, bool> explored = new Dictionary<State, bool>();
            State current = null;

            frontier.Enqueue(initialState);

            while(frontier.Count > 0)
            {
                current = frontier.Dequeue();

                ExploreTransitions(frontier, explored, current);
                SetAsExplored(visiter, explored, current);
            }

            if(anyState != null)
            {
                anyState.GetTransitions().ForEach((transition) =>
                {
                    current = transition.GetTrueState();
                    SetAsExplored(visiter, explored, current);
                });
            }
        }

        private void ExploreTransitions(Queue<State> frontier, Dictionary<State, bool> explored, State current)
        {
            current.GetTransitions().ForEach((transition) =>
            {
                State neighbour = transition.GetTrueState();

                if(!explored.ContainsKey(neighbour))
                {
                    frontier.Enqueue(neighbour);
                }
            });
        }

        private void SetAsExplored(Action<State> visiter, Dictionary<State, bool> explored, State current)
        {
            if(!explored.ContainsKey(current))
            {
                explored[current] = true;
                visiter.Invoke(current);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

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
        Dictionary<State, bool> visited = new Dictionary<State, bool>();
        Queue<State> explored = new Queue<State>();

        public void Enter()
        {
            if (anyState != null)
            {
                anyState.Subscribe();
            }

            SwitchState(initialState);
        }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = cloneLookup[newState];
        }

        public void Tick()
        {
            currentState?.Tick();
        }

        public void Bind(StateController controller)
        {
            Traverse((state) => 
            {
                cloneLookup[state].Bind(controller);
            });

            anyState.Bind(controller);
        }

        public StateMachine Clone()
        {
            StateMachine clone = Instantiate(this);

            if (anyState != null)
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
            visited.Clear();
            explored.Clear();

            explored.Enqueue(initialState);

            while (explored.Count > 0)
            {
                Visit(visiter);
            }

            if (anyState != null)
            {
                VisitAnyState(visiter);
            }
        }

        private void Visit(Action<State> visiter)
        {
            State current = explored.Dequeue();

            current.GetTransitions().ForEach((transition) =>
            {
                State neighbour = transition.GetTrueState();

                if (!visited.ContainsKey(neighbour))
                {
                    explored.Enqueue(neighbour);
                }
            });

            SetAsVisited(visiter, current);
        }

        private void SetAsVisited(Action<State> visiter, State current)
        {
            if (!visited.ContainsKey(current))
            {
                visited[current] = true;
                visiter.Invoke(current);
            }
        }

        private void VisitAnyState(Action<State> visiter)
        {
            anyState.GetTransitions().ForEach((transition) =>
            {
                State current = transition.GetTrueState();
                SetAsVisited(visiter, current);
            });
        }
    }
}

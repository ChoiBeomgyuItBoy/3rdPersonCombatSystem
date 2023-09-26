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
        Queue<State> explored = new Queue<State>();
        Dictionary<State, bool> visited = new Dictionary<State, bool>();

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = cloneLookup[newState];
        }

        public void Enter(StateController controller)
        {
            if (anyState != null)
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

            if (anyState != null)
            {
                clone.anyState = anyState.Clone();
            }
 
            clone.states = new List<State>();

            Traverse(clone, (state) => 
            {
                clone.cloneLookup[state] = state.Clone();
                clone.states.Add(clone.cloneLookup[state]);
            });

            return clone;
        }

        private void Traverse(StateMachine root, Action<State> visiter)
        {
            State current = null;

            root.explored.Enqueue(initialState);

            while (root.explored.Count > 0)
            {
                current = root.explored.Dequeue();
                Visit(root, visiter, current);
            }

            if (anyState != null)
            {
                VisitAnyState(root, visiter, current);
            }
        }

        private void Visit(StateMachine root, Action<State> visiter, State current)
        {
            current.GetTransitions().ForEach((transition) =>
            {
                State neighbour = transition.GetTrueState();

                if (!root.visited.ContainsKey(neighbour))
                {
                    root.explored.Enqueue(neighbour);
                }
            });

            SetAsVisited(root, visiter, current);
        }

        private void SetAsVisited(StateMachine root, Action<State> visiter, State current)
        {
            if (!root.visited.ContainsKey(current))
            {
                root.visited[current] = true;
                visiter.Invoke(current);
            }
        }

        private void VisitAnyState(StateMachine root, Action<State> visiter, State current)
        {
            anyState.GetTransitions().ForEach((transition) =>
            {
                current = transition.GetTrueState();
                SetAsVisited(root, visiter, current);
            });
        }
    }
}

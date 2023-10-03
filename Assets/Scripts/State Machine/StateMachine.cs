using System;
using System.Collections.Generic;
using UnityEditor;
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

        public List<State> GetStates()
        {   
            return states;
        }

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
            Traverse(state => 
            {
                cloneLookup[state].Bind(controller);
            });

            anyState.Bind(controller);
        }


#if UNITY_EDITOR
        public State CreateState()
        {
            State state = CreateInstance<State>();
            state.name = Guid.NewGuid().ToString();
            states.Add(state);
            AssetDatabase.AddObjectToAsset(state, this);
            AssetDatabase.SaveAssets();
            return state;
        }

        public StateTransition CreateTransition(State startState, State endState)
        {
            StateTransition transition = new StateTransition();
            transition.SetTrueState(endState);
            startState.GetTransitions().Add(transition);
            AssetDatabase.SaveAssets();
            return transition;
        }

        public void RemoveState(State state)
        {
            states.Remove(state);
            AssetDatabase.RemoveObjectFromAsset(state);
            AssetDatabase.SaveAssets();
        }

        public void RemoveTransition(State startState, State endState)
        {
            StateTransition transition = startState.GetTransition(endState);
            startState.GetTransitions().Remove(transition);
            AssetDatabase.SaveAssets();
        }

        public void RemoveTransitionsWithState(State endState)
        {
            states.ForEach(state =>
            {
                RemoveTransition(state, endState);
            });

            AssetDatabase.SaveAssets();
        }
#endif

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

            current.GetTransitions().ForEach(transition =>
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
            anyState.GetTransitions().ForEach(transition =>
            {
                State current = transition.GetTrueState();
                SetAsVisited(visiter, current);
            });
        }
    }
}

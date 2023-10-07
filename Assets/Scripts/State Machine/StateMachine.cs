using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/New State Machine")]
    public class StateMachine : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] State initialState;
        [SerializeField] State currentState;
        [SerializeField] AnyState anyState;
        [SerializeField] List<State> states = new List<State>();
        Dictionary<string, State> stateLookup = new Dictionary<string, State>();

        public List<State> GetStates()
        {   
            List<State> allStates = new List<State>();

            states.ForEach(state =>
            {
                if(stateLookup.ContainsKey(state.name))
                {
                    allStates.Add(state);
                }
            });

            return allStates;
        }

        public void Enter()
        {
            if(anyState != null)
            {
                anyState.Subscribe();
            }

            SwitchState(initialState.name);
        }

        public void SwitchState(string newStateName)
        {
            currentState?.Exit();
            currentState = stateLookup[newStateName];
        }

        public void Tick()
        {
            currentState?.Tick();
        }

        public void Bind(StateController controller)
        {
            states.ForEach(state => 
            {
                state.Bind(controller);
            });

            anyState.Bind(controller);
        }


#if UNITY_EDITOR
        public State CreateState()
        {
            State newState = CreateInstance<State>();
            newState.name = Guid.NewGuid().ToString();

            Undo.RegisterCreatedObjectUndo(newState, "(State Machine) State Created");
            Undo.RecordObject(this, "(State Machine) State Added");

            states.Add(newState);

            return newState;
        }

        public void RemoveState(State stateToDelete)
        {
            Undo.RecordObject(this, "(State Machine) State Removed");
            states.Remove(stateToDelete);

            CleanDanglingTransitions(stateToDelete);

            Undo.DestroyObjectImmediate(stateToDelete);
        }

        private void CleanDanglingTransitions(State stateToDelete)
        {
            states.ForEach(state =>
            {
                state.RemoveTransition(stateToDelete);
            });
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if(AssetDatabase.GetAssetPath(this) != "")
            {
                states.ForEach(state =>
                {
                    if(AssetDatabase.GetAssetPath(state) == "")
                    {
                        AssetDatabase.AddObjectToAsset(state, this);
                    }
                });
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
#endif

        public StateMachine Clone()
        {
            StateMachine clone = Instantiate(this);

            if(anyState != null)
            {
                clone.anyState = anyState.Clone();
            }
 
            clone.states = new List<State>();
            clone.stateLookup = new Dictionary<string, State>();

            states.ForEach((state) => 
            {
                clone.stateLookup[state.name] = state.Clone();
                clone.states.Add(clone.stateLookup[state.name]);
            });

            return clone;
        }

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            stateLookup.Clear();

            states.ForEach(state =>
            {
                if(state != null)
                {
                    stateLookup[state.name] = state;
                }
            });
        }
    }
}

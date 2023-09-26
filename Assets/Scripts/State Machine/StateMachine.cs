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

            states.ForEach((state) => 
            {
                clone.cloneLookup[state] = state.Clone();
                clone.states.Add(clone.cloneLookup[state]);
            });

            return clone;
        }  
    }
}

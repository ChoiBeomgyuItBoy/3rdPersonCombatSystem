using UnityEngine;

namespace CombatSystem.StateMachine
{
    public abstract class StateAction : ScriptableObject
    {
        protected StateController controller;
        bool started = false;

        public StateAction Clone()
        {
            return Instantiate(this);
        }

        public void Tick(StateController controller, State state)
        {
            if(this.controller == null)
            {
                this.controller = controller;
            }

            if(!started)
            {
                OnEnter();
                started = true;
                state.onExit += OnExit;
            }

            OnTick();
        }

        protected abstract void OnEnter();
        protected abstract void OnTick();
        protected abstract void OnExit();
    }
}
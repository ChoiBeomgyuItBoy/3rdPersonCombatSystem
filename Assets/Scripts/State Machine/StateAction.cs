using UnityEngine;

namespace CombatSystem.StateMachine
{
    public abstract class StateAction : ScriptableObject
    {
        protected StateController controller;
        protected State caller;
        bool started = false;

        public void Tick()
        {
            if(!started)
            {
                Enter(caller);
            }

            OnTick();
        }

        public virtual void Bind(StateController controller, State caller)
        {
            this.controller = controller;
            this.caller = caller;
        }

        public virtual StateAction Clone()
        {
            return Instantiate(this);
        }

        private void Enter(State caller)
        {
            OnEnter();
            started = true;
            caller.onExit += Exit;
        }

        private void Exit(State caller)
        {
            OnExit();
            started = false;
            caller.onExit -= Exit;
        }

        protected abstract void OnEnter();
        protected abstract void OnTick();
        protected abstract void OnExit();
    }
}
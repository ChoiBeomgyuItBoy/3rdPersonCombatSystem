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

        public void Tick(StateController controller, State caller)
        {
            if(this.controller == null)
            {
                this.controller = controller;
            }

            if(!started)
            {
                Enter(caller);
            }

            OnTick();
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
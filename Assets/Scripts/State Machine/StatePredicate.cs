using UnityEngine;

namespace CombatSystem.StateMachine
{
    public abstract class StatePredicate : ScriptableObject
    {
        protected StateController controller;
        bool started = false;

        public StatePredicate Clone()
        {
            return Instantiate(this);
        }

        public bool Check(StateController controller, State caller)
        {
            if(this.controller == null)
            {
                this.controller = controller;
            }

            if(!started)
            {
                Enter(caller);
            }

            return OnCheck();
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
        protected abstract bool OnCheck();
        protected abstract void OnExit();
    }
}
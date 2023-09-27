using UnityEngine;

namespace CombatSystem.StateMachine
{
    public abstract class StatePredicate : ScriptableObject
    {
        protected StateController controller;
        protected State caller;
        bool started = false;

        public bool Check()
        {
            if(!started)
            {
                Enter();
            }

            return OnCheck();
        }

        public void Bind(StateController controller, State caller)
        {
            this.controller = controller;
            this.caller = caller;
        }

        public StatePredicate Clone()
        {
            return Instantiate(this);
        }

        private void Enter()
        {
            OnEnter();
            started = true;
            caller.onExit += Exit;
        }

        private void Exit()
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
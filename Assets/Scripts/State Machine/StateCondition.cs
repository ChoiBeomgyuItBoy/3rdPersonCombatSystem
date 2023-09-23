using UnityEngine;

namespace CombatSystem.StateMachine
{
    public abstract class StateCondition : ScriptableObject
    {
        protected StateController controller;
        bool started = false;

        public StateCondition Clone()
        {
            return Instantiate(this);
        }

        public bool Check(StateController controller, State state)
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

            return OnCheck();
        }

        protected abstract void OnEnter();
        protected abstract bool OnCheck();
        protected abstract void OnExit();
    }
}
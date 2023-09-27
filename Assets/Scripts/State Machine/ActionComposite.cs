using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    public abstract class ActionComposite : StateAction
    {
        [SerializeField] List<StateAction> possibleActions = new List<StateAction>();

        protected List<StateAction> GetPossibleActions()
        {
            return possibleActions;
        }

        public override void Bind(StateController controller, State caller)
        {
            this.controller = controller;
            this.caller = caller;
            possibleActions.ForEach((action) => action.Bind(controller, caller));
        }

        public override StateAction Clone()
        {
            ActionComposite clone = Instantiate(this);
            clone.possibleActions = possibleActions.ConvertAll((action) => action.Clone());
            return clone;
        }
    }
}

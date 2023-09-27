using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Random Composite")]
    public class RandomComposite : ActionComposite
    {
        StateAction randomAction;

        protected override void OnEnter()
        {
            var actions = GetPossibleActions();
            randomAction = actions[Random.Range(0, actions.Count)];
        }

        protected override void OnTick()
        {
            randomAction.Tick();
        }

        protected override void OnExit() { }
    }
}
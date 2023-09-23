using CombatSystem.Combat;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Cancel Target")]
    public class CancelTarget : StateAction
    {
        protected override void OnEnter()
        {
            controller.GetComponentInChildren<Targeter>().Cancel();
        }

        protected override void OnTick() { }

        protected override void OnExit() { }
    }
}

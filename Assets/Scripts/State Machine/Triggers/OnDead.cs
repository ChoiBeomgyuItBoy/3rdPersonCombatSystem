using CombatSystem.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine.Triggers
{
    [CreateAssetMenu(menuName = "State Machine/Triggers/On Dead")]
    public class OnDead : StateTrigger
    {
        protected override UnityEvent GetEventTrigger()
        {
            return controller.GetComponent<Health>().onDie;
        }
    }
}

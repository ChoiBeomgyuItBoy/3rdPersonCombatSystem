using CombatSystem.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine.Triggers
{
    [CreateAssetMenu(menuName = "State Machine/Triggers/On Dead")]
    public class OnDead : StateTrigger
    {
        protected override UnityEventBase GetEventTrigger()
        {
            return controller.GetComponent<Health>().onDie;
        }
    }
}

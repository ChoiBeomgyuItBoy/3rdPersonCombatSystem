using CombatSystem.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.StateMachine.Triggers
{
    [CreateAssetMenu(menuName = "State Machine/Triggers/On Damage Taken")]
    public class OnDamageTaken : StateTrigger
    {
        protected override UnityEventBase GetEventTrigger()
        {
            return controller.GetComponent<Health>().onDamageTaken;
        }
    }
}

using CombatSystem.Combat;
using CombatSystem.Movement;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Face Target")]
    public class FaceTarget : StateAction
    {
        Targeter targeter;
        Mover mover;

        protected override void OnEnter()
        {
            targeter = controller.GetComponentInChildren<Targeter>();
            mover = controller.GetComponent<Mover>();
        }

        protected override void OnTick()
        {
            CombatTarget currentTarget = targeter.GetCurrentTarget();

            if(currentTarget == null) 
            {
                return;
            }

            mover.LookAt(currentTarget.transform.position);
        }

        protected override void OnExit() { }
    }
}

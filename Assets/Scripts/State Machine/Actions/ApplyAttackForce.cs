using CombatSystem.Combat;
using CombatSystem.Movement;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Apply Attack Force")]
    public class ApplyAttackForce : StateAction
    {
        Fighter fighter;
        ComboAttack currentAttack;
        ForceReceiver forceReceiver;
        bool alreadyAppliedForce = false;
        
        protected override void OnEnter()
        {
            fighter = controller.GetComponent<Fighter>();
            currentAttack = fighter.GetCurrentAttack();
            forceReceiver = controller.GetComponent<ForceReceiver>();

            alreadyAppliedForce = false;
        }

        protected override void OnTick()
        {
            float applyForceTime = currentAttack.GetApplyForceTime();

            if(fighter.GetAttackNormalizedTime() >= applyForceTime)
            {
                TryApplyForce();
            }
        }

        protected override void OnExit() { }

        private void TryApplyForce()
        {
            if(alreadyAppliedForce)
            {
                return;
            }

            forceReceiver.AddForce(controller.transform.forward * currentAttack.GetAttackForce());
            alreadyAppliedForce = true;
        }
    }
}

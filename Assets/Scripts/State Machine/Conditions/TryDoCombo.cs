using CombatSystem.Combat;
using CombatSystem.Movement;
using UnityEngine;

namespace CombatSystem.StateMachine.Conditions
{
    [CreateAssetMenu(menuName = "State Machine/Conditions/Try Do Combo")]
    public class TryDoCombo : StateCondition
    {
        [SerializeField] [Range(0,1)] float normalizedTimeReset = 0.5f;
        Fighter fighter;
        Animator animator;
        ComboAttack currentAttack;
        ForceReceiver forceReceiver;
        bool alreadyAppliedForce = false;

        protected override void OnEnter()
        {
            fighter = controller.GetComponent<Fighter>();
            animator = controller.GetComponent<Animator>();
            currentAttack = fighter.GetCurrentAttack();
            forceReceiver = controller.GetComponent<ForceReceiver>();
        }

        protected override bool OnCheck()
        {
            return CanContinue();
        }

        protected override void OnExit() { }   

        private bool CanContinue()
        {
            forceReceiver.MoveWithForces(Vector3.zero);

            float applyForceTime = currentAttack.GetApplyForceTime();

            if(GetNormalizedTime() >= applyForceTime)
            {
                TryApplyForce();
            }

            float successTime = currentAttack.GetTimeToContinue();

            if(GetNormalizedTime() < successTime)
            {
                return false;
            }

            if(fighter.CurrentAttackIsLast())
            {
                return false;
            }

            ResetNormalizedTime();
            return true;
        }

        private void TryApplyForce()
        {
            if(alreadyAppliedForce)
            {
                return;
            }

            forceReceiver.AddForce(controller.transform.forward * currentAttack.GetAttackForce());
            alreadyAppliedForce = true;
        }

        private void ResetNormalizedTime()
        {
            animator.Play(0, 0, normalizedTimeReset);
        }

        private float GetNormalizedTime()
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
            {
                return currentInfo.normalizedTime;
            }

            return 0;
        }
    }
}

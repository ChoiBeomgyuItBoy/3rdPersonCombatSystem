using CombatSystem.Combat;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Attack")]
    public class Attack : StateAction
    {
        protected override void OnEnter()
        {
            Animator animator = controller.GetComponent<Animator>();
            ComboAttack currentAttack = controller.GetComponent<Fighter>().GetCurrentAttack();
            animator.CrossFade(currentAttack.GetAnimationName(), 0.1f);
        }

        protected override void OnTick() { }

        protected override void OnExit() { }
    }
}
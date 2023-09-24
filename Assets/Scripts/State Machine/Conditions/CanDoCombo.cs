using CombatSystem.Combat;
using UnityEngine;

namespace CombatSystem.StateMachine.Conditions
{
    [CreateAssetMenu(menuName = "State Machine/Conditions/Can Do Combo")]
    public class CanDoCombo : StateCondition
    {
        [SerializeField] [Range(0,1)] float normalizedTimeReset = 0.5f;
        Fighter fighter;
        ComboAttack currentAttack;

        protected override void OnEnter()
        {
            fighter = controller.GetComponent<Fighter>();
            currentAttack = fighter.GetCurrentAttack();
        }

        protected override bool OnCheck()
        {
            float successTime = currentAttack.GetTimeToContinue();

            if(fighter.GetAttackNormalizedTime() < successTime)
            {
                return false;
            }

            if(fighter.CurrentAttackIsLast())
            {
                return false;
            }

            fighter.ResetNormalizedTime(normalizedTimeReset);

            return true;
        }

        protected override void OnExit() { }   
    }
}

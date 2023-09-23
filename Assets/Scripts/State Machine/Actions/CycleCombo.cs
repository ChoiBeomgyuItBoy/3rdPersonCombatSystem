using CombatSystem.Combat;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Cycle Combo")]
    public class CycleCombo : StateAction
    {
        [SerializeField] bool resetCombo = false;
        Fighter fighter;

        protected override void OnEnter()
        {
            fighter = controller.GetComponent<Fighter>();

            if(!resetCombo) 
            {
                return;
            }

            fighter.ResetCombo();
        }

        protected override void OnTick() { }

        protected override void OnExit()
        {
            if(resetCombo) 
            {
                return;
            }
            
            fighter.CycleCurrentAttack();
        }
    }
}

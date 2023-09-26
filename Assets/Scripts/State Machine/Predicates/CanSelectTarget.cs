using CombatSystem.Combat;
using UnityEngine;

namespace CombatSystem.StateMachine.Conditions
{
    [CreateAssetMenu(menuName = "State Machine/Predicates/Can Select Target")]
    public class CanSelectTarget : StatePredicate
    {
        [SerializeField] bool checkIfTargetIsNull = false;
        Targeter targeter;

        protected override void OnEnter()
        {
            targeter = controller.GetComponentInChildren<Targeter>();
        }

        protected override bool OnCheck()
        {
            if(checkIfTargetIsNull)
            {
                return targeter.GetCurrentTarget() != null;
            }
            else
            {
                return targeter.SelectTarget();
            }
        }

        protected override void OnExit() { }
    }
}

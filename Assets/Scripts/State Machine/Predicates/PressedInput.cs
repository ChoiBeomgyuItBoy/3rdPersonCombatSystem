using CombatSystem.InputManagement;
using UnityEngine;

namespace CombatSystem.StateMachine.Conditions
{
    [CreateAssetMenu(menuName = "State Machine/Predicates/Pressed Input")]
    public class PressedInput : StatePredicate
    {
        [SerializeField] string actionName = "";
        [SerializeField] bool thisFrame = false;
        InputReader inputReader;

        protected override void OnEnter()
        {
            inputReader = controller.GetComponent<InputReader>();
        }

        protected override bool OnCheck()
        {
            return inputReader.IsPressed(actionName, thisFrame);
        }

        protected override void OnExit() { }
    }
}

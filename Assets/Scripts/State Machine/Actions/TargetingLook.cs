using CombatSystem.InputManagement;
using CombatSystem.Movement;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Targeting Look")]
    public class TargetingLook : StateAction
    {
        [SerializeField] [Range(0,1)] float speedFraction = 1;
        InputReader inputReader;
        Mover mover;
        Animator animator;

        protected override void OnEnter()
        {
            mover = controller.GetComponent<Mover>();
            inputReader = controller.GetComponent<InputReader>();
            animator = controller.GetComponent<Animator>();
        }

        protected override void OnTick()
        {
            mover.MoveTo(GetMovementDirection(), speedFraction, true);
            UpdateBlendTree();
        }

        protected override void OnExit() { }

        private void UpdateBlendTree()
        {
            Vector2 inputValue = inputReader.GetInputAction("Locomotion").ReadValue<Vector2>();

            animator.SetFloat("targetingRight", inputValue.y, 0.05f, Time.deltaTime);
            animator.SetFloat("targetingForward", inputValue.x, 0.05f, Time.deltaTime);
        }

        private Vector3 GetMovementDirection()
        {
            Vector2 inputValue = inputReader.GetInputAction("Locomotion").ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3();
            Vector3 right = controller.transform.right * inputValue.x;
            Vector3 forward = controller.transform.forward * inputValue.y;

            movementDirection += right;
            movementDirection += forward;

            return movementDirection;
        }
    }
}
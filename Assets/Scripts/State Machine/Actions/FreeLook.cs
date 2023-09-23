using CombatSystem.InputManagement;
using CombatSystem.Movement;
using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Free Look")]
    public class FreeLook : StateAction
    {
        [SerializeField] [Range(0,1)] float speedFraction = 1;
        InputReader inputReader;
        Mover mover;
        Transform mainCamera;

        protected override void OnEnter()
        {
            mover = controller.GetComponent<Mover>();
            inputReader = controller.GetComponent<InputReader>();
            mainCamera = Camera.main.transform;
        }

        protected override void OnTick()
        {
            mover.MoveTo(GetMovementDirection(), speedFraction, true);
        }

        protected override void OnExit() { }

        private Vector3 GetMovementDirection()
        {
            Vector2 inputValue = inputReader.GetInputAction("Locomotion").ReadValue<Vector2>();

            Vector3 right = (inputValue.x * mainCamera.right).normalized;
            right.y = 0;

            Vector3 forward = (inputValue.y * mainCamera.forward).normalized;
            forward.y = 0;

            return right + forward;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem.InputManagement
{
    public class InputReader : MonoBehaviour
    {
        Controls controls;

        public bool IsPressed(string actionName, bool thisFrame)
        {
            if(controls == null)
            {
                Enable();
            }

            InputAction action = controls.FindAction(actionName);

            if(action == null)
            {
                Debug.Log($"Action with name {actionName} not found");
                return false;
            }

            return thisFrame? action.WasPressedThisFrame() : action.IsPressed();
        }

        public InputAction GetInputAction(string actionName)
        {
            if(controls == null)
            {
                Enable();
            }

            InputAction action = controls.FindAction(actionName);

            if(action == null)
            {
                Debug.Log($"Action with name {actionName} not found");
            }
            
            return action;
        }

        private void Enable()
        {
            controls = new Controls();
            controls.Player.Enable();
        }
    }
}

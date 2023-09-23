using UnityEngine;

namespace CombatSystem.StateMachine
{
    public class StateController : MonoBehaviour
    {
        [SerializeField] State initalState;
        [SerializeField] State currentState;

        public void SwitchState(State newState)
        {
            currentState = newState.Clone();
        }

        private void Start()
        {
            SwitchState(initalState);
        }

        private void Update()
        {
            currentState?.Tick(this);
        }
    }
}

using UnityEngine;

namespace CombatSystem.StateMachine
{
    public class StateController : MonoBehaviour
    {
        [SerializeField] StateMachine stateMachine;

        public void SwitchState(State newState)
        {
            stateMachine.SwitchState(newState);
        }

        private void Start()
        {
            stateMachine = stateMachine.Clone();
            stateMachine.Enter();
        }

        private void Update()
        {
            stateMachine.Tick(this);
        }
    }
}

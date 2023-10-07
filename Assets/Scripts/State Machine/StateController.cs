using UnityEngine;

namespace CombatSystem.StateMachine
{
    public class StateController : MonoBehaviour
    {
        [SerializeField] StateMachine stateMachine;

        public void SwitchState(string newStateName)
        {
            stateMachine.SwitchState(newStateName);
        }

        private void Start()
        {
            stateMachine = stateMachine.Clone();
            stateMachine.Bind(this);
            stateMachine.Enter();
        }

        private void Update()
        {
            stateMachine.Tick();
        }
    }
}

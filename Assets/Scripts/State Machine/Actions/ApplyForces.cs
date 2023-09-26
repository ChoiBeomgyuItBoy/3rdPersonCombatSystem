using CombatSystem.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Apply Forces")]
    public class ApplyForces : StateAction
    {
        [SerializeField] bool moveNoMotion = true;
        ForceReceiver forceReceiver;

        protected override void OnEnter()
        {
            forceReceiver = controller.GetComponent<ForceReceiver>();
            controller.GetComponent<NavMeshAgent>().enabled = false;
        }

        protected override void OnTick()
        {
            forceReceiver.ApplyForces();

            if(moveNoMotion)
            {
                forceReceiver.MoveWithForces(Vector3.zero);
            }
        }

        protected override void OnExit()
        {
            controller.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

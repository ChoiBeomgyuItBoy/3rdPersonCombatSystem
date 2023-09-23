using UnityEngine;

namespace CombatSystem.Movement
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] float drag = 0.3f;
        float verticalVelocity = 0;
        Vector3 impact;
        Vector3 damping;
        CharacterController controller;

        public void AddForce(Vector3 force)
        {
            impact += force;
        }

        public void MoveWithForces(Vector3 motion)
        {
            controller.Move((motion + GetTotalForce()) * Time.deltaTime);
        }

        public void ApplyForces()
        {
            if(controller.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref damping, drag);     
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private Vector3 GetTotalForce()
        {
            return impact + Vector3.up * verticalVelocity;
        }
    }
}
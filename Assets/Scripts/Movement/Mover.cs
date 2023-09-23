using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 6;
        NavMeshAgent agent;
        Animator animator;

        public void MoveTo(Vector3 destination, float speedFraction, bool isPlayer = false)
        {
            agent.destination = isPlayer? transform.position + destination : destination;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }

        public void LookAt(Vector3 target)
        {
            Vector3 lookPosition = target - transform.position;
            lookPosition.y = 0;

            transform.rotation =  Quaternion.LookRotation(lookPosition);
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            animator.SetFloat("movementSpeed", GetLocalVelocity().magnitude, 0.05f, Time.deltaTime);
        }

        private Vector3 GetLocalVelocity()
        {
            return transform.InverseTransformDirection(agent.velocity);
        }
    }
}

using Cinemachine;
using CombatSystem.Attributes;
using UnityEngine;

namespace CombatSystem.Combat
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
        [SerializeField] float checkRadius = 10;
        CombatTarget currentTarget;
        Camera mainCamera;
        const float targetGroupWeight = 1;
        const float targetGroupRadius = 2;

        public CombatTarget GetCurrentTarget()
        {
            return currentTarget;
        }

        public bool SelectTarget()
        {
            CombatTarget closestTarget = GetClosestTarget();

            if(closestTarget == null)
            {
                return false;
            }

            currentTarget = closestTarget;
            currentTarget.GetComponent<Health>().onDie.AddListener(Cancel);

            if(cinemachineTargetGroup.FindMember(currentTarget.transform) == -1)
            {
                cinemachineTargetGroup.AddMember(currentTarget.transform, targetGroupWeight, targetGroupRadius);
            }
            
            return true;
        }

        public void Cancel()
        {
            if(currentTarget == null)
            {
                return;
            }

            currentTarget.GetComponent<Health>().onDie.RemoveListener(Cancel);
            cinemachineTargetGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        private CombatTarget GetClosestTarget()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, checkRadius, Vector3.up, 0);
            CombatTarget closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach(var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if(target == null) 
                {
                    continue;
                }
                
                Vector2 viewDirection = mainCamera.WorldToViewportPoint(target.transform.position);
                Renderer renderer = target.GetComponentInChildren<Renderer>();

                if(!renderer.isVisible) 
                {
                    continue;
                }

                Vector2 centerView = viewDirection - new Vector2(0.5f, 0.5f);

                if(centerView.sqrMagnitude > closestTargetDistance) 
                {
                    continue;
                }

                closestTarget = target;
                closestTargetDistance = centerView.sqrMagnitude;
            }

            return closestTarget;
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
    }
}
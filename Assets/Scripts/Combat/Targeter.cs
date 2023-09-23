using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace CombatSystem.Combat
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
        List<CombatTarget> targets = new List<CombatTarget>();
        [SerializeField] CombatTarget currentTarget;
        Camera mainCamera;
        const float targetGroupWeight = 1;
        const float targetGroupRadius = 2;

        public CombatTarget GetCurrentTarget()
        {
            return currentTarget;
        }

        public bool SelectTarget()
        {
            if(targets.Count == 0)
            {
                return false;
            }

            CombatTarget closestTarget = GetClosestTarget();

            if(closestTarget == null)
            {
                return false;
            }

            currentTarget = closestTarget;

            if(!targets.Contains(closestTarget))
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
            
            cinemachineTargetGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            CombatTarget target = other.GetComponent<CombatTarget>();

            if(target != null) 
            {
                AddTarget(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CombatTarget target = other.GetComponent<CombatTarget>();

             if(target != null) 
            {
                RemoveTarget(target);
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void AddTarget(CombatTarget target)
        {
            targets.Add(target);
        }

        private void RemoveTarget(CombatTarget target)
        {
            if(currentTarget == target)
            {
                cinemachineTargetGroup.RemoveMember(currentTarget.transform);
                currentTarget = null;
            }

            targets.Remove(target);
        }

        private CombatTarget GetClosestTarget()
        {
            CombatTarget closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach(var target in targets)
            {
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
    }
}
using CombatSystem.Attributes;
using CombatSystem.Movement;
using UnityEngine;

namespace CombatSystem.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Transform damageCenter;
        [SerializeField] float damageRadius = 3;

        public void OnHit(GameObject user, WeaponConfig weaponConfig, ComboAttack currentAttack)
        {
            RaycastHit[] hits = Physics.SphereCastAll(damageCenter.position, damageRadius, Vector3.up, 0);
            
            foreach(var hit in hits)
            {
                Health health = hit.transform.GetComponent<Health>();
                ForceReceiver forceReceiver = hit.transform.GetComponent<ForceReceiver>();

                if(health != null && health != user.GetComponent<Health>()) 
                {
                    health.TakeDamage(GetTotalDamage(weaponConfig, currentAttack));
                }

                if(forceReceiver != null && forceReceiver != user.GetComponent<ForceReceiver>())
                {
                    forceReceiver.AddForce(GetTotalForce(user, health, currentAttack));
                }
            }
        }

        private float GetTotalDamage(WeaponConfig weaponConfig, ComboAttack currentAttack)
        {
            float baseDamage = weaponConfig.GetBaseDamage();
            float bonusPercentage = currentAttack.GetDamagePercentageBonus();
            float bonus = baseDamage * bonusPercentage;
            return baseDamage + bonus;
        }

        private Vector3 GetTotalForce(GameObject user, Health target, ComboAttack currentAttack)
        {
            Vector3 userPosition = user.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 forceDirection = (targetPosition - userPosition).normalized;
            return forceDirection * currentAttack.GetKnockback();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(damageCenter.position, damageRadius);
        }
    }
}
using CombatSystem.Attributes;
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

                if(health == null) continue;
                if(health == user.GetComponent<Health>()) continue;
                
                health.TakeDamage(GetTotalDamage(weaponConfig, currentAttack));
            }
        }

        private float GetTotalDamage(WeaponConfig weaponConfig, ComboAttack currentAttack)
        {
            float baseDamage = weaponConfig.GetBaseDamage();
            float bonusPercentage = currentAttack.GetDamagePercentageBonus();
            float bonus = baseDamage * bonusPercentage;
            return baseDamage + bonus;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(damageCenter.position, damageRadius);
        }
    }
}
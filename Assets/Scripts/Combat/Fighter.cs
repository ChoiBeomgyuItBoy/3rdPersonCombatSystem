using UnityEngine;

namespace CombatSystem.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;
        Weapon currentWeapon;
        int currentAttackIndex = 0;

        public void EquipWeapon()
        {
            currentWeapon = weaponConfig.Spawn(rightHand, leftHand, GetComponent<Animator>());
        }

        public ComboAttack GetCurrentAttack()
        {
            return weaponConfig.GetAttack(currentAttackIndex);
        }

        public bool CurrentAttackIsLast()
        {
            return currentAttackIndex == weaponConfig.GetComboLength() - 1;
        }

        public void CycleCurrentAttack()
        {
            currentAttackIndex++;
        }

        public void ResetCombo()
        {
            currentAttackIndex = 0;
        }

        // Animation Event
        public void Hit()
        {   
            currentWeapon.OnHit(gameObject, weaponConfig, GetCurrentAttack());
        }

        private void Start()
        {
            EquipWeapon();
        }
    }
}

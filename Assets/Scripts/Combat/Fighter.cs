using UnityEngine;

namespace CombatSystem.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;
        Weapon currentWeapon;
        Animator animator;
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

        public float GetAttackNormalizedTime()
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
            {
                return currentInfo.normalizedTime;
            }

            return 0;
        }

        public void ResetNormalizedTime(float resetValue)
        {
            animator.Play(0, 0, resetValue);
        }

        // Animation Event
        public void Hit()
        {   
            currentWeapon.OnHit(gameObject, weaponConfig, GetCurrentAttack());
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EquipWeapon();
        }
    }
}

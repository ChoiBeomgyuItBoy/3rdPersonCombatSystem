using UnityEngine;

namespace CombatSystem.Combat
{
    [CreateAssetMenu(menuName = "New Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Setup")]
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] Weapon equippedWeaponPrefab;
        [SerializeField] bool isRightHanded = true;
        
        [Header("Info")]
        [SerializeField] float baseDamage = 50;
        [SerializeField] ComboAttack[] combo;
        
        const string weaponName = "Weapon";

        public float GetBaseDamage()
        {
            return baseDamage;
        }

        public ComboAttack GetAttack(int index)
        {
            return combo[index];
        }

        public int GetComboLength()
        {
            return combo.Length;
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Weapon weaponInstance = null;

            if (equippedWeaponPrefab != null)
            {
                if (isRightHanded)
                {
                    weaponInstance = Instantiate(equippedWeaponPrefab, rightHand);
                }
                else
                {
                    weaponInstance = Instantiate(equippedWeaponPrefab, leftHand);
                }

                weaponInstance.gameObject.name = weaponName;
            }

            SetAnimatorOverride(animator);

            return weaponInstance;
        }


        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);

            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if(oldWeapon == null)
            {
                return;
            }

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon);
        }

        private void SetAnimatorOverride(Animator animator)
        {
            var defaultController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (defaultController != null)
            {
                animator.runtimeAnimatorController = defaultController.runtimeAnimatorController;
            }
        }

    }
}

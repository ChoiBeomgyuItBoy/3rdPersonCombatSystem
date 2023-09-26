using System;
using UnityEngine;

namespace CombatSystem.Combat
{
    [System.Serializable]
    public class ComboAttack 
    {
        [SerializeField] string animationName = "";
        [SerializeField] float attackForce = 1;
        [SerializeField] float knockback  = 3;
        [SerializeField] [Range(0,1)] float damagePercentageBonus = 0.2f;
        [SerializeField] [Range(0,0.9f)] float applyForceTime = 0.6f;
        [SerializeField] [Range(0,0.9f)] float timeToContinue = 0.6f;
        
        public string GetAnimationName()
        {
            return animationName;
        }

        public float GetTimeToContinue()
        {
            return timeToContinue;
        }

        public float GetDamagePercentageBonus()
        {
            return damagePercentageBonus;
        }

        public float GetAttackForce()
        {
            return attackForce;
        }

        public float GetApplyForceTime()
        {
            return applyForceTime;
        }

        public float GetKnockback()
        {
            return knockback;
        }
    }
}

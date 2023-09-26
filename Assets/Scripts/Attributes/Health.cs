using System;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth;
        public UnityEvent<float> onDamageTaken;
        public UnityEvent onDie;

        public bool IsDead()
        {
            return currentHealth == 0;
        }

        public void TakeDamage(float damage)
        {
            if(currentHealth == 0) 
            {
                return;
            }

            currentHealth = Mathf.Max(0, currentHealth - damage);
            onDamageTaken?.Invoke(damage);

            if(currentHealth == 0)
            {
                onDie?.Invoke();
            }
        }

        public float GetHealthFraction()
        {
            return currentHealth / maxHealth;
        }

        private void Awake()
        {
            currentHealth = maxHealth;
        }
    }
}
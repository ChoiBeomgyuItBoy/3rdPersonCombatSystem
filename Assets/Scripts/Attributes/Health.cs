using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth;
        public UnityEvent<float> onHealthChange;
        public UnityEvent onDie;

        public void ChangeHealth(float healthChange)
        {
            if(currentHealth == 0) 
            {
                return;
            }

            currentHealth = Mathf.Max(0, currentHealth + healthChange);
            onHealthChange?.Invoke(healthChange);

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
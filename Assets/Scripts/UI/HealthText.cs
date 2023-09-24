using TMPro;
using UnityEngine;

namespace CombatSystem.UI
{
    public class HealthText : MonoBehaviour
    {
        [SerializeField] TMP_Text healthText;

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            healthText.text = $"{amount}";
        }
    }   
}
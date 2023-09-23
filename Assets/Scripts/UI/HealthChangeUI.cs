using TMPro;
using UnityEngine;

namespace CombatSystem.UI
{
    public class HealthChangeUI : MonoBehaviour
    {
        [SerializeField] TMP_Text damageText;

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            damageText.text = $"{amount}";
        }
    }   
}
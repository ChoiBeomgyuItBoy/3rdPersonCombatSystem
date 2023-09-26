using CombatSystem.Attributes;
using UnityEngine;

namespace CombatSystem.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground;
        [SerializeField] Health health;

        void Update()
        {
            foreground.localScale = new Vector3(health.GetHealthFraction(), 1, 1);
        }
    }
}

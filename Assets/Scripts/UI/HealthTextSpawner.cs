using UnityEngine;

namespace CombatSystem.UI
{
    public class HealthTextSpawner : MonoBehaviour
    {
        [SerializeField] HealthText healthTextPrefab;

        public void Spawn(float healthChange)
        {
            var instance = Instantiate(healthTextPrefab, transform);
            instance.SetValue(healthChange);
        }
    }
}
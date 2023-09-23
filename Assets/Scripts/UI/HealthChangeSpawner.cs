using UnityEngine;

namespace CombatSystem.UI
{
    public class HealthChangeSpawner : MonoBehaviour
    {
        [SerializeField] HealthChangeUI healthChangePrefab;

        public void Spawn(float healthChange)
        {
            var instance = Instantiate(healthChangePrefab, transform);
            instance.SetValue(healthChange);
        }
    }
}
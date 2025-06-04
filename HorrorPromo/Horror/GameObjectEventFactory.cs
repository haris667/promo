using Quests;
using System.Collections;
using UnityEngine;

namespace Horror
{
    public class GameObjectEventFactory : MonoBehaviour
    {
        [SerializeField]
        private GameObject defaultPrefab;

        public GameObject Create(GameObjectEventSettings settings)
        {
            if (settings.prefab == null && defaultPrefab == null)
            {
                Debug.LogWarning("No prefab assigned for GameObject event!");
                return null;
            }

            GameObject prefabToUse = settings.prefab != null ? settings.prefab : defaultPrefab;
            GameObject spawnedObject = Instantiate(
                prefabToUse,
                settings.position,
                settings.rotation,
                settings.parent
            );

            spawnedObject.name = $"Event_{prefabToUse.name}_{Time.time}";

            // Настройка физики
            if (settings.applyVelocity)
            {
                Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                if (rb == null) rb = spawnedObject.AddComponent<Rigidbody>();
                rb.AddForce(settings.initialVelocity, settings.forceMode);
            }

            // Настройка времени жизни
            if (settings.destroyAfterDuration && settings.duration > 0)
            {
                if (settings.fadeOutBeforeDestroy)
                {
                    StartCoroutine(FadeAndDestroy(spawnedObject, settings.duration, settings.fadeDuration));
                }
                else
                {
                    Destroy(spawnedObject, settings.duration);
                }
            }

            // Визуальные эффекты
            if (settings.spawnParticles != null)
            {
                Instantiate(settings.spawnParticles, settings.position, settings.rotation);
            }

            return spawnedObject;
        }

        private IEnumerator FadeAndDestroy(GameObject target, float delay, float fadeTime)
        {
            yield return new WaitForSeconds(delay - fadeTime);

            // Пример для простого объекта с Renderer
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = renderer.material;
                Color originalColor = mat.color;
                float timer = 0f;

                while (timer < fadeTime)
                {
                    timer += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, timer / fadeTime);
                    mat.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                    yield return null;
                }
            }

            Destroy(target);
        }
    }

}

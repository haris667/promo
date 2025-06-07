using Cysharp.Threading.Tasks;
using Items;
using Quests;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Horror
{
    public class GameObjectEventCreator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _defaultPrefab;
        [SerializeField]
        private Transform _parentForSpawn;

        [Inject]
        private DiContainer _container;

        public async UniTask Create(GameObjectEventSettings settings)
        {
            if (settings.prefab == null && _defaultPrefab == null)
                Debug.LogWarning("No prefab assigned for GameObject event!");

            await UniTask.Delay(TimeSpan.FromSeconds(settings.delay));

            GameObject prefabToUse = settings.prefab != null ? settings.prefab : _defaultPrefab;
            GameObject spawnedObject = _container.InstantiatePrefab(prefabToUse, 
                                                                    settings.position, 
                                                                    settings.rotation, 
                                                                    _parentForSpawn);

            spawnedObject.name = $"Event_{prefabToUse.name}_{Time.time}";

            if (settings.applyVelocity)
            {
                Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                if (rb == null) rb = spawnedObject.AddComponent<Rigidbody>();
                rb.AddForce(settings.initialVelocity, settings.forceMode);
            }

            if (settings.destroyAfterDuration && settings.duration > 0)
            {
                if (settings.fadeOutBeforeDestroy)
                    StartCoroutine(FadeAndDestroy(spawnedObject, settings.duration, settings.fadeDuration));
                
                else
                    Destroy(spawnedObject, settings.duration);
                
            }
            if (settings.spawnParticles != null)
                Instantiate(settings.spawnParticles, settings.position, settings.rotation);  
        }

        private IEnumerator FadeAndDestroy(GameObject target, float delay, float fadeTime)
        {
            yield return new WaitForSeconds(delay - fadeTime);

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

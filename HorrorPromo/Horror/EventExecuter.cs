using Quests;
using Sound;
using Subtitles;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Horror
{
    //можно и без монобеха на UniTask все сделать, но решил вспомнить корутины, да и спавню я отсюда без фабрики
    public class EventExecuter : MonoBehaviour
    {
        [Inject] 
        private SignalBus _signalBus;
        [Inject] 
        private DiContainer _container;
        [SerializeField]
        private SubtitlesView _subtitlesView;
        [SerializeField]
        private AudioEventFactory _audioFactory;
        [SerializeField]
        private GameObjectEventFactory _gameObjectFactory;
        /*[Inject] private CameraController _cameraController;
        [Inject] private LightingSystem _lightingSystem;*/

        // Основной метод для выполнения события
        public void ExecuteEvent(GameEvent gameEvent)
        {
            StartCoroutine(ExecuteEventRoutine(gameEvent));
        }

        private IEnumerator ExecuteEventRoutine(GameEvent gameEvent)
        {
            foreach (var sound in gameEvent.sounds)
            {
                StartCoroutine(PlayDelayedSound(sound));
            }

            foreach (var visual in gameEvent.visuals)
            {
                SpawnVisualEffect(visual);
            }

            foreach (var physics in gameEvent.physicsEffects)
            {
                ApplyPhysicsEffect(physics);
            }

            if(gameEvent.subtitlesText.Any())
            {
                PlaySubtitles(gameEvent.authorType, gameEvent.subtitlesText, gameEvent.delaySubtitle);
            }


            // Ожидание завершения самого длительного эффекта
            float maxDuration = CalculateMaxDuration(gameEvent);
            yield return new WaitForSeconds(maxDuration);
        }

        private void PlaySubtitles(AuthorType type, string text, float delay = 0)
        {
            _subtitlesView.NewSubtitle(type, text, delay);
        }

        private float CalculateMaxDuration(GameEvent gameEvent)
        {
            float maxDuration = 0f;

            foreach (var sound in gameEvent.sounds)
            {
                float total = sound.delay + (sound.clip ? sound.clip.length : 0);
                if (total > maxDuration) maxDuration = total;
            }

            foreach (var visual in gameEvent.visuals)
            {
                if (visual.duration > maxDuration) maxDuration = visual.duration;
            }

            foreach (var physics in gameEvent.physicsEffects)
            {
                if (physics.duration > maxDuration) maxDuration = physics.duration;
            }

            return maxDuration;
        }

        private IEnumerator PlayDelayedSound(EventSoundSettings soundSettings)
        {
            if (soundSettings.delay > 0)
                yield return new WaitForSeconds(soundSettings.delay);

            if (soundSettings.clip != null)
            {
                _audioFactory.Create(soundSettings);
            }
        }

        private void SpawnVisualEffect(GameObjectEventSettings settings)
        {
            if (settings.prefab != null)
            {
                GameObject go = _gameObjectFactory.Create(settings);
            }
        }

        private void ApplyPhysicsEffect(HorrorPhysics physics)
        {
            switch (physics.type)
            {
                case HorrorPhysics.PhysicsType.ShakeCamera:
                    //_cameraController.Shake(physics.intensity, physics.duration);
                    break;

                case HorrorPhysics.PhysicsType.FlickerLights:
                    //_lightingSystem.FlickerLights(physics.duration, physics.intensity);
                    break;

                case HorrorPhysics.PhysicsType.MoveObject:
                    MoveObject(physics.targetObject, physics.intensity, physics.duration);
                    break;

                case HorrorPhysics.PhysicsType.SpawnEntity:
                    SpawnEntity(physics.targetObject, physics.intensity);
                    break;
            }
        }

        private void MoveObject(string objectTag, float intensity, float duration)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(objectTag);
            if (obj != null)
            {
                StartCoroutine(MoveObjectRoutine(obj, intensity, duration));
            }
        }

        private IEnumerator MoveObjectRoutine(GameObject obj, float intensity, float duration)
        {
            Vector3 originalPosition = obj.transform.position;
            Vector3 targetPosition = originalPosition + Random.insideUnitSphere * intensity;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                obj.transform.position = Vector3.Lerp(
                    originalPosition,
                    targetPosition,
                    elapsed / duration
                );

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void SpawnEntity(string entityPrefabName, float intensity)
        {
            /*GameObject prefab = Resources.Load<GameObject>($"Entities/{entityPrefabName}");
            if (prefab != null)
            {
                Vector3 spawnPosition = FindSpawnPosition(intensity);
                _container.InstantiatePrefab(prefab, spawnPosition, Quaternion.identity);
            }*/
        }

        private Vector3 FindSpawnPosition(float distanceFromPlayer)
        {
            // Логика поиска позиции вне поля зрения игрока
            Transform player = Camera.main.transform;
            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
            randomDirection.y = 0; // Оставляем на уровне пола

            return player.position + randomDirection.normalized * distanceFromPlayer;
        }
    }

}

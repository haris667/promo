using AI;
using Cysharp.Threading.Tasks;
using Quests;
using Sound;
using Subtitles;
using System;
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
        private AudioEventCreator _audioCreator;
        [SerializeField]
        private LightingEventCreator _lightingCreator;
        [SerializeField]
        private GameObjectEventCreator _gameObjectCreator;
        [SerializeField]
        private UsedItemsEventCreator _usedItemsCreator;
        [SerializeField]
        private TimelineEventCreator _timelineCreator;
        [SerializeField] 
        private AgentController _agentController;
        [SerializeField] 
        private ScreamerEventCreator _screamerCreator;

        /*[Inject] private CameraController _cameraController;
        [Inject] private LightingSystem _lightingSystem;*/

        // Основной метод для выполнения события
        public async UniTask ExecuteEvent(GameEvent gameEvent)
        {
            StartCoroutine(ExecuteEventRoutine(gameEvent, false));

            if (!gameEvent.hasRetry)
                return;
            
            for(int i = 0; i < gameEvent.amountReapets; i++)
            {
                //var duration = gameEvent.durationBeforeRepeat + RandomGeneratorHelper.GenerateRandomInRange(gameEvent.durationBeforeOffset, 2);
                await UniTask.Delay(TimeSpan.FromSeconds(gameEvent.durationBeforeRepeat));
                StartCoroutine(ExecuteEventRoutine(gameEvent, true));
            }
        }

        private IEnumerator ExecuteEventRoutine(GameEvent gameEvent, bool isRepeat)
        {
            foreach (var visual in gameEvent.visuals)
            {
                ApplyVisualEffect(visual);
            }

            foreach (var lighting in gameEvent.lightings)
            {
                ApplyLightingEffect(lighting);
            }

            foreach(var item in gameEvent.usedItems)
            {
                ApplyUsedItemEffect(item);
            }

            foreach(var timeline in gameEvent.timelines)
            {
                ApplyTimelineEffect(timeline);
            }

            foreach (var screamer in gameEvent.screamers)
            {
                ApplyScreamerEffect(screamer);
            }

            foreach (var agentEvent in gameEvent.agentEvents)
            {
                ApplyAgentEffect(agentEvent);
            }

            if (gameEvent.subtitlesText.Any() && !isRepeat)
            {
                ApplySubtitles(gameEvent.authorType, gameEvent.subtitlesText, gameEvent.delaySubtitle);
            }        

            if (!isRepeat)
            {
                foreach (var sound in gameEvent.sounds)
                    StartCoroutine(ApplySoundEffect(sound));
            }

            // Ожидание завершения самого длительного эффекта
            float maxDuration = CalculateMaxDuration(gameEvent);
            yield return new WaitForSeconds(maxDuration);
        }

        private void ApplySubtitles(AuthorType type, string text, float delay = 0)
        {
            _subtitlesView.Create(type, text, delay);
        }
        private void ApplyScreamerEffect(ScreamerEventSettings settings)
        {
            _screamerCreator.Create(settings).Forget();
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

            foreach (var lighting in gameEvent.lightings)
            {
                if (lighting.duration > maxDuration) maxDuration = lighting.duration;
            }

            return maxDuration;
        }

        private IEnumerator ApplySoundEffect(SoundEventSettings settings)
        {
            if (settings.delay > 0)
                yield return new WaitForSeconds(settings.delay);

            if (settings.clip != null)
            {
                _audioCreator.Create(settings);
            }
        }

        private async UniTask ApplyAgentEffect(AgentEventSettings settings)
        {
            if (_agentController != null)
            {
                // Запуск асинхронно без ожидания (агент будет работать параллельно)
                _agentController.ExecuteAgentEvent(settings);
            }
        }
        private  void ApplyLightingEffect(LightingEventSettings settings)
        {
            _lightingCreator.Create(settings);
        }

        private void ApplyVisualEffect(GameObjectEventSettings settings)
        {
            if (settings.prefab != null)
            {
                _gameObjectCreator.Create(settings);
            }
        }

        private void ApplyTimelineEffect(TimelineEventSettings settings)
        {
            _timelineCreator.Create(settings);
        }

        private void ApplyUsedItemEffect(UsedItemsEventSettings settings)
        {
            _usedItemsCreator.Create(settings);
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

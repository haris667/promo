using AI;
using Subtitles;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(menuName = "Configs/HorrorEventConfig")]
    public class EventConfig : ScriptableObject
    {
        public GameEvent[] events;
    }

    [System.Serializable]
    public class GameEvent
    {
        public TriggerType triggerType;
        public string questId;
        public string taskId; //к какому айди задачи привязан ивент
        public int progressTrigger;

        [Header("Effects")]
        public bool hasRetry = false;
        public int amountReapets = 1;
        public float durationBeforeRepeat;
        public float durationBeforeOffset; //рандомно прибавляется в диапазоне + и - этого числа к времени повтора

        public SoundEventSettings[] sounds;
        public GameObjectEventSettings[] visuals;
        public LightingEventSettings[] lightings;
        public UsedItemsEventSettings[] usedItems;
        public TimelineEventSettings[] timelines;
        public AgentEventSettings[] agentEvents;
        public ScreamerEventSettings[] screamers;

        [SerializeField] 
        private AgentController _agentController;

        [TextArea(3, 10)]
        public string subtitlesText;
        public float delaySubtitle = 0;
        public AuthorType authorType;
    }

    [System.Serializable]
    public class ScreamerEventSettings
    {
        [Tooltip("Префаб скримера (должен иметь аниматор)")]
        public GameObject screamerPrefab;

        [Tooltip("Локальная позиция относительно камеры игрока")]
        public Vector3 localPosition = new Vector3(0, 0, 1f);

        [Tooltip("Локальный поворот относительно камеры игрока")]
        public Vector3 localRotation = Vector3.zero;

        [Tooltip("Задержка перед появлением")]
        public float delay = 0f;

        [Tooltip("Длительность показа скримера")]
        public float duration = 2f;

        [Tooltip("Анимация для воспроизведения")]
        public string animationTrigger = "Scream";

        [Tooltip("Звук скримера")]
        public AudioClip sound;

        [Tooltip("Громкость звука")]
        [Range(0f, 1f)] public float volume = 1f;
    }

    [System.Serializable]
    public class AgentEventSettings
    {
        public Vector3 targetPosition;
        public Vector3 lookDirection; // Направление для финального поворота
        public string animationName; // Название анимации для триггера
        public float moveSpeed = 3.5f;
        public float rotationSpeed = 120f;
        public float stoppingDistance = 0.5f;
        public float delayBeforeAction;
    }

    [System.Serializable]
    public class SoundEventSettings
    {
        public AudioClip clip;
        public float delay;
        public float volume = 1f;
        public bool spatial = true;
        public Vector3 position;

        [Range(0f, 1f)] 
        public float spatialBlend = 1f;
        public float minDistance = 1f;
        public float maxDistance = 20f;
        public bool destroyAfterPlay = true;
        public bool loop = false;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
    }

    [System.Serializable]
    public class GameObjectEventSettings
    {
        public GameObject prefab;
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Transform parent = null;
        public float delay = 0;

        public float duration = 0f; // 0 = бесконечно
        public bool destroyAfterDuration = true;

        public bool applyVelocity = false;
        public Vector3 initialVelocity = Vector3.zero;
        public ForceMode forceMode = ForceMode.Impulse;

        public bool fadeOutBeforeDestroy = false;
        public float fadeDuration = 1f;
        public ParticleSystem spawnParticles = null;
    }

    [System.Serializable]
    public class LightingEventSettings
    {
        public float delay;
        public float newIntensity = 0;
        public float duration = 0.1f;
        public bool isLerped = false;
        public bool isRevertAfterDuration = true;
    }

    //тк с таймлайном я познакомился недавно, решение тут костыльное и будет он только один
    [System.Serializable]
    public class TimelineEventSettings
    {
        public float delay;
    }

    [System.Serializable]
    public class UsedItemsEventSettings
    {
        public float delay;
    }

    /*//конкретно этот - костыльный. Тк я не знаю способа в рантайме менять Global Volume ИМЕННО с блендингом
    //я переношу локальный Local Volume с более высоким приоритетом к игроку
    public class RenderEventSettings
    {

    }*/

    [System.Serializable]
    public class HorrorPhysics
    {
        public enum PhysicsType { ShakeCamera, FlickerLights, MoveObject, SpawnEntity }
        public PhysicsType type;
        public float intensity;
        public float duration;
        public string targetObject; // Для MoveObject
    }

    public enum TriggerType
    {
        TaskProgress,
        QuestStart,
        QuestComplete
    }
}



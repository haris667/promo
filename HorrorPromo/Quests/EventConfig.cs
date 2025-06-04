using Subtitles;
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
        public EventSoundSettings[] sounds;
        public GameObjectEventSettings[] visuals;
        public HorrorPhysics[] physicsEffects;

        [TextArea(3, 10)]
        public string subtitlesText;
        public float delaySubtitle = 0;
        public AuthorType authorType;
    }

    [System.Serializable]
    public class EventSoundSettings
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



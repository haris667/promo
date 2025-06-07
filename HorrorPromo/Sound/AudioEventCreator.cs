using Quests;
using UnityEngine;

namespace Sound
{
    //для инстанцирования конечно, лучше было бы какую нибудь одну ультимативную
    //абстрактную фабрику реализовать и уже сверху с ней работать.
    //Но уже каркас задан и не имею желания менять.
    //Все что обладает возможностями креатора в этом проекте - монобех и это хреново
    //ну и Factory - потому что креатор, я не реализую этот паттерн (фабр. метод или абстр. фабрика)
    //в этом проекте
    public class AudioEventCreator : MonoBehaviour
    {
        [SerializeField] 
        private GameObject soundPrefab;
        [SerializeField]
        private AudioSource eventAudioSource;

        private void Awake()
        {
            if (soundPrefab == null)
            {
                soundPrefab = new GameObject("SoundPrefab");
                soundPrefab.AddComponent<AudioSource>();
                soundPrefab.hideFlags = HideFlags.HideInHierarchy;
            }
        }

        public GameObject Create(SoundEventSettings soundSettings)
        {
            if (soundSettings.clip == null)
            {
                Debug.LogWarning("Attempted to create sound without AudioClip!");
                return null;
            }

            GameObject soundObject = Instantiate(soundPrefab);
            soundObject.name = $"Sound_{soundSettings.clip.name}";
            soundObject.transform.position = soundSettings.position;

            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            ConfigureAudioSource(audioSource, soundSettings);

            if (soundSettings.destroyAfterPlay && !soundSettings.loop)
            {
                Destroy(soundObject, soundSettings.delay + soundSettings.clip.length + 0.1f);
            }

            return soundObject;
        }

        private void ConfigureAudioSource(AudioSource source, SoundEventSettings settings)
        {
            source.clip = settings.clip;
            source.volume = settings.volume;
            source.loop = settings.loop;

            // Настройка пространственного звука
            if (settings.spatial)
            {
                source.spatialBlend = settings.spatialBlend;
                source.minDistance = settings.minDistance;
                source.maxDistance = settings.maxDistance;
                source.rolloffMode = settings.rolloffMode;
            }
            else
            {
                source.spatialBlend = 0f; // 2D звук
            }

            // Запуск с задержкой или немедленно
            if (settings.delay > 0)
            {
                source.PlayDelayed(settings.delay);
            }
            else
            {
                source.Play();
            }
        }
    }
}

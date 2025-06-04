using UnityEngine;

namespace Sound
{
    public class TemporarySound : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!audioSource.isPlaying && audioSource.loop == false)
            {
                Destroy(gameObject);
            }
        }
    }

}

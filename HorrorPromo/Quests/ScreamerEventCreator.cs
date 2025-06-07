using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Quests
{
    //вся реализация под перепись. Тут на скорость просто запустилось чтобы
    public class ScreamerEventCreator : MonoBehaviour
    {
        [Inject] 
        private DiContainer _container;
        [SerializeField]
        private FirstPersonController _player;

        public async UniTask Create(ScreamerEventSettings settings)
        {
            if (settings.screamerPrefab == null)
            {
                Debug.LogWarning("Screamer prefab is not assigned!");
                return;
            }

            await UniTask.Delay((int)(settings.delay * 1000));

            // Получаем камеру игрока
            Transform playerCamera = Camera.main.transform;
            _player.cameraCanMove = false;

            // Создаем скример как дочерний объект камеры
            GameObject screamer = Instantiate(
                settings.screamerPrefab,
                playerCamera.localPosition,
                playerCamera.GetChild(0).GetChild(0).rotation,
                playerCamera.GetChild(0)
            );

            //ForceLookAtCamera(screamer.transform, playerCamera.transform);

            // Настраиваем анимацию
            Animator animator = screamer.GetComponent<Animator>();
            if (animator != null && !string.IsNullOrEmpty(settings.animationTrigger))
            {
                animator.SetTrigger(settings.animationTrigger);
            }

            // Воспроизводим звук
            if (settings.sound != null)
            {
                AudioSource audioSource = screamer.AddComponent<AudioSource>();
                audioSource.spatialBlend = 0; // 2D звук
                audioSource.volume = settings.volume;
                audioSource.PlayOneShot(settings.sound);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.65));
            SceneManager.LoadScene("Boot");
            //_player.cameraCanMove = false; т.к. выше костыль
        }

        private void ForceLookAtCamera(Transform screamer, Transform camera)
        {
            Vector3 directionToCamera = camera.position - screamer.position;
            //directionToCamera.y = 0;
            screamer.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }
}



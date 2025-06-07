using Installers;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Items
{
    /// <summary>
    /// Для проверки нахождения персонажа в координатах
    /// </summary>
    public class InvisiblePlateView : ItemView
    {
        [Header("Invisible Plate Settings")]
        [SerializeField] 
        private float requiredTimeInside = 3f; // Необходимое время в секундах
        [SerializeField] 
        private string playerTag = "Player";   // Тег объекта игрока
        [SerializeField]
        private bool _isDestroyAtEnd = true;

        [Inject] 
        private SignalBus _signalBus;

        private Coroutine _timerCoroutine;
        private bool _isPlayerInside;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) 
                return;

            if (_isPlayerInside)
                return;

            _isPlayerInside = true;
            _timerCoroutine = StartCoroutine(TriggerTimer());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(playerTag))
                return;

            if (!_isPlayerInside)
                return;

            ResetTimer();
        }

        private IEnumerator TriggerTimer()
        {
            yield return new WaitForSeconds(requiredTimeInside);

            if (_isPlayerInside)
            {
                var destroyTrigger = new ReactiveProperty<bool>(false);
                destroyTrigger
                    .Where(value => value)
                    .Take(1) 
                    .Subscribe(_ => Destroy(gameObject))
                    .AddTo(this); 

                var signal = new PlayerInsideCoordSignal
                {
                    item = _config,
                    amount = 1,
                    destroySelfTrigger = destroyTrigger
                };

                _signalBus.Fire<PlayerInsideCoordSignal>(signal);
                Debug.Log("Метка поставлена, сигнал обнаружен");
            }
        }

        private void ResetTimer()
        {
            _isPlayerInside = false;

            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            ResetTimer();
        }
    }

}

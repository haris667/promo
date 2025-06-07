using Infrastructure.MVP;
using Installers;
using UnityEngine;
using Zenject;

namespace Items
{
    public class CoffeeMachineView : ItemView, IUseItem
    {
        [SerializeField]
        private AudioSource _audioSource;

        [Inject]
        private SignalBus _signalBus;

        private bool _isOn = false;

        //костыльком вставил чтобы хоррор ивент запускался. Для этого, по хорошему, нужен механизм отдельный для связи хоррор ивентов и предметов
        public void Use(bool fireSignal = true)
        {
            _isOn = !_isOn;
            if (!fireSignal)
                return;

            _signalBus.Fire(new ItemUsedSignal()
            {
                item = _config,
                amount = 1
            });

        }
    }
}
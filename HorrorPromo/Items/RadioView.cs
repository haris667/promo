﻿using Installers;
using UnityEngine;
using Zenject;

namespace Items
{
    public class RadioView : ItemView, IUseItem
    {
        [SerializeField]
        private AudioSource _audioSource;

        [Inject]
        private SignalBus _signalBus;

        private bool _isOn = false;

        //костыльком вставил чтобы хоррор ивент запускался. Для этого, по хорошему, нужен механизм отдельный для связи хоррор ивентов и предметов
        public void Use(bool useSignal = true)
        {
            _isOn = !_isOn;

            if (_isOn)
                _audioSource.Play();
            else
                _audioSource.Stop();

            if (!useSignal)
                return;

            _signalBus.Fire(new ItemUsedSignal()
            {
                item = _config,
                amount = 1
            });
        }
    }
}
using Installers;
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

        public void Use()
        {
            _isOn = !_isOn;

            if (_isOn)
                _audioSource.Play();
            else
                _audioSource.Stop();

            _signalBus.Fire(new ItemUsedSignal()
            {
                item = _config,
                amount = 1
            });
        }
    }
}
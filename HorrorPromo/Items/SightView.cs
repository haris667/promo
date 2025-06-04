using Infrastructure.MVP;
using Installers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Items
{
    public class SightView : AView
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private List<TextMeshProUGUI> _sights;

        [SerializeField]
        private Color _useColor;
        [SerializeField]
        private Color _baseColor; //это тоже бы в конфиг все выделить, и сюда уже из конфига сувать
                                  //если вернусь сюда, то сделаю так

        [Inject]
        private InputMap _inputMap;
        [Inject]
        private SignalBus _signalBus;

        protected override void Init()
        {
            _inputMap.Player.Aim.performed += OnAimPerformed;
            _inputMap.Player.Aim.canceled += OnAimCanceled;

            _signalBus.Subscribe<AimItemSignal>(SetUseColor);
            _signalBus.Subscribe<AimVoidSignal>(SetBaseColor);
        }

        private void OnDestroy()
        {
            _inputMap.Player.Aim.performed -= OnAimPerformed;
            _inputMap.Player.Aim.canceled -= OnAimCanceled;

            _signalBus.Unsubscribe<AimItemSignal>(SetUseColor);
            _signalBus.Unsubscribe<AimVoidSignal>(SetBaseColor);
        }

        private void OnAimPerformed(InputAction.CallbackContext context)
        {
            _animator.SetBool("Aim", true);
        }

        private void OnAimCanceled(InputAction.CallbackContext context)
        {
            _animator.SetBool("Aim", false);
        }
        
        private void SetUseColor()
        {
            for (int i = 0; i < _sights.Count; i++)
                _sights[i].color = _useColor;  
        }

        private void SetBaseColor()
        {
            for (int i = 0; i < _sights.Count; i++)
                _sights[i].color = _baseColor;
        }
    }
}

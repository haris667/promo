using Cysharp.Threading.Tasks;
using Infrastructure.MVP;
using Installers;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Subtitles
{    
    public class SubtitlesView : AView
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [TextArea(3, 10)]
        [SerializeField]
        private string _startSubtitle;

        [SerializeField]
        private bool _hasStartSubtitle;

        [SerializeField]
        private bool _isEndRemove = true;

        [Inject]
        private SignalBus _signalBus;

        private float _delayForShowText = 3;
        private float _punctuationPause = 1;

        protected override void Init() 
        {
            if(_hasStartSubtitle)
            {
                NewSubtitle(AuthorType.Player, _startSubtitle, 1f);
            }
        }

        public async UniTask NewSubtitle(AuthorType type, string text, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            _text.gameObject.SetActive(true);
            _text.text = GetAuthorName(type);

            await ShowText(text).ContinueWith(() => 
            {
                if(_isEndRemove)
                    _text.gameObject.SetActive(false);
            });
        }

        private bool IsPunctuation(char character)
        {
            return character == '.' || character == '!' || character == '?' || character == ',';
        }

        private async UniTask ShowText(string text)
        {
            for(int i = 0; i < text.Length; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime * _delayForShowText));

                if(i % 2 == 0)
                    _signalBus.Fire<SubtitleStartSignal>();

                _text.text += text[i];

                if (IsPunctuation(text[i]))
                    await UniTask.Delay(TimeSpan.FromSeconds(_punctuationPause));

            }
            _signalBus.Fire<SubtitleEndSignal>();
            await UniTask.Delay(TimeSpan.FromSeconds(_punctuationPause));
        }

        private string GetAuthorName(AuthorType type)
        {
            switch(type)
            {
                case AuthorType.Player:
                    return "Player: ";
                case AuthorType.Client:
                    return "Client: ";
            }

            return string.Empty;    
        }
    }

    public enum AuthorType
    {
        Player,
        Client
    }
    
}

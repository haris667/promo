using Cysharp.Threading.Tasks;
using Infrastructure.MVP;
using Installers;
using System;
using System.Linq;
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

        private bool _isPlaying = false;

        private float _timeForPreviousEnding;

        protected override void Init() 
        {
            if(_hasStartSubtitle)
            {
                Create(AuthorType.Player, _startSubtitle, 1f);
            }
        }

        public async UniTask Create(AuthorType type, string text, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timeForPreviousEnding));
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            _text.gameObject.SetActive(true);
            _text.text = GetAuthorName(type);

            if(!_isPlaying)
            {
                _isPlaying = true;
                await ShowText(text).ContinueWith(() =>
                {
                    if (_isEndRemove)
                        _text.gameObject.SetActive(false);
                });
            }
        }

        private bool IsPunctuation(char character)
        {
            return character == '.' || character == '!' || character == '?' || character == ',';
        }

        private async UniTask ShowText(string text)
        {
            _timeForPreviousEnding += (Time.fixedDeltaTime * _delayForShowText) * _text.text.Length +
                              GetPunctuationSymbolsAmount(text) * _punctuationPause + _punctuationPause;
                              

            for (int i = 0; i < text.Length; i++)
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

            _timeForPreviousEnding = 0;
            _isPlaying = false;
        }

        private int GetPunctuationSymbolsAmount(string text)
        {
            return text.Count(IsPunctuation);
        }

        private string GetAuthorName(AuthorType type)
        {
            switch(type)
            {
                case AuthorType.Player:
                    return "Player: ";
                case AuthorType.Client:
                    return "Client: ";
                case AuthorType.Electrician:
                    return "Electrcian: ";
            }

            return string.Empty;    
        }
    }

    public enum AuthorType
    {
        Player,
        Client,
        Electrician
    }
    
}

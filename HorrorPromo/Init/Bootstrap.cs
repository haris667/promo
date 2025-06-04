using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Init
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private ProjectInstaller _installer;
        [SerializeField]
        private Image _loadingLine;
        [SerializeField] 
        private float _loadingSpeed = 0.5f;

        [Inject]
        private InputMap _inputMap;

        private bool _isLoading = false;
        private float _currentFill = 0f;

        private async void Awake()
        {
            await Init().ContinueWith(() =>
            {
                _installer.Init();
                _installer.Install();
                StartNewScene();
            });
        }

        private async UniTask Init()
        {
            await UniTask.WaitUntil(() => _currentFill >= 1f);
        }

        private async void StartNewScene()
        {
            await SceneManager.LoadSceneAsync("Core");
        }

        private void StartLoading()
        {
            _isLoading = true;
            LoadingProgress().Forget();
        }

        private void StopLoading()
        {
            _isLoading = false;
            _currentFill = 0f;
            _loadingLine.fillAmount = 0f;
        }

        private async UniTaskVoid LoadingProgress()
        {
            while (_isLoading && _currentFill < 1f)
            {
                _currentFill += Time.deltaTime * _loadingSpeed;
                _loadingLine.fillAmount = _currentFill;
                await UniTask.Yield();
            }

            // Автоматический сброс после заполнения
            if (_currentFill >= 1f)
            {
                _currentFill = 0f;
                _loadingLine.fillAmount = 0f;
            }
        }

        private void OnEnable()
        {
            _inputMap.Enable();
            _inputMap.Player.Interaction.started += context => StartLoading();
            _inputMap.Player.Interaction.canceled += context => StopLoading();
        }

        private void OnDisable()
        {
            _inputMap.Disable();
            _inputMap.Player.Interaction.started -= context => StartLoading();
            _inputMap.Player.Interaction.canceled -= context => StopLoading();
        }
    }
}

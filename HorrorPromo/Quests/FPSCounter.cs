using TMPro;
using UnityEngine;

namespace Quests
{
    [RequireComponent(typeof(TMP_Text))]
    public class FPSCounter : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("How often to update the FPS text (in seconds)")]
        [SerializeField] private float _updateInterval = 0.5f;

        [Tooltip("Color for good FPS (>=60)")]
        [SerializeField] private Color _goodFpsColor = Color.green;

        [Tooltip("Color for average FPS (>=30)")]
        [SerializeField] private Color _averageFpsColor = Color.yellow;

        [Tooltip("Color for bad FPS (<30)")]
        [SerializeField] private Color _badFpsColor = Color.red;

        [Header("Advanced")]
        [Tooltip("Show milliseconds instead of FPS")]
        [SerializeField] private bool _showMilliseconds = false;

        [Tooltip("Enable VSync for testing")]
        [SerializeField] private bool _vSyncForTesting = false;

        private TMP_Text _fpsText;
        private float _accumulatedFrames = 0;
        private float _timeLeft;
        private int _currentFrames;
        private float _currentFps;

        private void Awake()
        {
            _fpsText = GetComponent<TMP_Text>();
            _timeLeft = _updateInterval;

            if (_vSyncForTesting)
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
            }
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            _accumulatedFrames += Time.timeScale / Time.deltaTime;
            _currentFrames++;

            if (_timeLeft <= 0f)
            {
                _currentFps = _accumulatedFrames / _currentFrames;

                if (_showMilliseconds)
                {
                    float frameTime = 1000f / _currentFps;
                    _fpsText.text = string.Format("{0:0.0} ms", frameTime);
                }
                else
                {
                    _fpsText.text = string.Format("{0:0} FPS", _currentFps);
                }

                // Change color based on FPS
                if (_currentFps >= 60)
                    _fpsText.color = _goodFpsColor;
                else if (_currentFps >= 30)
                    _fpsText.color = _averageFpsColor;
                else
                    _fpsText.color = _badFpsColor;

                _timeLeft = _updateInterval;
                _accumulatedFrames = 0f;
                _currentFrames = 0;
            }
        }

        // Call this to get current FPS from other scripts
        public float GetCurrentFPS() => _currentFps;

        // Call this to toggle milliseconds display
        public void ToggleMillisecondsDisplay(bool showMs)
        {
            _showMilliseconds = showMs;
        }
    }
}



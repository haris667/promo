using Installers;
using UnityEngine;
using Zenject;

namespace Sound
{
    //не люблю работать со звуком, попросил аи сгенерировать
    public class AudioPlayerManager : MonoBehaviour
    {
        [System.Serializable]
        public class SurfaceSound
        {
            public string surfaceTag;
            public AudioClip[] footstepSounds;
            public AudioClip[] jumpSounds;
            public AudioClip[] landSounds;
            [Range(0f, 1f)] public float volumeMultiplier = 1f;
            public float pitchVariation = 0.1f;
        }

        [Header("Settings")]
        [SerializeField] private float _footstepIntervalWalk = 0.5f;
        [SerializeField] private float _footstepIntervalRun = 0.3f;
        [SerializeField] private float _velocityThreshold = 0.1f;
        [SerializeField] private float _minLandingVelocity = 3f;
        [SerializeField] private LayerMask _groundCheckMask = -1;
        [SerializeField] private bool _hasPlayer = true;

        [Header("Surface Sounds")]
        [SerializeField] private AudioClip _zoomSound;
        [SerializeField] private AudioClip _subtitleSound;
        [SerializeField] private SurfaceSound _defaultSurface;
        [SerializeField] private SurfaceSound[] _surfaceSounds;

        [SerializeField] 
        private AudioSource _baseAudioSource;
        [SerializeField]
        private AudioSource _subtitleAudioSource;
        private FirstPersonController _characterController;
        private bool _isGrounded;
        private float _lastFootstepTime;
        private float _currentFootstepInterval;
        private SurfaceSound _currentSurface;
        private RaycastHit _groundHit;
        private bool _isInventoryOpen;

        [Inject]
        private InputMap _inputMap;

        [Inject]
        private SignalBus _signalBus;

        private void Awake()
        {
            _characterController = GetComponent<FirstPersonController>();
            _currentSurface = _defaultSurface;
            _currentFootstepInterval = _footstepIntervalWalk;
        }

        private void Start()
        {
            _subtitleAudioSource.dopplerLevel = 0;

            if(_hasPlayer)
            {
                _inputMap.Player.OpenInventory.performed += context => _isInventoryOpen = !_isInventoryOpen;
                _inputMap.Player.Aim.performed += context => _baseAudioSource.PlayOneShot(_zoomSound);
                _inputMap.Player.Aim.canceled += context => _baseAudioSource.PlayOneShot(_zoomSound);
            }

            _signalBus.Subscribe<ItemUsedSignal>(PlayOneShotUsed);
            _signalBus.Subscribe<ItemAddedToInventorySignal>(PlayOneShotAdded);

            _signalBus.Subscribe<SubtitleStartSignal>(PlaySubtitles);
            _signalBus.Subscribe<SubtitleEndSignal>(StopSubtitles);
        }

        private void PlaySubtitles(SubtitleStartSignal signal)
        {
            _subtitleAudioSource.PlayOneShot(_subtitleSound);
        }

        private void StopSubtitles(SubtitleEndSignal signal)
        {
            //_subtitleAudioSource.Stop();
        }

        private void PlayOneShotUsed(ItemUsedSignal signal)
        {
            if (signal.item.clipInteraction == null)
                return;

            _baseAudioSource.PlayOneShot(signal.item.clipInteraction);
        }

        private void PlayOneShotAdded(ItemAddedToInventorySignal signal)
        {
            if (signal.item.clipInteraction == null)
                return;

            _baseAudioSource.PlayOneShot(signal.item.clipInteraction);
        }

        private void Update()
        {
            HandleFootsteps();
        }

        private void UpdateCurrentSurface()
        {
            if (_groundHit.collider == null)
            {
                _currentSurface = _defaultSurface;
                return;
            }

            string surfaceTag = _groundHit.collider.tag;
            foreach (var surface in _surfaceSounds)
            {
                if (surface.surfaceTag == surfaceTag)
                {
                    _currentSurface = surface;
                    return;
                }
            }

            _currentSurface = _defaultSurface;
        }

        private void HandleFootsteps()
        {
            if (!_hasPlayer || !_characterController.isWalking || _isInventoryOpen)
                return;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            _currentFootstepInterval = isRunning ? _footstepIntervalRun : _footstepIntervalWalk;

            if (Time.time - _lastFootstepTime > _currentFootstepInterval)
            {
                PlayFootstepSound();
                _lastFootstepTime = Time.time;
            }
        }

        private void PlayFootstepSound()
        {
            if (_currentSurface.footstepSounds.Length == 0)
                return;

            AudioClip clip = GetRandomClip(_currentSurface.footstepSounds);
            PlaySound(clip, _currentSurface.volumeMultiplier, _currentSurface.pitchVariation);
        }

        public void PlayJumpSound()
        {
            if (_currentSurface.jumpSounds.Length == 0)
                return;

            AudioClip clip = GetRandomClip(_currentSurface.jumpSounds);
            PlaySound(clip, _currentSurface.volumeMultiplier, _currentSurface.pitchVariation);
        }

        public void PlayLandSound(float landingVelocity)
        {
            if (landingVelocity < _minLandingVelocity || _currentSurface.landSounds.Length == 0)
                return;

            float volume = Mathf.InverseLerp(_minLandingVelocity, 10f, landingVelocity);
            AudioClip clip = GetRandomClip(_currentSurface.landSounds);
            PlaySound(clip, volume * _currentSurface.volumeMultiplier, _currentSurface.pitchVariation);
        }

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
        }

        private void PlaySound(AudioClip clip, float volume, float pitchVariation)
        {
            if (clip == null) return;

            _baseAudioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
            _baseAudioSource.PlayOneShot(clip, volume);
        }

        // Для интеграции с системой передвижения
        public void OnJump() => PlayJumpSound();

        public void OnLand(float velocity) => PlayLandSound(velocity);
    }

}

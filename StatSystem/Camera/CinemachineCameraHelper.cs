using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Нужен для работы с камерой. Всякий доп. функционал для нее.
/// Например тряска по времени. Тип тряски нужно выбрать в инспекторе и эту камеру просунуть сюда в конструктор
/// </summary>
public class CinemachineCameraHelper
{
    private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _cameraNoise;

    /// <summary>
    /// Тут получать элементы из камеры.
    /// Например - CinemachineBasicMultiChannelPerlin. Таким образом и прочие, которые будут нужны
    /// ВАЖНО - на камере должны быть активны модификаторы, которые мы хоти получить
    /// </summary>
    public CinemachineCameraHelper(CinemachineVirtualCamera camera)
    {
        _camera = camera;
        _cameraNoise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    /// <summary>
    /// Для эффекта тряски камеры.
    /// ВАЖНО - тип тряски задан в инспекторе. (я не смог найти как из кода задавать свой тип, 
    /// если найду, отредачу)
    /// </summary>
    public async void AddNoiseAsync(float intensity, float time)
    {
        while (time > 0)
        {
            await Task.Run(() => Task.Delay(2)); // = ~0.02f // юнитаск бы сюда эххх
            time -= Time.fixedDeltaTime;

            _cameraNoise.m_AmplitudeGain = intensity;
            _cameraNoise.m_FrequencyGain = intensity;
        }
        _cameraNoise.m_AmplitudeGain = 0;
        _cameraNoise.m_FrequencyGain = 0;
    }
}
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

/// <summary>
/// ����� ��� ������ � �������. ������ ���. ���������� ��� ���.
/// �������� ������ �� �������. ��� ������ ����� ������� � ���������� � ��� ������ ��������� ���� � �����������
/// </summary>
public class CinemachineCameraHelper
{
    private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _cameraNoise;

    /// <summary>
    /// ��� �������� �������� �� ������.
    /// �������� - CinemachineBasicMultiChannelPerlin. ����� ������� � ������, ������� ����� �����
    /// ����� - �� ������ ������ ���� ������� ������������, ������� �� ���� ��������
    /// </summary>
    public CinemachineCameraHelper(CinemachineVirtualCamera camera)
    {
        _camera = camera;
        _cameraNoise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    /// <summary>
    /// ��� ������� ������ ������.
    /// ����� - ��� ������ ����� � ����������. (� �� ���� ����� ��� �� ���� �������� ���� ���, 
    /// ���� �����, ��������)
    /// </summary>
    public async void AddNoiseAsync(float intensity, float time)
    {
        while (time > 0)
        {
            await Task.Run(() => Task.Delay(2)); // = ~0.02f // ������� �� ���� ����
            time -= Time.fixedDeltaTime;

            _cameraNoise.m_AmplitudeGain = intensity;
            _cameraNoise.m_FrequencyGain = intensity;
        }
        _cameraNoise.m_AmplitudeGain = 0;
        _cameraNoise.m_FrequencyGain = 0;
    }
}
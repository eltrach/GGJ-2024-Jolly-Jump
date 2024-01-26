using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public CinemachineVirtualCamera _aimVirtualCamera;
    public float shakeDuration = 0.2f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;

    CinemachineBrain cnBrain;
    private void Start()
    {
        cnBrain = Camera.main.GetComponent<CinemachineBrain>();
        //GetCurrentActiveCamera();
    }
    [Button(" StartShake() ")]
    public void StartShake()
    {
        // Shake the virtual camera
       // GetCurrentActiveCamera();
        CinemachineBasicMultiChannelPerlin noise = GetCurrentActiveCamera().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = shakeAmplitude;
        noise.m_FrequencyGain = shakeFrequency;
        Invoke("StopShaking", shakeDuration);
    }
    void StopShaking()
    {
        // Stop the camera shake
        CinemachineBasicMultiChannelPerlin noise = GetCurrentActiveCamera().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
    public void CustomCameraShake(float _shakeAmplitude, float _shakeFrequency)
    {
        // Shake the virtual camera
        // GetCurrentActiveCamera();
        CinemachineBasicMultiChannelPerlin noise = GetCurrentActiveCamera().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = _shakeAmplitude;
        noise.m_FrequencyGain = _shakeFrequency;
        Invoke("StopShaking", shakeDuration);
    }
    CinemachineVirtualCamera GetCurrentActiveCamera()
    {
        _aimVirtualCamera = (CinemachineVirtualCamera)cnBrain.ActiveVirtualCamera;
        return _aimVirtualCamera;
    }
}

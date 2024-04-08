using Cinemachine;
using UnityEngine;
public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get; private set; }
    [SerializeField]private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    private void Awake()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        gameObject.SetActive(false);
    }
    public  void ShakeCamera(float itensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        startingIntensity = itensity;
        shakeTimerTotal = time;
        shakeTimer = time;
        if (cinemachineBasicMultiChannelPerlin == null) Debug.LogError($"cinemachineBasicMultiChannelPerlin");
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = itensity;
    }
    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                //time over 
                Debug.LogWarning("Time Over");
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
               cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                 Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }
}

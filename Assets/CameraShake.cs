using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        StartCoroutine(Shake());

        IEnumerator Shake()
        {
            noise.m_AmplitudeGain = amplitudeGain;
            noise.m_FrequencyGain = frequencyGain;
            yield return new WaitForSecondsRealtime(.1f);
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;

        }
    }
}

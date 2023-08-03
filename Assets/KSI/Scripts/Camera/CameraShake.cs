using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance { get; private set; }
	private CinemachineVirtualCamera cam;

	private float shakeTimer;

	private void Awake()
	{
		Instance = this;
		cam = GetComponent<CinemachineVirtualCamera>();
	}

	public void ShakeCamera(float intensity, float time)
	{
		CinemachineBasicMultiChannelPerlin bmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		bmcp.m_AmplitudeGain = intensity;
		bmcp.m_FrequencyGain = intensity;
		shakeTimer = time;
	}

	private void Update()
	{
		if (shakeTimer > 0)
		{
			shakeTimer -= Time.deltaTime;
			if (shakeTimer <= 0f)
			{
				CinemachineBasicMultiChannelPerlin bmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

				bmcp.m_AmplitudeGain = 0f;
				bmcp.m_FrequencyGain = 0f;
			}
		}
	}
}

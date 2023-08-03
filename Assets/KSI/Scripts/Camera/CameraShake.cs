using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance { get; private set; }
	private CinemachineVirtualCamera camera;

	private float shakeTimer;

	private void Awake()
	{
		Instance = this;
		camera = GetComponent<CinemachineVirtualCamera>();
	}

	public void ShakeCamera(float intensity, float time)
	{
		CinemachineBasicMultiChannelPerlin bmcp = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
				CinemachineBasicMultiChannelPerlin bmcp = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

				bmcp.m_AmplitudeGain = 0f;
				bmcp.m_FrequencyGain = 0f;
			}
		}
	}
}

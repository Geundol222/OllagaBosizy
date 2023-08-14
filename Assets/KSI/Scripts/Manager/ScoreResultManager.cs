using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private Slider sliderBoy;
	[SerializeField] private Slider sliderGirl;

	private float calculateAverage;
	public float Average { get { return (sliderBoy.value + sliderGirl.value) / 2; } }

	private void Start()
	{
		sliderBoy.onValueChanged.AddListener(OnSliderValueChanged);
		sliderGirl.onValueChanged.AddListener(OnSliderValueChanged);
	}

	private void OnSliderValueChanged(float value)
	{
		// 슬라이더 값이 변경될 때마다 호출됨
		calculateAverage = Average + value;
		Debug.Log("Average : " + calculateAverage);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUITest : MonoBehaviour
{
	[SerializeField] Slider slider;

	private Animator animater;

	private void Awake()
	{
		animater = GetComponent<Animator>();
	}

	public void FadeIn()
	{
		animater.SetBool("IsActive", true);
	}

	public void FadeOut()
	{
		animater.SetBool("IsActive", false);
	}

	public void SetProgress(float progress)
	{
		slider.value = progress;
	}
}

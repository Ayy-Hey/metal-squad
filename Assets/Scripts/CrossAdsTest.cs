using System;
using CrossAdPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossAdsTest : MonoBehaviour
{
	private void Start()
	{
		Singleton<CrossAdsManager>.Instance.Preload("http://bigmath.vn/CrossAdsConfigAndroid_metalsquad.txt", "http://bigmath.vn/CrossAdsConfigAndroid_metalsquad.txt", null);
		base.Invoke("TestShowFloatAd", 5f);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			this.ishiding = !this.ishiding;
			if (this.ishiding)
			{
				Singleton<CrossAdsManager>.Instance.ShowFloatAds();
			}
			else
			{
				Singleton<CrossAdsManager>.Instance.HideFloatAds();
			}
		}
	}

	private void TestShowFloatAd()
	{
		this.ishiding = false;
		Singleton<CrossAdsManager>.Instance.ShowFloatAds();
	}

	private void TestHideFloatAd()
	{
		Singleton<CrossAdsManager>.Instance.HideFloatAds();
	}

	private void TestShowAdList()
	{
		Singleton<CrossAdsManager>.Instance.ShowAdList();
	}

	private void TestHideAdList()
	{
		Singleton<CrossAdsManager>.Instance.HideAdList();
	}

	private void LoadNextSceneWithoutCanvas()
	{
		SceneManager.LoadScene("CrossAdTest3");
	}

	private bool ishiding;
}

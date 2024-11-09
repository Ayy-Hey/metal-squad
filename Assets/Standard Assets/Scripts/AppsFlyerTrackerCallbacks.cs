using System;
using UnityEngine;

public class AppsFlyerTrackerCallbacks : MonoBehaviour
{
	private void Start()
	{
		this.printCallback("AppsFlyerTrackerCallbacks on Start");
	}

	private void Update()
	{
	}

	public void didReceiveConversionData(string conversionData)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got conversion data = " + conversionData);
	}

	public void didReceiveConversionDataWithError(string error)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got conversion data error = " + error);
	}

	public void didFinishValidateReceipt(string validateResult)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got didFinishValidateReceipt  = " + validateResult);
	}

	public void didFinishValidateReceiptWithError(string error)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got idFinishValidateReceiptWithError error = " + error);
	}

	public void onAppOpenAttribution(string validateResult)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got onAppOpenAttribution  = " + validateResult);
	}

	public void onAppOpenAttributionFailure(string error)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got onAppOpenAttributionFailure error = " + error);
	}

	public void onInAppBillingSuccess()
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got onInAppBillingSuccess succcess");
	}

	public void onInAppBillingFailure(string error)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: got onInAppBillingFailure error = " + error);
	}

	public void onInviteLinkGenerated(string link)
	{
		this.printCallback("AppsFlyerTrackerCallbacks:: generated userInviteLink " + link);
	}

	public void onOpenStoreLinkGenerated(string link)
	{
		this.printCallback("onOpenStoreLinkGenerated:: generated store link " + link);
		Application.OpenURL(link);
	}

	private void printCallback(string str)
	{
		UnityEngine.Debug.Log(str);
	}
}

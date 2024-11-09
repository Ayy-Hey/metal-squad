using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
	public static void setCurrencyCode(string currencyCode)
	{
		AppsFlyer.cls_AppsFlyer.Call("setCurrencyCode", new object[]
		{
			currencyCode
		});
	}

	public static void setCustomerUserID(string customerUserID)
	{
		AppsFlyer.cls_AppsFlyer.Call("setAppUserId", new object[]
		{
			customerUserID
		});
	}

	public static void loadConversionData(string callbackObject)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyerHelper.CallStatic("createConversionDataListener", new object[]
				{
					@static,
					callbackObject
				});
			}
		}
	}

	[Obsolete("Use loadConversionData(string callbackObject)")]
	public static void loadConversionData(string callbackObject, string callbackMethod, string callbackFailedMethod)
	{
		AppsFlyer.loadConversionData(callbackObject);
	}

	public static void setCollectIMEI(bool shouldCollect)
	{
		AppsFlyer.cls_AppsFlyer.Call("setCollectIMEI", new object[]
		{
			shouldCollect
		});
	}

	public static void setCollectAndroidID(bool shouldCollect)
	{
		MonoBehaviour.print("AF.cs setCollectAndroidID");
		AppsFlyer.cls_AppsFlyer.Call("setCollectAndroidID", new object[]
		{
			shouldCollect
		});
	}

	public static void init(string key, string callbackObject)
	{
		AppsFlyer.init(key);
		if (callbackObject != null)
		{
			AppsFlyer.loadConversionData(callbackObject);
		}
	}

	public static void init(string key)
	{
		MonoBehaviour.print("AF.cs init");
		AppsFlyer.devKey = key;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				@static.Call("runOnUiThread", new object[]
				{
					new AndroidJavaRunnable(AppsFlyer.init_cb)
				});
			}
		}
	}

	private static void init_cb()
	{
		MonoBehaviour.print("AF.cs start tracking");
		AppsFlyer.trackAppLaunch();
	}

	public static void setAppsFlyerKey(string key)
	{
		MonoBehaviour.print("AF.cs setAppsFlyerKey");
	}

	public static void trackAppLaunch()
	{
		MonoBehaviour.print("AF.cs trackAppLaunch");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
				AppsFlyer.cls_AppsFlyer.Call("startTracking", new object[]
				{
					androidJavaObject,
					AppsFlyer.devKey
				});
				AppsFlyer.cls_AppsFlyer.Call("trackAppLaunch", new object[]
				{
					@static,
					AppsFlyer.devKey
				});
			}
		}
	}

	public static void setAppID(string packageName)
	{
		AppsFlyer.cls_AppsFlyer.Call("setAppId", new object[]
		{
			packageName
		});
	}

	public static void createValidateInAppListener(string aObject, string callbackMethod, string callbackFailedMethod)
	{
		MonoBehaviour.print("AF.cs createValidateInAppListener called");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyerHelper.CallStatic("createValidateInAppListener", new object[]
				{
					@static,
					aObject,
					callbackMethod,
					callbackFailedMethod
				});
			}
		}
	}

	public static void validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency, Dictionary<string, string> extraParams)
	{
		MonoBehaviour.print(string.Concat(new string[]
		{
			"AF.cs validateReceipt pk = ",
			publicKey,
			" data = ",
			purchaseData,
			"sig = ",
			signature
		}));
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = null;
				if (extraParams != null)
				{
					androidJavaObject = AppsFlyer.ConvertHashMap(extraParams);
				}
				MonoBehaviour.print("inside cls_activity");
				AppsFlyer.cls_AppsFlyer.Call("validateAndTrackInAppPurchase", new object[]
				{
					@static,
					publicKey,
					signature,
					purchaseData,
					price,
					currency,
					androidJavaObject
				});
			}
		}
	}

	public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = AppsFlyer.ConvertHashMap(eventValues);
				AppsFlyer.cls_AppsFlyer.Call("trackEvent", new object[]
				{
					@static,
					eventName,
					androidJavaObject
				});
			}
		}
	}

	private static AndroidJavaObject ConvertHashMap(Dictionary<string, string> dict)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		if (dict == null)
		{
			return null;
		}
		object[] array = new object[2];
		foreach (KeyValuePair<string, string> keyValuePair in dict)
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
			{
				keyValuePair.Key
			}))
			{
				using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
				{
					keyValuePair.Value
				}))
				{
					array[0] = androidJavaObject2;
					array[1] = androidJavaObject3;
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
		}
		return androidJavaObject;
	}

	public static void setImeiData(string imeiData)
	{
		MonoBehaviour.print("AF.cs setImeiData");
		AppsFlyer.cls_AppsFlyer.Call("setImeiData", new object[]
		{
			imeiData
		});
	}

	public static void setAndroidIdData(string androidIdData)
	{
		MonoBehaviour.print("AF.cs setImeiData");
		AppsFlyer.cls_AppsFlyer.Call("setAndroidIdData", new object[]
		{
			androidIdData
		});
	}

	public static void setIsDebug(bool isDebug)
	{
		MonoBehaviour.print("AF.cs setDebugLog");
		AppsFlyer.cls_AppsFlyer.Call("setDebugLog", new object[]
		{
			isDebug
		});
	}

	public static void setIsSandbox(bool isSandbox)
	{
	}

	public static void getConversionData()
	{
	}

	public static void handleOpenUrl(string url, string sourceApplication, string annotation)
	{
	}

	public static string getAppsFlyerId()
	{
		string result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				result = AppsFlyer.cls_AppsFlyer.Call<string>("getAppsFlyerUID", new object[]
				{
					@static
				});
			}
		}
		return result;
	}

	public static void setGCMProjectNumber(string googleGCMNumber)
	{
		AppsFlyer.cls_AppsFlyer.Call("setGCMProjectNumber", new object[]
		{
			googleGCMNumber
		});
	}

	public static void updateServerUninstallToken(string token)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				androidJavaObject.Call("updateServerUninstallToken", new object[]
				{
					@static,
					token
				});
			}
		}
	}

	public static void enableUninstallTracking(string senderId)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		androidJavaObject.Call("enableUninstallTracking", new object[]
		{
			senderId
		});
	}

	public static void setDeviceTrackingDisabled(bool state)
	{
		AppsFlyer.cls_AppsFlyer.Call("setDeviceTrackingDisabled", new object[]
		{
			state
		});
	}

	public static void setAdditionalData(Dictionary<string, string> extraData)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = AppsFlyer.ConvertHashMap(extraData);
				AppsFlyer.cls_AppsFlyer.Call("setAdditionalData", new object[]
				{
					androidJavaObject
				});
			}
		}
	}

	public static void stopTracking(bool isStopTracking)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
				AppsFlyer.cls_AppsFlyer.Call("stopTracking", new object[]
				{
					isStopTracking,
					androidJavaObject
				});
			}
		}
	}

	public static void setAppInviteOneLinkID(string oneLinkID)
	{
		AppsFlyer.cls_AppsFlyer.Call("setAppInviteOneLink", new object[]
		{
			oneLinkID
		});
	}

	public static void generateUserInviteLink(Dictionary<string, string> parameters, string callbackObject, string callbackMethod, string callbackFailedMethod)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
				AndroidJavaObject androidJavaObject2 = AppsFlyer.ConvertHashMap(parameters);
				AppsFlyer.ShareHelperInstance.Call("createOneLinkInviteListener", new object[]
				{
					androidJavaObject,
					androidJavaObject2,
					callbackObject,
					callbackMethod,
					callbackFailedMethod
				});
			}
		}
	}

	public static void trackCrossPromoteImpression(string appId, string campaign)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
				AppsFlyer.cls_AndroidShare.CallStatic("trackCrossPromoteImpression", new object[]
				{
					androidJavaObject,
					appId,
					campaign
				});
			}
		}
	}

	public static void trackAndOpenStore(string promotedAppId, string campaign, Dictionary<string, string> customParams)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
				AndroidJavaObject androidJavaObject2 = null;
				if (customParams != null)
				{
					androidJavaObject2 = AppsFlyer.ConvertHashMap(customParams);
				}
				AppsFlyer.ShareHelperInstance.Call("trackAndOpenStore", new object[]
				{
					androidJavaObject,
					promotedAppId,
					campaign,
					androidJavaObject2
				});
			}
		}
	}

	public static void setPreinstallAttribution(string mediaSource, string campaign, string siteId)
	{
		AppsFlyer.cls_AppsFlyer.Call("setPreinstallAttribution", new object[]
		{
			mediaSource,
			campaign,
			siteId
		});
	}

	public static void setMinTimeBetweenSessions(int seconds)
	{
		AppsFlyer.cls_AppsFlyer.Call("setMinTimeBetweenSessions", new object[]
		{
			seconds
		});
	}

	public static void setHost(string hostPrefixName, string hostName)
	{
		AppsFlyer.cls_AppsFlyer.Call("setHost", new object[]
		{
			hostPrefixName,
			hostName
		});
	}

	public static string getHost()
	{
		return AppsFlyer.cls_AppsFlyer.Call<string>("getHost", new object[0]);
	}

	private static AndroidJavaClass obj = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");

	private static AndroidJavaObject cls_AppsFlyer = AppsFlyer.obj.CallStatic<AndroidJavaObject>("getInstance", new object[0]);

	private static AndroidJavaClass propertiesClass = new AndroidJavaClass("com.appsflyer.AppsFlyerProperties");

	private static AndroidJavaClass cls_AppsFlyerHelper = new AndroidJavaClass("com.appsflyer.AppsFlyerUnityHelper");

	private static string devKey;

	private static AndroidJavaClass cls_UnityShareHelper = new AndroidJavaClass("com.appsflyer.UnityShareHelper");

	private static AndroidJavaObject ShareHelperInstance = AppsFlyer.cls_UnityShareHelper.CallStatic<AndroidJavaObject>("getInstance", new object[0]);

	private static AndroidJavaClass cls_AndroidShare = new AndroidJavaClass("com.appsflyer.share.CrossPromotionHelper");
}

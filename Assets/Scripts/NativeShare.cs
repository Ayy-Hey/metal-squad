using System;
using UnityEngine;

public static class NativeShare
{
	public static void Share(string body, string filePath = null, string url = null, string subject = "", string mimeType = "text/html", bool chooser = false, string chooserText = "Select sharing app")
	{
		NativeShare.ShareMultiple(body, new string[]
		{
			filePath
		}, url, subject, mimeType, chooser, chooserText);
	}

	public static void ShareMultiple(string body, string[] filePaths = null, string url = null, string subject = "", string mimeType = "text/html", bool chooser = false, string chooserText = "Select sharing app")
	{
		NativeShare.ShareAndroid(body, subject, url, filePaths, mimeType, chooser, chooserText);
	}

	public static void ShareAndroid(string body, string subject, string url, string[] filePaths, string mimeType, bool chooser, string chooserText)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent"))
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[0]))
			{
				using (androidJavaObject.Call<AndroidJavaObject>("setAction", new object[]
				{
					androidJavaClass.GetStatic<string>("ACTION_SEND")
				}))
				{
				}
				using (androidJavaObject.Call<AndroidJavaObject>("setType", new object[]
				{
					mimeType
				}))
				{
				}
				using (androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[]
				{
					androidJavaClass.GetStatic<string>("EXTRA_SUBJECT"),
					subject
				}))
				{
				}
				using (androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[]
				{
					androidJavaClass.GetStatic<string>("EXTRA_TEXT"),
					body
				}))
				{
				}
				if (!string.IsNullOrEmpty(url))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri"))
					{
						using (AndroidJavaObject androidJavaObject6 = androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[]
						{
							url
						}))
						{
							using (androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[]
							{
								androidJavaClass.GetStatic<string>("EXTRA_STREAM"),
								androidJavaObject6
							}))
							{
							}
						}
					}
				}
				else if (filePaths != null)
				{
					using (AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("android.net.Uri"))
					{
						using (AndroidJavaObject androidJavaObject8 = new AndroidJavaObject("java.util.ArrayList", new object[0]))
						{
							for (int i = 0; i < filePaths.Length; i++)
							{
								using (AndroidJavaObject androidJavaObject9 = androidJavaClass3.CallStatic<AndroidJavaObject>("parse", new object[]
								{
									"file://" + filePaths[i]
								}))
								{
									androidJavaObject8.Call<bool>("add", new object[]
									{
										androidJavaObject9
									});
								}
							}
							using (androidJavaObject.Call<AndroidJavaObject>("putParcelableArrayListExtra", new object[]
							{
								androidJavaClass.GetStatic<string>("EXTRA_STREAM"),
								androidJavaObject8
							}))
							{
							}
						}
					}
				}
				using (AndroidJavaClass androidJavaClass4 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass4.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						if (chooser)
						{
							AndroidJavaObject androidJavaObject11 = androidJavaClass.CallStatic<AndroidJavaObject>("createChooser", new object[]
							{
								androidJavaObject,
								chooserText
							});
							@static.Call("startActivity", new object[]
							{
								androidJavaObject11
							});
						}
						else
						{
							@static.Call("startActivity", new object[]
							{
								androidJavaObject
							});
						}
					}
				}
			}
		}
	}
}

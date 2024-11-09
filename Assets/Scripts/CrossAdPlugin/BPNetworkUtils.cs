using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JsonFx.Json;
using UnityEngine;

namespace CrossAdPlugin
{
	public class BPNetworkUtils
	{
		public static void SaveImageToResources(string imgPath, byte[] pngByte)
		{
			File.WriteAllBytes(imgPath, pngByte);
		}

		public static Texture2D LoadImageFromResouces(string filePath)
		{
			return (Texture2D)Resources.Load(filePath, typeof(Texture2D));
		}

		public static string HashFile(string filePath)
		{
			string result;
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				result = BPNetworkUtils.HashFile(fileStream);
			}
			return result;
		}

		public static string HashFile(FileStream stream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (stream != null)
			{
				stream.Seek(0L, SeekOrigin.Begin);
				MD5 md = MD5.Create();
				byte[] array = md.ComputeHash(stream);
				foreach (byte b in array)
				{
					stringBuilder.Append(b.ToString("x2"));
				}
				stream.Seek(0L, SeekOrigin.Begin);
			}
			return stringBuilder.ToString();
		}

		public static string HashString(string inputString)
		{
			byte[] bytes = new UTF8Encoding().GetBytes(inputString);
			byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
			return BitConverter.ToString(value).Replace("-", string.Empty).ToLower();
		}

		public static IEnumerator DownloadImage(CrossAdTextureInfo info, Action<CrossAdTextureInfo> callback = null)
		{
			BPDebug.LogMessage("Downloading ... " + info.url, false);
			WWW www = new WWW(info.url);
			while (!www.isDone)
			{
				yield return www;
			}
			Texture2D texture = www.texture;
			if (www.error != null)
			{
				texture = null;
				BPDebug.LogMessage("Downloaded failed: " + info.url + ": error == " + www.error.ToString(), false);
			}
			else
			{
				BPDebug.LogMessage("Downloaded success: " + info.url, false);
			}
			info.SetTexture(texture);
			if (callback != null)
			{
				callback(info);
			}
			yield break;
		}

		public static IEnumerator DownloadTxt<T>(string url, Action<T> successCallback, Action<string> faildCallback)
		{
			WWW www = new WWW(url);
			while (!www.isDone)
			{
				yield return null;
			}
			try
			{
				if (www.text != null)
				{
					try
					{
						T obj = JsonReader.Deserialize<T>(www.text);
						if (successCallback != null)
						{
							successCallback(obj);
						}
					}
					catch (Exception ex)
					{
						if (faildCallback != null)
						{
							faildCallback(ex.Message);
						}
					}
				}
				else if (faildCallback != null)
				{
					faildCallback(www.error);
				}
			}
			catch (Exception ex2)
			{
				if (faildCallback != null)
				{
					faildCallback(ex2.ToString());
				}
			}
			yield break;
		}

		public static IEnumerator CheckInternetConnection(Action<bool> action)
		{
			if (BPNetworkUtils.isChecking)
			{
				yield return new WaitForSeconds(0.1f);
			}
			BPNetworkUtils.isChecking = true;
			WWW www = new WWW("http://google.com");
			yield return www;
			if (www.error != null)
			{
				action(false);
			}
			else
			{
				action(true);
			}
			BPNetworkUtils.isChecking = false;
			yield break;
		}

		public static bool isChecking;
	}
}

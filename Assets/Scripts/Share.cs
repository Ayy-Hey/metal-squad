using System;
using System.Collections;
using System.IO;
using CrossAdPlugin;
using UnityEngine;

public class Share : Singleton<Share>
{
	public void ShareScreenshotWithText(string text)
	{
		string text2 = Application.persistentDataPath + "/" + this.ScreenshotName;
		if (File.Exists(text2))
		{
			File.Delete(text2);
		}
		ScreenCapture.CaptureScreenshot(this.ScreenshotName);
		base.StartCoroutine(this.delayedShare(text2, text));
	}

	private IEnumerator delayedShare(string screenShotPath, string text)
	{
		while (!File.Exists(screenShotPath))
		{
			yield return new WaitForSeconds(0.05f);
		}
		NativeShare.Share(text, screenShotPath, string.Empty, string.Empty, "image/png", true, string.Empty);
		yield break;
	}

	private float width
	{
		get
		{
			return (float)Screen.width;
		}
	}

	private float height
	{
		get
		{
			return (float)Screen.height;
		}
	}

	public void Screenshot()
	{
		base.StartCoroutine(this.GetScreenshot());
	}

	public IEnumerator GetScreenshot()
	{
		yield return new WaitForEndOfFrame();
		Texture2D screenshot = new Texture2D((int)this.width, (int)this.height, TextureFormat.ARGB32, false);
		screenshot.ReadPixels(new Rect(0f, 0f, this.width, this.height), 0, 0, false);
		screenshot.Apply();
		this.Save_Screenshot(screenshot);
		yield break;
	}

	private void Save_Screenshot(Texture2D screenshot)
	{
		string text = string.Concat(new string[]
		{
			Application.persistentDataPath,
			"/",
			DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss"),
			"_",
			this.ScreenshotName
		});
		File.WriteAllBytes(text, screenshot.EncodeToPNG());
		base.StartCoroutine(this.DelayedShare_Image(text));
	}

	public void Clear_SavedScreenShots()
	{
		string persistentDataPath = Application.persistentDataPath;
		DirectoryInfo directoryInfo = new DirectoryInfo(persistentDataPath);
		FileInfo[] files = directoryInfo.GetFiles("*.png");
		foreach (FileInfo fileInfo in files)
		{
			File.Delete(fileInfo.FullName);
		}
	}

	private IEnumerator DelayedShare_Image(string screenShotPath)
	{
		while (!File.Exists(screenShotPath))
		{
			yield return new WaitForSeconds(0.05f);
		}
		this.NativeShare_Image(screenShotPath);
		yield break;
	}

	private void NativeShare_Image(string screenShotPath)
	{
		string body = string.Empty;
		string subject = string.Empty;
		string empty = string.Empty;
		string chooserText = "Select sharing app";
		subject = "Test subject.";
		body = "Test text";
		NativeShare.Share(body, screenShotPath, empty, subject, "image/png", true, chooserText);
	}

	public string ScreenshotName = "screenshot.png";
}

using System;
using System.Collections;
using UnityEngine;

namespace CrossAdPlugin
{
	public class BPUtils
	{
		public static void LoadPrefab<T>(ref T component, string path, bool setActive, bool dontDetroyOnLoad) where T : MonoBehaviour
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load(path));
			string[] array = path.Split(new char[]
			{
				'/'
			}, StringSplitOptions.RemoveEmptyEntries);
			gameObject.name = "[" + array[array.Length - 1] + "]";
			if (dontDetroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			component = gameObject.GetComponent<T>();
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(setActive);
		}

		public static void SaveDate(string key, DateTime utcDate)
		{
			PlayerPrefs.SetInt(key + "sYear", utcDate.Year);
			PlayerPrefs.SetInt(key + "sMonth", utcDate.Month);
			PlayerPrefs.SetInt(key + "sDate", utcDate.Day);
			PlayerPrefs.SetInt(key + "sHour", utcDate.Hour);
			PlayerPrefs.SetInt(key + "sMinute", utcDate.Minute);
			PlayerPrefs.SetInt(key + "sSeconde", utcDate.Second);
		}

		public static DateTime GetDate(string key)
		{
			return new DateTime(PlayerPrefs.GetInt(key + "sYear", 1970), PlayerPrefs.GetInt(key + "sMonth", 1), PlayerPrefs.GetInt(key + "sDate", 1), PlayerPrefs.GetInt(key + "sHour", 0), PlayerPrefs.GetInt(key + "sMinute", 0), PlayerPrefs.GetInt(key + "sSeconde", 0), DateTimeKind.Utc);
		}

		public static string NumberToTimeFormat(int totalTimeInSeconds)
		{
			int num = totalTimeInSeconds / 60;
			int num2 = totalTimeInSeconds - num * 60;
			return ((num >= 10) ? num.ToString() : ("0" + num.ToString())) + " : " + ((num2 >= 10) ? num2.ToString() : ("0" + num2.ToString()));
		}

		private static IEnumerator Load(string scene, bool asyn)
		{
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
			Handheld.StartActivityIndicator();
			yield return new WaitForSeconds(0f);
			if (asyn)
			{
				Application.LoadLevelAsync(scene);
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
			}
			yield break;
		}

		public static void LoadLevelWithIndicator(string scene, bool asyn)
		{
			GameObject gameObject = new GameObject();
			MonoBehaviour monoBehaviour = gameObject.AddComponent<MonoBehaviour>();
			monoBehaviour.StartCoroutine(BPUtils.Load(scene, asyn));
		}

		public static class DateUtils
		{
			public static long CurrentTimeMilliseconds
			{
				get
				{
					return (long)(DateTime.UtcNow - BPUtils.DateUtils.Jan1St1970).TotalMilliseconds;
				}
			}

			public static long DateTimeToMillis(DateTime UtcDateTime)
			{
				return (long)(UtcDateTime - BPUtils.DateUtils.Jan1St1970).TotalMilliseconds;
			}

			public static readonly long DayInMiliSeconds = 86400000L;

			public static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}
	}
}

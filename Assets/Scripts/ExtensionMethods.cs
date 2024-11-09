using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public static class ExtensionMethods
{
	public static void PlayFromStart(this UITweener twn, bool resetAfter = false)
	{
		if (!resetAfter)
		{
			twn.ResetToBeginning();
		}
		twn.PlayForward();
		if (resetAfter)
		{
			twn.ResetToBeginning();
		}
	}

	public static string[] ToArrayString(this JsonData @this)
	{
		if (!@this.IsArray)
		{
			return null;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < @this.Count; i++)
		{
			list.Add(@this[i].ToString());
		}
		return list.ToArray();
	}

	public static float[] ToArrayFloat(this JsonData @this)
	{
		if (!@this.IsArray)
		{
			return null;
		}
		List<float> list = new List<float>();
		for (int i = 0; i < @this.Count; i++)
		{
			list.Add(@this[i].ToFloat());
		}
		return list.ToArray();
	}

	public static int[] ToArrayInt(this JsonData @this)
	{
		if (!@this.IsArray)
		{
			return null;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < @this.Count; i++)
		{
			list.Add(@this[i].ToInt());
		}
		return list.ToArray();
	}

	public static bool ToBool(this JsonData @this)
	{
		return bool.Parse(@this.ToString());
	}

	public static int ToInt(this JsonData @this)
	{
		return int.Parse(@this.ToString());
	}

	public static long ToLong(this JsonData @this)
	{
		return long.Parse(@this.ToString());
	}

	public static float ToFloat(this JsonData @this)
	{
		return float.Parse(@this.ToString());
	}

	public static string GetKeyByIndex(this JsonData jsonData, int index)
	{
		IEnumerable<string> collection = ((IDictionary)jsonData).Keys as IEnumerable<string>;
		List<string> list = new List<string>(collection);
		if (index > list.Count - 1)
		{
			return null;
		}
		return list[index];
	}

	public static JsonData Shuffle(this JsonData jsonData, bool apply = false)
	{
		if (jsonData == null || !jsonData.IsArray)
		{
			return jsonData;
		}
		if (!apply)
		{
			JsonData jsonData2 = jsonData;
			jsonData = new JsonData();
			for (int i = 0; i < jsonData2.Count; i++)
			{
				jsonData.Add(jsonData2[i]);
			}
		}
		Random random = new Random();
		int j = jsonData.Count;
		while (j > 1)
		{
			j--;
			int index = random.Next(j + 1);
			JsonData value = jsonData[index];
			jsonData[index] = jsonData[j];
			jsonData[j] = value;
		}
		return jsonData;
	}

	public static List<JsonData> ToList(this JsonData jsonData)
	{
		if (jsonData == null || !jsonData.IsArray)
		{
			return null;
		}
		List<JsonData> list = new List<JsonData>();
		for (int i = 0; i < jsonData.Count; i++)
		{
			list.Add(jsonData[i]);
		}
		return list;
	}

	public static bool ContainsKey(this JsonData data, string key)
	{
		bool result = false;
		if (data == null)
		{
			return result;
		}
		if (!data.IsObject)
		{
			return result;
		}
		if (data == null)
		{
			return result;
		}
		if (((IDictionary)data).Contains(key))
		{
			result = true;
		}
		return result;
	}

	public static int GetIndexByKey(this JsonData data, string key)
	{
		IEnumerable<string> collection = ((IDictionary)data).Keys as IEnumerable<string>;
		List<string> list = new List<string>(collection);
		return list.IndexOf(key);
	}

	public static int GetObjectIndexWithKeyValue(this JsonData data, string value, string key)
	{
		if (data.IsArray)
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (data[i].IsObject && data[i].ContainsKey(key) && data[i][key].ToString() == value)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public static bool ContainsString(this JsonData data, string value)
	{
		if (data.IsString && data.ToString().Contains(value))
		{
			return true;
		}
		if (data.IsArray)
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (data[i].ToString() == value)
				{
					return true;
				}
			}
		}
		return false;
	}
}

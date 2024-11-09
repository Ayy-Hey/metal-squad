using System;
using System.IO;
using JsonFx.Json;
using UnityEngine;

namespace CrossAdPlugin
{
	public class BPJsonUtil
	{
		public static void ExportTemplateJson<T>(string to_fileName_dot_json, T inputDataObj)
		{
			string path = Application.dataPath + "/Game/Resources/" + to_fileName_dot_json;
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			StreamWriter streamWriter = File.CreateText(path);
			streamWriter.Write(BPJsonUtil.ObjectToJson<T>(inputDataObj));
			streamWriter.Close();
		}

		public static T ReadJsonFromResources<T>(string from_file_txt)
		{
			TextAsset textAsset = Resources.Load(from_file_txt, typeof(TextAsset)) as TextAsset;
			return BPJsonUtil.JsonToObject<T>(textAsset.text);
		}

		public static string ObjectToJson<T>(T inputDataObj)
		{
			return JsonWriter.Serialize(inputDataObj);
		}

		public static T JsonToObject<T>(string json)
		{
			return JsonReader.Deserialize<T>(json);
		}
	}
}

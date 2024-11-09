using System;
using System.IO;
using UnityEngine;

namespace CrossAdPlugin
{
	public class CrossAdTextureInfo
	{
		public CrossAdTextureInfo(string url, int index = 0)
		{
			this.url = url;
			this.index = index;
			if (!Directory.Exists(CrossAdTextureInfo.GetCacheFolderPath()))
			{
				Directory.CreateDirectory(CrossAdTextureInfo.GetCacheFolderPath());
			}
			this.filePath = CrossAdTextureInfo.GetCacheFolderPath() + "/" + BPNetworkUtils.HashString(url) + ".png";
		}

		public void SetTexture(Texture2D tex)
		{
			this.texture = tex;
		}

		public Texture2D DonwloadedTexture
		{
			get
			{
				return this.texture;
			}
		}

		public static string GetCacheFolderPath()
		{
			return Application.persistentDataPath + "/cache";
		}

		private Texture2D texture;

		public string url;

		public int index;

		public string filePath;
	}
}

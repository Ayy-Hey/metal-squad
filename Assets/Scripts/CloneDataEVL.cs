using System;
using UnityEngine;

public class CloneDataEVL : MonoBehaviour
{
	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.run)
		{
			this.run = false;
			try
			{
				this.fileDataClone.datas = this.fileData.datas;
			}
			catch
			{
			}
		}
	}

	public DataEVL fileData;

	public DataEVL fileDataClone;

	public bool run;
}

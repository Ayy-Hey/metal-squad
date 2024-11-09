using System;
using UnityEngine;

public class AutoCheckLevelEditor : MonoBehaviour
{
	private void Start()
	{
		AutoCheckLevel.isAutoCheck = this.isAuto;
		if (this.isAuto)
		{
			ProfileManager.unlockAll = true;
		}
		AutoCheckLevel.typeCheck = (int)this._type;
	}

	public AutoCheckLevelEditor.TypeCheck _type;

	public bool isAuto;

	public enum TypeCheck
	{
		Pause,
		Win,
		Lost
	}
}

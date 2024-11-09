using System;
using UnityEngine;

public class LogUserCheat : MonoBehaviour
{
	private void Start()
	{
		if (LogUserCheat.isCheck)
		{
			return;
		}
		if (ProfileManager.userProfile.Coin > 1000 || ProfileManager.userProfile.Ms > 0)
		{
			
			ProfileManager.userProfile.ResetCoin();
			LogUserCheat.isCheck = true;
		}
	}

	public static bool isCheck;
}

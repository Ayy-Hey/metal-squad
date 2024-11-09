using System;
using UnityEngine;
using UnityEngine.UI;

public class CardBooster : CardBase
{
	[Header("---------------CardBooster---------------")]
	public int indexCard;

	public string nameDisplay;

	public Localization0 idName;

	public string inforBooster;

	public EBooster booster;

	public Text txt_Cost;

	public Image img_Cost;

	public Button btn_Cost;
}

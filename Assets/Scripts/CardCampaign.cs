using System;
using UnityEngine;

public class CardCampaign : CardBase
{
	[Header("---------------CardCampaign---------------")]
	public GameObject obj_Map;

	public GameObject obj_starOfMap;

	public TweenPosition twn_ListLevel;

	public TweenScale twn_ListLevelScale;

	public WorldMapProfile _mapProfile;

	[SerializeField]
	public LevelWorldMap[] levelWorldMap;
}

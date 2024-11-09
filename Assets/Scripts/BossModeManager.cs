using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class BossModeManager : MonoBehaviour
{
	public static BossModeManager Instance
	{
		get
		{
			if (!BossModeManager.instance)
			{
				BossModeManager.instance = UnityEngine.Object.FindObjectOfType<BossModeManager>();
			}
			return BossModeManager.instance;
		}
	}

	private void Awake()
	{
		BossModeManager.instance = base.GetComponent<BossModeManager>();
		this.Load();
	}

	public void Load()
	{
		base.StartCoroutine(this.LoadMap());
	}

	private IEnumerator LoadMap()
	{
		this.map = null;
		UnityEngine.Debug.Log(ProfileManager.bossCurrent.ToString());
		string path = "BossMode/Maps/Map_Boss_" + ProfileManager.bossCurrent.ToString();
		this.map = UnityEngine.Object.Instantiate<BossModeMap>(Resources.Load<BossModeMap>(path));
		yield return new WaitUntil(() => this.map);
		if (this.map)
		{
			CameraController.Instance.parallaxLayer1 = this.map.parallaxLayer;
			ProCamera2D.Instance.UpdateScreenSize(this.map.cameraStartSize, 0f, EaseType.EaseInOut);
			CameraController.Instance.NumericBoundaries.TopBoundary = this.map.startBoundary.x;
			CameraController.Instance.NumericBoundaries.BottomBoundary = this.map.startBoundary.y;
			CameraController.Instance.NumericBoundaries.LeftBoundary = this.map.startBoundary.z;
			CameraController.Instance.NumericBoundaries.RightBoundary = this.map.startBoundary.w;
			GameManager.Instance.audioManager.AudioBGCurrent = this.map.soundBG;
			PlayerManagerStory.Instance.typeBegin = this.map.playerTypeBegin;
			yield return new WaitUntil(() => GameManager.Instance.player);
			switch (ProfileManager.bossCurrent)
			{
			case EBoss.Heroic_Mech:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_HeroicMech>(Resources.Load<Boss_HeroicMech>("GameObject/Boss_HeroicMech"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.T_REX:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss1_3_New>(Resources.Load<Boss1_3_New>("GameObject/Boss1_3_New"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Iron_Mech:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss1_6>(Resources.Load<Boss1_6>("GameObject/Boss1_6"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Spider_Toxic:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Spider>(Resources.Load<Boss_Spider>("GameObject/Boss_Spider"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.War_Bunker:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_WarBunker>(Resources.Load<Boss_WarBunker>("GameObject/Boss_WarBunker"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Super_Spider:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_SuperSpider>(Resources.Load<Boss_SuperSpider>("GameObject/Boss_SuperSpider"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Mechanical_Scorpion:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Mechanical_Scorpion>(Resources.Load<Boss_Mechanical_Scorpion>("GameObject/Boss_Mechanical_Scorpion"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Tenguka:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss13>(Resources.Load<Boss13>("GameObject/Boss13"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Sun_Ray:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Sunray>(Resources.Load<Boss_Sunray>("GameObject/Boss_Sunray"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Heavy_Mech:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss5_1>(Resources.Load<Boss5_1>("GameObject/Boss5_1"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.U_F_O:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss5_2>(Resources.Load<Boss5_2>("GameObject/Boss5_2"), this.map.objBoss.transform.position, Quaternion.identity);
				GameManager.Instance.bossManager.Boss.GetComponent<Boss5_2>().posBegin = new Vector2(0f, 0f);
				break;
			case EBoss.Schneider_G25:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Schneider_G25>(Resources.Load<Boss_Schneider_G25>("GameObject/Boss_Schneider_G25"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Ultron:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Ultron>(Resources.Load<Boss_Ultron>("GameObject/Boss_Ultron"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			case EBoss.Mara_Devil:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_MaraDevil>(Resources.Load<Boss_MaraDevil>("GameObject/Boss_MaraDevil"), this.map.objBoss.transform.position, Quaternion.identity);
				break;
			}
			GameManager.Instance.bossManager.CreateBoss();
		}
		else
		{
			UnityEngine.Debug.LogError("______________________________Không load được map boss " + ProfileManager.bossCurrent);
		}
		yield break;
	}

	private static BossModeManager instance;

	public BossModeMap map;
}

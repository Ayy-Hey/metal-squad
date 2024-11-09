using System;
using System.Collections;
using PVPManager;
using SpawnEnemy;
using UnityEngine;
using UnityEngine.Events;

public class PVPMap : MonoBehaviour
{
	public void Init(int idTurn)
	{
		this.idTurn = idTurn;
		this.RunTurn();
		this.isInit = true;
	}

	public void RunTurn()
	{
		this.autoSpawnEnemy[this.idTurn].OnCompleted.RemoveAllListeners();
		this.autoSpawnEnemy[this.idTurn].OnCompleted.AddListener(new UnityAction(this.NextTurn));
		this.autoSpawnEnemy[this.idTurn].OnInit();
	}

	public void NextTurn()
	{
		if (this.idTurn < this.autoSpawnEnemy.Length - 1)
		{
			this.idTurn++;
			this.OnNextTurn.Invoke();
			this.RunTurn();
		}
		else
		{
			this.OnCompleteMap.Invoke();
			switch (this.boss)
			{
			case EBoss.Heroic_Mech:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_HeroicMech>(Resources.Load<Boss_HeroicMech>("GameObject/Boss_HeroicMech"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.T_REX:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss1_3_New>(Resources.Load<Boss1_3_New>("GameObject/Boss1_3_New"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Iron_Mech:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss1_6>(Resources.Load<Boss1_6>("GameObject/Boss1_6"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Spider_Toxic:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Spider>(Resources.Load<Boss_Spider>("GameObject/Boss_Spider"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.War_Bunker:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_WarBunker>(Resources.Load<Boss_WarBunker>("GameObject/Boss_WarBunker"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Super_Spider:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_SuperSpider>(Resources.Load<Boss_SuperSpider>("GameObject/Boss_SuperSpider"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Mechanical_Scorpion:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Mechanical_Scorpion>(Resources.Load<Boss_Mechanical_Scorpion>("GameObject/Boss_Mechanical_Scorpion"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Tenguka:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss13>(Resources.Load<Boss13>("GameObject/Boss13"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Sun_Ray:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Sunray>(Resources.Load<Boss_Sunray>("GameObject/Boss_Sunray"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Heavy_Mech:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss5_1>(Resources.Load<Boss5_1>("GameObject/Boss5_1"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.U_F_O:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss5_2>(Resources.Load<Boss5_2>("GameObject/Boss5_2"), this.tfCreateBoss.position, Quaternion.identity);
				GameManager.Instance.bossManager.Boss.GetComponent<Boss5_2>().posBegin = new Vector2(0f, 0f);
				break;
			case EBoss.Schneider_G25:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Schneider_G25>(Resources.Load<Boss_Schneider_G25>("GameObject/Boss_Schneider_G25"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Ultron:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_Ultron>(Resources.Load<Boss_Ultron>("GameObject/Boss_Ultron"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			case EBoss.Mara_Devil:
				GameManager.Instance.bossManager.Boss = UnityEngine.Object.Instantiate<Boss_MaraDevil>(Resources.Load<Boss_MaraDevil>("GameObject/Boss_MaraDevil"), this.tfCreateBoss.position, Quaternion.identity);
				break;
			}
			GameManager.Instance.bossManager.CreateBoss();
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.autoSpawnEnemy[this.idTurn].isInit)
		{
			this.autoSpawnEnemy[this.idTurn].OnUpdate(deltaTime);
		}
	}

	public void CreateBomb(int totalBomb)
	{
		base.StartCoroutine(this.CoroutineCreateBomb(totalBomb));
	}

	public void SetTimeNextTurnBomb(float time)
	{
		this.waitNextTurnBomb = new WaitForSeconds(time);
	}

	public void SetTotalTurnBomb(int num)
	{
		this.totalTurnBomb = num;
	}

	private IEnumerator CoroutineCreateBomb(int totalBomb)
	{
		Vector2 bombPos = Vector2.zero;
		float from = CameraController.Instance.LeftCamera();
		float d = (CameraController.Instance.RightCamera() - from) / (float)totalBomb;
		float to = 0f;
		int damage = 50 + 5 * PvP_LocalPlayer.Instance.EnemyTurn;
		for (int i = 0; i < this.totalTurnBomb; i++)
		{
			for (int j = 0; j < totalBomb; j++)
			{
				to = from + d;
				bombPos.y = 0f;
				bombPos.x = UnityEngine.Random.Range(from, to);
				RaycastHit2D raycastHit2D = Physics2D.Raycast(bombPos, Vector2.down, 15f, this.maskGround);
				if (raycastHit2D.collider)
				{
					bombPos.y = raycastHit2D.point.y;
				}
				GameManager.Instance.fxManager.ShowFxWarning(1f, bombPos, Fx_Warning.CameraLock.None, 0, null);
				bombPos.y = CameraController.Instance.TopCamera();
				GameManager.Instance.bombManager.CreateBombAirplane(bombPos, 1f, (float)damage, false);
				from = to;
			}
			from = CameraController.Instance.LeftCamera();
			yield return this.waitNextTurnBomb;
		}
		yield break;
	}

	private void OnDestroy()
	{
		try
		{
			if (GameManager.Instance.bossManager.Boss)
			{
				UnityEngine.Object.Destroy(GameManager.Instance.bossManager.Boss.gameObject);
				GameManager.Instance.hudManager.LineBlood.gameObject.SetActive(false);
			}
		}
		catch
		{
		}
	}

	[HideInInspector]
	public bool isInit;

	public float cameraStartSize = 3.6f;

	public Vector4 startBoundary;

	public AutoSpawnPvp[] autoSpawnEnemy;

	private int idTurn;

	public LayerMask maskGround;

	public Transform tfEffectAir;

	public EBoss boss;

	public Transform tfCreateBoss;

	public UnityEvent OnNextTurn;

	public UnityEvent OnCompleteMap;

	private WaitForSeconds waitNextTurnBomb;

	private int totalTurnBomb;
}

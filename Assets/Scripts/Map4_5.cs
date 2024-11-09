using System;
using System.Collections;
using SpawnEnemy;
using UnityEngine;

public class Map4_5 : MonoBehaviour
{
	private IEnumerator Start()
	{
		this._idSpawn = 0;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Endless)
		{
			for (int i = 0; i < this.autoSpawns.Length; i++)
			{
				for (int j = 0; j < this.autoSpawns[i].turns.Length; j++)
				{
					for (int k = 0; k < this.autoSpawns[i].turns[j].enemies.Length; k++)
					{
						this.autoSpawns[i].turns[j].enemies[k].Level = 0;
					}
				}
			}
		}
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.thangMay.ActiveStopRambo(false);
		this.isInit = true;
		yield break;
	}

	private void Update()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (this.trap.isAttackToPlay && GameManager.Instance.player.HPCurrent > 0f)
		{
			this.trap.isAttackToPlay = false;
			this.trap.Reset();
		}
		float deltaTime = Time.deltaTime;
		if (this.thangMay.isRunning)
		{
			this.thangMay.OnUpdate(deltaTime);
			this.tfBanhXe.Rotate(0f, 0f, -this.thangMay.speed * 2f);
			CameraController.Instance.NumericBoundaries.LeftBoundary = Mathf.SmoothDamp(CameraController.Instance.NumericBoundaries.LeftBoundary, this.thangMay.transform.position.x - 6f, ref this.veloL, 0.2f);
			CameraController.Instance.NumericBoundaries.RightBoundary = Mathf.SmoothDamp(CameraController.Instance.NumericBoundaries.RightBoundary, this.thangMay.transform.position.x + 8f, ref this.veloR, 0.2f);
		}
		else
		{
			this.trap.OnUpdate(deltaTime);
		}
		if (this._idSpawn < this.autoSpawns.Length)
		{
			float num = Mathf.Abs(this.thangMay.transform.position.y - this.autoSpawns[this._idSpawn].transform.position.y);
			if (num <= this.distanceToRunSpawn)
			{
				this.autoSpawns[this._idSpawn].OnInit();
				this._idSpawn++;
			}
		}
		for (int i = 0; i < this.autoSpawns.Length; i++)
		{
			if (this.autoSpawns[i].isInit)
			{
				this.autoSpawns[i].OnUpdate(deltaTime);
			}
		}
	}

	public void RunThangMay()
	{
		this.thangMay.listTfObjFollow.Add(GameManager.Instance.player.transform);
		this.thangMay.ActiveStopRambo(true);
		CameraController.Instance.StopMoveLeftBoundary = true;
		EnemyManager.Instance.enemyRandom.run = true;
		this.thangMay.Run(this.tfTargetThangMay.position, delegate
		{
			GameManager.Instance.player.transform.parent = null;
			this.thangMay.collsStopRambo[1].enabled = false;
			CameraController.Instance.StopMoveLeftBoundary = false;
			CameraController.Instance.NewCheckpoint(true, 15f);
			EnemyManager.Instance.enemyRandom.run = false;
		});
	}

	public ThangMayMap4 thangMay;

	public Transform tfTargetThangMay;

	public Transform tfBanhXe;

	public TrapJetpack trap;

	public AutoSpawnAuto[] autoSpawns;

	public float distanceToRunSpawn = 5f;

	private float veloL;

	private float veloR;

	private bool isInit;

	private int _idSpawn;
}

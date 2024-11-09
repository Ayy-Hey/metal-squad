using System;
using System.Collections;
using SpawnEnemy;
using UnityEngine;

public class Map4_3 : MonoBehaviour
{
	private void OnDisable()
	{
		try
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Endless)
			{
				CameraController.Instance.isVerticalDown = false;
			}
		}
		catch
		{
		}
	}

	private IEnumerator Start()
	{
		this.stepId = 0;
		this.timeReloadBomb = 3f;
		this.thangMay.ActiveStopRambo(false);
		CameraController.Instance.isVerticalDown = this.isVerticalDown;
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
		this.thangMay.listTfObjFollow.Add(GameManager.Instance.player.transform);
		this.isInit = true;
		yield return new WaitForSeconds(this.startTime);
		this.GoStep();
		if (PreGameOver.Instance)
		{
			PreGameOver.Instance.transform.GetChild(1).gameObject.SetActive(false);
		}
		yield break;
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		if (GameManager.Instance.player.isAutoRun && GameManager.Instance.StateManager.EState == EGamePlay.PREVIEW)
		{
			GameManager.Instance.player.OnMovement(BaseCharactor.EMovementBasic.Right);
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (this.thangMay.isRunning)
		{
			this.thangMay.OnUpdate(deltaTime);
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Campaign)
		{
			this._coolDownBomb -= deltaTime;
			if (this._coolDownBomb <= 0f)
			{
				this._coolDownBomb = this.timeReloadBomb;
				int num = UnityEngine.Random.Range(2, 4);
				float num2 = 8f / (float)num;
				float num3 = CameraController.Instance.camPos.x - 4f;
				for (int i = 0; i < num; i++)
				{
					this._posBomb.x = UnityEngine.Random.Range(num3 + (float)i * num2, num3 + num2 + (float)i * num2);
					this._posBomb.y = this.thangMay.transform.position.y + 0.5f;
					GameManager.Instance.fxManager.ShowFxWarning(1f, this._posBomb, Fx_Warning.CameraLock.None, 0, this.thangMay.transform);
					this._posBomb.y = CameraController.Instance.camPos.y + CameraController.Instance.Size().y;
					GameManager.Instance.bombManager.CreateBombAirplane(this._posBomb, 1f, this.damageBomb, false);
				}
			}
		}
		for (int j = 0; j < this.autoSpawns.Length; j++)
		{
			if (this.autoSpawns[j].isInit)
			{
				this.autoSpawns[j].OnUpdate(deltaTime);
			}
		}
		this._posClearCharacter = CameraController.Instance.transform.position;
		this._posClearCharacter.z = 0f;
		if (this.checkEnd)
		{
			bool flag = true;
			for (int k = 0; k < GameManager.Instance.ListEnemy.Count; k++)
			{
				if (GameManager.Instance.ListEnemy[k].isInCamera)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.checkEnd = false;
				this.thangMay.ActiveStopRambo(false);
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.StateManager.SetPreview();
				GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
				GameManager.Instance.player._PlayerSpine.ResetAnimTarget();
				GameManager.Instance.player.isAutoRun = true;
			}
		}
		if (this._isWarning)
		{
			this._timeWarning -= deltaTime;
			this.thanhChan[this.stepId - 1].color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 8f, 1f));
			if (this._timeWarning <= 0f)
			{
				this._isWarning = false;
				this.thanhChan[this.stepId - 1].gameObject.SetActive(false);
				this.colThanhChan[this.stepId - 1].enabled = false;
				this.RunThangMay();
			}
		}
	}

	public void GoStep()
	{
		EnemyManager.Instance.enemyRandom.run = true;
		if (this.stepId > 0 && this.stepId < this.autoSpawns.Length)
		{
			this._isWarning = true;
			this._timeWarning = 2f;
			return;
		}
		this.RunThangMay();
	}

	private void RunThangMay()
	{
		this.thangMay.SetColliderRun(true);
		this.thangMay.Run(this.autoSpawns[this.stepId].transform.position, delegate
		{
			EnemyManager.Instance.enemyRandom.run = false;
			this.thangMay.SetColliderRun(false);
			this.autoSpawns[this.stepId].OnInit();
			this.stepId++;
			if (this.stepId == this.autoSpawns.Length && GameMode.Instance.modePlay == GameMode.ModePlay.Endless)
			{
				this.thangMay.ActiveStopRambo(false);
			}
		});
	}

	public void EndMap()
	{
		this.checkEnd = true;
		if (PreGameOver.Instance)
		{
			PreGameOver.Instance.transform.GetChild(1).gameObject.SetActive(true);
		}
	}

	public AutoSpawnAuto[] autoSpawns;

	public SpriteRenderer[] thanhChan;

	public Collider2D[] colThanhChan;

	public ThangMayMap4 thangMay;

	public float startTime = 2f;

	public float timeReloadBomb;

	public float damageBomb = 100f;

	public bool isVerticalDown = true;

	private int stepId;

	private Vector3 _posClearCharacter;

	private bool isInit;

	private bool checkEnd;

	private bool _isWarning;

	private float _timeWarning;

	private float _coolDownBomb;

	private Vector2 _posBomb;
}

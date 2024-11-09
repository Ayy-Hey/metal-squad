using System;
using Com.LuisPedroFonseca.ProCamera2D;
using SpawnEnemy;
using UnityEngine;

public class StopCargoShip2 : MonoBehaviour
{
	private void Start()
	{
		this.baseTrigger.OnEnteredTrigger = delegate()
		{
			this.OnStop();
		};
		this.autoSpawnEnemies.OnCompleted = delegate()
		{
			GameManager.Instance.hudManager.HideControl();
			GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
			GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
			GameManager.Instance.StateManager.descr = LeanTween.moveLocalX(this.objGate, -0.63f, 2f).setOnComplete(delegate()
			{
				GameManager.Instance.hudManager.ShowControl(1.1f);
				this.spriteRenderer.sortingLayerName = "Gameplay";
				this.objGate.SetActive(false);
				CameraController.Instance.NewCheckpoint(true, 5f);
			});
		};
	}

	private void LateUpdate()
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING && this.isInit)
		{
			this.autoSpawnEnemies.OnUpdate(Time.deltaTime);
		}
	}

	private void OnStop()
	{
		if (this.isTrigger)
		{
			return;
		}
		this.isTrigger = true;
		GameManager.Instance.hudManager.HideControl();
		GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
		GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		GameManager.Instance.player.EMovement = BaseCharactor.EMovementBasic.Release;
		GameManager.Instance.StateManager.descr = LeanTween.moveLocalX(this.tfGate.gameObject, 0f, 5f).setOnComplete(delegate()
		{
			this.isInit = true;
			this.objCanRambo.SetActive(false);
			this.autoSpawnEnemies.OnStart(this.MAX_LEFT);
			CameraController.Instance.NumericBoundaries.RightBoundary = this.MAX_RIGHT;
			GameManager.Instance.hudManager.ShowControl(1.1f);
			CameraController.Instance.parallaxLayer1.StopAuto();
		});
	}

	public GameObject objCanRambo;

	[SerializeField]
	private Transform tfGate;

	[SerializeField]
	private BaseTrigger baseTrigger;

	private bool isTrigger;

	[SerializeField]
	private AutoSpawnEnemies autoSpawnEnemies;

	[SerializeField]
	private GameObject objGate;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private float MAX_RIGHT;

	[SerializeField]
	private float MAX_LEFT;

	private bool isInit;
}

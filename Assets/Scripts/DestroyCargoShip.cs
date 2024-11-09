using System;
using System.Collections;
using SpawnEnemy;
using UnityEngine;

public class DestroyCargoShip : MonoBehaviour
{
	private void Start()
	{
		AutoSpawnEnemies autoSpawnEnemies = this.autoSpawnEnemies;
		autoSpawnEnemies.OnCompleted = (Action)Delegate.Combine(autoSpawnEnemies.OnCompleted, new Action(delegate()
		{
			try
			{
				GameManager.Instance.listRevival[0].gameObject.SetActive(false);
				GameManager.Instance.listRevival[2].gameObject.SetActive(false);
				GameManager.Instance.listRevival[1].gameObject.SetActive(true);
			}
			catch
			{
			}
			if (GameMode.Instance.EMode == GameMode.Mode.NORMAL)
			{
				CameraController.Instance.NewCheckpoint(true, 5f);
				GameManager.Instance.listRevival[1].gameObject.SetActive(false);
				GameManager.Instance.listRevival[2].gameObject.SetActive(true);
				Vector3 position = this.tfCargoShip2.position;
				position.x = this.posCargoShip2.x;
				this.tfCargoShip2.position = position;
			}
			else
			{
				base.StartCoroutine(this.OnStart());
			}
		}));
	}

	private IEnumerator OnStart()
	{
		yield return new WaitForSeconds(4f);
		CameraController.Instance.NewCheckpoint(true, 5f);
		this.isInit = true;
		this.timeDelayExplosion = Time.timeSinceLevelLoad;
		GameManager.Instance.StateManager.descr = LeanTween.moveX(this.tfCargoShip2.gameObject, this.posCargoShip2.x, 6f).setOnComplete(delegate()
		{
			this.isRemoveCargoShip1 = true;
			GameManager.Instance.listRevival[1].gameObject.SetActive(false);
			GameManager.Instance.listRevival[2].gameObject.SetActive(true);
		});
		yield break;
	}

	private void Update()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (this.isRemoveCargoShip1)
		{
			Vector3 position = this.tfCargoShip.position;
			position.y -= Time.deltaTime * 0.5f;
			this.tfCargoShip.position = position;
			if (position.y <= -1f)
			{
				for (int i = 0; i < this.arrCollider2D.Length; i++)
				{
					this.arrCollider2D[i].enabled = false;
				}
			}
			if (position.y < -5f)
			{
				this.isInit = false;
			}
		}
		float f = Vector2.Distance(this.tfCargoShip.position, GameManager.Instance.player.GetPosition());
		if (Time.timeSinceLevelLoad - this.timeDelayExplosion >= 0.5f && Mathf.Abs(f) < 10f)
		{
			this.timeDelayExplosion = Time.timeSinceLevelLoad;
			int num = UnityEngine.Random.Range(1, 3);
			for (int j = 0; j < num; j++)
			{
				GameManager.Instance.fxManager.ShowEffect(5, this.tfExplosion[UnityEngine.Random.Range(0, this.tfExplosion.Length - 1)].position, Vector3.one * 1.5f, true, true);
			}
		}
	}

	[SerializeField]
	private Transform[] tfExplosion;

	[SerializeField]
	private Transform tfCargoShip;

	[SerializeField]
	private Transform tfCargoShip2;

	[SerializeField]
	private Vector3 posCargoShip2;

	private bool isInit;

	private float timeDelayExplosion;

	private float timeRunPlayer;

	private bool isRemoveCargoShip1;

	[SerializeField]
	private Collider2D[] arrCollider2D;

	public AutoSpawnEnemies autoSpawnEnemies;
}

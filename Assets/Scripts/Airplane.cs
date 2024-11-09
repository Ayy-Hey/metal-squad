using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Airplane : MonoBehaviour
{
	public EnemyCharactor enemy { get; set; }

	private void OnDisable()
	{
		try
		{
			GameManager.Instance.audioManager.maybay2.Stop();
			GameManager.Instance.CountEnemyDie++;
			EnemyManager.Instance.PoolAirplane.Store(this);
			if (this.isCreateWithJson)
			{
				DataLoader.LevelDataCurrent.points[this.CheckPoint].totalEnemy--;
			}
		}
		catch (Exception ex)
		{
		}
	}

	public void Init(int level)
	{
		if (this.enemy == null)
		{
			this.enemy = DataLoader.maybay[1];
		}
		this.level = level;
		if (this.RunAnim == null)
		{
			this.RunAnim = this.skeletonAnimation.skeleton.Data.FindAnimation(this.runAnim);
		}
		this.Damaged = this.enemy.enemy[level].Damage * GameMode.Instance.GetMode();
		this.skeletonAnimation.state.SetAnimation(0, this.RunAnim, true);
		this.step = 0;
	}

	public void UpdateObject()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (this.GetPosition().x < CameraController.Instance.Position.x - CameraController.Instance.Size().x - 2f)
		{
			base.gameObject.SetActive(false);
		}
		base.transform.Translate(Vector3.left * this.enemy.enemy[this.level].Speed * Time.deltaTime);
		Vector3 position = base.transform.position;
		position.y = CameraController.Instance.Position.y + CameraController.Instance.Size().y - 1.5f;
		base.transform.position = position;
		if (this.step >= this.arrTarget.Length)
		{
			return;
		}
		float num = Mathf.Abs(this.arrTarget[this.step].x - this.GetPosition().x);
		if (num < 0.1f)
		{
			GameManager.Instance.bombManager.CreateBombAirplane(base.transform.position, this.enemy.enemy[this.level].Radius_Damage, this.Damaged, true);
			this.step++;
		}
	}

	public Vector2 GetPosition()
	{
		return new Vector2(base.transform.position.x, base.transform.position.y);
	}

	private void OnBecameInvisible()
	{
	}

	private void OnBecameVisible()
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			this.isInit = true;
			this.step = 0;
			this.arrTarget = new Vector2[this.enemy.enemy[this.level].Number_Bomb];
			Vector2 position = this.GetPosition();
			float num = CameraController.Instance.camPos.x + CameraController.Instance.Size().x;
			for (int i = 0; i < this.arrTarget.Length; i++)
			{
				this.arrTarget[i] = new Vector2(num - this.enemy.enemy[this.level].Distance_Bomb * (float)(i + 1), position.y);
			}
			GameManager.Instance.audioManager.Maybay2();
		}
	}

	[SpineAnimation("", "", true, false)]
	public string runAnim;

	public Spine.Animation RunAnim;

	public SkeletonAnimation skeletonAnimation;

	private int level;

	private bool isInit;

	private Vector2[] arrTarget;

	private int step;

	private float Damaged;

	public int CheckPoint;

	public bool isCreateWithJson;

	public LayerMask layerMask;
}

using System;
using UnityEngine;

public class MiniLaserBoss1_3 : CachingMonoBehaviour
{
	public void InitObjec(float damage, float timeDelay, float timeActive, Vector2 laserDirection, Vector3 pos, Vector3 direction, Vector3 targetMove, float speedMove, Action<MiniLaserBoss1_3> disableAction)
	{
		this.delta = (this.time = 0f);
		this.isMoveDone = false;
		base.gameObject.SetActive(true);
		this.targetMove = targetMove;
		this.speedMove = speedMove;
		this.disableAction = disableAction;
		this.transform.position = pos;
		this.rotate = Quaternion.LookRotation(direction, Vector3.back);
		this.rotate.x = (this.rotate.y = 0f);
		this.transform.localRotation = this.rotate;
		this.laser.Init(damage, timeDelay, timeActive, laserDirection, new Action(this.hideAction));
		this.isInit = true;
	}

	public void UpdateObject(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.isMoveDone)
		{
			this.laser.UpdateObject(deltaTime);
			return;
		}
		this.transform.Translate(this.speedMove * deltaTime * this.transform.up, Space.World);
		if (this.time < 0.2f)
		{
			this.time += deltaTime;
			return;
		}
		this.rotate = Quaternion.LookRotation(this.targetMove - this.transform.position, Vector3.back);
		this.rotate.x = (this.rotate.y = 0f);
		this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, this.rotate, this.delta);
		this.delta += deltaTime / 5f;
		float num = Vector3.Distance(this.transform.position, this.targetMove);
		this.isMoveDone = (num < 0.2f);
		if (this.isMoveDone)
		{
			this.transform.eulerAngles = this.euler;
		}
	}

	private void hideAction()
	{
		try
		{
			this.isInit = false;
			base.gameObject.SetActive(false);
			this.disableAction(this);
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	[SerializeField]
	private LaserBoss1_3 laser;

	[SerializeField]
	private AnimationCurve[] curves;

	[HideInInspector]
	public bool isInit;

	private float speedMove;

	private Action<MiniLaserBoss1_3> disableAction;

	private Vector3 targetMove;

	private Vector3 euler = new Vector3(0f, 0f, 180f);

	private bool isMoveDone;

	private float time;

	private float delta;

	private Quaternion rotate;
}

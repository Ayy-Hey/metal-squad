using System;
using UnityEngine;

public class DayDu : CachingMonoBehaviour
{
	public void Init(Vector3 pos)
	{
		this.timeCheckStuck = 0f;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.down, 20f, this.maskGround);
		if (raycastHit2D.collider)
		{
			this.posTarget = raycastHit2D.point;
			this.posTarget.y = this.posTarget.y + 1f;
		}
		else
		{
			this.posTarget.x = pos.x;
			this.posTarget.y = CameraController.Instance.camPos.y;
		}
		this.show = true;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.show)
		{
			this.timeCheckStuck += deltaTime;
			if (this.timeCheckStuck > 3f)
			{
				this.Off();
			}
		}
		if (Vector3.Distance(this.transform.position, this.posTarget) >= 0.1f)
		{
			this.transform.position = Vector3.SmoothDamp(this.transform.position, this.posTarget, ref this.veloSmooth, 0.5f);
		}
		else if (!this.show)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void Off()
	{
		this.posTarget.y = this.posTarget.y + CameraController.Instance.Size().y * 2f;
		this.show = false;
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			EnemyManager.Instance.PoolDayDu.Store(this);
		}
		catch
		{
		}
	}

	public bool isInit;

	public LayerMask maskGround;

	private bool show;

	private Vector3 posTarget;

	private Vector3 veloSmooth;

	private float timeCheckStuck;
}

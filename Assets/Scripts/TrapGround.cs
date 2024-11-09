using System;
using UnityEngine;

public class TrapGround : MonoBehaviour
{
	public void OnInit()
	{
		this.timeCounter = 0f;
		this.isStart = false;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isStart)
		{
			return;
		}
		this.timeCounter += deltaTime;
		if (this.timeCounter >= 1f)
		{
			this.timeCounter = 0f;
			this.RotateGround();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag("Rambo") && !this.isStart)
		{
			this.isStart = true;
			this.timeCounter = 0f;
		}
	}

	private void RotateGround()
	{
		if (this.isRotate)
		{
			return;
		}
		this.objRevive.SetActive(false);
		this.isRotate = true;
		int num = (GameManager.Instance.player.GetPosition().x <= base.transform.position.x) ? 90 : -90;
		LeanTween.rotateZ(base.gameObject, (float)num, 0.5f).setOnComplete(delegate()
		{
			this.timeCounter = 0f;
			this.isStart = false;
			this.isRotate = false;
			LeanTween.rotateZ(base.gameObject, 0f, 0.5f).setDelay(1f).setOnComplete(delegate()
			{
				this.objRevive.SetActive(true);
			});
		});
	}

	private float timeCounter;

	private bool isStart;

	private bool isRotate;

	[SerializeField]
	private GameObject objRevive;
}

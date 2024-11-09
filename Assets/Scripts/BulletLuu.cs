using System;
using UnityEngine;

public class BulletLuu : CachingMonoBehaviour
{
	public void InitObject(float damage, float speed, float lineTime, Vector3 pos, Vector3 direction, Action<BulletLuu> actionHide)
	{
		this.damage = damage;
		this.speed = speed;
		this.lineTime = lineTime;
		this.speedRotate = Time.fixedDeltaTime;
		this.actionHide = actionHide;
		base.gameObject.transform.position = pos;
		base.gameObject.SetActive(true);
		Quaternion localRotation = Quaternion.LookRotation(direction, Vector3.back);
		localRotation.x = (localRotation.y = 0f);
		this.transform.localRotation = localRotation;
		this.euler = localRotation.eulerAngles;
		this.isInit = true;
	}

	public void UpdateObject(float delta)
	{
		if (!this.isInit)
		{
			return;
		}
		this.transform.Translate(this.speed * delta * this.transform.up, Space.World);
		if (this.lineTime > 0f)
		{
			this.lineTime -= delta;
			return;
		}
		this.euler.z = Mathf.MoveTowards(this.euler.z, 180f, this.speedRotate);
		this.speedRotate += delta * 0.75f;
		this.transform.eulerAngles = this.euler;
		if (!CameraController.Instance.IsInView(this.transform))
		{
			this.Hide(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component != null && component.IsInVisible())
				{
					return;
				}
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
		this.Hide(true);
	}

	private void Hide(bool showEff)
	{
		try
		{
			if (showEff)
			{
				GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
			}
			base.gameObject.SetActive(false);
			this.actionHide(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private Action<BulletLuu> actionHide;

	[HideInInspector]
	public bool isInit;

	private float lineTime;

	private float speed;

	private float speedRotate;

	private float damage;

	private Vector3 euler;
}

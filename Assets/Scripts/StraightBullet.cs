using System;
using UnityEngine;

public class StraightBullet : CachingMonoBehaviour
{
	public void InitObject(float damage, float speed, float lineTime, Vector3 pos, Vector3 direction, Action<StraightBullet> actionHide)
	{
		this.trail.Clear();
		this.damage = damage;
		this.speed = speed;
		this.lineTime = lineTime;
		this.speedRotate = Time.fixedDeltaTime;
		this.actionHide = actionHide;
		this.direction = direction;
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
		this.transform.Translate(this.speed * delta * this.direction, Space.World);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
			this.Hide(true);
		}
		else if (collision.CompareTag("Ground"))
		{
			this.Hide(true);
		}
	}

	private void Hide(bool showEff)
	{
		try
		{
			if (showEff)
			{
				GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, false, false);
			}
			base.gameObject.SetActive(false);
			this.actionHide(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.trail.Clear();
		this.isInit = false;
	}

	private Action<StraightBullet> actionHide;

	[HideInInspector]
	public bool isInit;

	private float lineTime;

	private float speed;

	private float speedRotate;

	private float damage;

	private Vector3 euler;

	private Vector3 direction;

	public TrailRenderer trail;
}

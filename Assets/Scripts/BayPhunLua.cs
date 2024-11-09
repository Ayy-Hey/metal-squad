using System;
using UnityEngine;

public class BayPhunLua : MonoBehaviour
{
	public void InitObject()
	{
		this.countDown = this.startTimeDelay;
	}

	private void Update()
	{
		this.OnUpdate();
	}

	private void Start()
	{
		this.InitObject();
	}

	public void OnUpdate()
	{
		if (this.render.isVisible)
		{
			this.countDown -= Time.deltaTime;
			if (this.countDown <= 0f)
			{
				if (this.isFire)
				{
					this.isFire = false;
					this.countDown = this.timeDelay;
					this.objLua.Stop();
					this.colliderLua.enabled = false;
				}
				else
				{
					this.isFire = true;
					this.countDown = this.timeFire;
					this.objLua.Play(true);
					this.colliderLua.enabled = true;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!this.isOneShot)
		{
			return;
		}
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
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (this.isOneShot)
		{
			return;
		}
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				GameManager.Instance.player.AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
	}

	[SerializeField]
	private Renderer render;

	[SerializeField]
	private Collider2D colliderLua;

	[SerializeField]
	private ParticleSystem objLua;

	[SerializeField]
	private bool isOneShot;

	[SerializeField]
	private float damage;

	[SerializeField]
	private float startTimeDelay;

	[SerializeField]
	private float timeDelay;

	[SerializeField]
	private float timeFire;

	private float distance;

	private bool isFire;

	private float countDown;
}

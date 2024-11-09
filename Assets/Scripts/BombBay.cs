using System;
using UnityEngine;

public class BombBay : CachingMonoBehaviour
{
	public void Init(float damage, float speed, Vector3 pos, Action<BombBay> hide)
	{
		this.hide = hide;
		this.damage = damage;
		this.speed = speed;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.isInit = true;
		base.CancelInvoke();
		base.Invoke("AutoDisable", 5f);
	}

	private void AutoDisable()
	{
		if (!this.isInit)
		{
			return;
		}
		this.Hide();
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.transform.Translate(0f, this.speed * deltaTime, 0f);
		this.speed += this.gravityScale * Physics2D.gravity.y * deltaTime;
	}

	private void Hide()
	{
		try
		{
			if (this.isInit)
			{
				this.isInit = false;
				base.gameObject.SetActive(false);
				this.hide(this);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.isInit = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo") || collision.CompareTag("Ground"))
		{
			for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
			{
				bool flag = Mathf.Abs(this.transform.position.x - GameManager.Instance.ListRambo[i].transform.position.x) < 1.5f && this.transform.position.y >= GameManager.Instance.ListRambo[i].transform.position.y;
				if (flag)
				{
					try
					{
						GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
					}
					catch
					{
					}
				}
			}
			GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.transform.position, Vector3.one);
			this.Hide();
			return;
		}
		if (collision.CompareTag("Gulf"))
		{
			this.Hide();
		}
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private bool useGravity;

	[SerializeField]
	private float gravityScale;

	private float damage;

	private float speed;

	private Action<BombBay> hide;
}

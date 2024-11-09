using System;
using UnityEngine;

public class Coin : CachingMonoBehaviour
{
	public void Show()
	{
		this.transform.localScale = new Vector3(0.8f, 0.8f);
		this.collider2d.isTrigger = false;
		this.rigidbody2D.gravityScale = 2f;
		this.rigidbody2D.velocity = Vector2.zero;
		this.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 1f) * (float)UnityEngine.Random.Range(300, 400));
		this.objCoins[0].SetActive(true);
		this.timeautoHide = 0f;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.timeautoHide += deltaTime;
		if (this.timeautoHide >= 7f)
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.objCoins[i].activeSelf)
				{
					this.sprite[i].color = new Color(1f, 1f, 1f, (float)((Mathf.PingPong(Time.time * 5f, 1f) <= 0.5f) ? 0 : 1));
				}
			}
		}
		if (this.timeautoHide > 10f)
		{
			base.gameObject.SetActive(false);
		}
		if (GameManager.Instance.player._PlayerBooster.IsMagnet)
		{
			float num = Vector2.Distance(this.transform.position, GameManager.Instance.player.Origin());
			if (num <= 4f)
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, GameManager.Instance.player.Origin(), Time.deltaTime * 10f);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (!coll.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		EventDispatcher.PostEvent("COIN_COLLECTED");
		base.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (!coll.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		EventDispatcher.PostEvent("COIN_COLLECTED");
		base.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		for (int i = 0; i < 3; i++)
		{
			this.sprite[i].color = Color.white;
			this.objCoins[i].SetActive(false);
		}
		this.isInit = false;
		try
		{
			GameManager.Instance.coinManager.PoolingCoin.Store(this);
		}
		catch
		{
		}
	}

	[SerializeField]
	private GameObject[] objCoins;

	private float timeautoHide;

	private bool isInit;

	[SerializeField]
	private SpriteRenderer[] sprite;

	[SerializeField]
	private Collider2D collider2d;
}

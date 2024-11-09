using System;
using UnityEngine;

public class Gift : CachingMonoBehaviour
{
	private void OnDisable()
	{
		GameManager.Instance.giftManager.GiftPool.Store(this);
	}

	public void Show(Gift.ETYPE eType, Vector3 pos)
	{
		base.gameObject.SetActive(true);
		this.collider2d.isTrigger = false;
		this.rigidbody2D.gravityScale = 2f;
		this.eType = eType;
		this.transform.position = pos;
		for (int i = 0; i < this.objItem.Length; i++)
		{
			this.objItem[i].SetActive(eType == (Gift.ETYPE)i);
		}
		this.rigidbody2D.AddForce(new Vector2(0f, 200f));
	}

	private void Update()
	{
		if (this.transform.position.y < -5f)
		{
			base.gameObject.SetActive(false);
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		Gift.ETYPE etype = this.eType;
		if (etype != Gift.ETYPE.HP)
		{
			if (etype == Gift.ETYPE.BINH_XANG)
			{
				GameManager.Instance.player._PlayerJetpack.AddPower();
			}
		}
		else
		{
			IHealth component = other.gameObject.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(100f, EWeapon.NONE);
			}
		}
		base.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		Gift.ETYPE etype = this.eType;
		if (etype != Gift.ETYPE.HP)
		{
			if (etype != Gift.ETYPE.BINH_XANG)
			{
			}
		}
		else
		{
			IHealth component = other.gameObject.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(100f, EWeapon.NONE);
			}
		}
		base.gameObject.SetActive(false);
	}

	[SerializeField]
	private Collider2D collider2d;

	private Gift.ETYPE eType;

	[SerializeField]
	private GameObject[] objItem;

	public enum ETYPE
	{
		HP,
		BINH_XANG
	}
}

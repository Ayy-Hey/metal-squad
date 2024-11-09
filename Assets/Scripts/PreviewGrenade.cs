using System;
using UnityEngine;

public class PreviewGrenade : CachingMonoBehaviour
{
	public void OnShoot(int IDWeapon, int Level)
	{
		this.IDWeapon = IDWeapon;
		float d = 430f * this.rigidbody2D.mass;
		this.rigidbody2D.AddForce(new Vector2(0.7f, 1f) * d);
		int num = Mathf.Clamp(Level / 10, 0, 2);
		this.imgGrenade.sprite = PopupManager.Instance.sprite_Grenade[num].Sprites[IDWeapon];
	}

	private void OnDisable()
	{
		switch (this.IDWeapon)
		{
		case 0:
			PreviewWeapon.Instance.CreateGrenadeBasic(this.transform.position).transform.localScale = Vector3.one * 2.5f;
			break;
		case 1:
			PreviewWeapon.Instance.enemy[0].ShowIce();
			PreviewWeapon.Instance.enemy[1].ShowIce();
			PreviewWeapon.Instance.CreateGrenadeIce(this.transform.position).transform.localScale = Vector3.one * 2.5f;
			break;
		case 2:
			PreviewWeapon.Instance.CreateGrenadeBasic(this.transform.position).transform.localScale = Vector3.one * 2.5f;
			PreviewWeapon.Instance.CreateFire(this.transform.position);
			break;
		case 3:
			PreviewWeapon.Instance.CreateGrenadeToxic(this.transform.position).transform.localScale = Vector3.one * 2.5f;
			PreviewWeapon.Instance.CreateFire2(this.transform.position);
			break;
		}
		PreviewWeapon.Instance.PoolPreviewGrenade.Store(this);
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.CompareTag("Ground"))
		{
			base.gameObject.SetActive(false);
		}
	}

	private int IDWeapon;

	public SpriteRenderer imgGrenade;
}

using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class BulletCt_UnitDamge : MonoBehaviour
	{
		public void Init(Action<Vector3, bool> attackCallback, bool main = false)
		{
			this.isMain = main;
			this.attackAction = attackCallback;
			base.gameObject.SetActive(true);
			for (int i = 0; i < this.bulletSprites.Length; i++)
			{
				this.bulletSprites[i].SetMaterial();
			}
			if (this.trail)
			{
				this.trail.Clear();
			}
			this.isInit = true;
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Obstacle") || coll.CompareTag("Rambo"))
			{
				return;
			}
			this.isInit = false;
			GameManager.Instance.fxManager.CreateFxBullet2(1, base.transform.position, 0, (float)((!this.isMain) ? 1 : 2), true);
			if (this.attackAction != null)
			{
				this.attackAction(base.transform.position, this.isMain);
			}
			base.gameObject.SetActive(false);
		}

		private Action<Vector3, bool> attackAction;

		[HideInInspector]
		public bool isInit;

		[SerializeField]
		private TrailRenderer trail;

		[SerializeField]
		private CustomBulletSpriteMaterial[] bulletSprites;

		private bool isMain;
	}
}

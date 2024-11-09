using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class PlayerFire : MonoBehaviour
	{
		private void Awake()
		{
			this._transform = base.transform;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
			{
				base.StartCoroutine(this.Fire());
			}
		}

		private IEnumerator Fire()
		{
			while (UnityEngine.Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
			{
				GameObject bullet = this.BulletPool.nextThing;
				bullet.transform.position = this.WeaponTip.position;
				bullet.transform.rotation = this._transform.rotation;
				float angle = this._transform.rotation.eulerAngles.y - 90f;
				float radians = angle * 0.0174532924f;
				Vector2 vForce = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)) * this.FireShakeForce;
				ProCamera2DShake.Instance.ApplyShakesTimed(new Vector2[]
				{
					vForce
				}, new Vector3[]
				{
					Vector3.zero
				}, new float[]
				{
					this.FireShakeDuration
				}, 0.1f, false);
				yield return new WaitForSeconds(this.FireRate);
			}
			yield break;
		}

		public Pool BulletPool;

		public Transform WeaponTip;

		public float FireRate = 0.3f;

		public float FireShakeForce = 0.2f;

		public float FireShakeDuration = 0.05f;

		private Transform _transform;
	}
}

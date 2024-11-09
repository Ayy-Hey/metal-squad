using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class PlayerHealth : MonoBehaviour
	{
		private void Awake()
		{
			this._renderers = base.GetComponentsInChildren<Renderer>();
			this._originalColor = this._renderers[0].material.color;
		}

		private void Hit(int damage)
		{
			this.Health -= damage;
			base.StartCoroutine(this.HitAnim());
			if (this.Health <= 0)
			{
			}
		}

		private IEnumerator HitAnim()
		{
			ProCamera2DShake.Instance.Shake("PlayerHit");
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].material.color = Color.white;
			}
			yield return new WaitForSeconds(0.05f);
			for (int j = 0; j < this._renderers.Length; j++)
			{
				this._renderers[j].material.color = this._originalColor;
			}
			yield break;
		}

		public int Health = 100;

		private Renderer[] _renderers;

		private Color _originalColor;
	}
}

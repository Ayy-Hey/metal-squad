using System;
using System.Collections;
using UnityEngine;

namespace Player
{
	public class PlayerHit : MonoBehaviour
	{
		public void OnShow()
		{
			if (this.isShow)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.anim.Play(0);
			this.isShow = true;
			base.StartCoroutine(this.OnAutoHide());
		}

		private IEnumerator OnAutoHide()
		{
			yield return new WaitForSeconds(0.3f);
			this.isShow = false;
			base.gameObject.SetActive(false);
			yield break;
		}

		public Animator anim;

		private bool isShow;
	}
}

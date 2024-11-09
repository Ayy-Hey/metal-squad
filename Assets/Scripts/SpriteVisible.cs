using System;
using UnityEngine;

public class SpriteVisible : CachingMonoBehaviour
{
	private void OnBecameInvisible()
	{
		if (this.baseEnemy != null)
		{
			this.baseEnemy.isInCamera = false;
		}
		if (this.OnInvisible != null)
		{
			this.OnInvisible();
		}
	}

	private void OnBecameVisible()
	{
		if (this.baseEnemy != null)
		{
			this.baseEnemy.isInCamera = true;
		}
		if (this.OnVisible != null)
		{
			this.OnVisible();
		}
	}

	[SerializeField]
	private BaseEnemy baseEnemy;

	public Action OnInvisible;

	public Action OnVisible;
}

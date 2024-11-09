using System;
using UnityEngine;

public class EffGunShop : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(0f, 0f, this.speed);
	}

	public float speed;
}

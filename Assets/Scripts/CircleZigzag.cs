using System;
using UnityEngine;

public class CircleZigzag : CachingMonoBehaviour
{
	public void Shoot(Vector3 pos, Vector3 direction, float Damage)
	{
	}

	public override void UpdateObject()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void OnDisable()
	{
	}

	private float Damage;

	private bool isShoot;
}

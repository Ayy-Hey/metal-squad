using System;
using UnityEngine;

public class DemoBullet2 : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.parent.OnHide(true);
	}

	public DemoBullet1 parent;
}

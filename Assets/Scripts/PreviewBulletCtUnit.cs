using System;
using UnityEngine;

public class PreviewBulletCtUnit : MonoBehaviour
{
	public void Init(Action<Vector3, bool> attackCallback, bool main = false)
	{
		this.isMain = main;
		this.attackAction = attackCallback;
		base.gameObject.SetActive(true);
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

	private bool isMain;
}

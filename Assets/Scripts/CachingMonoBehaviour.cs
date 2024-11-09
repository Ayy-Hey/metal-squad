using System;
using UnityEngine;

public class CachingMonoBehaviour : MonoBehaviour
{
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		this.rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	public virtual void InitObject()
	{
	}

	public virtual void UpdateObject()
	{
	}

	[NonSerialized]
	public new Transform transform;

	[NonSerialized]
	public Rigidbody2D rigidbody2D;
}

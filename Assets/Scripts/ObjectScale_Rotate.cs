using System;
using UnityEngine;

public class ObjectScale_Rotate : CachingMonoBehaviour
{
	private void Start()
	{
		if (this.type == ObjectScale_Rotate.Type.Scale)
		{
			this._scale = this.transform.localScale;
			this._scaleTo = this._scale * this.scaleTo;
		}
	}

	public void Update()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		ObjectScale_Rotate.Type type = this.type;
		if (type != ObjectScale_Rotate.Type.Rotate)
		{
			if (type == ObjectScale_Rotate.Type.Scale)
			{
				this.transform.localScale = Vector3.Slerp(this._scale, this._scaleTo, Mathf.PingPong(this.time, 1f) * this.speed);
				this.time += Time.deltaTime;
			}
		}
		else
		{
			this.transform.Rotate(0f, 0f, this.speed);
		}
	}

	public ObjectScale_Rotate.Type type;

	public float speed;

	public float scaleTo;

	private Vector3 _scale;

	private Vector3 _scaleTo;

	private float time;

	private bool isRun;

	public enum Type
	{
		Scale,
		Rotate
	}
}

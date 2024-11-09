using System;
using System.Collections.Generic;
using UnityEngine;

public class ThangMayMap4 : CachingMonoBehaviour
{
	public void Run(Vector3 to, Action callback)
	{
		this._target = to;
		this._callback = callback;
		this._oldPos = (this._pos = this.transform.position);
		this.isRunning = true;
	}

	public void SetColliderRun(bool isRun)
	{
		this.runColl.enabled = isRun;
		this.idleColl.enabled = !isRun;
	}

	public void ActiveStopRambo(bool isActive)
	{
		for (int i = 0; i < this.collsStopRambo.Length; i++)
		{
			this.collsStopRambo[i].enabled = isActive;
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isRunning)
		{
			return;
		}
		this._pos = Vector3.MoveTowards(this._pos, this._target, this.speed * deltaTime);
		switch (this.type)
		{
		case ThangMayMap4.Type.Down:
		case ThangMayMap4.Type.Cross_Down:
			this.transform.position = this._pos;
			this._deltaPos = this._pos - this._oldPos;
			this._oldPos = this._pos;
			for (int i = 0; i < this.listTfObjFollow.Count; i++)
			{
				if (this.listTfObjFollow[i].gameObject.activeSelf)
				{
					this._posObjFollow = this.listTfObjFollow[i].position;
					this._posObjFollow += this._deltaPos;
					this.listTfObjFollow[i].position = this._posObjFollow;
				}
				else
				{
					this.listTfObjFollow[i].parent = null;
					this.listTfObjFollow.RemoveAt(i);
				}
			}
			break;
		case ThangMayMap4.Type.Up:
		case ThangMayMap4.Type.Cross_Up:
			this._deltaPos = this._pos - this._oldPos;
			this._oldPos = this._pos;
			for (int j = 0; j < this.listTfObjFollow.Count; j++)
			{
				if (this.listTfObjFollow[j].gameObject.activeSelf)
				{
					this._posObjFollow = this.listTfObjFollow[j].position;
					this._posObjFollow += this._deltaPos;
					this.listTfObjFollow[j].position = this._posObjFollow;
				}
				else
				{
					this.listTfObjFollow[j].parent = null;
					this.listTfObjFollow.RemoveAt(j);
				}
			}
			this.transform.position = this._pos;
			break;
		}
		if (this._pos == this._target)
		{
			this.isRunning = false;
			try
			{
				this._callback();
			}
			catch
			{
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (this.listTfObjFollow == null)
		{
			this.listTfObjFollow = new List<Transform>();
		}
		if (this.maskObjFollow.Contains(collision.gameObject.layer))
		{
			try
			{
				Transform transform = collision.transform.GetComponentInParent<BaseEnemy>().transform;
				ThangMayMap4.Type type = this.type;
				if (type != ThangMayMap4.Type.Down)
				{
					if (type != ThangMayMap4.Type.Cross_Up)
					{
					}
				}
				if (!this.listTfObjFollow.Contains(transform))
				{
					this.listTfObjFollow.Add(transform);
				}
			}
			catch
			{
				if (!this.listTfObjFollow.Contains(collision.transform))
				{
					this.listTfObjFollow.Add(collision.transform);
				}
			}
		}
	}

	[HideInInspector]
	public bool isRunning;

	public ThangMayMap4.Type type;

	public float speed;

	public List<Transform> listTfObjFollow;

	public Collider2D[] collsStopRambo;

	public Collider2D runColl;

	public Collider2D idleColl;

	[SerializeField]
	private LayerMask maskObjFollow;

	private Action _callback;

	private Vector3 _pos;

	private Vector3 _posObjFollow;

	private Vector3 _oldPos;

	private Vector3 _deltaPos;

	private Vector3 _target;

	public enum Type
	{
		Down,
		Up,
		Cross_Down,
		Cross_Up
	}
}

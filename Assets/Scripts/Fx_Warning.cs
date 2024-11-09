using System;
using UnityEngine;

public class Fx_Warning : CachingMonoBehaviour
{
	public void Init(float time, Vector3 pos, Fx_Warning.CameraLock camLock, Action<Fx_Warning> callback)
	{
		this._camLock = camLock;
		this._time = time;
		this._timeLine = 0f;
		this.callback = callback;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		Fx_Warning.CameraLock camLock2 = this._camLock;
		if (camLock2 != Fx_Warning.CameraLock.X)
		{
			if (camLock2 == Fx_Warning.CameraLock.Y)
			{
				this._lockValue = pos.y - CameraController.Instance.camPos.y;
			}
		}
		else
		{
			this._lockValue = pos.x - CameraController.Instance.camPos.x;
		}
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		Fx_Warning.CameraLock camLock = this._camLock;
		if (camLock != Fx_Warning.CameraLock.X)
		{
			if (camLock == Fx_Warning.CameraLock.Y)
			{
				this._pos = this.transform.position;
				this._pos.y = CameraController.Instance.camPos.y + this._lockValue;
				this.transform.position = this._pos;
			}
		}
		else
		{
			this._pos = this.transform.position;
			this._pos.x = CameraController.Instance.camPos.x + this._lockValue;
			this.transform.position = this._pos;
		}
		this._timeLine += deltaTime;
		if (this._timeLine >= this._time)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			if (this.callback != null)
			{
				this.callback(this);
			}
		}
		catch
		{
		}
	}

	public bool isInit;

	private float _time;

	private Action<Fx_Warning> callback;

	private float _timeLine;

	private Fx_Warning.CameraLock _camLock;

	private float _lockValue;

	private Vector3 _pos;

	public enum CameraLock
	{
		None,
		X,
		Y
	}
}

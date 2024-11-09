using System;
using UnityEngine;

public class BayCau : CachingMonoBehaviour
{
	private void OnValidate()
	{
		this.myRenderer = base.GetComponent<Renderer>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Rambo"))
		{
			if (this.selfControll)
			{
				this.isRun = true;
			}
			else
			{
				this.controllerBay.isRun = true;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			if (this.selfControll)
			{
				this.isRun = true;
			}
			else
			{
				this.controllerBay.isRun = true;
			}
		}
	}

	public void OnStart()
	{
		this._finished = false;
		BayCau.Style style = this.style;
		if (style != BayCau.Style.Falling)
		{
			if (style != BayCau.Style.Moving)
			{
				if (style == BayCau.Style.Spinning)
				{
					this._startEulerZ = this.transform.eulerAngles.z;
				}
			}
			else
			{
				this._startPoint = this.transform.position;
				this._time = 0f;
			}
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (this.followRambo)
		{
			float num = this.transform.position.x - GameManager.Instance.player.transform.position.x;
			this.isRun = (num <= this.distance);
		}
		if (!this.isRun)
		{
			return;
		}
		if (this._finished)
		{
			return;
		}
		if (this.delayTime > 0f)
		{
			this.OnDelay(deltaTime);
			this.delayTime -= deltaTime;
			return;
		}
		if (!this._isRunning)
		{
			this._isRunning = true;
			this.PreRunning();
		}
		else
		{
			this.Running(deltaTime);
		}
	}

	private void OnDelay(float deltaTime)
	{
		BayCau.Style style = this.style;
		if (style != BayCau.Style.Falling)
		{
			if (style != BayCau.Style.Moving)
			{
				if (style != BayCau.Style.Spinning)
				{
				}
			}
		}
		else
		{
			this.OnDelayFalling(deltaTime);
		}
	}

	private void PreRunning()
	{
		BayCau.Style style = this.style;
		if (style != BayCau.Style.Falling)
		{
			if (style != BayCau.Style.Moving)
			{
				if (style == BayCau.Style.Spinning)
				{
					this.PreSpinning();
				}
			}
			else
			{
				this.PreMoving();
			}
		}
		else
		{
			this.PreFalling();
		}
		if (this.nextBay)
		{
			this.nextBay.delayTime = this.nextDelayTime;
			this.nextBay.isRun = true;
		}
		if (this.OnActionStart != null)
		{
			this.OnActionStart(this);
		}
	}

	private void Running(float deltaTime)
	{
		BayCau.Style style = this.style;
		if (style != BayCau.Style.Falling)
		{
			if (style != BayCau.Style.Moving)
			{
				if (style == BayCau.Style.Spinning)
				{
					this.Spinning(deltaTime);
				}
			}
			else
			{
				this.Moving(deltaTime);
			}
		}
		else
		{
			this.Falling(deltaTime);
		}
	}

	private void OnDelayFalling(float deltaTime)
	{
		if (this.vibrating)
		{
			this._euler = this.transform.eulerAngles;
			float t = Mathf.PingPong(Time.time / this.vibratingDeltaTime, 1f);
			this._euler.z = Mathf.Lerp(-this.vibratingAmount, this.vibratingAmount, t);
			this.transform.eulerAngles = this._euler;
		}
	}

	private void PreFalling()
	{
		this._speedFall = this.speedFall;
	}

	private void Falling(float deltaTime)
	{
		this.transform.Translate(0f, this._speedFall * deltaTime, 0f);
		this._speedFall += this.gravity * deltaTime;
	}

	private void PreMoving()
	{
		float num = Vector3.Distance(this.transform.position, this._startPoint);
		float num2 = Vector3.Distance(this.transform.position, this.targetMovePoint);
		if (num2 > num)
		{
			this._targetMovePoint = this.targetMovePoint;
		}
		else
		{
			this._targetMovePoint = this._startPoint;
		}
		this._speedMove = Vector3.Distance(this.transform.position, this._targetMovePoint) / this.runTime;
	}

	private void Moving(float deltaTime)
	{
		if (this._pingPongDelayTime > 0f)
		{
			this._pingPongDelayTime -= deltaTime;
			if (this._pingPongDelayTime <= 0f)
			{
				this.PreMoving();
			}
			return;
		}
		if (this.isPingPong && this.pingPongDelayTime == 0f)
		{
			float t = Mathf.PingPong(this._time / this.runTime, 1f);
			this._pos = this.transform.position;
			this._pos.x = Mathf.SmoothStep(this._startPoint.x, this.targetMovePoint.x, t);
			this._pos.y = Mathf.SmoothStep(this._startPoint.y, this.targetMovePoint.y, t);
			this.transform.position = this._pos;
			this._time += deltaTime;
		}
		else
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetMovePoint, this._speedMove * deltaTime);
			if (this.transform.position == this._targetMovePoint)
			{
				if (this.isPingPong)
				{
					this._pingPongDelayTime = this.pingPongDelayTime;
					this._isRunning = false;
				}
				else
				{
					this._finished = true;
				}
			}
		}
	}

	private void PreSpinning()
	{
		this._euler = this.transform.eulerAngles;
		if (this._euler.z == this._startEulerZ)
		{
			this._targetEulerZ = this.targetEulerZ;
		}
		else
		{
			this._targetEulerZ = this._startEulerZ;
		}
		this._speedSpin = Mathf.Abs(this._euler.z - this._targetEulerZ) / this.runTime;
	}

	private void Spinning(float deltaTime)
	{
		if (this._pingPongDelayTime > 0f)
		{
			this._pingPongDelayTime -= deltaTime;
			return;
		}
		this._euler.z = Mathf.MoveTowards(this._euler.z, this._targetEulerZ, this._speedSpin * deltaTime);
		this.transform.eulerAngles = this._euler;
		if (this._euler.z == this._targetEulerZ)
		{
			if (this.isPingPong)
			{
				this._pingPongDelayTime = this.pingPongDelayTime;
				this._isRunning = false;
			}
			else
			{
				this._finished = true;
			}
		}
	}

	public Action<BayCau> OnActionStart;

	public bool isRun;

	public Renderer myRenderer;

	public bool followRambo;

	public float distance;

	public float delayTime;

	public float runTime;

	public bool isPingPong;

	public float pingPongDelayTime;

	public bool vibrating;

	public float vibratingDeltaTime;

	public float vibratingAmount;

	public bool selfControll = true;

	public bool hasNext;

	public BayCau nextBay;

	public BayCau controllerBay;

	public float nextDelayTime;

	public BayCau.Style style;

	public float targetEulerZ;

	public float gravity = -9.82f;

	public float speedFall;

	public Vector3 targetMovePoint;

	private bool _isRunning;

	private bool _finished;

	private float _pingPongDelayTime;

	private Vector3 _pos;

	private float _speedSpin;

	private float _startEulerZ;

	private Vector3 _euler;

	private float _targetEulerZ;

	private float _speedFall;

	private float _time;

	private Vector3 _startPoint;

	private float _speedMove;

	private Vector3 _targetMovePoint;

	public enum Style
	{
		Spinning,
		Falling,
		Moving
	}
}

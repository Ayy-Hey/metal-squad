using System;
using Spine.Unity;
using UnityEngine;

public class BaseCharactor : CachingMonoBehaviour, IMoving
{
	public Vector2 Origin()
	{
		return new Vector2(this.tfOrigin.localPosition.x + this.GetPosition().x, this.tfOrigin.localPosition.y + this.GetPosition().y);
	}

	public Vector2 GetPosition()
	{
		Vector2 zero = Vector2.zero;
		if (this.transform != null && base.gameObject.activeSelf)
		{
			zero = new Vector2(this.transform.position.x, this.transform.position.y);
		}
		return zero;
	}

	public BaseCharactor.EMovementBasic GetState()
	{
		return this.EMovement;
	}

	public float HPCurrent
	{
		get
		{
			return this.HP;
		}
		set
		{
			this.HP = value;
		}
	}

	private void UpdateBaseCharacter(float deltaTime)
	{
		this.timeSpeed += deltaTime;
		if (this.timeSpeed >= 5f && this.active_Count_Down)
		{
			this.Normal_Speed();
			this.active_Count_Down = false;
		}
		if (this.TIME_FREEZE > 0f)
		{
			this.TIME_FREEZE -= deltaTime;
			if (this.TIME_FREEZE <= 0f)
			{
				this.UnFreezeNoSlow();
			}
		}
	}

	public virtual void OnUpdate(float deltaTime)
	{
		this.UpdateBaseCharacter(deltaTime);
	}

	public void UnFreeze()
	{
		if (this.EMovement != BaseCharactor.EMovementBasic.FREEZE || this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		this.EMovement = BaseCharactor.EMovementBasic.Release;
		LeanTween.cancel(base.gameObject);
		this.DownSpeed();
	}

	public void Freeze(Vector3 pos, float speed, float TimeEndSlow, float speed_down)
	{
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		this.TIME_END_SLOW = TimeEndSlow;
		this.SPEED_DOWN = speed_down;
		float num = Vector3.Distance(pos, this.transform.position);
		float time = num / speed;
		LeanTween.move(base.gameObject, pos, time);
		this.IsGround = false;
		this.EMovement = BaseCharactor.EMovementBasic.FREEZE;
	}

	public void FreezeNoSlow(float TimeFreeze)
	{
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		this.TIME_FREEZE = TimeFreeze;
		this.rigidbody2D.velocity = Vector2.zero;
		this.rigidbody2D.gravityScale = 0f;
		this.EMovement = BaseCharactor.EMovementBasic.FREEZE;
	}

	public void UnFreezeNoSlow()
	{
		if (this.EMovement != BaseCharactor.EMovementBasic.FREEZE || this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		this.EMovement = BaseCharactor.EMovementBasic.Release;
		this.rigidbody2D.gravityScale = 3f;
	}

	public virtual void DownSpeed()
	{
		this.timeSpeed = 0f;
		this.active_Count_Down = true;
	}

	public virtual void Normal_Speed()
	{
	}

	public BaseCharactor.EMovementBasic EMovement = BaseCharactor.EMovementBasic.Release;

	private float TIME_END_SLOW = 5f;

	private float SPEED_DOWN = 0.3f;

	private float TIME_FREEZE;

	public SkeletonAnimation skeletonAnimation;

	public Transform tfOrigin;

	public float Radius;

	public MeshRenderer meshRenderer;

	public bool IsMainCharater;

	public bool IsGround;

	public bool isDetectedGround;

	private float HP;

	private float timeSpeed;

	private bool active_Count_Down;

	public bool isRunForward = true;

	public enum EMovementBasic
	{
		Left,
		Right,
		Sit,
		Release,
		DIE,
		FREEZE
	}
}

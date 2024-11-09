using System;
using System.Collections;
using SWS;
using UnityEngine;

public class NPC_FakeControl : MonoBehaviour
{
	private IEnumerator Start()
	{
		this.spline = UnityEngine.Object.FindObjectOfType<splineMove>();
		this.splineTransform = this.spline.gameObject.transform;
		yield return new WaitUntil(() => GameManager.Instance.NPC != null);
		this.NPC_Player = GameManager.Instance.NPC.GetComponent<PlayerMain>();
		this.NPC_Transform = GameManager.Instance.NPC.transform;
		yield return new WaitUntil(() => this.NPC_Player.IsInit);
		this.IsInit = true;
		yield break;
	}

	public void OnUpdate()
	{
		if (!this.IsInit)
		{
			return;
		}
		this.RunTimer(Time.deltaTime);
		this.KeepDistanceToMainPlayer();
		if (this.IsIdle)
		{
			return;
		}
		if (this.IsSit)
		{
			this.OnSit();
			return;
		}
		if (this.IsMove())
		{
			this.OnMove();
			return;
		}
	}

	private void KeepDistanceToMainPlayer()
	{
		if (Vector2.Distance(this.NPC_Transform.position, GameManager.Instance.player.transform.position) >= this.MaxDistanceToMainPlayer)
		{
			this.IsIdle = false;
			this.IsSit = false;
		}
	}

	private IEnumerator FakeTouchJump()
	{
		this.Jump(true);
		yield return new WaitForSeconds(0.5f);
		this.Jump(false);
		yield break;
	}

	public void Jump(bool isJump)
	{
		this.NPC_Player._PlayerInput.OnJump(isJump);
		if (!isJump)
		{
			UnityEngine.Debug.Log("++++++++++ Start check stuck timer after jump");
			this.LastNPCPosition = this.NPC_Transform.position;
			this.StartTimer(3f, new Action(this.OnCheckStuckFinished));
		}
	}

	private void OnCheckStuckFinished()
	{
		if (Vector2.Distance(this.LastNPCPosition, this.NPC_Transform.position) < 2f)
		{
			UnityEngine.Debug.Log("+++++++++ NPC is stuck -> Jump now");
			base.StartCoroutine(this.FakeTouchJump());
		}
	}

	private void StartTimer(float countdown, Action timeoutFunc)
	{
		this.OnCountdownTimerHasExpired = timeoutFunc;
		this.Countdown = countdown;
		this.isTimerRunning = true;
	}

	private void RunTimer(float deltaTime)
	{
		if (!this.isTimerRunning)
		{
			return;
		}
		this.Countdown -= deltaTime;
		if (this.Countdown > 0f)
		{
			return;
		}
		this.isTimerRunning = false;
		if (this.OnCountdownTimerHasExpired != null)
		{
			this.OnCountdownTimerHasExpired();
			this.OnCountdownTimerHasExpired = null;
		}
	}

	public void Sit(bool IsSit)
	{
		this.IsSit = IsSit;
	}

	private void OnSit()
	{
		this.NPC_Player._PlayerInput.OnMove(new Vector2(0.2f, -1f));
	}

	private bool IsMove()
	{
		return this.spline.state == splineMove.MoveState.PLAYING;
	}

	private void OnMove()
	{
		Vector2 vector = this.splineTransform.position - this.NPC_Transform.position;
		vector.y = 0f;
		if (vector.magnitude > 0.5f)
		{
			this.NPC_Player._PlayerInput.OnMove(Vector2.ClampMagnitude(vector, 1f));
		}
	}

	public void SetIdle(bool IsIdle)
	{
		this.IsIdle = IsIdle;
		if (IsIdle)
		{
			this.OnIdle();
		}
	}

	private void OnIdle()
	{
		this.NPC_Player._PlayerInput.OnMoveEnd();
	}

	public void SetShoot(bool IsShooting)
	{
		if (IsShooting)
		{
			this.NPC_Player._PlayerInput.OnShootDown();
		}
		else
		{
			this.NPC_Player._PlayerInput.OnShootUp();
		}
	}

	public void ShowSkill()
	{
		GameManager.Instance.skillManager.ShowSkill(this.NPC_Player, this.NPC_Player.IDChar);
	}

	private splineMove spline;

	private Transform splineTransform;

	private PlayerMain NPC_Player;

	private Transform NPC_Transform;

	private bool IsInit;

	private bool IsSit;

	private bool IsIdle;

	private float MaxDistanceToMainPlayer = 5f;

	private Vector2 LastNPCPosition;

	public Action OnCountdownTimerHasExpired;

	private bool isTimerRunning;

	private float Countdown = 5f;
}

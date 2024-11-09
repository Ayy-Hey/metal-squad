using System;
using System.Collections;
using SWS;
using UnityEngine;

public class NPC_ActionBox : MonoBehaviour
{
	private void Start()
	{
		if (this.Action == NPC_ActionBox.NPC_Action.RANDOM)
		{
			this.Action = (NPC_ActionBox.NPC_Action)UnityEngine.Random.Range(0, 5);
			this.SitTime = 5f;
			this.IdleTime = 5f;
			this.ShootTime = 5f;
		}
		this.spline = UnityEngine.Object.FindObjectOfType<splineMove>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		PlayerMain component = col.gameObject.GetComponent<PlayerMain>();
		NPC_FakeControl component2 = col.gameObject.GetComponent<NPC_FakeControl>();
		if (component == null || component2 == null)
		{
			UnityEngine.Debug.Log("Player is null");
			return;
		}
		if (!component.isNPC)
		{
			UnityEngine.Debug.Log("Player is not NPC");
			return;
		}
		switch (this.Action)
		{
		case NPC_ActionBox.NPC_Action.JUMP:
			base.StartCoroutine(this.FakeTouchJump(component2));
			break;
		case NPC_ActionBox.NPC_Action.SIT:
			base.StartCoroutine(this.FakeSit(component2, this.SitTime));
			break;
		case NPC_ActionBox.NPC_Action.IDLE:
			base.StartCoroutine(this.FakeIdle(component2, this.IdleTime));
			break;
		case NPC_ActionBox.NPC_Action.SHOOT:
			base.StartCoroutine(this.FakeShoot(component2, this.ShootTime));
			break;
		case NPC_ActionBox.NPC_Action.SKILL:
			this.FakeShowSkill(component2);
			break;
		}
	}

	private IEnumerator FakeTouchJump(NPC_FakeControl fakeControl)
	{
		fakeControl.Jump(true);
		yield return new WaitForSeconds(0.5f);
		fakeControl.Jump(false);
		yield break;
	}

	private IEnumerator FakeSit(NPC_FakeControl fakeControl, float seconds)
	{
		fakeControl.Sit(true);
		yield return new WaitForSeconds(seconds);
		fakeControl.Sit(false);
		base.gameObject.SetActive(false);
		yield break;
	}

	private IEnumerator FakeIdle(NPC_FakeControl fakeControl, float seconds)
	{
		fakeControl.SetIdle(true);
		yield return new WaitForSeconds(seconds);
		fakeControl.SetIdle(false);
		base.gameObject.SetActive(false);
		yield break;
	}

	private IEnumerator FakeShoot(NPC_FakeControl fakeControl, float seconds)
	{
		fakeControl.SetShoot(true);
		yield return new WaitForSeconds(seconds);
		fakeControl.SetShoot(false);
		yield break;
	}

	private void FakeShowSkill(NPC_FakeControl fakeControl)
	{
		fakeControl.ShowSkill();
		base.gameObject.SetActive(false);
	}

	public NPC_ActionBox.NPC_Action Action;

	public float SitTime;

	public float IdleTime;

	public float ShootTime;

	private splineMove spline;

	public enum NPC_Action
	{
		JUMP,
		SIT,
		IDLE,
		SHOOT,
		SKILL,
		RANDOM
	}
}

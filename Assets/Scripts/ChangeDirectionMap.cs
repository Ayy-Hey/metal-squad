using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class ChangeDirectionMap : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.triger.TriggerTarget = GameManager.Instance.player.transform;
		BaseTrigger baseTrigger = this.triger;
		baseTrigger.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger.OnEnteredTrigger, new Action(delegate()
		{
			EventDispatcher.PostEvent<MyMessage>("Show_Anim_Gogo", new MyMessage
			{
				IntValue = (int)this.direction
			});
		}));
		yield break;
	}

	public BaseTrigger triger;

	public ChangeDirectionMap.EDIRECTION direction = ChangeDirectionMap.EDIRECTION.RIGHT;

	public enum EDIRECTION
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
}

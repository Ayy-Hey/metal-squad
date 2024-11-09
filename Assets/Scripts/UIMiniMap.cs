using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.mmPlayer.parent.gameObject.SetActive(GameMode.Instance.modePlay == GameMode.ModePlay.Campaign || GameMode.Instance.modePlay == GameMode.ModePlay.Special_Campaign);
		bool isBoss = GameManager.Instance.bossManager.Boss != null;
		this.imgIcon.sprite = this.spriteIcons[(!isBoss) ? 1 : 0];
		this.isInit = true;
		yield break;
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		this.ProgressMiniMap();
	}

	private void ProgressMiniMap()
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.BEGIN || GameManager.Instance.StateManager.EState == EGamePlay.NONE)
		{
			return;
		}
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		float num;
		if (orientaltion != CameraController.Orientation.VERTICAL)
		{
			num = GameManager.Instance.player.GetPosition().x;
		}
		else
		{
			num = Mathf.Abs(GameManager.Instance.player.GetPosition().y);
		}
		float num2 = Mathf.Abs(num / GameManager.Instance.hudManager.Map_Length);
		if (num2 > this.percent_distance_completed)
		{
			this.percent_distance_completed = Mathf.Min(num2, 0.99f);
		}
		float value = num2 * 168f;
		this.mmPlayer.anchoredPosition = new Vector2(Mathf.Clamp(value, 0f, 168f), 0f);
	}

	private bool isInit;

	public float percent_distance_completed;

	public RectTransform mmPlayer;

	public List<Sprite> spriteIcons;

	public Image imgIcon;
}

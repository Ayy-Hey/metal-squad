using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Level16Time : MonoBehaviour
{
	private void Start()
	{
		this.strBuilderTime = new StringBuilder();
		PreGameOver.Instance.OnEnded = delegate()
		{
			this.objTime.SetActive(true);
			this.isInit = true;
			this.timeCountdown = this.TimeEnd;
		};
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.timeCountdown <= 0f)
		{
			if (!this.GameOver)
			{
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.StateManager.SetPreview();
				this.GameOver = true;
				LeanTween.moveX(this.objBoss, this.tfLastPos.position.x, 2f).setOnComplete(delegate()
				{
					EventDispatcher.PostEvent("LostGame");
				});
			}
			return;
		}
		this.timeCountdown -= Time.deltaTime;
		int num = (int)(this.timeCountdown / 60f);
		int num2 = (int)(this.timeCountdown - (float)(num * 60));
		this.strBuilderTime.Length = 0;
		if (num < 10)
		{
			this.strBuilderTime.Append("0");
		}
		this.strBuilderTime.Append(num);
		this.strBuilderTime.Append(":");
		if (num2 < 10)
		{
			this.strBuilderTime.Append("0");
		}
		this.strBuilderTime.Append(num2);
		this.txtTime.text = this.strBuilderTime.ToString();
	}

	public float TimeEnd = 180f;

	public Text txtTime;

	public GameObject objTime;

	private bool isInit;

	private float timeCountdown;

	private StringBuilder strBuilderTime;

	private bool GameOver;

	public GameObject objBoss;

	public Transform tfLastPos;
}

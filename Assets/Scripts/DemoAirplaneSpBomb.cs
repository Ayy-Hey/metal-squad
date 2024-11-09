using System;
using UnityEngine;

public class DemoAirplaneSpBomb : DemoBaseObject
{
	private void OnEnable()
	{
		base.Init();
		this.run = true;
	}

	private void Update()
	{
		if (this.run)
		{
			base.transform.Translate(Time.deltaTime * this.speed, 0f, 0f);
			this.bombTimer += Time.deltaTime;
			if (this.bombTimer >= 1f)
			{
				this.bombTimer = 0f;
				this.bomb = DemoSpecialSkillManager.instance.bombPool.New();
				if (this.bomb != null)
				{
					this.bomb.transform.localPosition = base.transform.localPosition;
					this.bomb.gameObject.SetActive(true);
				}
			}
			if (base.transform.localPosition.x > this.endX)
			{
				this.run = false;
			}
		}
	}

	public float speed;

	public float endX;

	private bool run;

	private float bombTimer;

	private DemoBomb bomb;
}

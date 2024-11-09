using System;
using UnityEngine;

public class OptionControl1 : OptionControl
{
	public override void Show()
	{
		base.Show();
	}

	public override void ShowWithTutorial()
	{
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			base.ShowWithTutorial();
			switch (TutorialGamePlay.Step)
			{
			case 0:
				for (int i = 0; i < this.UButtons.Length; i++)
				{
					Color baseColor = this.UButtons[i].baseColor;
					baseColor.a = 0f;
					this.UButtons[i].UpdateBaseColor(baseColor);
				}
				break;
			case 1:
			{
				for (int j = 0; j < this.UButtons.Length; j++)
				{
					Color baseColor2 = this.UButtons[j].baseColor;
					baseColor2.a = 0f;
					this.UButtons[j].UpdateBaseColor(baseColor2);
				}
				Color baseColor3 = this.UButtons[2].baseColor;
				baseColor3.a = 0.5f;
				this.UButtons[2].UpdateBaseColor(baseColor3);
				break;
			}
			case 2:
			{
				for (int k = 0; k < this.UButtons.Length; k++)
				{
					Color baseColor4 = this.UButtons[k].baseColor;
					baseColor4.a = 0f;
					this.UButtons[k].UpdateBaseColor(baseColor4);
				}
				Color baseColor5 = this.UButtons[2].baseColor;
				baseColor5.a = 0.5f;
				this.UButtons[2].UpdateBaseColor(baseColor5);
				Color baseColor6 = this.UButtons[4].baseColor;
				baseColor6.a = 0.5f;
				this.UButtons[4].UpdateBaseColor(baseColor6);
				break;
			}
			case 3:
				for (int l = 0; l < this.UButtons.Length; l++)
				{
					Color baseColor7 = this.UButtons[l].baseColor;
					baseColor7.a = 0.5f;
					this.UButtons[l].UpdateBaseColor(baseColor7);
				}
				break;
			}
		}
	}

	public void TouchFire(bool down)
	{
		if (down)
		{
			GameManager.Instance.player._PlayerInput.OnShootDown();
		}
		else
		{
			GameManager.Instance.player._PlayerInput.OnShootUp();
		}
	}
}

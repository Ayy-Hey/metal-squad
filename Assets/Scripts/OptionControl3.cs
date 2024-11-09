using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionControl3 : OptionControl
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
			Image component = this.joysticks[1].joystickBase.GetComponent<Image>();
			Image component2 = this.joysticks[1].joystick.GetComponent<Image>();
			switch (TutorialGamePlay.Step)
			{
			case 0:
				component.color = new Color(1f, 1f, 1f, 0f);
				component2.color = new Color(1f, 1f, 1f, 0f);
				for (int i = 0; i < this.UButtons.Length; i++)
				{
					Color baseColor = this.UButtons[i].baseColor;
					baseColor.a = 0f;
					this.UButtons[i].UpdateBaseColor(baseColor);
				}
				break;
			case 1:
			{
				component.color = new Color(1f, 1f, 1f, 0f);
				component2.color = new Color(1f, 1f, 1f, 0f);
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
				component.color = new Color(1f, 1f, 1f, 0.5f);
				component2.color = new Color(1f, 1f, 1f, 0.5f);
				for (int k = 0; k < this.UButtons.Length; k++)
				{
					Color baseColor4 = this.UButtons[k].baseColor;
					baseColor4.a = 0f;
					this.UButtons[k].UpdateBaseColor(baseColor4);
				}
				Color baseColor5 = this.UButtons[2].baseColor;
				baseColor5.a = 0.5f;
				this.UButtons[2].UpdateBaseColor(baseColor5);
				break;
			}
			case 3:
				for (int l = 0; l < this.UButtons.Length; l++)
				{
					Color baseColor6 = this.UButtons[l].baseColor;
					baseColor6.a = 0.5f;
					this.UButtons[l].UpdateBaseColor(baseColor6);
				}
				break;
			}
		}
	}
}

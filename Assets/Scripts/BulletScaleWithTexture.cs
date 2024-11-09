using System;
using UnityEngine;

public class BulletScaleWithTexture : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.mesh)
		{
			this.mesh = base.GetComponent<MeshRenderer>();
		}
		if (this.texture)
		{
			this.ResetScale();
		}
	}

	public void SetTexture(Texture tex)
	{
		this.CheckStart();
		Material material = this.mesh.material;
		this.texture = tex;
		material.mainTexture = tex;
		this.ResetScale();
	}

	private void ResetScale()
	{
		this.scale = base.transform.localScale;
		this.scale.x = (float)this.texture.width / 100f;
		this.scale.y = (float)this.texture.height / 100f;
		base.transform.localScale = this.scale;
	}

	private void OnBecameInvisible()
	{
		try
		{
			base.transform.parent.gameObject.SetActive(false);
		}
		catch
		{
		}
	}

	private void CheckStart()
	{
		if (!this.shaderSoft)
		{
			this.shaderSoft = Shader.Find("Mobile/Particles/Additive");
		}
		if (!this.shaderNonSoft)
		{
			this.shaderNonSoft = Shader.Find("Sprites/Default");
		}
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay != GameMode.ModePlay.Campaign)
		{
			if (modePlay != GameMode.ModePlay.Boss_Mode)
			{
				this.SetShaderSoft();
			}
			else
			{
				EBoss bossCurrent = ProfileManager.bossCurrent;
				switch (bossCurrent)
				{
				case EBoss.Spider_Toxic:
				case EBoss.War_Bunker:
				case EBoss.Super_Spider:
				case EBoss.Mechanical_Scorpion:
					break;
				default:
					if (bossCurrent != EBoss.Schneider_G25 && bossCurrent != EBoss.Ultron)
					{
						this.SetShaderSoft();
						goto IL_142;
					}
					break;
				}
				this.SetShaderNonSoft();
				IL_142:;
			}
		}
		else
		{
			switch (GameManager.Instance.Level)
			{
			case ELevel.LEVEL_13:
			case ELevel.LEVEL_14:
			case ELevel.LEVEL_15:
			case ELevel.LEVEL_16:
			case ELevel.LEVEL_17:
			case ELevel.LEVEL_18:
			case ELevel.LEVEL_19:
			case ELevel.LEVEL_20:
			case ELevel.LEVEL_21:
			case ELevel.LEVEL_22:
			case ELevel.LEVEL_23:
			case ELevel.LEVEL_24:
			case ELevel.LEVEL_25:
			case ELevel.LEVEL_26:
			case ELevel.LEVEL_27:
			case ELevel.LEVEL_28:
			case ELevel.LEVEL_29:
			case ELevel.LEVEL_30:
			case ELevel.LEVEL_31:
			case ELevel.LEVEL_32:
			case ELevel.LEVEL_33:
			case ELevel.LEVEL_34:
			case ELevel.LEVEL_35:
			case ELevel.LEVEL_36:
				this.SetShaderNonSoft();
				break;
			default:
				this.SetShaderSoft();
				break;
			}
		}
	}

	private void SetShaderSoft()
	{
		if (!this.useShaderSoft)
		{
			this.useShaderSoft = true;
			this.mesh.material.shader = this.shaderSoft;
		}
	}

	private void SetShaderNonSoft()
	{
		if (this.useShaderSoft)
		{
			this.useShaderSoft = false;
			this.mesh.material.shader = this.shaderNonSoft;
		}
	}

	public MeshRenderer mesh;

	public Texture texture;

	private Vector3 scale;

	private Shader shaderSoft;

	private Shader shaderNonSoft;

	private bool useShaderSoft = true;
}

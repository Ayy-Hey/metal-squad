using System;
using UnityEngine;
using UnityEngine.UI;

public class EffSelectLevel : MonoBehaviour
{
	public void Show(Transform parent, bool isBoss = false)
	{
		if (isBoss)
		{
			this.imgEff.sprite = this.spriteEff[1];
		}
		else
		{
			this.imgEff.sprite = this.spriteEff[0];
		}
		base.gameObject.SetActive(true);
		base.transform.parent = parent;
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = Vector3.one;
		this.particle.startColor = ((!isBoss) ? this.nomalColor : this.bossColor);
	}

	private void Update()
	{
		this.imgEff.transform.Rotate(0f, 0f, 1f);
	}

	public void Hide()
	{
		base.transform.parent = this.tranf_parent;
		base.transform.localPosition = Vector3.zero;
		base.gameObject.SetActive(false);
	}

	public Image imgEff;

	public Sprite[] spriteEff;

	public ParticleSystem particle;

	public Transform tranf_parent;

	public Color bossColor;

	public Color nomalColor;
}

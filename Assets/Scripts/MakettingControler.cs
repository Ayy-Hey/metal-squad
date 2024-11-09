using System;
using UnityEngine;
using UnityEngine.UI;

public class MakettingControler : MonoBehaviour
{
	private void OnEnable()
	{
		if (!SplashScreen._isBuildMarketing)
		{
			if (this.imgBtnChangeBG)
			{
				this.imgBtnChangeBG.gameObject.SetActive(false);
			}
			base.gameObject.SetActive(false);
			return;
		}
		if (GameManager.Instance)
		{
			if (this.meshs != null && this.meshs.Length > 0)
			{
				for (int i = 0; i < this.meshs.Length; i++)
				{
					this.meshs[i].material.mainTexture = ((MakettingControler.eBG != MakettingControler.EBG.Blue) ? this.textureGreen : this.textureBlue);
				}
			}
		}
		else
		{
			this.ShowBtnChange();
		}
	}

	public void ChangeBG()
	{
		if (!SplashScreen._isBuildMarketing)
		{
			return;
		}
		MakettingControler.eBG = ((MakettingControler.eBG != MakettingControler.EBG.Blue) ? MakettingControler.EBG.Blue : MakettingControler.EBG.Green);
		this.ShowBtnChange();
	}

	private void ShowBtnChange()
	{
		if (this.imgBtnChangeBG)
		{
			this.imgBtnChangeBG.color = ((MakettingControler.eBG != MakettingControler.EBG.Blue) ? Color.green : Color.blue);
		}
		if (this.txtBtnChangBG)
		{
			this.txtBtnChangBG.text = MakettingControler.eBG.ToString();
		}
	}

	public static MakettingControler.EBG eBG;

	public Image imgBtnChangeBG;

	public Text txtBtnChangBG;

	public Texture2D textureBlue;

	public Texture2D textureGreen;

	public MeshRenderer[] meshs;

	public enum EBG
	{
		Green,
		Blue,
		White
	}
}

using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UILevelCard : MonoBehaviour
{
	private void Start()
	{
		if (this.ObjPrice)
		{
			this.ObjPrice.SetActive(!ProfileManager.InforChars[2].IsUnLocked);
		}
	}

	public void LerpScale(float scale)
	{
		Vector3 b = Vector3.one * scale;
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, b, Time.deltaTime * 10f);
	}

	public void SetStar(int number)
	{
		for (int i = 0; i < Mathf.Min(3, number); i++)
		{
			this.Stars[i].enabled = true;
		}
	}

	public void SetUnLock(bool isUnlocked)
	{
		this.Locked.SetActive(!isUnlocked);
	}

	public void LerpBGShadow(bool center)
	{
		this.isCenter = center;
		Color color = new Color(1f, 1f, 1f, 0f);
		Color color2 = new Color(1f, 1f, 1f, 0.4f);
		this.BGShadow.color = Color.Lerp(this.BGShadow.color, (!center) ? color2 : color, Time.time);
	}

	public void LerpColor(bool center)
	{
		this.isCenter = center;
		Color color = new Color(1f, 1f, 1f, 1f);
		Color color2 = new Color(0.5f, 0.5f, 0.5f, 1f);
		this.charactor.color = Color.Lerp(this.charactor.color, (!center) ? color2 : color, Time.time);
		if (this.skeleton_Lock.gameObject.activeSelf)
		{
			this.skeleton_Lock.color = Color.Lerp(this.skeleton_Lock.color, (!center) ? color2 : color, Time.time);
		}
	}

	public void LerpPosition(bool center)
	{
		if (this.ObjPrice != null && !ProfileManager.InforChars[this.level].IsUnLocked)
		{
			this.ObjPrice.SetActive(center);
		}
		this.isCenter = center;
		this.charactor.transform.localPosition = Vector3.Lerp(this.charactor.transform.localPosition, (!center) ? new Vector3(this.charactor.transform.localPosition.x, this.yMin) : new Vector3(this.charactor.transform.localPosition.x, this.yMax), Time.deltaTime * 5f);
	}

	public void SetUnlockChar(bool isUnlocked)
	{
		this.isUnLocked = isUnlocked;
		this.imgTextChar.gameObject.SetActive(!isUnlocked);
		this.skeleton_Lock.gameObject.SetActive(!isUnlocked);
	}

	public SkeletonGraphic charactor;

	public SkeletonGraphic skeleton_Lock;

	public RectTransform rectTransform;

	public int level;

	public Image[] Stars;

	public GameObject Locked;

	public GameObject CLicked;

	public Image BGShadow;

	public Image imgTextChar;

	private Color colorTextChar1;

	private Color colorTextChar2;

	private float yMax = -62f;

	private float yMin = -110f;

	private bool isCenter;

	public AudioSource mAudio;

	public bool isUnLocked;

	public GameObject ObjPrice;
}

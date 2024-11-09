using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EffectUnlock : MonoBehaviour
{
	public void UnLock(ETypeWeapon type, int idWeapon)
	{
		base.gameObject.SetActive(true);
		this.imgBackground.enabled = true;
		this.objAnimation.SetActive(true);
		SingletonGame<AudioController>.Instance.PlaySound(this.mAudio, 1f);
		this.txtRankup.text = PopupManager.Instance.GetText(Localization0.Unlock, null).ToUpper();
		this.obj_TextRankUp.SetActive(true);
		Vector3 vt3Pos = Vector3.zero;
		Vector3 vt3Scale = Vector3.one;
		this.objMain.transform.localScale = Vector3.one;
		this.objMain.transform.position = Vector3.zero;
		vt3Pos = this.formWeapon2.cardsWeapon[idWeapon].transform.position;
		vt3Scale = this.animGun1[idWeapon].Scales[0];
		if (type != ETypeWeapon.PRIMARY)
		{
			if (type != ETypeWeapon.SPECIAL)
			{
				if (type == ETypeWeapon.KNIFE)
				{
					this.imgMain.sprite = PopupManager.Instance.sprite_Melee[idWeapon];
				}
			}
			else
			{
				this.imgMain.sprite = PopupManager.Instance.sprite_GunSpecial[0].Sprites[idWeapon];
			}
		}
		else
		{
			this.imgMain.sprite = PopupManager.Instance.sprite_GunMain[0].Sprites[idWeapon];
		}
		base.StartCoroutine(this.AutoDisable(vt3Pos, vt3Scale));
	}

	public void UnlockKnife(int idWeapon)
	{
		base.gameObject.SetActive(true);
		this.imgBackground.enabled = true;
		this.objAnimation.SetActive(true);
		SingletonGame<AudioController>.Instance.PlaySound(this.mAudio, 1f);
		this.txtRankup.text = PopupManager.Instance.GetText(Localization0.Unlock, null).ToUpper();
		this.obj_TextRankUp.SetActive(true);
		Vector3 vt3Pos = Vector3.zero;
		Vector3 vt3Scale = Vector3.one;
		this.objMain.transform.localScale = Vector3.one;
		this.objMain.transform.position = Vector3.zero;
		vt3Pos = this.formWeapon2.cardsWeapon[idWeapon].transform.position;
		vt3Scale = this.animKnife[idWeapon].Scales[0];
		this.imgMain.sprite = PopupManager.Instance.sprite_Melee[idWeapon];
		base.StartCoroutine(this.AutoDisable(vt3Pos, vt3Scale));
	}

	public void RankupGun(ETypeWeapon type, int idWeapon, int rankold, int ranknew)
	{
		base.gameObject.SetActive(true);
		this.imgBackground.enabled = true;
		this.objAnimation.SetActive(true);
		SingletonGame<AudioController>.Instance.PlaySound(this.mAudio, 1f);
		this.txtRankup.text = PopupManager.Instance.GetText(Localization0.Rankup, null).ToUpper();
		this.obj_TextRankUp.SetActive(true);
		this.objMain.transform.localScale = Vector3.one;
		this.objMain.transform.position = Vector3.zero;
		switch (type)
		{
		case ETypeWeapon.PRIMARY:
			this.imgMain.sprite = PopupManager.Instance.sprite_GunMain[rankold].Sprites[idWeapon];
			base.StartCoroutine(this.RankEffect(type, idWeapon, ranknew));
			break;
		case ETypeWeapon.SPECIAL:
			this.imgMain.sprite = PopupManager.Instance.sprite_GunSpecial[rankold].Sprites[idWeapon];
			base.StartCoroutine(this.RankEffect(type, idWeapon, ranknew));
			break;
		case ETypeWeapon.KNIFE:
			this.imgMain.sprite = PopupManager.Instance.sprite_Melee[idWeapon];
			base.StartCoroutine(this.RankEffectKnife(idWeapon, ranknew));
			break;
		case ETypeWeapon.GRENADE:
			this.imgMain.sprite = PopupManager.Instance.sprite_Grenade[rankold].Sprites[idWeapon];
			base.StartCoroutine(this.RankEffectGrenade(idWeapon, ranknew));
			break;
		}
	}

	public void RankupGrenade(int idWeapon, int rankold, int ranknew)
	{
		base.gameObject.SetActive(true);
		this.imgBackground.enabled = true;
		this.objAnimation.SetActive(true);
		this.txtRankup.text = PopupManager.Instance.GetText(Localization0.Rankup, null).ToUpper();
		this.obj_TextRankUp.SetActive(true);
		SingletonGame<AudioController>.Instance.PlaySound(this.mAudio, 1f);
		this.objMain.transform.localScale = Vector3.one;
		this.objMain.transform.position = Vector3.zero;
		this.imgMain.sprite = PopupManager.Instance.sprite_Grenade[rankold].Sprites[idWeapon];
		base.StartCoroutine(this.RankEffectGrenade(idWeapon, ranknew));
	}

	private IEnumerator RankEffectGrenade(int idWeapon, int ranknew)
	{
		yield return new WaitForSeconds(1f);
		if (this.OnShowRankOldEnd != null)
		{
			this.OnShowRankOldEnd();
		}
		Vector3 vt3Pos = Vector3.zero;
		Vector3 vt3Scale = Vector3.one;
		vt3Pos = this.formWeapon2.cardsWeapon[idWeapon].transform.position;
		vt3Scale = this.animGrenade[idWeapon].Scales[ranknew];
		this.imgMain.sprite = PopupManager.Instance.sprite_Grenade[ranknew].Sprites[idWeapon];
		base.StartCoroutine(this.AutoDisable(vt3Pos, vt3Scale));
		yield break;
	}

	private IEnumerator RankEffectKnife(int idWeapon, int ranknew)
	{
		yield return new WaitForSeconds(1f);
		if (this.OnShowRankOldEnd != null)
		{
			this.OnShowRankOldEnd();
		}
		Vector3 vt3Pos = Vector3.zero;
		Vector3 vt3Scale = Vector3.one;
		vt3Pos = this.formWeapon2.cardsWeapon[idWeapon].transform.position;
		vt3Scale = this.animKnife[idWeapon].Scales[0];
		this.imgMain.sprite = PopupManager.Instance.sprite_Melee[idWeapon];
		base.StartCoroutine(this.AutoDisable(vt3Pos, vt3Scale));
		yield break;
	}

	private IEnumerator RankEffect(ETypeWeapon type, int idWeapon, int ranknew)
	{
		yield return new WaitForSeconds(1f);
		if (this.OnShowRankOldEnd != null)
		{
			this.OnShowRankOldEnd();
		}
		Vector3 vt3Pos = Vector3.zero;
		Vector3 vt3Scale = Vector3.one;
		vt3Pos = this.formWeapon2.cardsWeapon[idWeapon].transform.position;
		vt3Scale = this.animGun1[idWeapon].Scales[0];
		if (type != ETypeWeapon.PRIMARY)
		{
			if (type == ETypeWeapon.SPECIAL)
			{
				this.imgMain.sprite = PopupManager.Instance.sprite_GunSpecial[ranknew].Sprites[idWeapon];
			}
		}
		else
		{
			this.imgMain.sprite = PopupManager.Instance.sprite_GunMain[ranknew].Sprites[idWeapon];
		}
		base.StartCoroutine(this.AutoDisable(vt3Pos, vt3Scale));
		yield break;
	}

	private IEnumerator AutoDisable(Vector3 vt3Pos, Vector3 vt3Scale)
	{
		yield return new WaitForSeconds(2f);
		this.txtRankup.text = string.Empty;
		this.obj_TextRankUp.SetActive(false);
		this.imgBackground.enabled = false;
		this.objAnimation.SetActive(false);
		Vector3 tranOld = this.objMain.transform.position;
		this.objMain.transform.position = vt3Pos;
		vt3Pos = this.objMain.transform.localPosition;
		this.objMain.transform.position = tranOld;
		LeanTween.cancel(this.objMain);
		LeanTween.moveLocal(this.objMain, vt3Pos, 0.5f);
		LeanTween.scale(this.objMain, vt3Scale, 0.5f);
		yield return new WaitForSeconds(0.5f);
		base.gameObject.SetActive(false);
		yield break;
	}

	public void EndEffectUnlock()
	{
		this.txtRankup.text = string.Empty;
		this.obj_TextRankUp.SetActive(false);
		this.imgBackground.enabled = false;
		this.objAnimation.SetActive(false);
		base.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
		SingletonGame<AudioController>.Instance.StopSound();
	}

	public FormWeapon2 formWeapon2;

	public EffectUnlock.WeaponUI[] animGun1;

	public EffectUnlock.WeaponUI[] animGun2;

	public EffectUnlock.WeaponUI[] animKnife;

	public EffectUnlock.WeaponUI[] animGrenade;

	public Image imgBackground;

	public GameObject objAnimation;

	public Action OnShowRankOldEnd;

	public Text txtRankup;

	public GameObject obj_TextRankUp;

	public AudioClip mAudio;

	public Image imgMain;

	public GameObject objMain;

	[Serializable]
	public class WeaponUI
	{
		public Vector3[] Positions;

		public Vector3[] Scales;
	}
}

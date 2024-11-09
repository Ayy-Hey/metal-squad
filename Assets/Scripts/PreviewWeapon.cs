using System;
using System.Collections.Generic;
using UnityEngine;

public class PreviewWeapon : MonoBehaviour
{
	public static PreviewWeapon Instance
	{
		get
		{
			if (PreviewWeapon.instance == null)
			{
				PreviewWeapon.instance = UnityEngine.Object.FindObjectOfType<PreviewWeapon>();
			}
			return PreviewWeapon.instance;
		}
	}

	private void Start()
	{
		for (int i = 0; i < this.rambo.Length; i++)
		{
			this.rambo[i].gameObject.SetActive(ProfileManager.settingProfile.IDChar == i);
		}
		this.rambo[ProfileManager.settingProfile.IDChar].OnInit();
		this.timeCounter = float.MinValue;
		this.PoolPreviewBullet = new ObjectPooling<PreviewBullet>(10, null, null);
		for (int j = 0; j < this.ListPreviewBullet.Count; j++)
		{
			this.PoolPreviewBullet.Store(this.ListPreviewBullet[j]);
		}
		this.PoolPreviewBulletCt = new ObjectPooling<PreviewBulletCt>(0, null, null);
		for (int k = 0; k < this.ListPreviewBulletCt.Count; k++)
		{
			this.PoolPreviewBulletCt.Store(this.ListPreviewBulletCt[k]);
		}
		this.PoolPreviewBulletFc = new ObjectPooling<PreviewBulletFc>(0, null, null);
		for (int l = 0; l < this.ListPreviewBulletFc.Count; l++)
		{
			this.PoolPreviewBulletFc.Store(this.ListPreviewBulletFc[l]);
		}
		this.PoolPreviewEffect = new ObjectPooling<PreviewEffect>(10, null, null);
		for (int m = 0; m < this.ListPreviewEffect.Count; m++)
		{
			this.PoolPreviewEffect.Store(this.ListPreviewEffect[m]);
		}
		this.PoolFxBullet = new ObjectPooling<FxBullet>(10, null, null);
		for (int n = 0; n < this.ListFxBullet.Count; n++)
		{
			this.PoolFxBullet.Store(this.ListFxBullet[n]);
		}
		this.PoolPreviewGrenade = new ObjectPooling<PreviewGrenade>(5, null, null);
		for (int num = 0; num < this.ListPreviewGrenade.Count; num++)
		{
			this.PoolPreviewGrenade.Store(this.ListPreviewGrenade[num]);
		}
		this.PoolGrenadeBasic = new ObjectPooling<FxGrenadeBasic>(this.ListGrenadeBasic.Count, null, null);
		for (int num2 = 0; num2 < this.ListGrenadeBasic.Count; num2++)
		{
			this.PoolGrenadeBasic.Store(this.ListGrenadeBasic[num2]);
		}
		this.PoolGrenadeIce = new ObjectPooling<FxGrenadeIce>(this.ListGrenadeIce.Count, null, null);
		for (int num3 = 0; num3 < this.ListGrenadeIce.Count; num3++)
		{
			this.PoolGrenadeIce.Store(this.ListGrenadeIce[num3]);
		}
		this.PoolFxFire = new ObjectPooling<FxFire>(this.ListFxFire.Count, null, null);
		this.PoolFxFire2 = new ObjectPooling<FxFire2>(this.ListFxFire2.Count, null, null);
		for (int num4 = 0; num4 < this.ListFxFire2.Count; num4++)
		{
			this.PoolFxFire2.Store(this.ListFxFire2[num4]);
		}
		for (int num5 = 0; num5 < this.ListFxFire.Count; num5++)
		{
			this.PoolFxFire.Store(this.ListFxFire[num5]);
		}
		this.PoolGrenadeToxic = new ObjectPooling<FxGrenadeToxic>(this.ListGrenadeToxic.Count, null, null);
		for (int num6 = 0; num6 < this.ListGrenadeToxic.Count; num6++)
		{
			this.PoolGrenadeToxic.Store(this.ListGrenadeToxic[num6]);
		}
		this.PoolFxNo1 = new ObjectPooling<FxNo_Particle>(0, null, null);
		for (int num7 = 0; num7 < this.ListFxNo1.Count; num7++)
		{
			this.PoolFxNo1.Store(this.ListFxNo1[num7]);
		}
		this.PoolFxNo2 = new ObjectPooling<FxNo_Spine>(0, null, null);
		for (int num8 = 0; num8 < this.ListFxNo2.Count; num8++)
		{
			this.PoolFxNo2.Store(this.ListFxNo2[num8]);
		}
		this.PoolFxMainCt = new ObjectPooling<FxNo_Anim>(0, null, null);
		for (int num9 = 0; num9 < this.ListFxMainCt.Count; num9++)
		{
			this.PoolFxMainCt.Store(this.ListFxMainCt[num9]);
		}
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < this.ListFxBullet.Count; i++)
		{
			if (this.ListFxBullet[i] != null && this.ListFxBullet[i].gameObject.activeSelf)
			{
				this.ListFxBullet[i].OnUpdate(deltaTime);
			}
		}
		for (int j = 0; j < this.ListPreviewBulletCt.Count; j++)
		{
			if (this.ListPreviewBulletCt[j] && this.ListPreviewBulletCt[j].isInit)
			{
				this.ListPreviewBulletCt[j].OnUpdate(deltaTime);
			}
		}
		for (int k = 0; k < this.ListPreviewBulletFc.Count; k++)
		{
			if (this.ListPreviewBulletFc[k] && this.ListPreviewBulletFc[k].isInit)
			{
				this.ListPreviewBulletFc[k].OnUpdate(deltaTime);
			}
		}
		for (int l = 0; l < this.ListGrenadeToxic.Count; l++)
		{
			if (this.ListGrenadeToxic[l] != null && this.ListGrenadeToxic[l].gameObject.activeSelf)
			{
				this.ListGrenadeToxic[l].OnUpdate(deltaTime);
			}
		}
		for (int m = 0; m < this.ListFxFire2.Count; m++)
		{
			if (this.ListFxFire2[m] != null && this.ListFxFire2[m].gameObject.activeSelf)
			{
				this.ListFxFire2[m].OnUpdate(deltaTime);
			}
		}
		for (int n = 0; n < this.ListFxFire.Count; n++)
		{
			if (this.ListFxFire[n] != null && this.ListFxFire[n].gameObject.activeSelf)
			{
				this.ListFxFire[n].OnUpdate(deltaTime);
			}
		}
		for (int num = 0; num < this.ListGrenadeBasic.Count; num++)
		{
			if (this.ListGrenadeBasic[num] != null && this.ListGrenadeBasic[num].gameObject.activeSelf)
			{
				this.ListGrenadeBasic[num].OnUpdate(deltaTime);
			}
		}
		for (int num2 = 0; num2 < this.ListGrenadeIce.Count; num2++)
		{
			if (this.ListGrenadeIce[num2] != null && this.ListGrenadeIce[num2].gameObject.activeSelf)
			{
				this.ListGrenadeIce[num2].OnUpdate(deltaTime);
			}
		}
		for (int num3 = 0; num3 < this.ListPreviewBullet.Count; num3++)
		{
			if (this.ListPreviewBullet[num3] != null && this.ListPreviewBullet[num3].gameObject.activeSelf)
			{
				this.ListPreviewBullet[num3].OnUpdate(deltaTime);
			}
		}
		this.rambo[ProfileManager.settingProfile.IDChar].OnUpdate(deltaTime);
	}

	private void OnDestroy()
	{
		for (int i = 0; i < this.ListFxBullet.Count; i++)
		{
			if (this.ListFxBullet[i] != null && this.ListFxBullet[i].gameObject.activeSelf)
			{
				this.ListFxBullet[i].gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < this.ListPreviewBullet.Count; j++)
		{
			this.ListPreviewBullet[j].gameObject.SetActive(false);
		}
		for (int k = 0; k < this.ListPreviewBulletCt.Count; k++)
		{
			if (this.ListPreviewBulletCt[k] && this.ListPreviewBulletCt[k].isInit)
			{
				this.ListPreviewBulletCt[k].gameObject.SetActive(false);
			}
		}
		for (int l = 0; l < this.ListPreviewBulletFc.Count; l++)
		{
			if (this.ListPreviewBulletFc[l] && this.ListPreviewBulletFc[l].isInit)
			{
				this.ListPreviewBulletFc[l].gameObject.SetActive(false);
			}
		}
		for (int m = 0; m < this.ListPreviewEffect.Count; m++)
		{
			this.ListPreviewEffect[m].gameObject.SetActive(false);
		}
		for (int n = 0; n < this.ListPreviewGrenade.Count; n++)
		{
			this.ListPreviewGrenade[n].gameObject.SetActive(false);
		}
		for (int num = 0; num < this.ListGrenadeBasic.Count; num++)
		{
			this.ListGrenadeBasic[num].gameObject.SetActive(false);
		}
		for (int num2 = 0; num2 < this.ListGrenadeIce.Count; num2++)
		{
			this.ListGrenadeIce[num2].gameObject.SetActive(false);
		}
		for (int num3 = 0; num3 < this.ListFxFire.Count; num3++)
		{
			this.ListFxFire[num3].gameObject.SetActive(false);
		}
		for (int num4 = 0; num4 < this.ListFxFire2.Count; num4++)
		{
			this.ListFxFire2[num4].gameObject.SetActive(false);
		}
		for (int num5 = 0; num5 < this.ListGrenadeToxic.Count; num5++)
		{
			this.ListGrenadeToxic[num5].gameObject.SetActive(false);
		}
		for (int num6 = 0; num6 < base.transform.childCount; num6++)
		{
			base.transform.GetChild(num6).gameObject.SetActive(false);
		}
		PreviewWeapon.instance = null;
	}

	public void OnShow(ETypeWeapon type, int id, int Level, int RankUpeed, bool unlocked)
	{
		for (int i = 0; i < this.rambo.Length; i++)
		{
			this.rambo[i].gameObject.SetActive(ProfileManager.settingProfile.IDChar == i);
		}
		this.rambo[ProfileManager.settingProfile.IDChar].OnInit();
		this.rambo[ProfileManager.settingProfile.IDChar].OnShow(type, id, Level, RankUpeed);
		if (!unlocked)
		{
			Level = 0;
		}
		int num = Level / 10;
		int num2 = Level % 10;
		for (int j = 0; j < this.ListFxFire2.Count; j++)
		{
			if (this.ListFxFire2[j] != null && this.ListFxFire2[j].gameObject.activeSelf)
			{
				this.ListFxFire2[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < this.ListFxFire.Count; k++)
		{
			if (this.ListFxFire[k] != null && this.ListFxFire[k].gameObject.activeSelf)
			{
				this.ListFxFire[k].gameObject.SetActive(false);
			}
		}
		for (int l = 0; l < this.ListPreviewEffect.Count; l++)
		{
			if (this.ListPreviewEffect[l] != null && this.ListPreviewEffect[l].gameObject.activeSelf)
			{
				this.ListPreviewEffect[l].gameObject.SetActive(false);
			}
		}
		this.OffItemSpecial();
		if (type == ETypeWeapon.SPECIAL)
		{
			this.itemSpecialGun[id].SetActive(true);
		}
	}

	public FxGrenadeBasic CreateGrenadeBasic(Vector2 pos)
	{
		FxGrenadeBasic fxGrenadeBasic = this.PoolGrenadeBasic.New();
		if (fxGrenadeBasic == null)
		{
			fxGrenadeBasic = UnityEngine.Object.Instantiate<Transform>(this.ListGrenadeBasic[0].transform).GetComponent<FxGrenadeBasic>();
			fxGrenadeBasic.transform.parent = this.ListGrenadeBasic[0].transform.parent;
			this.ListGrenadeBasic.Add(fxGrenadeBasic);
		}
		fxGrenadeBasic.gameObject.SetActive(true);
		fxGrenadeBasic.transform.position = pos;
		fxGrenadeBasic.OnInit();
		return fxGrenadeBasic;
	}

	public void CreateEffect(Vector2 pos, string name)
	{
		PreviewEffect previewEffect = this.PoolPreviewEffect.New();
		if (previewEffect == null)
		{
			previewEffect = UnityEngine.Object.Instantiate<Transform>(this.ListPreviewEffect[0].transform).GetComponent<PreviewEffect>();
			previewEffect.transform.parent = this.ListPreviewEffect[0].transform.parent;
			this.ListPreviewEffect.Add(previewEffect);
		}
		previewEffect.gameObject.SetActive(true);
		previewEffect.transform.position = pos;
		previewEffect.OnShow(name);
	}

	public PreviewBullet CreateBullet(Vector3 pos, ETypeWeapon EWeapon, int rank, int IDWeapon, Vector2 Direction, float offsetRotationZ = 0f)
	{
		PreviewBullet previewBullet = this.PoolPreviewBullet.New();
		if (previewBullet == null)
		{
			previewBullet = UnityEngine.Object.Instantiate<Transform>(this.ListPreviewBullet[0].transform).GetComponent<PreviewBullet>();
			previewBullet.transform.parent = this.ListPreviewBullet[0].transform.parent;
			this.ListPreviewBullet.Add(previewBullet);
		}
		previewBullet.gameObject.SetActive(true);
		previewBullet.transform.position = pos;
		previewBullet.OnShoot(EWeapon, rank, IDWeapon, Direction, offsetRotationZ);
		return previewBullet;
	}

	public PreviewBulletCt CreateBulletCt(int rank, Vector3 pos, Vector3 direction)
	{
		PreviewBulletCt ct = this.PoolPreviewBulletCt.New();
		if (!ct)
		{
			ct = UnityEngine.Object.Instantiate<PreviewBulletCt>(this.ListPreviewBulletCt[0]);
			ct.gameObject.transform.parent = this.ListPreviewBulletCt[0].gameObject.transform.parent;
			this.ListPreviewBulletCt.Add(ct);
		}
		ct.OnInit(rank, pos, direction, delegate
		{
			this.PoolPreviewBulletCt.Store(ct);
		});
		return ct;
	}

	public PreviewBulletFc CreateBulletFc(int rank, Vector3 pos, Vector3 direction, float angle = 0f, float scale = 1f, bool isMain = true, Collider2D igroneColl = null)
	{
		PreviewBulletFc fc = this.PoolPreviewBulletFc.New();
		if (!fc)
		{
			fc = UnityEngine.Object.Instantiate<PreviewBulletFc>(this.ListPreviewBulletFc[0]);
			this.ListPreviewBulletFc.Add(fc);
			fc.gameObject.transform.parent = this.ListPreviewBulletFc[0].gameObject.transform.parent;
		}
		fc.OnInit(rank, pos, direction, delegate
		{
			this.PoolPreviewBulletFc.Store(fc);
		}, angle, scale, isMain, igroneColl);
		return fc;
	}

	public void CreateGrenade(Vector2 pos, int IDWeapon, int Level)
	{
		PreviewGrenade previewGrenade = this.PoolPreviewGrenade.New();
		if (previewGrenade == null)
		{
			previewGrenade = UnityEngine.Object.Instantiate<Transform>(this.ListPreviewGrenade[0].transform).GetComponent<PreviewGrenade>();
			previewGrenade.transform.parent = this.ListPreviewGrenade[0].transform.parent;
			this.ListPreviewGrenade.Add(previewGrenade);
		}
		previewGrenade.gameObject.SetActive(true);
		previewGrenade.transform.position = pos;
		previewGrenade.OnShoot(IDWeapon, Level);
	}

	public FxGrenadeIce CreateGrenadeIce(Vector2 pos)
	{
		FxGrenadeIce fxGrenadeIce = this.PoolGrenadeIce.New();
		if (fxGrenadeIce == null)
		{
			fxGrenadeIce = UnityEngine.Object.Instantiate<Transform>(this.ListGrenadeIce[0].transform).GetComponent<FxGrenadeIce>();
			fxGrenadeIce.transform.parent = this.ListGrenadeIce[0].transform.parent;
			this.ListGrenadeIce.Add(fxGrenadeIce);
		}
		fxGrenadeIce.gameObject.SetActive(true);
		fxGrenadeIce.transform.position = pos;
		fxGrenadeIce.OnInit();
		return fxGrenadeIce;
	}

	public void CreateFire(Vector2 pos)
	{
		FxFire fxFire = this.PoolFxFire.New();
		if (fxFire == null)
		{
			fxFire = UnityEngine.Object.Instantiate<Transform>(this.ListFxFire[0].transform).GetComponent<FxFire>();
			fxFire.transform.parent = this.ListFxFire[0].transform.parent;
			this.ListFxFire.Add(fxFire);
		}
		fxFire.gameObject.SetActive(true);
		fxFire.transform.position = pos;
		fxFire.OnPreview(pos);
	}

	public FxGrenadeToxic CreateGrenadeToxic(Vector2 pos)
	{
		FxGrenadeToxic fxGrenadeToxic = this.PoolGrenadeToxic.New();
		if (fxGrenadeToxic == null)
		{
			fxGrenadeToxic = UnityEngine.Object.Instantiate<Transform>(this.ListGrenadeToxic[0].transform).GetComponent<FxGrenadeToxic>();
			fxGrenadeToxic.transform.parent = this.ListGrenadeToxic[0].transform.parent;
			this.ListGrenadeToxic.Add(fxGrenadeToxic);
		}
		fxGrenadeToxic.gameObject.SetActive(true);
		fxGrenadeToxic.transform.position = pos;
		fxGrenadeToxic.OnInit();
		return fxGrenadeToxic;
	}

	public void CreateFire2(Vector2 pos)
	{
		FxFire2 fxFire = this.PoolFxFire2.New();
		if (fxFire == null)
		{
			fxFire = UnityEngine.Object.Instantiate<Transform>(this.ListFxFire2[0].transform).GetComponent<FxFire2>();
			fxFire.transform.parent = this.ListFxFire2[0].transform.parent;
			this.ListFxFire2.Add(fxFire);
		}
		fxFire.gameObject.SetActive(true);
		fxFire.transform.position = pos;
		fxFire.OnPreview(pos);
	}

	public void CreateFxBullet(Vector2 pos)
	{
		FxBullet fxBullet = this.PoolFxBullet.New();
		if (fxBullet == null)
		{
			fxBullet = UnityEngine.Object.Instantiate<Transform>(this.ListFxBullet[0].transform).GetComponent<FxBullet>();
			fxBullet.transform.parent = this.ListFxBullet[0].transform.parent;
			this.ListFxBullet.Add(fxBullet);
		}
		fxBullet.Show(pos);
	}

	public void CreateFxNo1(Vector3 pos, float scale)
	{
		FxNo_Particle fxNo_Particle = this.PoolFxNo1.New();
		if (!fxNo_Particle)
		{
			fxNo_Particle = UnityEngine.Object.Instantiate<FxNo_Particle>(this.ListFxNo1[0]);
			this.ListFxNo1.Add(fxNo_Particle);
			fxNo_Particle.gameObject.transform.parent = this.ListFxNo1[0].gameObject.transform.parent;
		}
		fxNo_Particle.Init(pos, scale, delegate(FxNo_Particle no)
		{
			this.PoolFxNo1.Store(no);
		});
	}

	public void CreateFxNo2(int idAnim, int idSkin, Vector3 pos, float scale = 1f)
	{
		FxNo_Spine fxNo_Spine = this.PoolFxNo2.New();
		if (!fxNo_Spine)
		{
			fxNo_Spine = UnityEngine.Object.Instantiate<FxNo_Spine>(this.ListFxNo2[0]);
			this.ListFxNo2.Add(fxNo_Spine);
			fxNo_Spine.gameObject.transform.parent = this.ListFxNo2[0].gameObject.transform.parent;
		}
		fxNo_Spine.Init(idAnim, pos, Vector3.one * scale, delegate(FxNo_Spine no)
		{
			this.PoolFxNo2.Store(no);
		}, idSkin, true);
	}

	public void CreateFxMainCt(Vector3 pos, float scale = 1f)
	{
		FxNo_Anim fxNo_Anim = this.PoolFxMainCt.New();
		if (!fxNo_Anim)
		{
			fxNo_Anim = UnityEngine.Object.Instantiate<FxNo_Anim>(this.ListFxMainCt[0]);
			this.ListFxMainCt.Add(fxNo_Anim);
			fxNo_Anim.gameObject.transform.parent = this.ListFxMainCt[0].gameObject.transform.parent;
		}
		fxNo_Anim.Init(pos, Vector3.one * scale, delegate(FxNo_Anim no)
		{
			this.PoolFxMainCt.Store(no);
		});
	}

	public void OffItemSpecial()
	{
		for (int i = 0; i < this.itemSpecialGun.Length; i++)
		{
			if (this.itemSpecialGun[i])
			{
				this.itemSpecialGun[i].SetActive(false);
			}
		}
	}

	private static PreviewWeapon instance;

	public List<PreviewBullet> ListPreviewBullet;

	public ObjectPooling<PreviewBullet> PoolPreviewBullet;

	public List<PreviewBulletCt> ListPreviewBulletCt;

	public ObjectPooling<PreviewBulletCt> PoolPreviewBulletCt;

	public List<PreviewBulletFc> ListPreviewBulletFc;

	public ObjectPooling<PreviewBulletFc> PoolPreviewBulletFc;

	public List<PreviewEffect> ListPreviewEffect;

	public ObjectPooling<PreviewEffect> PoolPreviewEffect;

	public List<PreviewGrenade> ListPreviewGrenade;

	public ObjectPooling<PreviewGrenade> PoolPreviewGrenade;

	[SerializeField]
	private List<FxGrenadeBasic> ListGrenadeBasic;

	public ObjectPooling<FxGrenadeBasic> PoolGrenadeBasic;

	[SerializeField]
	private List<FxGrenadeIce> ListGrenadeIce;

	public ObjectPooling<FxGrenadeIce> PoolGrenadeIce;

	[SerializeField]
	private List<FxFire> ListFxFire;

	public ObjectPooling<FxFire> PoolFxFire;

	[SerializeField]
	private List<FxFire2> ListFxFire2;

	public ObjectPooling<FxFire2> PoolFxFire2;

	[SerializeField]
	private List<FxGrenadeToxic> ListGrenadeToxic;

	public ObjectPooling<FxGrenadeToxic> PoolGrenadeToxic;

	[SerializeField]
	private List<FxBullet> ListFxBullet;

	public ObjectPooling<FxBullet> PoolFxBullet;

	[SerializeField]
	private List<FxNo_Particle> ListFxNo1;

	public ObjectPooling<FxNo_Particle> PoolFxNo1;

	[SerializeField]
	private List<FxNo_Spine> ListFxNo2;

	public ObjectPooling<FxNo_Spine> PoolFxNo2;

	[SerializeField]
	private List<FxNo_Anim> ListFxMainCt;

	public ObjectPooling<FxNo_Anim> PoolFxMainCt;

	public float timeCounter;

	public PreviewRambo[] rambo;

	public PreviewEnemy[] enemy;

	public GameObject[] itemSpecialGun;
}

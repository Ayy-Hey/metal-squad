using System;
using System.Collections;
using DigitalRuby.LightningBolt;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PreviewRambo : MonoBehaviour
{
	public void OnInit()
	{
		if (this.isInit)
		{
			return;
		}
		this.isInit = true;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.GunTipBonePrimary[0] = this.skeletonAnimation.skeleton.FindBone("GunTip_M4A1-1");
		this.GunTipBonePrimary[1] = this.skeletonAnimation.skeleton.FindBone("GunTip_Machine-1");
		this.GunTipBonePrimary[2] = this.skeletonAnimation.skeleton.FindBone("GunTip_Ice-1");
		this.GunTipBonePrimary[3] = this.skeletonAnimation.skeleton.FindBone("GunTip_Shotgun-1");
		this.GunTipBonePrimary[4] = this.skeletonAnimation.skeleton.FindBone("GunTip_MGL140-1");
		this.GunTipBonePrimary[5] = this.skeletonAnimation.skeleton.FindBone("GunTip_Ct9-1");
		this.GunTipBoneSpecial[1] = this.skeletonAnimation.skeleton.FindBone("GunTip_Sniper-1");
		this.GunTipBoneSpecial[0] = this.skeletonAnimation.skeleton.FindBone("GunTip_Frame-1");
		this.GunTipBoneSpecial[5] = this.skeletonAnimation.skeleton.FindBone("GunTip_Thunder-1");
		this.GunTipBoneSpecial[2] = this.skeletonAnimation.skeleton.FindBone("GunTip_Laser-1");
		this.GunTipBoneSpecial[4] = this.skeletonAnimation.skeleton.FindBone("GunTip_Rocket-4");
		this.GunTipBonePrimary[3] = this.skeletonAnimation.skeleton.FindBone("GunTip_Fc10-1");
		this.GunTipRocket[0] = this.skeletonAnimation.skeleton.FindBone("GunTip_Rocket-2");
		this.GunTipRocket[1] = this.skeletonAnimation.skeleton.FindBone("GunTip_Rocket-3");
		this.GunTipRocket[2] = this.skeletonAnimation.skeleton.FindBone("GunTip_Rocket-1");
		this.GunTipSniper = this.skeletonAnimation.skeleton.FindBone("GunTip_Sniper-1");
		this.GunTipMachineGun = this.skeletonAnimation.skeleton.FindBone("GunTip_Machine-3");
	}

	public void OnSetSkin(ETypeWeapon EWeapon, int IDWeapon, int Level, int RankUpped, bool isUnlocked)
	{
		this.OnSetSkinByRank(EWeapon, IDWeapon, RankUpped, isUnlocked);
	}

	public void OnSetSkinByRank(ETypeWeapon EWeapon, int IDWeapon, int rank, bool isUnlocked)
	{
		this.rankCurrent = rank;
		if (EWeapon != ETypeWeapon.PRIMARY)
		{
			if (EWeapon == ETypeWeapon.SPECIAL)
			{
				this.SkinCurrent = this.SkinSpecial[rank].SkinGun[IDWeapon];
			}
		}
		else
		{
			this.SkinCurrent = this.SkinPrimary[rank].SkinGun[IDWeapon];
		}
	}

	public void OnShow(ETypeWeapon EWeapon, int IDWeapon, int Level, int RankUpped)
	{
		string id = this.SkinCurrent + EWeapon.ToString() + IDWeapon.ToString();
		this.rankCurrent = RankUpped;
		this.ID = id;
		this.isMoving = false;
		this.objFireGun.SetActive(false);
		this.thunderGun[0].gameObject.SetActive(false);
		this.thunderGun[1].gameObject.SetActive(false);
		this.thunderConnect[0].gameObject.SetActive(false);
		this.thunderConnect[1].gameObject.SetActive(false);
		this.objLaser.gameObject.SetActive(false);
		this.lineRendererLaser[2].gameObject.SetActive(false);
		this.EWeapon = EWeapon;
		this.IDWeapon = IDWeapon;
		this.LevelWeapon = Level;
		switch (EWeapon)
		{
		case ETypeWeapon.PRIMARY:
			this.animShootCurrent = this.skeletonAnimation.skeleton.Data.FindAnimation(this.AnimPrimaryShoot[IDWeapon].Anim[this.rankCurrent]);
			break;
		case ETypeWeapon.SPECIAL:
			this.animShootCurrent = this.skeletonAnimation.skeleton.Data.FindAnimation(this.AnimSpecialShoot[IDWeapon].Anim[this.rankCurrent]);
			break;
		case ETypeWeapon.KNIFE:
			this.animShootCurrent = this.skeletonAnimation.skeleton.Data.FindAnimation(this.AnimKnife[IDWeapon]);
			break;
		case ETypeWeapon.GRENADE:
			this.animShootCurrent = this.skeletonAnimation.skeleton.Data.FindAnimation(this.AnimGrenade);
			break;
		}
		this.pos = base.transform.localPosition;
		this.pos.x = this.defaultPosX;
		if (EWeapon != ETypeWeapon.PRIMARY)
		{
			if (EWeapon != ETypeWeapon.SPECIAL)
			{
				this.skeletonAnimation.state.SetAnimation(0, this.animShootCurrent, true);
			}
			else
			{
				int value = ProfileManager.rifleGunCurrentId.Data.Value;
				int rankUpped = ProfileManager.weaponsRifle[value].GetRankUpped();
				string skin = this.SkinPrimary[rankUpped].SkinGun[value];
				this.skeletonAnimation.skeleton.SetSkin(skin);
				this.skeletonAnimation.state.SetAnimation(0, this.animRun, true);
				this.pos.x = this.startRunPosX;
				this.isMoving = true;
				this.timeMoving = 0f;
			}
		}
		else
		{
			this.skeletonAnimation.skeleton.SetSkin(this.SkinCurrent);
			this.skeletonAnimation.state.SetAnimation(0, this.animShootCurrent, true);
		}
		base.transform.localPosition = this.pos;
	}

	public void OnUpdate(float deltaTime)
	{
		if (this.isMoving)
		{
			this.timeMoving += deltaTime;
			this.pos.x = Mathf.Lerp(this.startRunPosX, this.defaultPosX, this.timeMoving);
			base.transform.localPosition = this.pos;
			if (this.pos.x == this.defaultPosX)
			{
				int idweapon = this.IDWeapon;
				if (idweapon != 0)
				{
					if (idweapon != 5)
					{
						if (idweapon == 2)
						{
							this.objLaser.gameObject.SetActive(true);
						}
					}
					else
					{
						this.thunderGun[0].OnInit(null);
						this.thunderGun[1].OnInit(null);
						this.thunderConnect[0].OnInit(null);
						this.thunderConnect[1].OnInit(null);
						Vector3 position = this.tfEnemy[0].position;
						LightningBoltScript lightningBoltScript = this.thunderGun[1];
						Vector3 vector = this.thunderGun[0].EndPosition = position;
						this.thunderGun[0].EndEffect.transform.position = vector;
						lightningBoltScript.EndPosition = vector;
						this.thunderConnect[0].StartPosition = position;
						LightningBoltScript lightningBoltScript2 = this.thunderConnect[0];
						vector = this.tfEnemy[1].position;
						this.thunderConnect[0].EndEffect.transform.position = vector;
						lightningBoltScript2.EndPosition = vector;
						LightningBoltScript lightningBoltScript3 = this.thunderConnect[1];
						vector = position;
						this.thunderConnect[1].StartEffect.transform.position = vector;
						lightningBoltScript3.StartPosition = vector;
						LightningBoltScript lightningBoltScript4 = this.thunderConnect[1];
						vector = this.tfEnemy[2].position;
						this.thunderConnect[1].EndEffect.transform.position = vector;
						lightningBoltScript4.EndPosition = vector;
					}
				}
				else
				{
					this.objFireGun.SetActive(true);
				}
				PreviewWeapon.Instance.OffItemSpecial();
				this.skeletonAnimation.skeleton.SetSkin(this.SkinCurrent);
				this.skeletonAnimation.state.SetAnimation(0, this.animShootCurrent, true);
				this.isMoving = false;
			}
			return;
		}
		if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 1 && this.lineRendererLaser[2].gameObject.activeSelf)
		{
			Vector2 v = new Vector2(this.GunTipSniper.WorldX + base.transform.position.x, this.GunTipSniper.WorldY * 0.8f + base.transform.position.y);
			this.lineRendererLaser[2].transform.position = v;
		}
		if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 2 && this.objLaser.activeSelf)
		{
			Vector2 v2 = new Vector2(this.GunTipBoneSpecial[this.IDWeapon].WorldX + base.transform.position.x, this.GunTipBoneSpecial[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
			this.objLaser.transform.position = v2;
		}
		if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 0 && this.objFireGun.activeSelf)
		{
			Vector2 v3 = new Vector2(this.GunTipBoneSpecial[this.IDWeapon].WorldX + base.transform.position.x, this.GunTipBoneSpecial[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
			this.objFireGun.transform.position = v3;
		}
		if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 5 && this.thunderGun[0].gameObject.activeSelf)
		{
			Vector2 v4 = new Vector2(this.GunTipBoneSpecial[this.IDWeapon].WorldX + base.transform.position.x, this.GunTipBoneSpecial[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
			this.thunderGun[0].StartEffect.transform.position = (this.thunderGun[0].StartPosition = (this.thunderGun[1].StartPosition = v4));
		}
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		Vector3 v = base.transform.position;
		if (e.Data.Name.Equals("thown"))
		{
			v.y += 1f;
			PreviewWeapon.Instance.CreateGrenade(v, this.IDWeapon, this.LevelWeapon);
		}
		if (e.Data.Name.Equals("Shoot2"))
		{
			Vector2 v2 = new Vector2(this.GunTipMachineGun.WorldX * 0.8f + base.transform.position.x, this.GunTipMachineGun.WorldY * 0.8f + base.transform.position.y);
			PreviewWeapon.Instance.CreateBullet(v2, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, 0f);
		}
		if (e.Data.Name.Equals("Shoot"))
		{
			switch (this.EWeapon)
			{
			case ETypeWeapon.PRIMARY:
			{
				int idweapon = this.IDWeapon;
				if (idweapon != 3)
				{
					if (idweapon != 5)
					{
						v = new Vector2(this.GunTipBonePrimary[this.IDWeapon].WorldX * 0.8f + base.transform.position.x, this.GunTipBonePrimary[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
						PreviewWeapon.Instance.CreateBullet(v, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, 0f);
					}
					else
					{
						v = this.GunTipBonePrimary[this.IDWeapon].GetWorldPosition(base.transform);
						PreviewWeapon.Instance.CreateBulletCt(this.rankCurrent, v, Vector3.right);
					}
				}
				else
				{
					v = new Vector2(this.GunTipBonePrimary[this.IDWeapon].WorldX * 0.8f + base.transform.position.x, this.GunTipBonePrimary[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
					for (int i = 0; i < 3; i++)
					{
						PreviewWeapon.Instance.CreateBullet(v, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, (float)i * 15f - 15f);
					}
				}
				break;
			}
			case ETypeWeapon.SPECIAL:
			{
				int idweapon2 = this.IDWeapon;
				if (idweapon2 != 1)
				{
					if (idweapon2 != 4)
					{
						if (idweapon2 == 3)
						{
							v = this.GunTipBonePrimary[this.IDWeapon].GetWorldPosition(base.transform);
							PreviewWeapon.Instance.CreateBulletFc(this.rankCurrent, v, Vector3.right, 0f, 0.9f, true, null);
						}
					}
					else
					{
						v = new Vector2(this.GunTipBoneSpecial[this.IDWeapon].WorldX * 0.8f + base.transform.position.x, this.GunTipBoneSpecial[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
						Vector2[] array = new Vector2[]
						{
							new Vector2(base.transform.position.x + this.GunTipBonePrimary[this.IDWeapon].WorldX * 0.8f, base.transform.position.y + this.GunTipBonePrimary[this.IDWeapon].WorldY * 0.8f) - new Vector2(base.transform.position.x + this.GunTipRocket[0].WorldX * 0.8f, base.transform.position.y + this.GunTipRocket[0].WorldY * 0.8f),
							new Vector2(base.transform.position.x + this.GunTipRocket[1].WorldX * 0.8f, base.transform.position.y + this.GunTipRocket[1].WorldY * 0.8f) - new Vector2(base.transform.position.x + this.GunTipRocket[0].WorldX * 0.8f, base.transform.position.y + this.GunTipRocket[0].WorldY * 0.8f),
							new Vector2(base.transform.position.x + this.GunTipRocket[2].WorldX * 0.8f, base.transform.position.y + this.GunTipRocket[2].WorldY * 0.8f) - new Vector2(base.transform.position.x + this.GunTipRocket[0].WorldX * 0.8f, base.transform.position.y + this.GunTipRocket[0].WorldY * 0.8f)
						};
						for (int j = 0; j < 3; j++)
						{
							PreviewBullet previewBullet = PreviewWeapon.Instance.CreateBullet(v, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, 0f);
							previewBullet.SetTarget(this.tfEnemy[j]);
							previewBullet.transform.rotation = Quaternion.FromToRotation(-Vector3.down, array[j]);
						}
					}
				}
				else
				{
					v = new Vector2(this.GunTipBoneSpecial[this.IDWeapon].WorldX * 0.8f + base.transform.position.x, this.GunTipBoneSpecial[this.IDWeapon].WorldY * 0.8f + base.transform.position.y);
					PreviewWeapon.Instance.CreateBullet(v, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, 0f);
				}
				break;
			}
			}
		}
	}

	private IEnumerator CreateBulletMachineGun(Vector2 pos1, Vector2 pos2)
	{
		PreviewWeapon.Instance.CreateBullet(pos1, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, 0f);
		yield return new WaitForSeconds(0.1f);
		PreviewWeapon.Instance.CreateBullet(pos2, this.EWeapon, this.rankCurrent, this.IDWeapon, Vector2.right, 0f);
		yield break;
	}

	public SkeletonAnimation skeletonAnimation;

	public PreviewRambo.SkinWeapon[] SkinPrimary;

	public PreviewRambo.SkinWeapon[] SkinSpecial;

	public string SkinCurrent;

	public PreviewRambo.ShootPreview[] AnimPrimaryShoot;

	public PreviewRambo.ShootPreview[] AnimSpecialShoot;

	public AnimationReferenceAsset animRun;

	[SpineAnimation("", "", true, false)]
	public string AnimGrenade;

	[SpineAnimation("", "", true, false)]
	public string[] AnimKnife;

	public Bone[] GunTipBonePrimary = new Bone[6];

	public Bone[] GunTipBoneSpecial = new Bone[6];

	private Spine.Animation animShootCurrent;

	private ETypeWeapon EWeapon;

	private int IDWeapon;

	private int LevelWeapon;

	public GameObject objFireGun;

	public LightningBoltScript[] thunderGun;

	public LightningBoltScript[] thunderConnect;

	public Transform[] tfEnemy;

	public GameObject objLaser;

	public LineRenderer[] lineRendererLaser;

	public Bone[] GunTipRocket = new Bone[3];

	public Bone GunTipSniper;

	public Bone GunTipMachineGun;

	public AudioSource _audioFire;

	public AudioSource _audioLaser;

	public float defaultPosX = -235f;

	public float startRunPosX = -400f;

	private int rankCurrent;

	private bool isInit;

	private bool isMoving;

	private float timeMoving;

	private Vector3 pos;

	private string ID = string.Empty;

	[Serializable]
	public class SkinWeapon
	{
		[SpineSkin("", "", true, false)]
		public string[] SkinGun;
	}

	[Serializable]
	public struct ShootPreview
	{
		[SpineAnimation("", "", true, false, startsWith = "preview")]
		public string[] Anim;
	}
}

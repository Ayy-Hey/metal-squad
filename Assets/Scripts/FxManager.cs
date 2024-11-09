using System;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using Player;
using UnityEngine;

public class FxManager : MonoBehaviour
{
	public void OnInit()
	{
		this.isInit = true;
		this.ListFxIce = new List<FxIce>();
		this.PoolFxIce = new ObjectPooling<FxIce>(0, null, null);
		this.ListGrenadeBasic = new List<FxGrenadeBasic>();
		this.PoolGrenadeBasic = new ObjectPooling<FxGrenadeBasic>(0, null, null);
		this.ListGrenadeIce = new List<FxGrenadeIce>();
		this.PoolGrenadeIce = new ObjectPooling<FxGrenadeIce>(0, null, null);
		this.ListFxFire = new List<FxFire>();
		this.PoolFxFire = new ObjectPooling<FxFire>(0, null, null);
		this.ListFxFire2 = new List<FxFire2>();
		this.PoolFxFire2 = new ObjectPooling<FxFire2>(0, null, null);
		this.ListGrenadeToxic = new List<FxGrenadeToxic>();
		this.PoolGrenadeToxic = new ObjectPooling<FxGrenadeToxic>(0, null, null);
		this.ListFxBullet = new List<FxBullet>();
		this.PoolFxBullet = new ObjectPooling<FxBullet>(0, null, null);
		this.ListCritical = new List<FxCritical>();
		this.ListEffectExplosion = new List<EffectExplosion>();
		this.EffectExplosionPool = new ObjectPooling<EffectExplosion>(0, new Action<EffectExplosion>(this.ResetEffectExplosion), new Action<EffectExplosion>(this.InitEffectExplosion));
		this.ListExplosionSpine1 = new List<ExplosionSpine1>();
		this.ExplosionSpine1Pool = new ObjectPooling<ExplosionSpine1>(5, null, null);
		this.ListEffectExplosion1 = new List<EffectExplosion1>();
		this.EffectExplosion1Pool = new ObjectPooling<EffectExplosion1>(0, null, null);
		this.ListFxNo01 = new List<FxNo_Particle>();
		this.PoolFxNo01 = new ObjectPooling<FxNo_Particle>(0, null, null);
		this.ListEffNo02 = new List<FxNo_Anim>();
		this.PoolEffNo02 = new ObjectPooling<FxNo_Anim>(0, null, null);
		this.ListEffNoBulletLightning = new List<FxNo_Anim>();
		this.PoolEffNoBulletLightning = new ObjectPooling<FxNo_Anim>(0, null, null);
		this.ListFxNoSpine01 = new List<FxNo_Spine>();
		this.PoolFxNoSpine01 = new ObjectPooling<FxNo_Spine>(0, null, null);
		this.ListFxWarning = new List<Fx_Warning>();
		this.PoolFxWarning = new ObjectPooling<Fx_Warning>(0, null, null);
		this.ListFxFlame01 = new List<FxNo_Particle>();
		this.PoolFxFlame01 = new ObjectPooling<FxNo_Particle>(0, null, null);
		this.ListTfHasFxFlame01 = new List<Transform>();
		this.ListFxJump = new List<PlayerEffectJump>();
		this.PoolFxJump = new ObjectPooling<PlayerEffectJump>(0, null, null);
		this.ListFxEarnGold = new List<PlayerAnGold>();
		this.PoolFxEarnGold = new ObjectPooling<PlayerAnGold>(0, null, null);
		this.ListFxBullet2 = new List<FxNo_Spine>();
		this.PoolFxBullet2 = new ObjectPooling<FxNo_Spine>(0, null, null);
		this.LoadAndCreateFxBooster();
	}

	public void LoadAndCreateFxBooster()
	{
		for (int i = 0; i < 3; i++)
		{
			string text = "GameObject/Fx/FxCritical";
			if (!text.Equals(string.Empty))
			{
				FxCritical fxCritical = UnityEngine.Object.Instantiate(Resources.Load(text, typeof(FxCritical))) as FxCritical;
				fxCritical.TryAwake();
				fxCritical.transform.SetParent(this.ParentCritical);
				this.ListCritical.Add(fxCritical);
			}
		}
		this.PoolCritical = new ObjectPooling<FxCritical>(this.ListCritical.Count, null, null);
		for (int j = 0; j < this.ListCritical.Count; j++)
		{
			this.PoolCritical.Store(this.ListCritical[j]);
		}
	}

	public void CreateFxLighting()
	{
		if (this.isCreateLighting)
		{
			return;
		}
		this.isCreateLighting = true;
		this.ListLignting = new List<LightningBoltScript>();
		for (int i = 0; i < 3; i++)
		{
			string text = "GameObject/Bullet/GunLighting";
			if (!text.Equals(string.Empty))
			{
				LightningBoltScript lightningBoltScript = UnityEngine.Object.Instantiate(Resources.Load(text, typeof(LightningBoltScript))) as LightningBoltScript;
				lightningBoltScript.TryAwake();
				lightningBoltScript.transform.SetParent(this.ParentLighting);
				this.ListLignting.Add(lightningBoltScript);
			}
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListFxBullet.Count; i++)
		{
			if (this.ListFxBullet[i] != null && this.ListFxBullet[i].gameObject.activeSelf)
			{
				this.ListFxBullet[i].OnUpdate(deltaTime);
			}
		}
		for (int j = 0; j < this.ListCritical.Count; j++)
		{
			if (this.ListCritical[j] != null && this.ListCritical[j].gameObject.activeSelf)
			{
				this.ListCritical[j].OnUpdate(deltaTime);
			}
		}
		for (int k = 0; k < this.ListGrenadeToxic.Count; k++)
		{
			if (this.ListGrenadeToxic[k] != null && this.ListGrenadeToxic[k].gameObject.activeSelf)
			{
				this.ListGrenadeToxic[k].OnUpdate(deltaTime);
			}
		}
		for (int l = 0; l < this.ListFxFire2.Count; l++)
		{
			if (this.ListFxFire2[l] != null && this.ListFxFire2[l].gameObject.activeSelf)
			{
				this.ListFxFire2[l].OnUpdate(deltaTime);
			}
		}
		for (int m = 0; m < this.ListFxFire.Count; m++)
		{
			if (this.ListFxFire[m] != null && this.ListFxFire[m].gameObject.activeSelf)
			{
				this.ListFxFire[m].OnUpdate(deltaTime);
			}
		}
		for (int n = 0; n < this.ListGrenadeIce.Count; n++)
		{
			if (this.ListGrenadeIce[n] != null && this.ListGrenadeIce[n].gameObject.activeSelf)
			{
				this.ListGrenadeIce[n].OnUpdate(deltaTime);
			}
		}
		for (int num = 0; num < this.ListGrenadeBasic.Count; num++)
		{
			if (this.ListGrenadeBasic[num] != null && this.ListGrenadeBasic[num].gameObject.activeSelf)
			{
				this.ListGrenadeBasic[num].OnUpdate(deltaTime);
			}
		}
		for (int num2 = 0; num2 < this.ListFxIce.Count; num2++)
		{
			if (this.ListFxIce[num2] != null && this.ListFxIce[num2].gameObject.activeSelf)
			{
				this.ListFxIce[num2].OnUpdate(deltaTime);
			}
		}
		for (int num3 = 0; num3 < this.ListFxWarning.Count; num3++)
		{
			if (this.ListFxWarning[num3] && this.ListFxWarning[num3].isInit)
			{
				this.ListFxWarning[num3].OnUpdate(deltaTime);
			}
		}
		this.UpdateFxNo01(deltaTime);
		this.UpdateFxFlame01(deltaTime);
	}

	public void DestroyAll()
	{
		try
		{
			for (int i = 0; i < this.ListFxBullet.Count; i++)
			{
				if (this.ListFxBullet[i] != null && this.ListFxBullet[i].gameObject.activeSelf)
				{
					this.ListFxBullet[i].gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < this.ListCritical.Count; j++)
			{
				if (this.ListCritical[j] != null && this.ListCritical[j].gameObject.activeSelf)
				{
					this.ListCritical[j].gameObject.SetActive(false);
				}
			}
			for (int k = 0; k < this.ListGrenadeToxic.Count; k++)
			{
				if (this.ListGrenadeToxic[k] != null && this.ListGrenadeToxic[k].gameObject.activeSelf)
				{
					this.ListGrenadeToxic[k].gameObject.SetActive(false);
				}
			}
			for (int l = 0; l < this.ListFxFire2.Count; l++)
			{
				if (this.ListFxFire2[l] != null && this.ListFxFire2[l].gameObject.activeSelf)
				{
					this.ListFxFire2[l].gameObject.SetActive(false);
				}
			}
			for (int m = 0; m < this.ListFxFire.Count; m++)
			{
				if (this.ListFxFire[m] != null && this.ListFxFire[m].gameObject.activeSelf)
				{
					this.ListFxFire[m].gameObject.SetActive(false);
				}
			}
			for (int n = 0; n < this.ListGrenadeBasic.Count; n++)
			{
				if (this.ListGrenadeBasic[n] != null && this.ListGrenadeBasic[n].gameObject.activeSelf)
				{
					this.ListGrenadeBasic[n].gameObject.SetActive(false);
				}
			}
			for (int num = 0; num < this.ListFxIce.Count; num++)
			{
				if (this.ListFxIce[num] != null && this.ListFxIce[num].gameObject.activeSelf)
				{
					this.ListFxIce[num].gameObject.SetActive(false);
				}
			}
			for (int num2 = 0; num2 < this.ListGrenadeIce.Count; num2++)
			{
				if (this.ListGrenadeIce[num2] != null && this.ListGrenadeIce[num2].gameObject.activeSelf)
				{
					this.ListGrenadeIce[num2].gameObject.SetActive(false);
				}
			}
			for (int num3 = 0; num3 < this.ListEffectExplosion.Count; num3++)
			{
				if (this.ListEffectExplosion[num3] != null && this.ListEffectExplosion[num3].gameObject.activeSelf)
				{
					this.ListEffectExplosion[num3].gameObject.SetActive(false);
				}
			}
			for (int num4 = 0; num4 < this.ListFxWarning.Count; num4++)
			{
				if (this.ListFxWarning[num4] && this.ListFxWarning[num4].isInit)
				{
					this.ListFxWarning[num4].gameObject.SetActive(false);
				}
			}
			for (int num5 = 0; num5 < this.ListFxJump.Count; num5++)
			{
				if (this.ListFxJump[num5])
				{
					this.ListFxJump[num5].gameObject.SetActive(false);
				}
			}
			for (int num6 = 0; num6 < this.ListFxEarnGold.Count; num6++)
			{
				if (this.ListFxEarnGold[num6])
				{
					this.ListFxEarnGold[num6].gameObject.SetActive(false);
				}
			}
			this.DestroyFxNo01();
			this.DestroyFxFlame01();
		}
		catch
		{
		}
	}

	public void ShowEffectLighting(PlayerMain player, BaseEnemy enemy, Vector2 StartPosition)
	{
		for (int i = 0; i < this.ListLignting.Count; i++)
		{
			if (i < enemy.ListLigntingConnected.Count && enemy.ListLigntingConnected[i].HP > 0f && enemy.isActiveAndEnabled && enemy.isInCamera)
			{
				this.ListLignting[i].OnInit(player);
				this.ListLignting[i].StartPosition = StartPosition;
				this.ListLignting[i].EndPosition = enemy.ListLigntingConnected[i].Origin3();
				if (this.ListLignting[i].EndEffect != null)
				{
					this.ListLignting[i].EndEffect.SetActive(true);
					this.ListLignting[i].EndEffect.transform.position = this.ListLignting[i].EndPosition;
				}
			}
			else
			{
				this.ListLignting[i].gameObject.SetActive(false);
			}
		}
	}

	public void ReleaseEffectLighting()
	{
		for (int i = 0; i < this.ListLignting.Count; i++)
		{
			this.ListLignting[i].gameObject.SetActive(false);
		}
	}

	public void CreateFxBullet(Vector2 pos)
	{
		FxBullet fxBullet = this.PoolFxBullet.New();
		if (fxBullet == null)
		{
			fxBullet = UnityEngine.Object.Instantiate<FxBullet>(this.fxBullet);
			fxBullet.gameObject.transform.parent = this.parentFxBullet;
			this.ListFxBullet.Add(fxBullet);
		}
		fxBullet.Show(pos);
	}

	public void CreateIce(Vector2 pos)
	{
		FxIce fxIce = this.PoolFxIce.New();
		if (fxIce == null)
		{
			fxIce = UnityEngine.Object.Instantiate<FxIce>(this.fxIce);
			fxIce.gameObject.transform.parent = this.parentFxIce;
			this.ListFxIce.Add(fxIce);
		}
		fxIce.gameObject.SetActive(true);
		fxIce.transform.position = pos;
		fxIce.OnInit();
	}

	public void CreateFire2(Vector2 pos)
	{
		FxFire2 fxFire = this.PoolFxFire2.New();
		if (fxFire == null)
		{
			fxFire = UnityEngine.Object.Instantiate<FxFire2>(this.fxFire2);
			fxFire.gameObject.transform.parent = this.parentFxFire2;
			this.ListFxFire2.Add(fxFire);
		}
		fxFire.gameObject.SetActive(true);
		fxFire.transform.position = pos;
		fxFire.OnInit();
	}

	public void CreateFire(Vector2 pos)
	{
		FxFire fxFire = this.PoolFxFire.New();
		if (fxFire == null)
		{
			fxFire = UnityEngine.Object.Instantiate<FxFire>(this.fxFire);
			fxFire.gameObject.transform.parent = this.parentFxFire;
			this.ListFxFire.Add(fxFire);
		}
		fxFire.gameObject.SetActive(true);
		fxFire.transform.position = pos;
		fxFire.OnInit();
	}

	public FxGrenadeBasic CreateGrenadeBasic(Vector2 pos)
	{
		FxGrenadeBasic fxGrenadeBasic = this.PoolGrenadeBasic.New();
		if (fxGrenadeBasic == null)
		{
			fxGrenadeBasic = UnityEngine.Object.Instantiate<FxGrenadeBasic>(this.fxGrenadeBasic);
			fxGrenadeBasic.gameObject.transform.parent = this.parentFxGrenadeBasic;
			this.ListGrenadeBasic.Add(fxGrenadeBasic);
		}
		fxGrenadeBasic.gameObject.SetActive(true);
		fxGrenadeBasic.transform.position = pos;
		fxGrenadeBasic.OnInit();
		return fxGrenadeBasic;
	}

	public FxGrenadeToxic CreateGrenadeToxic(Vector2 pos)
	{
		FxGrenadeToxic fxGrenadeToxic = this.PoolGrenadeToxic.New();
		if (fxGrenadeToxic == null)
		{
			fxGrenadeToxic = UnityEngine.Object.Instantiate<FxGrenadeToxic>(this.fxGrenadeToxic);
			fxGrenadeToxic.gameObject.transform.parent = this.parentFxGrenadeToxic;
			this.ListGrenadeToxic.Add(fxGrenadeToxic);
		}
		fxGrenadeToxic.gameObject.SetActive(true);
		fxGrenadeToxic.transform.position = pos;
		fxGrenadeToxic.OnInit();
		return fxGrenadeToxic;
	}

	public FxGrenadeIce CreateGrenadeIce(Vector2 pos)
	{
		FxGrenadeIce fxGrenadeIce = this.PoolGrenadeIce.New();
		if (fxGrenadeIce == null)
		{
			fxGrenadeIce = UnityEngine.Object.Instantiate<FxGrenadeIce>(this.fxGrenadeIce);
			fxGrenadeIce.gameObject.transform.parent = this.parentFxGrenadeIce;
			this.ListGrenadeIce.Add(fxGrenadeIce);
		}
		fxGrenadeIce.gameObject.SetActive(true);
		fxGrenadeIce.transform.position = pos;
		fxGrenadeIce.OnInit();
		return fxGrenadeIce;
	}

	public FxCritical CreateCritical(Vector3 pos)
	{
		FxCritical fxCritical = this.PoolCritical.New();
		if (fxCritical == null)
		{
			fxCritical = UnityEngine.Object.Instantiate<Transform>(this.ListCritical[0].transform).GetComponent<FxCritical>();
			fxCritical.gameObject.transform.parent = this.ListCritical[0].gameObject.transform.parent;
			this.ListCritical.Add(fxCritical);
		}
		fxCritical.gameObject.SetActive(true);
		fxCritical.gameObject.transform.position = pos;
		fxCritical.OnInit();
		return fxCritical;
	}

	public void ShowExplosionSpine(Vector3 pos, int type)
	{
		ExplosionSpine1 explosionSpine = this.ExplosionSpine1Pool.New();
		if (explosionSpine == null)
		{
			explosionSpine = UnityEngine.Object.Instantiate<ExplosionSpine1>(this.explosionSpine1);
			explosionSpine.gameObject.transform.SetParent(this.parentExplosionSpine1);
			this.ListExplosionSpine1.Add(explosionSpine);
		}
		explosionSpine.gameObject.SetActive(true);
		explosionSpine.Show(pos, type);
	}

	public void ShowEffect(int type, Vector3 pos, Vector3 vtScale, bool isShake = true, bool isPlaySound = true)
	{
		EffectExplosion effectExplosion = this.EffectExplosionPool.New();
		if (effectExplosion == null)
		{
			effectExplosion = UnityEngine.Object.Instantiate<EffectExplosion>(this.effectExplosion);
			effectExplosion.gameObject.transform.SetParent(this.parentEffectExplosion);
			this.ListEffectExplosion.Add(effectExplosion);
		}
		effectExplosion.gameObject.SetActive(true);
		try
		{
			base.StartCoroutine(effectExplosion.Show(type, pos, vtScale, isShake, isPlaySound));
		}
		catch
		{
		}
	}

	private void ResetEffectExplosion(EffectExplosion effectEx)
	{
	}

	private void InitEffectExplosion(EffectExplosion effectEx)
	{
	}

	public void ShowExplosion1(Vector3 pos)
	{
		EffectExplosion1 effectExplosion = this.EffectExplosion1Pool.New();
		if (!effectExplosion)
		{
			effectExplosion = UnityEngine.Object.Instantiate<EffectExplosion1>(this.effectExplosion1);
			effectExplosion.gameObject.transform.SetParent(this.parentEffectExplosion1);
			this.ListEffectExplosion1.Add(effectExplosion);
		}
		effectExplosion.Show(pos, delegate(EffectExplosion1 eff)
		{
			this.EffectExplosion1Pool.Store(eff);
		});
	}

	public void ShowFxNo01(Vector3 pos, float scale = 1f)
	{
		FxNo_Particle fxNo_Particle = this.PoolFxNo01.New();
		if (!fxNo_Particle)
		{
			fxNo_Particle = UnityEngine.Object.Instantiate<FxNo_Particle>(this.fxNo01);
			fxNo_Particle.gameObject.transform.SetParent(this.parentFxNo01);
			this.ListFxNo01.Add(fxNo_Particle);
		}
		fxNo_Particle.Init(pos, scale, delegate(FxNo_Particle _fx)
		{
			this.PoolFxNo01.Store(_fx);
		});
	}

	private void UpdateFxNo01(float deltaTime)
	{
		for (int i = 0; i < this.ListFxNo01.Count; i++)
		{
			if (this.ListFxNo01[i] && this.ListFxNo01[i].isInit)
			{
				this.ListFxNo01[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyFxNo01()
	{
		for (int i = 0; i < this.ListFxNo01.Count; i++)
		{
			if (this.ListFxNo01[i] && this.ListFxNo01[i].isInit)
			{
				this.ListFxNo01[i].gameObject.SetActive(false);
			}
		}
	}

	public void ShowEffNo02(Vector3 pos, Vector3 scale)
	{
		FxNo_Anim fxNo_Anim = this.PoolEffNo02.New();
		if (!fxNo_Anim)
		{
			fxNo_Anim = UnityEngine.Object.Instantiate<FxNo_Anim>(this.effNo02);
			fxNo_Anim.gameObject.transform.SetParent(this.parentEffNo02);
			this.ListEffNo02.Add(fxNo_Anim);
		}
		fxNo_Anim.Init(pos, scale, delegate(FxNo_Anim _eff)
		{
			this.PoolEffNo02.Store(_eff);
		});
	}

	public void ShowEffNoBulletLightning(Vector3 pos, Vector3 scale)
	{
		FxNo_Anim fxNo_Anim = this.PoolEffNoBulletLightning.New();
		if (!fxNo_Anim)
		{
			fxNo_Anim = UnityEngine.Object.Instantiate<FxNo_Anim>(this.effNoBulletLightning);
			this.ListEffNoBulletLightning.Add(fxNo_Anim);
			fxNo_Anim.gameObject.transform.SetParent(this.parentEffNoBulletLightning);
		}
		fxNo_Anim.Init(pos, scale, delegate(FxNo_Anim obj)
		{
			this.PoolEffNoBulletLightning.Store(obj);
		});
	}

	public void ShowFxNoSpine01(int idAnim, Vector3 pos, Vector3 scale)
	{
		FxNo_Spine fxNo_Spine = this.PoolFxNoSpine01.New();
		if (!fxNo_Spine)
		{
			fxNo_Spine = UnityEngine.Object.Instantiate<FxNo_Spine>(this.fxNoSpine01);
			this.ListFxNoSpine01.Add(fxNo_Spine);
			fxNo_Spine.gameObject.transform.SetParent(this.parentFxNoSpine01);
		}
		fxNo_Spine.Init(idAnim, pos, scale, delegate(FxNo_Spine _fx)
		{
			this.PoolFxNoSpine01.Store(_fx);
		}, 0, true);
	}

	public void OnShowFxJump(Vector3 pos)
	{
		PlayerEffectJump playerEffectJump = this.PoolFxJump.New();
		if (!playerEffectJump)
		{
			playerEffectJump = UnityEngine.Object.Instantiate<PlayerEffectJump>(this.fxJump, this.ParrentFxJump);
			this.ListFxJump.Add(playerEffectJump);
		}
		playerEffectJump.OnPlay(pos);
	}

	public void OnShowFxEarnGold(Vector3 pos)
	{
		PlayerAnGold playerAnGold = this.PoolFxEarnGold.New();
		if (!playerAnGold)
		{
			playerAnGold = UnityEngine.Object.Instantiate<PlayerAnGold>(this.fxEarnGold, this.ParrentFxEarnGold);
			this.ListFxEarnGold.Add(playerAnGold);
		}
		playerAnGold.OnPlay(pos);
	}

	public void ShowFxWarning(float time, Vector3 pos, Fx_Warning.CameraLock camLock, int idOrientaltion = 0, Transform parent = null)
	{
		switch (idOrientaltion)
		{
		case 1:
			if (this.hasWarningT)
			{
				return;
			}
			break;
		case 2:
			if (this.hasWarningB)
			{
				return;
			}
			break;
		case 3:
			if (this.hasWarningL)
			{
				return;
			}
			break;
		case 4:
			if (this.hasWarningR)
			{
				return;
			}
			break;
		}
		Fx_Warning fx_Warning = this.PoolFxWarning.New();
		if (!fx_Warning)
		{
			fx_Warning = UnityEngine.Object.Instantiate<Fx_Warning>(this.fx_Warning);
			this.ListFxWarning.Add(fx_Warning);
		}
		if (parent)
		{
			fx_Warning.gameObject.transform.parent = parent;
		}
		else
		{
			fx_Warning.gameObject.transform.parent = this.ParentWarning;
		}
		switch (idOrientaltion)
		{
		case 1:
			this.hasWarningT = true;
			break;
		case 2:
			this.hasWarningB = true;
			break;
		case 3:
			this.hasWarningL = true;
			break;
		case 4:
			this.hasWarningR = true;
			break;
		}
		fx_Warning.Init(time, pos, camLock, delegate(Fx_Warning w)
		{
			switch (idOrientaltion)
			{
			case 1:
				this.hasWarningT = false;
				break;
			case 2:
				this.hasWarningB = false;
				break;
			case 3:
				this.hasWarningL = false;
				break;
			case 4:
				this.hasWarningR = false;
				break;
			}
			this.PoolFxWarning.Store(w);
		});
	}

	public void CreateFxFlame01(int type, Transform parent, float scale = 1f)
	{
		if (this.ListTfHasFxFlame01.Contains(parent))
		{
			try
			{
				parent.GetComponentInChildren<FxNo_Particle>().PlayParticle();
			}
			catch
			{
			}
			return;
		}
		this.ListTfHasFxFlame01.Add(parent);
		FxNo_Particle fxNo_Particle = this.PoolFxFlame01.New();
		if (!fxNo_Particle)
		{
			fxNo_Particle = UnityEngine.Object.Instantiate<FxNo_Particle>(this.fxFlame01);
			this.ListFxFlame01.Add(fxNo_Particle);
		}
		fxNo_Particle.Init(type, parent, scale, delegate(FxNo_Particle fx)
		{
			this.ListTfHasFxFlame01.Remove(parent);
			this.PoolFxFlame01.Store(fx);
		});
	}

	private void UpdateFxFlame01(float deltaTime)
	{
		for (int i = 0; i < this.ListFxFlame01.Count; i++)
		{
			if (this.ListFxFlame01[i] && this.ListFxFlame01[i].isInit)
			{
				this.ListFxFlame01[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyFxFlame01()
	{
		for (int i = 0; i < this.ListFxFlame01.Count; i++)
		{
			if (this.ListFxFlame01[i] && this.ListFxFlame01[i].isInit)
			{
				this.ListFxFlame01[i].gameObject.SetActive(false);
			}
		}
	}

	public void CreateFxBullet2(int skin, Vector3 pos, int anim = 1, float scale = 1f, bool randomRotation = true)
	{
		if (!this.fxBullet2)
		{
			this.fxBullet2 = Resources.Load<FxNo_Spine>("GameObject/Fx/FxBullet2");
			this.parentFxBullet2 = new GameObject("GroupFxBullet2").transform;
			this.parentFxBullet2.parent = this.parentFxBullet.parent;
		}
		FxNo_Spine fxNo_Spine = this.PoolFxBullet2.New();
		if (!fxNo_Spine)
		{
			fxNo_Spine = UnityEngine.Object.Instantiate<FxNo_Spine>(this.fxBullet2);
			this.ListFxBullet2.Add(fxNo_Spine);
			fxNo_Spine.gameObject.transform.parent = this.parentFxBullet2;
		}
		int idAnim = (anim != 0) ? UnityEngine.Random.Range(1, 3) : anim;
		Vector3 scale2 = Vector3.one * scale;
		fxNo_Spine.Init(idAnim, pos, scale2, delegate(FxNo_Spine _fx)
		{
			this.PoolFxBullet2.Store(_fx);
		}, skin, randomRotation);
	}

	public void CreateFxCreateEnemy(int type, Vector3 pos, float scale = 1f)
	{
		if (!this.tfParentFxCreateEnemy)
		{
			this.tfParentFxCreateEnemy = new GameObject("Group_FxCreateEnemy").transform;
			this.tfParentFxCreateEnemy.parent = this.tfGroupFx;
			this.ListFxCreateEnemy = new List<FxNo_Spine>();
			this.PoolFxCreateEnemy = new ObjectPooling<FxNo_Spine>(0, null, null);
		}
		FxNo_Spine fxNo_Spine = this.PoolFxCreateEnemy.New();
		if (!fxNo_Spine)
		{
			fxNo_Spine = UnityEngine.Object.Instantiate<FxNo_Spine>(this.fxCreateEnemy);
			this.ListFxCreateEnemy.Add(fxNo_Spine);
			fxNo_Spine.gameObject.transform.parent = this.tfParentFxCreateEnemy;
		}
		Vector3 scale2 = Vector3.one * scale;
		fxNo_Spine.Init(type, pos, scale2, delegate(FxNo_Spine _fx)
		{
			this.PoolFxCreateEnemy.Store(_fx);
		}, 0, false);
	}

	public void CreateFxNoMainCt(Vector3 pos, float scale = 1f)
	{
		if (!this.tfParentFxNoMainCt)
		{
			this.tfParentFxNoMainCt = new GameObject("Group_Fx_NoMainCt").transform;
			this.tfParentFxNoMainCt.parent = this.tfGroupFx;
			this.ListFxNoMainCt = new List<FxNo_Anim>();
			this.PoolFxNoMainCt = new ObjectPooling<FxNo_Anim>(0, null, null);
		}
		FxNo_Anim fxNo_Anim = this.PoolFxNoMainCt.New();
		if (!fxNo_Anim)
		{
			fxNo_Anim = UnityEngine.Object.Instantiate<FxNo_Anim>(this.fxNoMainCt);
			this.ListFxNoMainCt.Add(fxNo_Anim);
			fxNo_Anim.gameObject.transform.parent = this.tfParentFxNoMainCt;
		}
		fxNo_Anim.Init(pos, Vector3.one * scale, delegate(FxNo_Anim no)
		{
			this.PoolFxNoMainCt.Store(no);
		});
	}

	private bool isInit;

	[SerializeField]
	private Transform tfGroupFx;

	[Header("Fx Ice:")]
	[SerializeField]
	private Transform parentFxIce;

	[SerializeField]
	private FxIce fxIce;

	private List<FxIce> ListFxIce;

	public ObjectPooling<FxIce> PoolFxIce;

	[Header("Fx Grenade basic:")]
	[SerializeField]
	private Transform parentFxGrenadeBasic;

	[SerializeField]
	private FxGrenadeBasic fxGrenadeBasic;

	private List<FxGrenadeBasic> ListGrenadeBasic;

	public ObjectPooling<FxGrenadeBasic> PoolGrenadeBasic;

	[SerializeField]
	[Header("Fx grenade ice:")]
	private Transform parentFxGrenadeIce;

	[SerializeField]
	private FxGrenadeIce fxGrenadeIce;

	private List<FxGrenadeIce> ListGrenadeIce;

	public ObjectPooling<FxGrenadeIce> PoolGrenadeIce;

	[Header("Fx grenade toxic:")]
	[SerializeField]
	private Transform parentFxGrenadeToxic;

	[SerializeField]
	private FxGrenadeToxic fxGrenadeToxic;

	private List<FxGrenadeToxic> ListGrenadeToxic;

	public ObjectPooling<FxGrenadeToxic> PoolGrenadeToxic;

	[Header("Fx Fire:")]
	[SerializeField]
	private Transform parentFxFire;

	[SerializeField]
	private FxFire fxFire;

	private List<FxFire> ListFxFire;

	public ObjectPooling<FxFire> PoolFxFire;

	[Header("Fx Fire2:")]
	[SerializeField]
	private Transform parentFxFire2;

	[SerializeField]
	private FxFire2 fxFire2;

	private List<FxFire2> ListFxFire2;

	public ObjectPooling<FxFire2> PoolFxFire2;

	[Header("Fx Bullet:")]
	[SerializeField]
	private Transform parentFxBullet;

	[SerializeField]
	private FxBullet fxBullet;

	private List<FxBullet> ListFxBullet;

	public ObjectPooling<FxBullet> PoolFxBullet;

	[Header("Critical:")]
	[SerializeField]
	private Transform ParentCritical;

	private List<FxCritical> ListCritical;

	public ObjectPooling<FxCritical> PoolCritical;

	[Header("Effect Bomb:")]
	[SerializeField]
	private Transform parentEffectExplosion;

	[SerializeField]
	private EffectExplosion effectExplosion;

	private List<EffectExplosion> ListEffectExplosion;

	public ObjectPooling<EffectExplosion> EffectExplosionPool;

	[SerializeField]
	[Header("Explosion Spine 1:")]
	private Transform parentExplosionSpine1;

	[SerializeField]
	private ExplosionSpine1 explosionSpine1;

	private List<ExplosionSpine1> ListExplosionSpine1;

	public ObjectPooling<ExplosionSpine1> ExplosionSpine1Pool;

	[SerializeField]
	[Header("Explosion 1:")]
	private Transform parentEffectExplosion1;

	[SerializeField]
	private EffectExplosion1 effectExplosion1;

	private List<EffectExplosion1> ListEffectExplosion1;

	public ObjectPooling<EffectExplosion1> EffectExplosion1Pool;

	[SerializeField]
	[Header("FX No 0:")]
	private Transform parentFxNo01;

	[SerializeField]
	private FxNo_Particle fxNo01;

	private List<FxNo_Particle> ListFxNo01;

	public ObjectPooling<FxNo_Particle> PoolFxNo01;

	[Header("Eff No 02:")]
	[SerializeField]
	private Transform parentEffNo02;

	[SerializeField]
	private FxNo_Anim effNo02;

	private List<FxNo_Anim> ListEffNo02;

	public ObjectPooling<FxNo_Anim> PoolEffNo02;

	[Header("Eff No Bullet Lightning:")]
	[SerializeField]
	private Transform parentEffNoBulletLightning;

	[SerializeField]
	private FxNo_Anim effNoBulletLightning;

	private List<FxNo_Anim> ListEffNoBulletLightning;

	public ObjectPooling<FxNo_Anim> PoolEffNoBulletLightning;

	[Header("Fx No Spine 0:")]
	[SerializeField]
	private Transform parentFxNoSpine01;

	[SerializeField]
	private FxNo_Spine fxNoSpine01;

	private List<FxNo_Spine> ListFxNoSpine01;

	public ObjectPooling<FxNo_Spine> PoolFxNoSpine01;

	[SerializeField]
	[Header("Lighting:")]
	private Transform ParentLighting;

	private List<LightningBoltScript> ListLignting;

	private bool isCreateLighting;

	[SerializeField]
	[Header("Fx Warning:")]
	private Transform ParentWarning;

	[SerializeField]
	private Fx_Warning fx_Warning;

	private List<Fx_Warning> ListFxWarning;

	private ObjectPooling<Fx_Warning> PoolFxWarning;

	private bool hasWarningL;

	private bool hasWarningR;

	private bool hasWarningT;

	private bool hasWarningB;

	[Header("Fx Flame 01")]
	[SerializeField]
	private FxNo_Particle fxFlame01;

	private List<FxNo_Particle> ListFxFlame01;

	private ObjectPooling<FxNo_Particle> PoolFxFlame01;

	private List<Transform> ListTfHasFxFlame01;

	[SerializeField]
	[Header("Fx Hero Jump")]
	private PlayerEffectJump fxJump;

	[SerializeField]
	private Transform ParrentFxJump;

	private List<PlayerEffectJump> ListFxJump;

	public ObjectPooling<PlayerEffectJump> PoolFxJump;

	[SerializeField]
	[Header("Fx Hero Earn Gold")]
	private PlayerAnGold fxEarnGold;

	[SerializeField]
	private Transform ParrentFxEarnGold;

	private List<PlayerAnGold> ListFxEarnGold;

	public ObjectPooling<PlayerAnGold> PoolFxEarnGold;

	[Header("Fx Bullet 2:")]
	[SerializeField]
	private Transform parentFxBullet2;

	private FxNo_Spine fxBullet2;

	private List<FxNo_Spine> ListFxBullet2;

	public ObjectPooling<FxNo_Spine> PoolFxBullet2;

	[SerializeField]
	[Header("Fx Create Enemy:")]
	private FxNo_Spine fxCreateEnemy;

	private Transform tfParentFxCreateEnemy;

	private List<FxNo_Spine> ListFxCreateEnemy;

	public ObjectPooling<FxNo_Spine> PoolFxCreateEnemy;

	[SerializeField]
	[Header("Fx No Main Ct:")]
	private FxNo_Anim fxNoMainCt;

	private Transform tfParentFxNoMainCt;

	private List<FxNo_Anim> ListFxNoMainCt;

	public ObjectPooling<FxNo_Anim> PoolFxNoMainCt;
}

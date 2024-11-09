using System;
using System.Collections.Generic;
using Photon.Pun;
using PlayerWeapon;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
	public void InitObject()
	{
		this.ListBulletEnemy = new List<BulletEnemy>();
		this.BulletEnemyPool = new ObjectPooling<BulletEnemy>(0, null, null);
		this.ListRocketEnemy = new List<RocketEnemy>();
		this.RocketEnemyPool = new ObjectPooling<RocketEnemy>(0, null, null);
		this.ListBulletAnim = new List<BulletAnim>();
		this.BulletAnimPool = new ObjectPooling<BulletAnim>(0, null, null);
		this.ListBulletSungCoi = new List<BulletSungCoi>();
		this.BulletSungCoiPool = new ObjectPooling<BulletSungCoi>(0, null, null);
		this.ListDrone = new List<Drone>();
		this.DronePool = new ObjectPooling<Drone>(0, null, null);
		this.ListRocketThuyen = new List<RocketThuyen>();
		this.RocketThuyenPool = new ObjectPooling<RocketThuyen>(0, null, null);
		this.ListRocketBossType1 = new List<RocketBossType1>();
		this.PoolRocketBossType1 = new ObjectPooling<RocketBossType1>(0, null, null);
		this.ListBulletLightning = new List<BulletLightning>();
		this.PoolBulletLightning = new ObjectPooling<BulletLightning>(0, null, null);
		this.ListBulletTail = new List<List<BulletWithTail>>();
		this.PoolBulletTail = new List<ObjectPooling<BulletWithTail>>();
		this.ListBulletFlash = new List<BulletMiniBoss5_2>();
		this.PoolBulletFlash = new ObjectPooling<BulletMiniBoss5_2>(0, null, null);
		this.ListBulletBeSung = new List<BulletBeSung>();
		if (this.bulletEyeBotParent == null)
		{
			this.bulletEyeBotParent = new GameObject("bulletEyeBotParemt").transform;
			this.bulletEyeBotParent.SetParent(this.ParentBulletPlayer1[0].parent);
		}
		this.isInit = true;
	}

	public void LoadAndCreateBulletEyeBot()
	{
		this.ListBulletEyeBot = new List<BaseBullet>();
		this.PoolBulletEyeBot = new ObjectPooling<BaseBullet>(0, null, null);
	}

	public void LoadAndCreateBullet(PlayerMain player)
	{
		this.ListBulletPlayer1 = new List<List<BaseBullet>>();
		this.PoolBulletPlayer1 = new List<ObjectPooling<BaseBullet>>();
		this.ListBulletPlayer2 = new List<List<BaseBullet>>();
		this.PoolBulletPlayer2 = new List<ObjectPooling<BaseBullet>>();
		for (int i = 0; i < 6; i++)
		{
			this.ListBulletPlayer1.Add(new List<BaseBullet>());
			this.PoolBulletPlayer1.Add(new ObjectPooling<BaseBullet>(0, null, null));
			this.ListBulletPlayer2.Add(new List<BaseBullet>());
			this.PoolBulletPlayer2.Add(new ObjectPooling<BaseBullet>(0, null, null));
		}
		this.ListBulletFcPath2 = new List<BulletFcPath2>();
		this.PoolBulletFcPath2 = new ObjectPooling<BulletFcPath2>(0, null, null);
		if (player._PlayerData.IDGUN2 == 5)
		{
			GameManager.Instance.fxManager.CreateFxLighting();
		}
	}

	public void UpdateObject(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListBulletTail.Count; i++)
		{
			for (int j = 0; j < this.ListBulletTail[i].Count; j++)
			{
				if (this.ListBulletTail[i][j] != null && this.ListBulletTail[i][j].gameObject.activeSelf)
				{
					this.ListBulletTail[i][j].OnUpdate(deltaTime);
				}
			}
		}
		for (int k = 0; k < this.ListBulletEnemy.Count; k++)
		{
			if (this.ListBulletEnemy[k] != null && this.ListBulletEnemy[k].gameObject.activeSelf)
			{
				this.ListBulletEnemy[k].UpdateObject();
			}
		}
		for (int l = 0; l < this.ListBulletSungCoi.Count; l++)
		{
			if (this.ListBulletSungCoi[l] && this.ListBulletSungCoi[l].isInit)
			{
				this.ListBulletSungCoi[l].OnUpdate(deltaTime);
			}
		}
		for (int m = 0; m < this.ListDrone.Count; m++)
		{
			if (this.ListDrone[m] && this.ListDrone[m].isInit)
			{
				this.ListDrone[m].OnUpdate(deltaTime);
			}
		}
		for (int n = 0; n < this.ListRocketThuyen.Count; n++)
		{
			if (this.ListRocketThuyen[n] && this.ListRocketThuyen[n].isInit)
			{
				this.ListRocketThuyen[n].OnUpdate(deltaTime);
			}
		}
		for (int num = 0; num < this.ListBulletPlayer1.Count; num++)
		{
			for (int num2 = 0; num2 < this.ListBulletPlayer1[num].Count; num2++)
			{
				if (this.ListBulletPlayer1[num][num2] != null && this.ListBulletPlayer1[num][num2].gameObject.activeSelf)
				{
					this.ListBulletPlayer1[num][num2].OnUpdate(deltaTime);
				}
			}
		}
		for (int num3 = 0; num3 < this.ListBulletPlayer2.Count; num3++)
		{
			for (int num4 = 0; num4 < this.ListBulletPlayer2[num3].Count; num4++)
			{
				if (this.ListBulletPlayer2[num3][num4] != null && this.ListBulletPlayer2[num3][num4].gameObject.activeSelf)
				{
					this.ListBulletPlayer2[num3][num4].OnUpdate(deltaTime);
				}
			}
		}
		for (int num5 = 0; num5 < this.ListBulletFcPath2.Count; num5++)
		{
			if (this.ListBulletFcPath2[num5] && this.ListBulletFcPath2[num5].isInit)
			{
				this.ListBulletFcPath2[num5].OnUpdate(deltaTime);
			}
		}
		for (int num6 = 0; num6 < this.ListRocketBossType1.Count; num6++)
		{
			if (this.ListRocketBossType1[num6] && this.ListRocketBossType1[num6].isInit)
			{
				this.ListRocketBossType1[num6].OnUpdate(deltaTime);
			}
		}
		for (int num7 = 0; num7 < this.ListBulletLightning.Count; num7++)
		{
			if (this.ListBulletLightning[num7] && this.ListBulletLightning[num7].isInit)
			{
				this.ListBulletLightning[num7].OnUpdate(deltaTime);
			}
		}
		for (int num8 = 0; num8 < this.ListBulletFlash.Count; num8++)
		{
			if (this.ListBulletFlash[num8] && this.ListBulletFlash[num8].isInit)
			{
				this.ListBulletFlash[num8].UpdateObject();
			}
		}
		for (int num9 = 0; num9 < this.ListBulletBeSung.Count; num9++)
		{
			if (this.ListBulletBeSung[num9] && this.ListBulletBeSung[num9].isInit)
			{
				this.ListBulletBeSung[num9].OnUpdate(deltaTime);
			}
		}
	}

	public void FixedUpdateObject(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListBulletAnim.Count; i++)
		{
			if (this.ListBulletAnim[i] != null && this.ListBulletAnim[i].gameObject.activeSelf)
			{
				this.ListBulletAnim[i].UpdateObject();
			}
		}
		for (int j = 0; j < this.ListRocketEnemy.Count; j++)
		{
			if (this.ListRocketEnemy[j] != null && this.ListRocketEnemy[j].gameObject.activeSelf)
			{
				this.ListRocketEnemy[j].FixedUpdateObject();
			}
		}
		if (this.ListBulletEyeBot != null)
		{
			for (int k = 0; k < this.ListBulletEyeBot.Count; k++)
			{
				if (this.ListBulletEyeBot[k] != null && this.ListBulletEyeBot[k].gameObject.activeSelf)
				{
					this.ListBulletEyeBot[k].OnUpdate(deltaTime);
				}
			}
		}
	}

	public void DestroyAll()
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListBulletPlayer1.Count; i++)
		{
			for (int j = 0; j < this.ListBulletPlayer1[i].Count; j++)
			{
				if (this.ListBulletPlayer1[i][j] != null && this.ListBulletPlayer1[i][j].gameObject.activeSelf)
				{
					this.ListBulletPlayer1[i][j].gameObject.SetActive(false);
				}
			}
		}
		for (int k = 0; k < this.ListBulletPlayer2.Count; k++)
		{
			for (int l = 0; l < this.ListBulletPlayer2[k].Count; l++)
			{
				if (this.ListBulletPlayer2[k][l] != null && this.ListBulletPlayer2[k][l].gameObject.activeSelf)
				{
					this.ListBulletPlayer2[k][l].gameObject.SetActive(false);
				}
			}
		}
		for (int m = 0; m < this.ListBulletFcPath2.Count; m++)
		{
			if (this.ListBulletFcPath2[m] && this.ListBulletFcPath2[m].isInit)
			{
				this.ListBulletFcPath2[m].gameObject.SetActive(false);
			}
		}
		try
		{
			for (int n = 0; n < this.ListBulletTail.Count; n++)
			{
				for (int num = 0; num < this.ListBulletTail[n].Count; num++)
				{
					if (this.ListBulletTail[n][num] != null && this.ListBulletTail[n][num].gameObject.activeSelf)
					{
						this.ListBulletTail[n][num].gameObject.SetActive(false);
					}
				}
			}
		}
		catch
		{
		}
		for (int num2 = 0; num2 < this.ListBulletAnim.Count; num2++)
		{
			if (this.ListBulletAnim[num2] != null && this.ListBulletAnim[num2].gameObject.activeSelf)
			{
				this.ListBulletAnim[num2].gameObject.SetActive(false);
			}
		}
		for (int num3 = 0; num3 < this.ListBulletEnemy.Count; num3++)
		{
			if (this.ListBulletEnemy[num3] != null && this.ListBulletEnemy[num3].gameObject.activeSelf)
			{
				this.ListBulletEnemy[num3].gameObject.SetActive(false);
			}
		}
		for (int num4 = 0; num4 < this.ListRocketEnemy.Count; num4++)
		{
			if (this.ListRocketEnemy[num4] != null && this.ListRocketEnemy[num4].gameObject.activeSelf)
			{
				this.ListRocketEnemy[num4].gameObject.SetActive(false);
			}
		}
		for (int num5 = 0; num5 < this.ListBulletSungCoi.Count; num5++)
		{
			if (this.ListBulletSungCoi[num5] && this.ListBulletSungCoi[num5].isInit)
			{
				this.ListBulletSungCoi[num5].gameObject.SetActive(false);
			}
		}
		for (int num6 = 0; num6 < this.ListDrone.Count; num6++)
		{
			if (this.ListDrone[num6] && this.ListDrone[num6].isInit)
			{
				this.ListDrone[num6].gameObject.SetActive(false);
			}
		}
		for (int num7 = 0; num7 < this.ListRocketThuyen.Count; num7++)
		{
			if (this.ListRocketThuyen[num7] && this.ListRocketThuyen[num7].isInit)
			{
				this.ListRocketThuyen[num7].gameObject.SetActive(false);
			}
		}
		for (int num8 = 0; num8 < this.ListRocketBossType1.Count; num8++)
		{
			if (this.ListRocketBossType1[num8] && this.ListRocketBossType1[num8].isInit)
			{
				this.ListRocketBossType1[num8].Hide();
			}
		}
		for (int num9 = 0; num9 < this.ListBulletLightning.Count; num9++)
		{
			if (this.ListBulletLightning[num9] && this.ListBulletLightning[num9].isInit)
			{
				this.ListBulletLightning[num9].Hide();
			}
		}
		for (int num10 = 0; num10 < this.ListBulletFlash.Count; num10++)
		{
			if (this.ListBulletFlash[num10] && this.ListBulletFlash[num10].isInit)
			{
				this.ListBulletFlash[num10].gameObject.SetActive(false);
			}
		}
		for (int num11 = 0; num11 < this.ListBulletBeSung.Count; num11++)
		{
			if (this.ListBulletBeSung[num11] && this.ListBulletBeSung[num11].isInit)
			{
				this.ListBulletBeSung[num11].gameObject.SetActive(false);
			}
		}
	}

	public void DestroyObject()
	{
		if (!this.isInit)
		{
			return;
		}
		this.isInit = false;
		for (int i = 0; i < this.ListBulletPlayer1.Count; i++)
		{
			for (int j = 0; j < this.ListBulletPlayer1[i].Count; j++)
			{
				if (this.ListBulletPlayer1[i][j] != null && this.ListBulletPlayer1[i][j].gameObject.activeSelf)
				{
					this.ListBulletPlayer1[i][j].gameObject.SetActive(false);
				}
			}
		}
		for (int k = 0; k < this.ListBulletPlayer2.Count; k++)
		{
			for (int l = 0; l < this.ListBulletPlayer2[k].Count; l++)
			{
				if (this.ListBulletPlayer2[k][l] != null && this.ListBulletPlayer2[k][l].gameObject.activeSelf)
				{
					this.ListBulletPlayer2[k][l].gameObject.SetActive(false);
				}
			}
		}
		for (int m = 0; m < this.ListBulletFcPath2.Count; m++)
		{
			if (this.ListBulletFcPath2[m] && this.ListBulletFcPath2[m].isInit)
			{
				this.ListBulletFcPath2[m].gameObject.SetActive(false);
			}
		}
		try
		{
			for (int n = 0; n < this.ListBulletTail.Count; n++)
			{
				for (int num = 0; num < this.ListBulletTail[n].Count; num++)
				{
					if (this.ListBulletTail[n][num] != null && this.ListBulletTail[n][num].gameObject.activeSelf)
					{
						this.ListBulletTail[n][num].gameObject.SetActive(false);
					}
				}
			}
		}
		catch
		{
		}
		for (int num2 = 0; num2 < this.ListBulletAnim.Count; num2++)
		{
			if (this.ListBulletAnim[num2] != null && this.ListBulletAnim[num2].gameObject.activeSelf)
			{
				this.ListBulletAnim[num2].gameObject.SetActive(false);
			}
		}
		for (int num3 = 0; num3 < this.ListBulletEnemy.Count; num3++)
		{
			if (this.ListBulletEnemy[num3] != null && this.ListBulletEnemy[num3].gameObject.activeSelf)
			{
				this.ListBulletEnemy[num3].gameObject.SetActive(false);
			}
		}
		for (int num4 = 0; num4 < this.ListRocketEnemy.Count; num4++)
		{
			if (this.ListRocketEnemy[num4] != null && this.ListRocketEnemy[num4].gameObject.activeSelf)
			{
				this.ListRocketEnemy[num4].gameObject.SetActive(false);
			}
		}
		for (int num5 = 0; num5 < this.ListBulletSungCoi.Count; num5++)
		{
			if (this.ListBulletSungCoi[num5] && this.ListBulletSungCoi[num5].isInit)
			{
				this.ListBulletSungCoi[num5].gameObject.SetActive(false);
			}
		}
		for (int num6 = 0; num6 < this.ListDrone.Count; num6++)
		{
			if (this.ListDrone[num6] && this.ListDrone[num6].isInit)
			{
				this.ListDrone[num6].gameObject.SetActive(false);
			}
		}
		for (int num7 = 0; num7 < this.ListBulletBeSung.Count; num7++)
		{
			if (this.ListBulletBeSung[num7] && this.ListBulletBeSung[num7].isInit)
			{
				this.ListBulletBeSung[num7].gameObject.SetActive(false);
			}
		}
	}

	public void Pause()
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListBulletPlayer1.Count; i++)
		{
			for (int j = 0; j < this.ListBulletPlayer1[i].Count; j++)
			{
				if (this.ListBulletPlayer1[i][j] != null && this.ListBulletPlayer1[i][j].gameObject.activeSelf)
				{
					this.ListBulletPlayer1[i][j].OnPause();
				}
			}
		}
		for (int k = 0; k < this.ListBulletPlayer2.Count; k++)
		{
			for (int l = 0; l < this.ListBulletPlayer2[k].Count; l++)
			{
				if (this.ListBulletPlayer2[k][l] != null && this.ListBulletPlayer2[k][l].gameObject.activeSelf)
				{
					this.ListBulletPlayer2[k][l].OnPause();
				}
			}
		}
		try
		{
			for (int m = 0; m < this.ListBulletTail.Count; m++)
			{
				for (int n = 0; n < this.ListBulletTail[m].Count; n++)
				{
					if (this.ListBulletTail[m][n] != null && this.ListBulletTail[m][n].gameObject.activeSelf)
					{
						this.ListBulletTail[m][n].Pause();
					}
				}
			}
		}
		catch
		{
		}
		for (int num = 0; num < this.ListRocketEnemy.Count; num++)
		{
			if (this.ListRocketEnemy[num] != null && this.ListRocketEnemy[num].gameObject.activeSelf)
			{
				this.ListRocketEnemy[num].Pause();
			}
		}
		for (int num2 = 0; num2 < this.ListBulletEnemy.Count; num2++)
		{
			if (this.ListBulletEnemy[num2] != null && this.ListBulletEnemy[num2].gameObject.activeSelf)
			{
				this.ListBulletEnemy[num2].Pause();
			}
		}
	}

	public void Resume()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.ListBulletPlayer1 != null)
		{
			for (int i = 0; i < this.ListBulletPlayer1.Count; i++)
			{
				for (int j = 0; j < this.ListBulletPlayer1[i].Count; j++)
				{
					if (this.ListBulletPlayer1[i][j] != null && this.ListBulletPlayer1[i][j].gameObject.activeSelf)
					{
						this.ListBulletPlayer1[i][j].OnResume();
					}
				}
			}
		}
		if (this.ListBulletPlayer2 != null)
		{
			for (int k = 0; k < this.ListBulletPlayer2.Count; k++)
			{
				for (int l = 0; l < this.ListBulletPlayer2[k].Count; l++)
				{
					if (this.ListBulletPlayer2[k][l] != null && this.ListBulletPlayer2[k][l].gameObject.activeSelf)
					{
						this.ListBulletPlayer2[k][l].OnResume();
					}
				}
			}
		}
		try
		{
			for (int m = 0; m < this.ListBulletTail.Count; m++)
			{
				for (int n = 0; n < this.ListBulletTail[m].Count; n++)
				{
					if (this.ListBulletTail[m][n] != null && this.ListBulletTail[m][n].gameObject.activeSelf)
					{
						this.ListBulletTail[m][n].Resume();
					}
				}
			}
		}
		catch
		{
		}
		for (int num = 0; num < this.ListRocketEnemy.Count; num++)
		{
			if (this.ListRocketEnemy[num] != null && this.ListRocketEnemy[num].gameObject.activeSelf)
			{
				this.ListRocketEnemy[num].Resume();
			}
		}
		for (int num2 = 0; num2 < this.ListBulletEnemy.Count; num2++)
		{
			if (this.ListBulletEnemy[num2] != null && this.ListBulletEnemy[num2].gameObject.activeSelf)
			{
				this.ListBulletEnemy[num2].Resume();
			}
		}
	}

	public void CreateBulletAnim(ETypeBullet type, Vector3 pos, Vector2 direction, float Damage, float Speed)
	{
		BulletAnim bulletAnim = this.BulletAnimPool.New();
		if (bulletAnim == null)
		{
			bulletAnim = UnityEngine.Object.Instantiate<BulletAnim>(this.bulletAnim);
			bulletAnim.gameObject.transform.parent = this.parentBulletAnim;
			this.ListBulletAnim.Add(bulletAnim);
		}
		bulletAnim.gameObject.SetActive(true);
		bulletAnim.transform.rotation = Quaternion.FromToRotation(Vector3.right, -direction);
		bulletAnim.Setup(type);
		bulletAnim.SetValue(pos, direction, Damage, Speed);
	}

	public RocketEnemy CreateRocketEnemy(Vector3 pos)
	{
		RocketEnemy rocketEnemy = this.RocketEnemyPool.New();
		if (rocketEnemy == null)
		{
			rocketEnemy = UnityEngine.Object.Instantiate<RocketEnemy>(this.rocketEnemy);
			rocketEnemy.gameObject.transform.parent = this.parentRocketEnemy;
			this.ListRocketEnemy.Add(rocketEnemy);
		}
		rocketEnemy.gameObject.SetActive(true);
		rocketEnemy.InitObject();
		rocketEnemy.transform.localScale = Vector3.one;
		rocketEnemy.transform.position = pos;
		return rocketEnemy;
	}

	public BulletEnemy CreateBulletEnemy(int type, Vector2 direction, Vector3 pos, float Damage, float Speed, float angle = 0f)
	{
		BulletEnemy bulletEnemy = this.BulletEnemyPool.New();
		if (bulletEnemy == null)
		{
			bulletEnemy = UnityEngine.Object.Instantiate<BulletEnemy>(this.bulletEnemy);
			bulletEnemy.gameObject.transform.parent = this.parentBulletEnemy;
			this.ListBulletEnemy.Add(bulletEnemy);
		}
		bulletEnemy.gameObject.SetActive(true);
		bulletEnemy.InitObject();
		bulletEnemy.SetValue(EWeapon.NONE, type, pos, direction, Damage, Speed, angle);
		return bulletEnemy;
	}

	public BaseBullet CreateRocketPlayer(PlayerMain player, Vector3 pos)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer2[4].New();
		if (baseBullet == null)
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				baseBullet = PhotonNetwork.Instantiate("GameObject/Bullet/Rocket_Online", pos, Quaternion.identity, 0, null).GetComponent<RocketPlayer>();
			}
			else
			{
				baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletRocket);
			}
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer2[4];
			this.ListBulletPlayer2[4].Add(baseBullet);
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode && ((RocketPlayer)baseBullet).syncRocketPlayer != null && !((RocketPlayer)baseBullet).syncRocketPlayer.IsRemote)
		{
			base.StartCoroutine(((RocketPlayer)baseBullet).syncRocketPlayer.SendRpc_Init(pos));
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.OnInit(player, Vector3.zero, 0f, true);
		baseBullet.transform.position = pos;
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num > 0.5f)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
		}
		return baseBullet;
	}

	public BulletWithTail CreatBulletWithTail(Vector2 direction, Vector3 pos, float Damage, float Speed)
	{
		BulletWithTail bulletWithTail = this.PoolBulletTail[0].New();
		if (bulletWithTail == null)
		{
			bulletWithTail = UnityEngine.Object.Instantiate<Transform>(this.ListBulletTail[0][0].transform).GetComponent<BulletWithTail>();
			bulletWithTail.gameObject.transform.parent = this.ListBulletTail[0][0].gameObject.transform.parent;
			this.ListBulletTail[0].Add(bulletWithTail);
		}
		bulletWithTail.gameObject.SetActive(true);
		bulletWithTail.InitObject();
		bulletWithTail.SetValue(pos, direction, Damage, Speed);
		return bulletWithTail;
	}

	public BulletWithTail CreatBulletWithTail2(Vector2 direction, Vector3 pos, float Damage, float Speed)
	{
		BulletWithTail bulletWithTail = this.PoolBulletTail[1].New();
		if (bulletWithTail == null)
		{
			bulletWithTail = UnityEngine.Object.Instantiate<Transform>(this.ListBulletTail[1][0].transform).GetComponent<BulletWithTail>();
			bulletWithTail.gameObject.transform.parent = this.ListBulletTail[1][0].gameObject.transform.parent;
			this.ListBulletTail[1].Add(bulletWithTail);
		}
		bulletWithTail.gameObject.SetActive(true);
		bulletWithTail.InitObject();
		bulletWithTail.SetValue(pos, direction, Damage, Speed);
		return bulletWithTail;
	}

	public void CreateM4A1(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[0].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletM4A1);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[0];
			this.ListBulletPlayer1[0].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
		CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
	}

	public void CreateM4A1ForNPC(PlayerMain player, Vector3 pos, Vector2 Direction, float Damaged, float Speed)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[0].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletM4A1);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[0];
			this.ListBulletPlayer1[0].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInitNPC(player, Direction, Damaged, Speed);
	}

	public void CreateEyeBotBullet(PlayerMain player, Vector3 pos, Vector2 Direction, float Damaged, float Speed)
	{
		BaseBullet baseBullet = this.PoolBulletEyeBot.New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletEyeBot);
			baseBullet.gameObject.transform.SetParent(this.bulletEyeBotParent);
			this.ListBulletEyeBot.Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInitNPC(player, Direction, Damaged, Speed);
	}

	public void CreateMachine(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[1].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletMachine);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[1];
			this.ListBulletPlayer1[1].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num > 0.5f)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
		}
	}

	public void CreateSpread(PlayerMain player, Vector3 pos, Vector2 Direction, float offsetAngle, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[3].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletSpread);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[3];
			this.ListBulletPlayer1[3].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInit(player, Direction, offsetAngle, hasDamage);
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num > 0.5f)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
		}
	}

	public void CreateICE(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[2].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletIce);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[2];
			this.ListBulletPlayer1[2].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
		CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
	}

	public void CreateMGL140(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[4].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletMGL);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[4];
			this.ListBulletPlayer1[4].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
		CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
	}

	public void CreateSniper(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer2[1].New();
		if (baseBullet == null)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletSnip);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer2[1];
			this.ListBulletPlayer2[1].Add(baseBullet);
		}
		baseBullet.gameObject.SetActive(true);
		baseBullet.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
		CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
	}

	public void CreateBulletCt9(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer1[5].New();
		if (!baseBullet)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletCt9);
			this.ListBulletPlayer1[5].Add(baseBullet);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer1[5];
		}
		baseBullet.gameObject.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
	}

	public void CreateBulletFc(PlayerMain player, Vector3 pos, Vector2 Direction, bool hasDamage = true)
	{
		BaseBullet baseBullet = this.PoolBulletPlayer2[3].New();
		if (!baseBullet)
		{
			baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletFc);
			this.ListBulletPlayer2[3].Add(baseBullet);
			baseBullet.gameObject.transform.parent = this.ParentBulletPlayer2[3];
		}
		baseBullet.gameObject.transform.position = pos;
		baseBullet.OnInit(player, Direction, 0f, hasDamage);
	}

	public void CreateBulletFcPath2(PlayerMain player, Vector3 pos, Vector2 direction, float angle, Collider2D igroneColl, bool hasDamage = true)
	{
		BulletFcPath2 fc2 = this.PoolBulletFcPath2.New();
		if (!fc2)
		{
			fc2 = UnityEngine.Object.Instantiate<BulletFcPath2>(this.bulletFcPath2);
			this.ListBulletFcPath2.Add(fc2);
			fc2.gameObject.transform.SetParent(this.parentBulletFcPath2);
		}
		fc2.Init(player, pos, direction, angle, igroneColl, delegate
		{
			this.PoolBulletFcPath2.Store(fc2);
		}, hasDamage);
	}

	public void CreateBulletSungCoi(float speed, float damage, float timeRotateToTaget, Vector3 pos, Vector2 direction)
	{
		BulletSungCoi bulletSungCoi = this.BulletSungCoiPool.New();
		if (!bulletSungCoi)
		{
			bulletSungCoi = UnityEngine.Object.Instantiate<BulletSungCoi>(this.bulletSungCoi, pos, Quaternion.identity, this.parentBulletSungCoi);
			this.ListBulletSungCoi.Add(bulletSungCoi);
		}
		bulletSungCoi.Init(speed, damage, timeRotateToTaget, pos, direction);
	}

	public Drone CreateDrone(float damage, float speed, Vector3 pos, Action hideAction)
	{
		Drone drone = this.DronePool.New();
		if (!drone)
		{
			drone = UnityEngine.Object.Instantiate<Drone>(this.drone);
			drone.gameObject.transform.parent = this.parentDrone;
			this.ListDrone.Add(drone);
		}
		drone.Init(damage, speed, pos, hideAction);
		return drone;
	}

	public void CreateRocketThuyen(float damage, float speed, Vector3 pos, Vector3 direction)
	{
		RocketThuyen rocketThuyen = this.RocketThuyenPool.New();
		if (!rocketThuyen)
		{
			rocketThuyen = UnityEngine.Object.Instantiate<RocketThuyen>(this.rocketThuyen);
			rocketThuyen.gameObject.transform.parent = this.parentRocketThuyen;
			this.ListRocketThuyen.Add(rocketThuyen);
		}
		rocketThuyen.Init(damage, speed, pos, direction);
	}

	public RocketBossType1 CreateRocketBossType1(float damage, float speed, Vector3 direction, Vector3 pos, Transform target, float timeDelayTarget)
	{
		RocketBossType1 rocket = this.PoolRocketBossType1.New();
		if (!rocket)
		{
			rocket = UnityEngine.Object.Instantiate<RocketBossType1>(this.rocketBossType1);
			rocket.gameObject.transform.parent = this.parentRocketBossType1;
			this.ListRocketBossType1.Add(rocket);
		}
		rocket.Init(damage, speed, direction, pos, target, timeDelayTarget, delegate(RocketBossType1 rocketBoss)
		{
			this.PoolRocketBossType1.Store(rocket);
		});
		return rocket;
	}

	public BulletLightning CreateBulletLightning(float damage, float speed, Vector3 pos, Vector3 direction)
	{
		BulletLightning bulletLightning = this.PoolBulletLightning.New();
		if (!bulletLightning)
		{
			bulletLightning = UnityEngine.Object.Instantiate<BulletLightning>(this.bulletLightning);
			bulletLightning.gameObject.transform.parent = this.parentBulletLightning;
			this.ListBulletLightning.Add(bulletLightning);
		}
		bulletLightning.Init(damage, speed, pos, direction, delegate(BulletLightning b)
		{
			this.PoolBulletLightning.Store(b);
		});
		return bulletLightning;
	}

	public void CreateBulletFlash(float damage, float speed, Vector3 pos, Vector3 direction)
	{
		BulletMiniBoss5_2 bulletMiniBoss5_ = this.PoolBulletFlash.New();
		if (!bulletMiniBoss5_)
		{
			bulletMiniBoss5_ = UnityEngine.Object.Instantiate<BulletMiniBoss5_2>(this.bulletFlash);
			this.ListBulletFlash.Add(bulletMiniBoss5_);
			bulletMiniBoss5_.gameObject.transform.parent = this.parentBulletFlash;
		}
		bulletMiniBoss5_.Init(pos, direction, damage, speed, delegate(BulletMiniBoss5_2 b)
		{
			this.PoolBulletFlash.Store(b);
		});
	}

	public void CreateBulletBeSung(float speed, float damage, Vector3 pos, Vector3 direction, float scale = 1f)
	{
		if (!this.tfParentBulletBeSung)
		{
			this.tfParentBulletBeSung = new GameObject("Group_BulletBeSung").transform;
			this.tfParentBulletBeSung.parent = this.parentBulletEnemy.parent;
			this.PoolBulletBeSung = new ObjectPooling<BulletBeSung>(0, null, null);
		}
		BulletBeSung bullet = this.PoolBulletBeSung.New();
		if (!bullet)
		{
			bullet = UnityEngine.Object.Instantiate<BulletBeSung>(this.bulletBeSung);
			bullet.gameObject.transform.parent = this.tfParentBulletBeSung;
			this.ListBulletBeSung.Add(bullet);
		}
		bullet.Init(speed, damage, scale, pos, direction, delegate
		{
			this.PoolBulletBeSung.Store(bullet);
		});
	}

	[SerializeField]
	[Header("Bullet Enemy")]
	private Transform parentBulletEnemy;

	[SerializeField]
	private BulletEnemy bulletEnemy;

	private List<BulletEnemy> ListBulletEnemy;

	public ObjectPooling<BulletEnemy> BulletEnemyPool;

	[Header("Rocket Enemy")]
	[SerializeField]
	private Transform parentRocketEnemy;

	[SerializeField]
	private RocketEnemy rocketEnemy;

	private List<RocketEnemy> ListRocketEnemy;

	public ObjectPooling<RocketEnemy> RocketEnemyPool;

	[SerializeField]
	[Header("Bullet Animation")]
	private Transform parentBulletAnim;

	[SerializeField]
	private BulletAnim bulletAnim;

	private List<BulletAnim> ListBulletAnim;

	public ObjectPooling<BulletAnim> BulletAnimPool;

	[Header("Bullet Sung Coi")]
	[SerializeField]
	private Transform parentBulletSungCoi;

	[SerializeField]
	private BulletSungCoi bulletSungCoi;

	private List<BulletSungCoi> ListBulletSungCoi;

	public ObjectPooling<BulletSungCoi> BulletSungCoiPool;

	[SerializeField]
	[Header("Drone cua EnemyDrone")]
	private Transform parentDrone;

	[SerializeField]
	private Drone drone;

	private List<Drone> ListDrone;

	public ObjectPooling<Drone> DronePool;

	[SerializeField]
	[Header("Rocket Thuyen")]
	private Transform parentRocketThuyen;

	[SerializeField]
	private RocketThuyen rocketThuyen;

	private List<RocketThuyen> ListRocketThuyen;

	public ObjectPooling<RocketThuyen> RocketThuyenPool;

	[SerializeField]
	[Header("Rocket Boss Type1")]
	private Transform parentRocketBossType1;

	[SerializeField]
	private RocketBossType1 rocketBossType1;

	private List<RocketBossType1> ListRocketBossType1;

	public ObjectPooling<RocketBossType1> PoolRocketBossType1;

	[SerializeField]
	[Header("Bullet Lightning")]
	private Transform parentBulletLightning;

	[SerializeField]
	private BulletLightning bulletLightning;

	private List<BulletLightning> ListBulletLightning;

	public ObjectPooling<BulletLightning> PoolBulletLightning;

	[SerializeField]
	[Header("Bullet Flash:")]
	private Transform parentBulletFlash;

	[SerializeField]
	private BulletMiniBoss5_2 bulletFlash;

	private List<BulletMiniBoss5_2> ListBulletFlash;

	private ObjectPooling<BulletMiniBoss5_2> PoolBulletFlash;

	[Header("Bullet Be Sung:")]
	[SerializeField]
	private BulletBeSung bulletBeSung;

	private Transform tfParentBulletBeSung;

	private List<BulletBeSung> ListBulletBeSung;

	private ObjectPooling<BulletBeSung> PoolBulletBeSung;

	private List<List<BulletWithTail>> ListBulletTail;

	public List<ObjectPooling<BulletWithTail>> PoolBulletTail;

	[SerializeField]
	private Transform[] ParentBulletTail;

	[SerializeField]
	[Header("__________________Bullet Player:")]
	private Transform[] ParentBulletPlayer1;

	[SerializeField]
	private BaseBullet bulletM4A1;

	[SerializeField]
	private BaseBullet bulletMachine;

	[SerializeField]
	private BaseBullet bulletIce;

	[SerializeField]
	private BaseBullet bulletSnip;

	[SerializeField]
	private BaseBullet bulletMGL;

	[SerializeField]
	private BaseBullet bulletCt9;

	[SerializeField]
	private Transform[] ParentBulletPlayer2;

	[SerializeField]
	private BaseBullet bulletSpread;

	[SerializeField]
	private BaseBullet bulletRocket;

	[SerializeField]
	private BaseBullet bulletFc;

	private List<List<BaseBullet>> ListBulletPlayer1;

	public List<ObjectPooling<BaseBullet>> PoolBulletPlayer1;

	private List<List<BaseBullet>> ListBulletPlayer2;

	public List<ObjectPooling<BaseBullet>> PoolBulletPlayer2;

	[SerializeField]
	private BulletFcPath2 bulletFcPath2;

	[SerializeField]
	private Transform parentBulletFcPath2;

	private List<BulletFcPath2> ListBulletFcPath2;

	public ObjectPooling<BulletFcPath2> PoolBulletFcPath2;

	[SerializeField]
	[Header("__________________Bullet SP:")]
	private Transform bulletEyeBotParent;

	[SerializeField]
	private BaseBullet bulletEyeBot;

	private List<BaseBullet> ListBulletEyeBot;

	public ObjectPooling<BaseBullet> PoolBulletEyeBot;

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private bool isCreateBulletSkill3;
}

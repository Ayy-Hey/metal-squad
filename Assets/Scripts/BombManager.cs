using System;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
	public void InitObject()
	{
		this.isInit = true;
		this.listBomb = new List<Bomb>();
		this.bombPool = new ObjectPooling<Bomb>(0, null, null);
		this.ListBombAirplane = new List<BombAirplane>();
		this.BombAirplanePool = new ObjectPooling<BombAirplane>(0, new Action<BombAirplane>(this.ResetBombAirplane), new Action<BombAirplane>(this.InitBombAirplane));
		this.ListBombBoss4_1 = new List<BombBoss4_1>();
		this.BombBoss4_1Pool = new ObjectPooling<BombBoss4_1>(0, null, null);
		this.ListBom = new List<BombV2>();
		this.BombPool = new ObjectPooling<BombV2>(0, null, null);
		this.ListBombCicle1 = new List<BombCicle1>();
		this.PoolBombCicle1 = new ObjectPooling<BombCicle1>(0, null, null);
		this.LoadAndCreateGrenade();
	}

	private void LoadAndCreateGrenade()
	{
		this.ListBaseGrenade = new List<List<BaseGrenade>>();
		this.PoolBaseGrenade = new List<ObjectPooling<BaseGrenade>>();
		for (int i = 0; i < 4; i++)
		{
			this.ListBaseGrenade.Add(new List<BaseGrenade>());
			this.PoolBaseGrenade.Add(new ObjectPooling<BaseGrenade>(0, null, null));
		}
	}

	public void UpdateObject(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListBaseGrenade.Count; i++)
		{
			for (int j = 0; j < this.ListBaseGrenade[i].Count; j++)
			{
				if (this.ListBaseGrenade[i][j] != null && this.ListBaseGrenade[i][j].gameObject.activeSelf)
				{
					this.ListBaseGrenade[i][j].OnUpdate();
				}
			}
		}
		for (int k = 0; k < this.ListBom.Count; k++)
		{
			if (this.ListBom[k].gameObject.activeSelf)
			{
				this.ListBom[k].OnUpdate();
			}
		}
		for (int l = 0; l < this.listBomb.Count; l++)
		{
			if (this.listBomb[l].isInit && this.listBomb[l].gameObject.activeSelf)
			{
				this.listBomb[l].UpdateObject();
			}
		}
		for (int m = 0; m < this.ListBombAirplane.Count; m++)
		{
			if (this.ListBombAirplane[m].gameObject.activeSelf)
			{
				this.ListBombAirplane[m].UpdateObject();
			}
		}
		for (int n = 0; n < this.ListBombBoss4_1.Count; n++)
		{
			if (this.ListBombBoss4_1[n].gameObject.activeSelf)
			{
				this.ListBombBoss4_1[n].OnUpdate();
			}
		}
		this.UpdateBombCicle1(deltaTime);
	}

	public void DestroyObject()
	{
		base.StopAllCoroutines();
		this.isInit = false;
		try
		{
			for (int i = 0; i < this.ListBaseGrenade.Count; i++)
			{
				for (int j = 0; j < this.ListBaseGrenade[i].Count; j++)
				{
					if (this.ListBaseGrenade[i][j] != null && this.ListBaseGrenade[i][j].gameObject.activeSelf)
					{
						this.ListBaseGrenade[i][j].gameObject.SetActive(false);
					}
				}
			}
		}
		catch
		{
		}
		for (int k = 0; k < this.ListBombBoss4_1.Count; k++)
		{
			if (this.ListBombBoss4_1[k] != null && this.ListBombBoss4_1[k].gameObject.activeSelf)
			{
				this.ListBombBoss4_1[k].gameObject.SetActive(false);
			}
		}
		for (int l = 0; l < this.listBomb.Count; l++)
		{
			if (this.listBomb[l] != null && this.listBomb[l].gameObject.activeSelf)
			{
				this.listBomb[l].gameObject.SetActive(false);
			}
		}
		for (int m = 0; m < this.ListBombAirplane.Count; m++)
		{
			if (this.ListBombAirplane[m] != null && this.ListBombAirplane[m].gameObject.activeSelf)
			{
				this.ListBombAirplane[m].gameObject.SetActive(false);
			}
		}
		this.DestroyBombCicle();
	}

	public void SetPause()
	{
		try
		{
			for (int i = 0; i < this.ListBaseGrenade.Count; i++)
			{
				for (int j = 0; j < this.ListBaseGrenade[i].Count; j++)
				{
					if (this.ListBaseGrenade[i][j] != null && this.ListBaseGrenade[i][j].gameObject.activeSelf)
					{
						this.ListBaseGrenade[i][j].OnPause();
					}
				}
			}
		}
		catch
		{
		}
		for (int k = 0; k < this.listBomb.Count; k++)
		{
			if (this.listBomb[k].isInit && this.listBomb[k].gameObject.activeSelf)
			{
				this.listBomb[k].SetPause();
			}
		}
		for (int l = 0; l < this.ListBombAirplane.Count; l++)
		{
			if (this.ListBombAirplane[l].gameObject.activeSelf)
			{
				this.ListBombAirplane[l].SetPause();
			}
		}
	}

	public void SetResume()
	{
		try
		{
			for (int i = 0; i < this.ListBaseGrenade.Count; i++)
			{
				for (int j = 0; j < this.ListBaseGrenade[i].Count; j++)
				{
					if (this.ListBaseGrenade[i][j] != null && this.ListBaseGrenade[i][j].gameObject.activeSelf)
					{
						this.ListBaseGrenade[i][j].OnResume();
					}
				}
			}
		}
		catch
		{
		}
		for (int k = 0; k < this.listBomb.Count; k++)
		{
			if (this.listBomb[k].isInit && this.listBomb[k].gameObject.activeSelf)
			{
				this.listBomb[k].SetResume();
			}
		}
		for (int l = 0; l < this.ListBombAirplane.Count; l++)
		{
			if (this.ListBombAirplane[l].gameObject.activeSelf)
			{
				this.ListBombAirplane[l].SetResume();
			}
		}
	}

	public void CreateBombV2(Vector3 pos, float Damage)
	{
		BombV2 bombV = this.BombPool.New();
		if (bombV == null)
		{
			bombV = UnityEngine.Object.Instantiate<BombV2>(this.bombV2);
			bombV.gameObject.transform.SetParent(this.parentBombV2);
			this.ListBom.Add(bombV);
		}
		bombV.gameObject.SetActive(true);
		bombV.transform.position = pos;
		bombV.transform.rotation = Quaternion.identity;
		bombV.OnInit(Damage);
	}

	public void CreateBombBoss4_1(Vector3 pos)
	{
		BombBoss4_1 bombBoss4_ = this.BombBoss4_1Pool.New();
		if (bombBoss4_ == null)
		{
			bombBoss4_ = UnityEngine.Object.Instantiate<BombBoss4_1>(this.bombBoss4_1);
			bombBoss4_.gameObject.transform.parent = this.parentBombBoss4_1;
			this.ListBombBoss4_1.Add(bombBoss4_);
		}
		bombBoss4_.gameObject.SetActive(true);
		bombBoss4_.transform.position = pos;
		bombBoss4_.OnInit();
	}

	public void CreateBombAirplane(Vector3 pos, float radius, float damage, bool isAirBomb = false)
	{
		BombAirplane bombAirplane = this.BombAirplanePool.New();
		if (bombAirplane == null)
		{
			bombAirplane = UnityEngine.Object.Instantiate<BombAirplane>(this.bombAirplane);
			bombAirplane.gameObject.transform.parent = this.parentBombAirplane;
			this.ListBombAirplane.Add(bombAirplane);
		}
		bombAirplane.gameObject.SetActive(true);
		bombAirplane.gameObject.transform.position = pos;
		bombAirplane.Init(radius, damage, isAirBomb);
	}

	private void ResetBombAirplane(BombAirplane bomb)
	{
		if (bomb != null)
		{
			bomb.gameObject.SetActive(true);
			bomb.InitObject();
		}
	}

	private void InitBombAirplane(BombAirplane bomb)
	{
	}

	public void CreateBomb(Vector3 pos, int type, bool isFlip, float damage, int force, float radius_damage)
	{
		Bomb bomb = this.bombPool.New();
		if (bomb == null)
		{
			bomb = UnityEngine.Object.Instantiate<Bomb>(this.bomb);
			bomb.gameObject.transform.parent = this.parentBomb;
			this.listBomb.Add(bomb);
		}
		bomb.gameObject.SetActive(true);
		bomb.InitObject();
		bomb.transform.position = pos;
		bomb.Throw(isFlip, type, (float)force, damage, radius_damage);
	}

	public void ThrowGrendeBasic(PlayerMain player, Vector3 pos, bool FlipX, bool hasDamage = true)
	{
		BaseGrenade baseGrenade = this.PoolBaseGrenade[0].New();
		if (baseGrenade == null)
		{
			baseGrenade = UnityEngine.Object.Instantiate<BaseGrenade>(this.gBasic);
			baseGrenade.gameObject.transform.parent = this.ParentGrenade[0];
			this.ListBaseGrenade[0].Add(baseGrenade);
		}
		baseGrenade.gameObject.SetActive(true);
		baseGrenade.transform.position = pos;
		baseGrenade.OnInit(player, FlipX, hasDamage);
	}

	public void ThrowGrendeIce(PlayerMain player, Vector3 pos, bool FlipX, bool hasDamage = true)
	{
		BaseGrenade baseGrenade = this.PoolBaseGrenade[1].New();
		if (baseGrenade == null)
		{
			baseGrenade = UnityEngine.Object.Instantiate<BaseGrenade>(this.gIce);
			baseGrenade.gameObject.transform.parent = this.ParentGrenade[1];
			this.ListBaseGrenade[1].Add(baseGrenade);
		}
		baseGrenade.gameObject.SetActive(true);
		baseGrenade.transform.position = pos;
		baseGrenade.OnInit(player, FlipX, hasDamage);
	}

	public void ThrowGrendeFire(PlayerMain player, Vector3 pos, bool FlipX, bool hasDamage = true)
	{
		BaseGrenade baseGrenade = this.PoolBaseGrenade[2].New();
		if (baseGrenade == null)
		{
			baseGrenade = UnityEngine.Object.Instantiate<BaseGrenade>(this.gFire);
			baseGrenade.gameObject.transform.parent = this.ParentGrenade[2];
			this.ListBaseGrenade[2].Add(baseGrenade);
		}
		baseGrenade.gameObject.SetActive(true);
		baseGrenade.transform.position = pos;
		baseGrenade.OnInit(player, FlipX, hasDamage);
	}

	public void ThrowGrendeSmoke(PlayerMain player, Vector3 pos, bool FlipX, bool hasDamage = true)
	{
		BaseGrenade baseGrenade = this.PoolBaseGrenade[3].New();
		if (baseGrenade == null)
		{
			baseGrenade = UnityEngine.Object.Instantiate<BaseGrenade>(this.gSmoke);
			baseGrenade.gameObject.transform.parent = this.ParentGrenade[3];
			this.ListBaseGrenade[3].Add(baseGrenade);
		}
		baseGrenade.gameObject.SetActive(true);
		baseGrenade.transform.position = pos;
		baseGrenade.OnInit(player, FlipX, hasDamage);
	}

	public void CreateBombCicle1(float damage, float delay, Vector3 pos, float size = 1f)
	{
		BombCicle1 bombCicle = this.PoolBombCicle1.New();
		if (!bombCicle)
		{
			bombCicle = UnityEngine.Object.Instantiate<BombCicle1>(this.bombCicle1);
			this.ListBombCicle1.Add(bombCicle);
			bombCicle.gameObject.transform.parent = this.parentBombCicle1;
		}
		bombCicle.Init(damage, delay, pos, size, delegate(BombCicle1 b)
		{
			this.PoolBombCicle1.Store(b);
		});
	}

	private void UpdateBombCicle1(float deltaTime)
	{
		for (int i = 0; i < this.ListBombCicle1.Count; i++)
		{
			if (this.ListBombCicle1[i] && this.ListBombCicle1[i].isInit)
			{
				this.ListBombCicle1[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyBombCicle()
	{
		for (int i = 0; i < this.ListBombCicle1.Count; i++)
		{
			if (this.ListBombCicle1[i] && this.ListBombCicle1[i].isInit)
			{
				this.ListBombCicle1[i].gameObject.SetActive(false);
			}
		}
	}

	public Transform parentBomb;

	public Bomb bomb;

	private List<Bomb> listBomb;

	public ObjectPooling<Bomb> bombPool;

	[Header("Bomb Airplane")]
	public Transform parentBombAirplane;

	public BombAirplane bombAirplane;

	private List<BombAirplane> ListBombAirplane;

	public ObjectPooling<BombAirplane> BombAirplanePool;

	[Header("Bomb Boss 4.1")]
	public Transform parentBombBoss4_1;

	public BombBoss4_1 bombBoss4_1;

	private List<BombBoss4_1> ListBombBoss4_1;

	public ObjectPooling<BombBoss4_1> BombBoss4_1Pool;

	[Header("BombV2")]
	public Transform parentBombV2;

	public BombV2 bombV2;

	private List<BombV2> ListBom;

	public ObjectPooling<BombV2> BombPool;

	[Header("Bomb Cicle 1")]
	public Transform parentBombCicle1;

	public BombCicle1 bombCicle1;

	private List<BombCicle1> ListBombCicle1;

	private ObjectPooling<BombCicle1> PoolBombCicle1;

	private List<List<BaseGrenade>> ListBaseGrenade;

	public List<ObjectPooling<BaseGrenade>> PoolBaseGrenade;

	[Header("__________________Grenade Player:")]
	[SerializeField]
	private Transform[] ParentGrenade;

	[SerializeField]
	private BaseGrenade gBasic;

	[SerializeField]
	private BaseGrenade gIce;

	[SerializeField]
	private BaseGrenade gFire;

	[SerializeField]
	private BaseGrenade gSmoke;

	[HideInInspector]
	public bool isInit;
}

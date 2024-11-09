using System;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
	public static TrapManager Instance
	{
		get
		{
			if (TrapManager.instance == null)
			{
				TrapManager.instance = UnityEngine.Object.FindObjectOfType<TrapManager>();
			}
			return TrapManager.instance;
		}
	}

	public void OnInit()
	{
		if (this.isinit)
		{
			return;
		}
		this.isinit = true;
		this.PoolDalan = new ObjectPooling<Dalan>(2, null, null);
		for (int i = 0; i < this.ListDaLan.Count; i++)
		{
			this.PoolDalan.Store(this.ListDaLan[i]);
		}
		for (int j = 0; j < this.ListBayChong.Count; j++)
		{
			this.ListBayChong[j].OnInit();
		}
		for (int k = 0; k < this.ListTrapGround.Count; k++)
		{
			this.ListTrapGround[k].OnInit();
		}
		for (int l = 0; l < this.ListBayDap.Count; l++)
		{
			this.ListBayDap[l].InitObject();
		}
		for (int m = 0; m < this.listPhunLua.Count; m++)
		{
			this.listPhunLua[m].InitObject();
		}
		UnityEngine.Debug.Log("_____________Init Trap Manager____________");
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isinit)
		{
			return;
		}
		for (int i = 0; i < this.dalanManager.Length; i++)
		{
			this.dalanManager[i].OnUpdate(deltaTime);
		}
		for (int j = 0; j < this.ListBayChong.Count; j++)
		{
			this.ListBayChong[j].OnUpdate(deltaTime);
		}
		for (int k = 0; k < this.ListTrapGround.Count; k++)
		{
			this.ListTrapGround[k].OnUpdate(deltaTime);
		}
		for (int l = 0; l < this.listPhunLua.Count; l++)
		{
			this.listPhunLua[l].OnUpdate();
		}
	}

	public void OnDestroyObject()
	{
		if (!this.isinit)
		{
			return;
		}
		this.isinit = false;
		TrapManager.instance = null;
	}

	private void OnDestroy()
	{
		this.OnDestroyObject();
	}

	public void CreateDalan(Vector3 pos)
	{
		Dalan dalan = this.PoolDalan.New();
		if (dalan == null)
		{
			dalan = UnityEngine.Object.Instantiate<Transform>(this.ListDaLan[0].transform).GetComponent<Dalan>();
			dalan.transform.parent = this.ListDaLan[0].transform.parent;
			this.ListDaLan.Add(dalan);
		}
		dalan.OnInit(pos);
	}

	private static TrapManager instance;

	private bool isinit;

	[SerializeField]
	private List<Dalan> ListDaLan;

	public ObjectPooling<Dalan> PoolDalan;

	[SerializeField]
	private DalanManager[] dalanManager;

	[SerializeField]
	private List<BayChong> ListBayChong;

	[SerializeField]
	private List<TrapGround> ListTrapGround;

	[SerializeField]
	private List<BayDap> ListBayDap;

	[SerializeField]
	private List<BayPhunLua> listPhunLua;
}

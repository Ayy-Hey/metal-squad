using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class JetpackManager : MonoBehaviour
{
	public static JetpackManager Instance
	{
		get
		{
			if (JetpackManager.instance == null)
			{
				JetpackManager.instance = UnityEngine.Object.FindObjectOfType<JetpackManager>();
			}
			return JetpackManager.instance;
		}
	}

	public void OnInit2()
	{
		this.trapJetpack.OnInit();
		if (this.objTutorial != null)
		{
			this.objTutorial.SetActive(false);
		}
	}

	public void OnInit()
	{
		this.isInit = true;
		this.RocketPool = new ObjectPooling<RocketJetpack>(5, null, null);
		for (int i = 0; i < this.RocketList.Count; i++)
		{
			this.RocketPool.Store(this.RocketList[i]);
		}
		this.WarningPool = new ObjectPooling<WarningJetpack>(5, null, null);
		for (int j = 0; j < this.WarningList.Count; j++)
		{
			this.WarningPool.Store(this.WarningList[j]);
		}
		this.TrapLaserPool = new ObjectPooling<TrapLaser>(5, null, null);
		for (int k = 0; k < this.TrapLaserList.Count; k++)
		{
			this.TrapLaserPool.Store(this.TrapLaserList[k]);
		}
		string value = string.Empty;
		try
		{
			value = Resources.Load<TextAsset>("Map/TrapLaser/TrapLevel22").text;
			this.dataTrapLaser = JsonConvert.DeserializeObject<DataTrapLaser>(value);
		}
		catch
		{
		}
		GameManager.Instance.isJetpackMode = true;
		if (this.canvas != null)
		{
			this.canvas.SetActive(true);
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.trapJetpack != null)
		{
			this.trapJetpack.OnUpdate(deltaTime);
		}
		this.OnUpdateTrap();
		for (int i = 0; i < this.TrapLaserList.Count; i++)
		{
			if (this.TrapLaserList[i] != null && this.TrapLaserList[i].gameObject.activeSelf)
			{
				this.TrapLaserList[i].UpdateTrap();
			}
		}
		for (int j = 0; j < this.RocketList.Count; j++)
		{
			if (this.RocketList[j] != null && this.RocketList[j].gameObject.activeSelf)
			{
				this.RocketList[j].UpdateObject();
			}
		}
		for (int k = 0; k < this.WarningList.Count; k++)
		{
			if (this.WarningList[k] != null && this.WarningList[k].gameObject.activeSelf)
			{
				this.WarningList[k].UpdateObject();
			}
		}
	}

	public void OnDestroyAll()
	{
		for (int i = 0; i < this.RocketList.Count; i++)
		{
			if (this.RocketList[i] != null && !this.RocketList[i].gameObject.activeSelf)
			{
				this.RocketList[i].gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < this.WarningList.Count; j++)
		{
			if (this.WarningList[j] != null && !this.WarningList[j].gameObject.activeSelf)
			{
				this.WarningList[j].gameObject.SetActive(false);
			}
		}
	}

	public void OnPause()
	{
		for (int i = 0; i < this.TrapLaserList.Count; i++)
		{
			if (this.TrapLaserList[i] != null && this.TrapLaserList[i].gameObject.activeSelf)
			{
				this.TrapLaserList[i].Pause();
			}
		}
	}

	public void OnResume()
	{
		for (int i = 0; i < this.TrapLaserList.Count; i++)
		{
			if (this.TrapLaserList[i] != null && this.TrapLaserList[i].gameObject.activeSelf)
			{
				this.TrapLaserList[i].Resume();
			}
		}
	}

	private void OnDestroy()
	{
		try
		{
			GameManager.Instance.isJetpackMode = false;
			this.OnDestroyAll();
		}
		catch
		{
		}
	}

	public void CreateRocket(float y)
	{
		RocketJetpack rocketJetpack = this.RocketPool.New();
		if (rocketJetpack == null)
		{
			rocketJetpack = UnityEngine.Object.Instantiate<Transform>(this.RocketList[0].transform).GetComponent<RocketJetpack>();
			rocketJetpack.gameObject.SetActive(false);
			rocketJetpack.transform.parent = this.RocketList[0].transform.parent;
			this.RocketList.Add(rocketJetpack);
		}
		rocketJetpack.gameObject.SetActive(true);
		rocketJetpack.Shoot(y);
	}

	public void CreateWarning(float y)
	{
		WarningJetpack warningJetpack = this.WarningPool.New();
		if (warningJetpack == null)
		{
			warningJetpack = UnityEngine.Object.Instantiate<Transform>(this.WarningList[0].transform).GetComponent<WarningJetpack>();
			warningJetpack.gameObject.SetActive(false);
			warningJetpack.transform.parent = this.WarningList[0].transform.parent;
			this.WarningList.Add(warningJetpack);
		}
		warningJetpack.gameObject.SetActive(true);
		warningJetpack.ShowWarning(y);
	}

	private void OnUpdateTrap()
	{
		if (object.ReferenceEquals(this.dataTrapLaser, null) || object.ReferenceEquals(GameManager.Instance.player, null))
		{
			return;
		}
		if (!this.dataTrapLaser.isReaded)
		{
			return;
		}
		for (int i = 0; i < this.dataTrapLaser.InforTrapLaser.Count; i++)
		{
			if (!this.dataTrapLaser.InforTrapLaser[i].isCreated)
			{
				float num = Mathf.Abs(GameManager.Instance.player.GetPosition().x - this.dataTrapLaser.InforTrapLaser[i].x);
				if (num < 10f)
				{
					Vector2 pos = new Vector2(this.dataTrapLaser.InforTrapLaser[i].x, this.dataTrapLaser.InforTrapLaser[i].y);
					this.CreateTrapLaser(pos, this.dataTrapLaser.InforTrapLaser[i].isrotate, this.dataTrapLaser.InforTrapLaser[i].distance, this.dataTrapLaser.InforTrapLaser[i].angle);
					this.dataTrapLaser.InforTrapLaser[i].isCreated = true;
				}
			}
		}
	}

	private void CreateTrapLaser(Vector2 pos, bool isRotate, float distance, float angle)
	{
	}

	private static JetpackManager instance;

	private bool isInit;

	public List<RocketJetpack> RocketList;

	public ObjectPooling<RocketJetpack> RocketPool;

	public List<WarningJetpack> WarningList;

	public ObjectPooling<WarningJetpack> WarningPool;

	public List<TrapLaser> TrapLaserList;

	public ObjectPooling<TrapLaser> TrapLaserPool;

	public GameObject ObjFly;

	public GameObject ObjFire;

	private DataTrapLaser dataTrapLaser;

	public List<GameObject> ListLinePower;

	public TrapJetpack trapJetpack;

	[SerializeField]
	private GameObject canvas;

	[SerializeField]
	private GameObject objTutorial;
}

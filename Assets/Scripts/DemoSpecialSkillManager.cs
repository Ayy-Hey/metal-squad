using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSpecialSkillManager : MonoBehaviour
{
	private void Awake()
	{
		DemoSpecialSkillManager.instance = this;
	}

	private void Start()
	{
		this.bombPool = new ObjectPooling<DemoBomb>(4, null, null);
		this.efBombPool = new ObjectPooling<DemoEfBomb>(25, null, null);
		this.bullet1Pool = new ObjectPooling<DemoBullet1>(27, null, null);
		for (int i = 0; i < this.listBomb.Count; i++)
		{
			this.bombPool.Store(this.listBomb[i]);
		}
		for (int j = 0; j < this.listEfBomb.Count; j++)
		{
			this.efBombPool.Store(this.listEfBomb[j]);
		}
		for (int k = 0; k < this.listBullet1.Count; k++)
		{
			this.bullet1Pool.Store(this.listBullet1[k]);
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			this.Show(this.testId);
		}
	}

	public void Show(int charId)
	{
		this.demoSpecialCam.SetActive(true);
		this.panelDemoSkill.SetActive(true);
		for (int i = 0; i < this.demoSpecialObjs.Length; i++)
		{
			this.demoSpecialObjs[i].gameObject.SetActive(i == charId);
		}
		if (charId != 0)
		{
			if (charId != 1)
			{
				if (charId == 2)
				{
					this.timeShow = 13f;
				}
			}
			else
			{
				this.timeShow = 11f;
			}
		}
		else
		{
			this.timeShow = 12f;
		}
		base.StartCoroutine(this.AutoClose());
	}

	private IEnumerator AutoClose()
	{
		yield return new WaitForSeconds(this.timeShow);
		this.Close();
		yield break;
	}

	public void Close()
	{
		if (this.panelDemoSkill.activeSelf)
		{
			
		}
		AudioClick.Instance.OnClick();
		this.demoSpecialCam.gameObject.SetActive(false);
		this.panelDemoSkill.SetActive(false);
		for (int i = 0; i < this.demoSpecialObjs.Length; i++)
		{
			this.demoSpecialObjs[i].OnHide();
		}
		for (int j = 0; j < this.listBomb.Count; j++)
		{
			this.listBomb[j].OnHide();
		}
		for (int k = 0; k < this.listBullet1.Count; k++)
		{
			this.listBullet1[k].OnHide(false);
		}
		for (int l = 0; l < this.listEfBomb.Count; l++)
		{
			this.listEfBomb[l].OnHide();
		}
		base.StopAllCoroutines();
	}

	public static DemoSpecialSkillManager instance;

	public GameObject demoSpecialCam;

	public GameObject panelDemoSkill;

	public DemoBaseObject[] demoSpecialObjs;

	public List<DemoBomb> listBomb;

	public List<DemoEfBomb> listEfBomb;

	public List<DemoBullet1> listBullet1;

	public ObjectPooling<DemoBomb> bombPool;

	public ObjectPooling<DemoEfBomb> efBombPool;

	public ObjectPooling<DemoBullet1> bullet1Pool;

	private float timeShow;

	public int testId;
}

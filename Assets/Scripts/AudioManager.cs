using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public void StopAll()
	{
		try
		{
			this.AudioBGCurrent.Stop();
			this.AudioBoss.Stop();
			this.maybay1.Stop();
			this.maybay2.Stop();
			SingletonGame<AudioController>.Instance.StopAll();
		}
		catch
		{
		}
	}

	public void Init()
	{
		this.isInit = true;
		this.ThrownGrenadePool = new ObjectPooling<Audios>(5, null, null);
		this.BoomPool = new ObjectPooling<Audios>(10, null, null);
		this.EnemyDiePool = new ObjectPooling<Audios>(5, null, null);
		this.KnifePool = new ObjectPooling<Audios>(5, null, null);
		this.CoinPool = new ObjectPooling<Audios>(5, null, null);
		for (int i = 0; i < this.ListCoin.Count; i++)
		{
			this.CoinPool.Store(this.ListCoin[i]);
		}
		for (int j = 0; j < this.ListKnife.Count; j++)
		{
			this.KnifePool.Store(this.ListKnife[j]);
		}
		for (int k = 0; k < this.ListThrownGrenade.Count; k++)
		{
			this.ThrownGrenadePool.Store(this.ListThrownGrenade[k]);
		}
		for (int l = 0; l < this.ListBoom.Count; l++)
		{
			this.BoomPool.Store(this.ListBoom[l]);
		}
		for (int m = 0; m < this.ListEnemyDie.Count; m++)
		{
			this.EnemyDiePool.Store(this.ListEnemyDie[m]);
		}
	}

	public void DestroyObject()
	{
		this.isInit = false;
		for (int i = 0; i < this.ListThrownGrenade.Count; i++)
		{
			if (this.ListThrownGrenade[i] != null && this.ListThrownGrenade[i].gameObject.activeSelf)
			{
				this.ListThrownGrenade[i].gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < this.ListBoom.Count; j++)
		{
			if (this.ListBoom[j] != null && this.ListBoom[j].gameObject.activeSelf)
			{
				this.ListBoom[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < this.ListEnemyDie.Count; k++)
		{
			if (this.ListEnemyDie[k] != null && this.ListEnemyDie[k].gameObject.activeSelf)
			{
				this.ListEnemyDie[k].gameObject.SetActive(false);
			}
		}
		for (int l = 0; l < this.ListKnife.Count; l++)
		{
			if (this.ListKnife[l] != null && this.ListKnife[l].gameObject.activeSelf)
			{
				this.ListKnife[l].gameObject.SetActive(false);
			}
		}
	}

	public void Items()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		this.item.Play();
	}

	public void Click()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		this.click.Play();
	}

	public void Combo(int index)
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		int idchar = ProfileManager.settingProfile.IDChar;
		if (idchar != 1)
		{
			for (int i = 0; i < this.combo.Length; i++)
			{
				if (index < this.combo.Length - 1 && i % 2 == 0)
				{
					this.combo[i].Stop();
				}
			}
			this.combo[Mathf.Min(index, this.combo.Length - 1)].volume = 0.3f;
			this.combo[Mathf.Min(index, this.combo.Length - 1)].Play();
		}
		else
		{
			for (int j = 0; j < this.comboGirl.Length; j++)
			{
				if (index < this.comboGirl.Length - 1 && j % 2 == 0)
				{
					this.comboGirl[j].Stop();
				}
			}
			this.comboGirl[Mathf.Min(index, this.comboGirl.Length - 1)].volume = 0.3f;
			this.comboGirl[Mathf.Min(index, this.comboGirl.Length - 1)].Play();
		}
	}

	public void Maybay1()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		this.maybay1.Play();
	}

	public void Maybay2()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		this.maybay2.Play();
	}

	public Audios PlayKnife()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return null;
		}
		Audios audios = this.KnifePool.New();
		if (audios == null)
		{
			audios = UnityEngine.Object.Instantiate<Transform>(this.ListKnife[0].transform).GetComponent<Audios>();
			audios.transform.parent = this.ListKnife[0].transform.parent;
			this.ListKnife.Add(audios);
		}
		audios.gameObject.SetActive(true);
		audios.Init();
		audios.audioSource.Play();
		return audios;
	}

	public void PlayThrownGrenade()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		Audios audios = this.ThrownGrenadePool.New();
		if (audios == null)
		{
			audios = UnityEngine.Object.Instantiate<Transform>(this.ListThrownGrenade[0].transform).GetComponent<Audios>();
			audios.transform.parent = this.ListThrownGrenade[0].transform.parent;
			this.ListThrownGrenade.Add(audios);
		}
		audios.gameObject.SetActive(true);
		audios.Init();
		audios.audioSource.Play();
	}

	public void PlayBoom()
	{
		if (!ProfileManager.settingProfile.IsSound || GameManager.Instance.StateManager.EState == EGamePlay.LOST || GameManager.Instance.StateManager.EState == EGamePlay.WIN)
		{
			return;
		}
		Audios audios = this.BoomPool.New();
		if (audios == null)
		{
			audios = UnityEngine.Object.Instantiate<Transform>(this.ListBoom[0].transform).GetComponent<Audios>();
			audios.transform.parent = this.ListBoom[0].transform.parent;
			this.ListBoom.Add(audios);
		}
		audios.gameObject.SetActive(true);
		audios.Init();
		audios.audioSource.Play();
	}

	public void PlayEnemyDie()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		Audios audios = this.EnemyDiePool.New();
		if (audios == null)
		{
			audios = UnityEngine.Object.Instantiate<Transform>(this.ListEnemyDie[0].transform).GetComponent<Audios>();
			audios.transform.parent = this.ListEnemyDie[0].transform.parent;
			this.ListEnemyDie.Add(audios);
		}
		audios.gameObject.SetActive(true);
		audios.Init();
		audios.audioSource.volume = 0.2f;
		audios.audioSource.Play();
	}

	public void PlayPickCoin()
	{
		if (!ProfileManager.settingProfile.IsSound)
		{
			return;
		}
		Audios audios = this.CoinPool.New();
		if (audios == null)
		{
			audios = UnityEngine.Object.Instantiate<Transform>(this.ListCoin[0].transform).GetComponent<Audios>();
			audios.transform.parent = this.ListCoin[0].transform.parent;
			this.ListCoin.Add(audios);
		}
		audios.gameObject.SetActive(true);
		audios.Init();
		audios.audioSource.volume = 0.2f;
		audios.audioSource.Play();
	}

	public void BG_Completed()
	{
		if (!ProfileManager.settingProfile.IsMusic)
		{
			return;
		}
		try
		{
			SingletonGame<AudioController>.Instance.StopAll();
			this.AudioBGCurrent.Stop();
			this.AudioBoss.Stop();
			this.maybay1.Stop();
			this.maybay2.Stop();
		}
		catch
		{
		}
		this.MissionCompleted.Play();
	}

	public void BG_Failed()
	{
		if (!ProfileManager.settingProfile.IsMusic)
		{
			return;
		}
		try
		{
			SingletonGame<AudioController>.Instance.StopAll();
			this.AudioBGCurrent.Stop();
			this.AudioBoss.Stop();
			this.maybay1.Stop();
			this.maybay2.Stop();
		}
		catch
		{
		}
		this.MissionFailed.Play();
	}

	public void Boss_Background()
	{
		if (!ProfileManager.settingProfile.IsMusic)
		{
			return;
		}
		LeanTween.value(base.gameObject, 1f, 0f, 2f).setOnUpdate(delegate(float val)
		{
			this.AudioBGCurrent.volume = val;
		});
		this.AudioBoss.Play();
		this.AudioBoss.volume = 0f;
		LeanTween.value(base.gameObject, 0f, 0.5f, 2f).setDelay(2f).setOnUpdate(delegate(float val)
		{
			this.AudioBoss.volume = val;
		});
	}

	public void Background()
	{
		if (!ProfileManager.settingProfile.IsMusic)
		{
			return;
		}
		switch (GameMode.Instance.modePlay)
		{
		case GameMode.ModePlay.Campaign:
		case GameMode.ModePlay.Special_Campaign:
			this.AudioBGCurrent.Play();
			if (this.AudioBoss != null && this.AudioBGCurrent.volume <= 0.3f)
			{
				this.AudioBGCurrent.volume = 0f;
				this.AudioBoss.Play();
				this.AudioBoss.volume = 0.5f;
			}
			break;
		case GameMode.ModePlay.Boss_Mode:
			if (this.AudioBoss != null)
			{
				this.AudioBoss.Play();
				this.AudioBoss.loop = true;
			}
			break;
		case GameMode.ModePlay.Endless:
			break;
		default:
			this.AudioBGCurrent.Play();
			break;
		}
	}

	[HideInInspector]
	public bool isInit;

	[Header("Object Pool")]
	public List<Audios> ListThrownGrenade;

	public ObjectPooling<Audios> ThrownGrenadePool;

	public List<Audios> ListBoom;

	public ObjectPooling<Audios> BoomPool;

	public List<Audios> ListEnemyDie;

	public ObjectPooling<Audios> EnemyDiePool;

	public List<Audios> ListKnife;

	public ObjectPooling<Audios> KnifePool;

	public List<Audios> ListCoin;

	public ObjectPooling<Audios> CoinPool;

	[Header("Audio Single")]
	public AudioSource maybay1;

	public AudioSource maybay2;

	public AudioSource[] combo;

	public AudioSource[] comboGirl;

	public AudioSource click;

	public AudioSource item;

	[Header("Background")]
	public AudioSource MissionCompleted;

	public AudioSource MissionFailed;

	public AudioSource AudioBGCurrent;

	public AudioSource AudioBoss;
}

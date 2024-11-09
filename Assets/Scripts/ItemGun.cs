using System;
using UnityEngine;

public class ItemGun : CachingMonoBehaviour
{
	private void Start()
	{
		if (this.ShowOnStart)
		{
			if (this.ShowRandom)
			{
				this.Show(UnityEngine.Random.Range(0, 5), this.transform.position);
			}
			else
			{
				this.Show(this.ID, this.transform.position);
			}
		}
	}

	private void OnDisable()
	{
		this.isInit = false;
		GameManager.Instance.giftManager.PoolItemGun.Store(this);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.timeClear += Time.deltaTime;
		if (this.timeClear >= 10f)
		{
			base.gameObject.SetActive(false);
			this.isInit = false;
		}
	}

	public void Show(int ID, Vector3 pos)
	{
		if (this.isInit)
		{
			return;
		}
		this.timeClear = 0f;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.ID = ID;
		for (int i = 0; i < this.objGunItem.Length; i++)
		{
			this.objGunItem[i].SetActive(ID == i);
		}
		this.rigidbody2D.velocity = Vector2.zero;
		this.rigidbody2D.AddForce(Vector2.up * 300f);
		this.isInit = true;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.gameObject.CompareTag("Rambo") || this.ID == -1)
		{
			return;
		}
		PlayerMain component = col.gameObject.GetComponent<PlayerMain>();
		if (component == null)
		{
			UnityEngine.Debug.Log("Player is null");
			return;
		}
		component._PlayerData.IDGUN2 = this.ID;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
		{
			if (!component.IsRemotePlayer && component.syncRamboState != null)
			{
				component.syncRamboState.SendRpc_SwitchGun(false, this.ID);
				component._PlayerInput.SwitchGun(false);
			}
		}
		else
		{
			component._PlayerInput.SwitchGun(false);
		}
		base.gameObject.SetActive(false);
	}

	public GameObject[] objGunItem;

	public int ID;

	public bool ShowOnStart;

	public bool ShowRandom;

	private bool isInit;

	private float timeClear;
}

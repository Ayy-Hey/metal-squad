using System;
using System.Collections.Generic;
using UnityEngine;

public class BayBomb : CachingMonoBehaviour
{
	private void Start()
	{
		if (this.gameMode != GameMode.Instance.EMode)
		{
			base.gameObject.SetActive(false);
			return;
		}
		for (int i = 0; i < this.listWarnings.Count; i++)
		{
			this.listWarnings[i].gameObject.SetActive(false);
		}
		this.poolBombBay = new ObjectPooling<BombBay>(this.listBombs.Count, null, null);
		for (int j = 0; j < this.listBombs.Count; j++)
		{
			this.listBombs[j].gameObject.SetActive(false);
			this.poolBombBay.Store(this.listBombs[j]);
		}
		this.damage *= GameMode.Instance.GetMode();
		this.bombCount = 0;
	}

	private void Update()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (!this.isRunning)
		{
			if (this.bombCount > 0)
			{
				this.UpdateRocket(deltaTime);
			}
			return;
		}
		this.isRunning = (GameManager.Instance.player.transform.position.x - this.transform.position.x <= this.rangeAttack);
		if (this.delay > 0f)
		{
			this.delay -= deltaTime;
		}
		if (this.delay <= 0f && this.isRunning && this.bombCount < this.maxAttackInTime)
		{
			this.bombCount++;
			this.CreatRocket();
			this.delay = UnityEngine.Random.Range(this.minDelay, this.maxDelay);
		}
		this.UpdateRocket(deltaTime);
		if (!this.isRunning)
		{
			this.objectCanhBao.SetActive(false);
		}
	}

	private void UpdateRocket(float deltaTime)
	{
		for (int i = 0; i < this.listBombs.Count; i++)
		{
			if (this.listBombs[i] && this.listBombs[i].isInit)
			{
				this.listBombs[i].OnUpdate(deltaTime);
			}
		}
	}

	private void CreatRocket()
	{
		Vector3 position = CameraController.Instance.transform.position;
		Vector2 vector = CameraController.Instance.Size();
		this.createPointBomb.x = UnityEngine.Random.Range(position.x - vector.x, position.x + vector.x);
		this.createPointBomb.y = position.y + vector.y;
		RaycastHit2D hit = Physics2D.Raycast(this.createPointBomb, Vector2.down, 20f, this.maskGround);
		int idWarning = -1;
		if (hit)
		{
			for (int i = 0; i < this.listWarnings.Count; i++)
			{
				if (!this.listWarnings[i].activeSelf)
				{
					idWarning = i;
					break;
				}
			}
			if (idWarning < 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.listWarnings[0]);
				this.listWarnings.Add(gameObject);
				gameObject.transform.parent = this.listWarnings[0].transform.parent;
				idWarning = this.listWarnings.IndexOf(gameObject);
			}
			this.listWarnings[idWarning].SetActive(true);
			this.listWarnings[idWarning].transform.position = hit.point;
		}
		BombBay bombBay = this.poolBombBay.New();
		if (!bombBay)
		{
			bombBay = UnityEngine.Object.Instantiate<BombBay>(this.listBombs[0]);
			this.listBombs.Add(bombBay);
			bombBay.gameObject.transform.parent = this.listBombs[0].gameObject.transform.parent;
		}
		bombBay.Init(this.damage, 0f, this.createPointBomb, delegate(BombBay bom)
		{
			this.bombCount--;
			if (idWarning >= 0)
			{
				this.listWarnings[idWarning].SetActive(false);
			}
			this.poolBombBay.Store(bom);
		});
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo") && !this.isRunning)
		{
			this.delay = 0f;
			this.isRunning = true;
		}
	}

	public GameMode.Mode gameMode;

	[HideInInspector]
	public bool isRunning;

	[SerializeField]
	private LayerMask maskGround;

	[SerializeField]
	private float rangeAttack;

	[SerializeField]
	private int maxAttackInTime;

	[SerializeField]
	private float minDelay;

	[SerializeField]
	private float maxDelay;

	[SerializeField]
	private List<GameObject> listWarnings;

	[SerializeField]
	private List<BombBay> listBombs;

	[SerializeField]
	private float damage;

	[SerializeField]
	private GameObject objectCanhBao;

	private ObjectPooling<BombBay> poolBombBay;

	private float delay;

	private Vector3 createPointBomb;

	private int bombCount;
}

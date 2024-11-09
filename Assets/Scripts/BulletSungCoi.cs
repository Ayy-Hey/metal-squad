using System;
using UnityEngine;

public class BulletSungCoi : CachingMonoBehaviour
{
	private void OnValidate()
	{
		if (!this.trailDuoi)
		{
			this.trailDuoi = base.GetComponentInChildren<TrailRenderer>();
		}
		this.trailDuoi.sortingLayerID = base.GetComponent<SpriteRenderer>().sortingLayerID;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		try
		{
			ISkill component = collision.GetComponent<ISkill>();
			if (component == null || !component.IsInVisible())
			{
				this.Hide();
				Vector3 position = this.transform.position;
				GameManager.Instance.fxManager.ShowEffect(5, position, Vector3.one, true, true);
				for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
				{
					Vector3 position2 = GameManager.Instance.ListRambo[i].transform.position;
					if (Mathf.Abs(position.x - position2.x) < 1f && position.y + 1f >= position2.y)
					{
						GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
					}
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public void Init(float speed, float damage, float timeRotateToTaget, Vector3 pos, Vector2 direction)
	{
		this.speed = speed;
		this.damage = damage;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.trailDuoi.Clear();
		base.GetComponent<Collider2D>().isTrigger = true;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);
		rotation.x = (rotation.y = 0f);
		this.transform.rotation = rotation;
		this.euler = rotation.eulerAngles;
		this.targetRotate = ((this.euler.z >= 180f) ? UnityEngine.Random.Range(180f, this.euler.z - 90f) : UnityEngine.Random.Range(this.euler.z + 90f, 180f));
		this.speedRotate = Mathf.Abs(this.euler.z - this.targetRotate) / timeRotateToTaget;
		this._coolDownHide = 5f;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.transform.Translate(deltaTime * this.speed * this.transform.up, Space.World);
		this.euler.z = Mathf.MoveTowards(this.euler.z, this.targetRotate, this.speedRotate * deltaTime);
		this.transform.eulerAngles = this.euler;
		if (!CameraController.Instance.IsInView(this.transform))
		{
			if (this._coolDownHide > 0f)
			{
				this._coolDownHide -= deltaTime;
			}
			else
			{
				this.Hide();
			}
		}
	}

	private void Hide()
	{
		this.isInit = false;
		GameManager.Instance.bulletManager.BulletSungCoiPool.Store(this);
		base.gameObject.SetActive(false);
	}

	[SerializeField]
	private TrailRenderer trailDuoi;

	private float speed;

	private float damage;

	private float speedRotate;

	private Vector3 euler;

	private float targetRotate;

	[HideInInspector]
	public bool isInit;

	private float _coolDownHide;
}

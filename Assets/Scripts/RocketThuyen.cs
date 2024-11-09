using System;
using UnityEngine;

public class RocketThuyen : CachingMonoBehaviour
{
	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			GameManager.Instance.bulletManager.RocketThuyenPool.Store(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			ISkill component = collision.GetComponent<ISkill>();
			if (component != null && component.IsInVisible())
			{
				return;
			}
			collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
		}
		GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
		base.gameObject.SetActive(false);
	}

	public void Init(float damage, float speed, Vector3 pos, Vector3 direction)
	{
		this.speed = speed;
		this.damage = damage;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this._oldPos = pos;
		this.rotate = Quaternion.LookRotation(direction, Vector3.back);
		this.rotate.x = (this.rotate.y = 0f);
		this.transform.rotation = this.rotate;
		this._vectorX = (float)((direction.x <= 0f) ? -1 : 1);
		this._coolDownHide = 5f;
		this._time = 0f;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (!CameraController.Instance.IsInView(this.transform))
		{
			if (this._coolDownHide <= 0f)
			{
				this.isInit = false;
				base.gameObject.SetActive(false);
				return;
			}
			this._coolDownHide -= deltaTime;
		}
		this._pos = this.transform.position;
		if (this._time < 1f)
		{
			this._time += deltaTime;
			this._pos.x = this._oldPos.x + this.curveX.Evaluate(this._time) * this._vectorX;
			this._pos.y = this._oldPos.y + this.curveY.Evaluate(this._time);
			this.transform.position = this._pos;
			this.LockRambo();
		}
		else
		{
			if (this._time < 1.5f)
			{
				this._time += deltaTime;
				this.LockRambo();
			}
			this.transform.Translate(deltaTime * this.speed * this.transform.up, Space.World);
			this.speed += deltaTime * this.speed / 2f;
		}
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.rotate, deltaTime * 2f);
	}

	private void LockRambo()
	{
		this.rotate = Quaternion.LookRotation(GameManager.Instance.player.tfOrigin.transform.position - this._pos, Vector3.back);
		this.rotate.x = (this.rotate.y = 0f);
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private AnimationCurve curveX;

	[SerializeField]
	private AnimationCurve curveY;

	private float damage;

	private float speed;

	private Quaternion rotate;

	private Vector3 _pos;

	private Vector3 _oldPos;

	private Vector3 _up;

	private float _time;

	private float _vectorX;

	private float _coolDownHide;
}

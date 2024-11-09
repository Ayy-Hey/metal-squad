using System;
using UnityEngine;

public class BulletBi : CachingMonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			this._veloY = this._maxVeloY;
		}
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					collision.GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
					this.Hide();
					GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
				}
			}
			catch
			{
			}
		}
	}

	public void Init(float damage, float speed, float startVeloY, float maxVeloY, bool isMoveLeft, Vector3 pos, Action<BulletBi> hideAction = null)
	{
		this.hide = hideAction;
		this._damage = damage;
		this._speed = speed;
		this._vectorX = (float)((!isMoveLeft) ? 1 : -1);
		this._veloY = startVeloY;
		this._maxVeloY = maxVeloY;
		this._coolDownHide = 2f;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (!this.renderer.isVisible)
		{
			if (this._coolDownHide > 0f)
			{
				this._coolDownHide -= deltaTime;
			}
			else
			{
				this.Hide();
			}
			return;
		}
		this.transform.Translate(this._speed * deltaTime * this._vectorX, this._veloY * deltaTime, 0f);
		this._veloY -= this.g * deltaTime;
	}

	public void Hide()
	{
		try
		{
			this.isInit = false;
			base.gameObject.SetActive(false);
			if (this.hide != null)
			{
				this.hide(this);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private Renderer renderer;

	[Range(0f, 10f)]
	[SerializeField]
	private float g = 9.8f;

	private Action<BulletBi> hide;

	private float _damage;

	private float _speed;

	private float _vectorX;

	private float _veloY;

	private float _maxVeloY;

	private Vector3 _pos;

	private float _coolDownHide;
}

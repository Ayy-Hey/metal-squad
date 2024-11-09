using System;
using UnityEngine;

public class RocketSpider : CachingMonoBehaviour
{
	public void Init(int id, float damage, float speed, Vector3 pos, Vector3 direction, Action<RocketSpider> onHide)
	{
		this._Id = id;
		this._damage = damage;
		this._speed = speed;
		this.hide = onHide;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.trail.Clear();
		this.rotate = Quaternion.LookRotation(direction, Vector3.back);
		this.rotate.x = (this.rotate.y = 0f);
		this.transform.rotation = this.rotate;
		this._coolDownHide = 7f;
		this._targetIsRambo = false;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (!this.objectRenderer.isVisible && this._coolDownHide > 0f)
		{
			this._coolDownHide -= deltaTime;
			if (this._coolDownHide <= 0f)
			{
				this.Hide();
				return;
			}
		}
		this.transform.Translate(this._speed * deltaTime * this.transform.up, Space.World);
		this._speed += deltaTime * 9.8f;
		if (!this._targetIsRambo && this.transform.position.y - CameraController.Instance.transform.position.y > 9f)
		{
			this._targetIsRambo = true;
			Vector3 a = GameManager.Instance.player.tfOrigin.transform.position + Vector3.right + Vector3.left * (float)this._Id;
			this.rotate = Quaternion.LookRotation(a - this.transform.position, Vector3.back);
			this.rotate.x = (this.rotate.y = 0f);
			this.transform.rotation = this.rotate;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.Hide();
		try
		{
			ISkill component = collision.GetComponent<ISkill>();
			if (component == null || !component.IsInVisible())
			{
				for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
				{
					float num = Vector3.Distance(this.transform.position, GameManager.Instance.ListRambo[i].tfOrigin.transform.position);
					if (num <= 1.5f)
					{
						GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
					}
				}
				if (collision.CompareTag("Ground"))
				{
					GameManager.Instance.fxManager.ShowEffect(0, this.transform.position, Vector3.one, true, true);
				}
				else
				{
					GameManager.Instance.fxManager.ShowExplosionSpine(this.transform.position, 0);
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void Hide()
	{
		try
		{
			this.isInit = false;
			base.gameObject.SetActive(false);
			this.hide(this);
		}
		catch
		{
		}
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private Renderer objectRenderer;

	[SerializeField]
	private TrailRenderer trail;

	private Action<RocketSpider> hide;

	private int _Id;

	private float _damage;

	private float _speed;

	private Quaternion rotate;

	private bool _targetIsRambo;

	private float _coolDownHide;
}

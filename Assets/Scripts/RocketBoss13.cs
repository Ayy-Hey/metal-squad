using System;
using UnityEngine;

public class RocketBoss13 : CachingMonoBehaviour
{
	public void InitObject(float damage, float startSpeed, float timeDelay, Vector3 pos, Vector3 direction, Transform target, Action<RocketBoss13> disableAction)
	{
		this._startSpeed = Mathf.Max(startSpeed, 5f);
		this._damage = damage;
		this._timeDelay = timeDelay;
		this.transform.position = pos;
		this._targetTransfrom = target;
		this.SetTagetRotation(direction);
		this.transform.rotation = this._rotation;
		this._rotation.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(120, 240));
		if (this.OnDisableAction == null)
		{
			this.OnDisableAction = disableAction;
		}
		this._coolDownHide = 5f;
		this.spriteRenderer.sortingOrder = 7;
		this.particle.sortingOrder = 7;
		this._deltaTime = 0.01f;
		this._locked = false;
		this.isInited = true;
	}

	public override void UpdateObject()
	{
		if (!this.isInited)
		{
			return;
		}
		this._timeDelay -= 0.01f;
		if (this._timeDelay <= 0f && !this._locked)
		{
			this._locked = true;
		}
		if (this._locked && this._timeDelay > -1f)
		{
			this.SetTagetRotation(this.transform.position - this._targetTransfrom.position);
		}
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this._rotation, this._deltaTime);
		this._deltaTime += Time.deltaTime / 20f;
		this.rigidbody2D.velocity = this.transform.up * this._startSpeed;
		if (!this.spriteRenderer.isVisible)
		{
			this._coolDownHide -= Time.fixedDeltaTime;
			if (this._coolDownHide <= 0f)
			{
				this.InActive();
			}
		}
	}

	private void SetTagetRotation(Vector3 direction)
	{
		this._rotation = Quaternion.LookRotation(direction, Vector3.forward);
		this._rotation.x = (this._rotation.y = 0f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Rambo"))
		{
			ISkill component = collision.GetComponent<ISkill>();
			if (component != null && component.IsInVisible())
			{
				return;
			}
			this._hpScript = collision.transform.GetComponent<IHealth>();
			if (this._hpScript != null)
			{
				this._hpScript.AddHealthPoint(-this._damage, EWeapon.NONE);
			}
		}
		try
		{
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.InActive();
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
	}

	private void InActive()
	{
		this.isInited = false;
		base.gameObject.SetActive(false);
		this.OnDisableAction(this);
	}

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private ParticleSystemRenderer particle;

	internal bool isInited;

	private Action<RocketBoss13> OnDisableAction;

	private float _startSpeed;

	private float _timeDelay;

	private float _damage;

	private Transform _targetTransfrom;

	private Quaternion _rotation;

	private float _deltaTime;

	private bool _locked;

	private float _distanceRambo;

	private IHealth _hpScript;

	private float _coolDownHide;
}

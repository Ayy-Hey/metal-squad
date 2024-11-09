using System;
using System.Collections;
using UnityEngine;

public class RocketBoss1_3 : CachingMonoBehaviour
{
	public void InitObject(float damage, float startSpeed, float timeDelay, Vector3 pos, Vector3 direction, Transform target, Action<RocketBoss1_3> disableAction)
	{
		base.gameObject.SetActive(true);
		this.spriteRenderer.enabled = true;
		this.col.enabled = true;
		this._startSpeed = startSpeed;
		this._damage = damage;
		this._timeDelay = timeDelay;
		this.transform.position = pos;
		this._targetTransfrom = target;
		this._rotation = Quaternion.LookRotation(direction, Vector3.back);
		this._rotation.x = (this._rotation.y = 0f);
		this.transform.rotation = this._rotation;
		if (this.OnDisableAction == null)
		{
			this.OnDisableAction = disableAction;
		}
		this.transformWarning.gameObject.SetActive(false);
		this.spriteRenderer.sortingOrder = 0;
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.particles[i].sortingOrder = 0;
		}
		this.isInit = true;
	}

	public void UpdateObject(float _deltaTime)
	{
		if (this.isInit)
		{
			if (this._timeDelay > 0f)
			{
				this.rigidbody2D.velocity = this.transform.up * this._startSpeed;
				this._timeDelay -= _deltaTime;
				if (this._timeDelay <= 0f)
				{
					this._timeDelay = 0f;
					this.transformWarning.parent = this.transform.parent;
					this.transformWarning.rotation = Quaternion.identity;
					this.transformWarning.gameObject.SetActive(true);
					this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
					float x = UnityEngine.Random.Range(this._targetTransfrom.position.x - 1f, this._targetTransfrom.position.x + 1f);
					this.transform.position = new Vector3(x, this.transform.position.y, 0f);
					this.spriteRenderer.sortingOrder = 7;
					for (int i = 0; i < this.particles.Length; i++)
					{
						this.particles[i].sortingOrder = 9 - i;
					}
				}
			}
			if (this._timeDelay == 0f)
			{
				this.rigidbody2D.velocity = this.transform.up * this._startSpeed;
				this._startSpeed += _deltaTime * 6f;
				this.hit = Physics2D.Raycast(this.transform.position, Vector2.down, 20f, this.mask);
				if (this.hit.collider != null)
				{
					this.transformWarning.position = this.hit.point;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ISkill component = collision.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
		{
			this._hpScript = null;
			this._distanceRambo = Vector3.Distance(this.transform.position, GameManager.Instance.ListRambo[i].transform.position);
			if (this._distanceRambo <= 1.5f || collision.transform == GameManager.Instance.ListRambo[i].transform)
			{
				this._hpScript = GameManager.Instance.ListRambo[i].GetComponent<IHealth>();
				if (this._hpScript != null)
				{
					this._hpScript.AddHealthPoint(-this._damage, EWeapon.NONE);
				}
			}
		}
		if (collision.CompareTag("Ground"))
		{
			GameManager.Instance.fxManager.ShowEffNo02(this.transform.position, Vector3.one);
		}
		else
		{
			GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
		}
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.clip, 0.3f);
		}
		catch
		{
		}
		base.StartCoroutine(this.InActive());
	}

	private IEnumerator InActive()
	{
		this.isInit = false;
		this.transformWarning.parent = this.transform;
		this.transformWarning.gameObject.SetActive(false);
		this.transformWarning.localPosition = Vector3.zero;
		this.spriteRenderer.enabled = false;
		this.col.enabled = false;
		this.rigidbody2D.velocity = Vector2.zero;
		yield return new WaitForSeconds(0.5f);
		base.gameObject.SetActive(false);
		this.OnDisableAction(this);
		yield break;
	}

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private Transform transformWarning;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Collider2D col;

	[SerializeField]
	private ParticleSystemRenderer[] particles;

	[SerializeField]
	private AudioClip clip;

	public bool isInit;

	private Action<RocketBoss1_3> OnDisableAction;

	private float _startSpeed;

	private float _timeDelay;

	private float _damage;

	private Transform _targetTransfrom;

	private Vector3 _targetMove;

	private Quaternion _rotation;

	private float _deltaTime;

	private RaycastHit2D hit;

	private float _distanceRambo;

	private IHealth _hpScript;
}

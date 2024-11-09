using System;
using UnityEngine;

public class RocketBossType1 : CachingMonoBehaviour
{
	public float TimeTarget
	{
		get
		{
			return this.timeTarget;
		}
		set
		{
			this._coolDownTarget = value;
			this.timeTarget = value;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.transform.position, Vector2.down, 5f, this.groundMask);
			this.Hide();
			GameManager.Instance.fxManager.ShowEffNo02(raycastHit2D.point, Vector3.one);
		}
		if (collision.CompareTag("Rambo"))
		{
			ISkill component = collision.GetComponent<ISkill>();
			if (component != null && component.IsInVisible())
			{
				return;
			}
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(this.transform.position, collision.transform.position - this.transform.position, 5f, collision.gameObject.layer);
			this.Hide();
			try
			{
				collision.GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
			}
			catch
			{
			}
			GameManager.Instance.fxManager.ShowFxNo01(raycastHit2D2.point, 1f);
		}
	}

	public void Init(float damage, float speed, Vector3 direction, Vector3 pos, Transform target, float timeDelayTarget, Action<RocketBossType1> onHide)
	{
		if (this.isFirst)
		{
			this.isFirst = false;
			this.groundMask = LayerMask.GetMask(new string[]
			{
				"Ground"
			});
		}
		this.OnHide = onHide;
		this._damage = damage;
		this._speed = speed;
		this._target = target;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.ProcessRotate(direction);
		this.transform.rotation = this._rotate;
		this._coolDownHide = 5f;
		this._coolDownDelayTarget = timeDelayTarget;
		this._coolDownTarget = this.timeTarget;
		int num = UnityEngine.Random.Range(-15, 15);
		this._eulerDelayZ = (int)this._rotate.eulerAngles.z;
		this._eulerDelayZ += num;
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
			this._coolDownHide -= deltaTime;
			if (this._coolDownHide <= 0f)
			{
				this.Hide();
				UnityEngine.Debug.Log("dkm___________________________" + this._coolDownHide);
				return;
			}
		}
		if (this._coolDownDelayTarget > 0f)
		{
			this._coolDownDelayTarget -= deltaTime;
			this.RotateTo((float)this._eulerDelayZ, 0.5f);
		}
		else if (this._coolDownTarget > 0f)
		{
			this._coolDownTarget -= deltaTime;
			this.ProcessRotate(this._target.position - this.transform.position);
			this.RotateTo(this._rotate.eulerAngles.z, 0.2f);
		}
		this.transform.Translate(this._speed * deltaTime * this.transform.up, Space.World);
		this._speed += 3.6f * deltaTime;
	}

	public void Hide()
	{
		this.isInit = false;
		base.gameObject.SetActive(false);
		if (this.OnHide != null)
		{
			this.OnHide(this);
			this.OnHide = delegate(RocketBossType1 A_0)
			{
			};
		}
	}

	private void RotateTo(float targetZ, float time)
	{
		this._euler = this.transform.eulerAngles;
		this._euler.z = Mathf.SmoothDampAngle(this._euler.z, targetZ, ref this._veloRotate, time);
		this.transform.eulerAngles = this._euler;
	}

	private void ProcessRotate(Vector3 direction)
	{
		this._rotate = Quaternion.LookRotation(direction, Vector3.back);
		this._rotate.x = (this._rotate.y = 0f);
	}

	[HideInInspector]
	public bool isInit;

	public Renderer renderer;

	[SerializeField]
	private float timeTarget;

	private Action<RocketBossType1> OnHide;

	private float _speed;

	private float _damage;

	private Quaternion _rotate;

	private Vector3 _euler;

	private Transform _target;

	private float _coolDownDelayTarget;

	private float _coolDownTarget;

	private float _coolDownHide;

	private float _veloRotate;

	private int _eulerDelayZ;

	private LayerMask groundMask;

	private bool isFirst = true;
}

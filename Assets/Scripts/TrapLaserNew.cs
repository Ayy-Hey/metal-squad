using System;
using System.Collections;
using UnityEngine;

public class TrapLaserNew : CachingMonoBehaviour
{
	private void OnValidate()
	{
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
		if (!this.laser)
		{
			this.laser = base.GetComponentInChildren<LaserHandMade>();
		}
	}

	private void Start()
	{
		this._canAttack = false;
		this._coolDown = 0f;
		this._direction = this.end.position - this.start.position;
		this._distance = Vector3.Distance(this.end.position, this.start.position);
		this.level = Mathf.Clamp(this.level, 0, this.data.datas.Length - 1);
	}

	private void Update()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (this.audioSource != null)
			{
				this.audioSource.mute = true;
			}
			return;
		}
		if (this.isInit)
		{
			float num = Vector2.Distance(this.transform.position, GameManager.Instance.player.GetPosition());
			if (num >= 12f)
			{
				if (this.audioSource != null)
				{
					this.audioSource.mute = true;
				}
				return;
			}
		}
		if (!this._canAttack)
		{
			this._canAttack = (this.gunRenderer.isVisible && this.transform.position.x - GameManager.Instance.player.transform.position.x <= this.data.datas[this.level].maxVision);
			if (this._canAttack)
			{
				this._coolDown = this.data.datas[this.level].timeReload * 0.5f;
				base.StartCoroutine(this.ChangeLaser());
			}
			return;
		}
		float deltaTime = Time.deltaTime;
		if (this._coolDown > 0f)
		{
			this._coolDown -= deltaTime;
		}
		if (this._coolDown <= 0f && this._canAttack && !this._isAttack)
		{
			this._isAttack = true;
			this._coolDownAttack = this.timeAttack;
			this.laser.Off();
			base.StartCoroutine(this.ChangeLaser());
			if (this.audioSource && ProfileManager.settingProfile.IsSound)
			{
				this.audioSource.mute = false;
				this.audioSource.volume = 0.5f;
				this.audioSource.Play();
			}
		}
		if (this._isAttack)
		{
			if (this._coolDownAttack > 0f)
			{
				this._coolDownAttack -= deltaTime;
				this._hit = Physics2D.Raycast(this.start.position, this._direction, this._distance, this.mask);
				try
				{
					if (this._hit.collider != null)
					{
						this._hit.collider.GetComponent<IHealth>().AddHealthPoint(-this.data.datas[this.level].damage * GameMode.Instance.GetMode(), EWeapon.NONE);
					}
				}
				catch
				{
				}
			}
			else
			{
				this._coolDown = this.data.datas[this.level].timeReload;
				this._isAttack = false;
				this.laser.Off();
				base.StartCoroutine(this.ChangeLaser());
				if (this.audioSource)
				{
					this.audioSource.mute = false;
					this.audioSource.Stop();
				}
			}
		}
		if (this._showLaser)
		{
			this.laser.OnShow(deltaTime, this.start.position, this.end.position);
		}
	}

	private IEnumerator ChangeLaser()
	{
		this.isInit = true;
		this._showLaser = false;
		if (this._isAttack)
		{
			this.laser.SetMainMaterials(1);
			this.laser.SetColor(Color.white);
		}
		else
		{
			this.laser.SetMainMaterials(0);
			this.laser.SetColor(Color.red);
			if (this.useFx)
			{
				this.laser.DisableEff();
			}
		}
		yield return this.waitChange;
		this._showLaser = true;
		if (this.useFx && this._isAttack)
		{
			this.laser.ActiveEff();
		}
		yield break;
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private int level;

	[SerializeField]
	private float timeAttack = 0.5f;

	[SerializeField]
	private LayerMask mask;

	[Tooltip("Đối tượng hiển thị ụ laser")]
	[SerializeField]
	private Renderer gunRenderer;

	[SerializeField]
	private bool useFx;

	[SerializeField]
	private Transform start;

	[SerializeField]
	private Transform end;

	[SerializeField]
	private LaserHandMade laser;

	[SerializeField]
	private AudioSource audioSource;

	private bool _canAttack;

	private float _coolDown;

	private bool _isAttack;

	private bool _showLaser;

	private float _coolDownAttack;

	private float _distance;

	private Vector2 _direction;

	private RaycastHit2D _hit;

	private WaitForSeconds waitChange = new WaitForSeconds(0.2f);

	private bool isInit;
}

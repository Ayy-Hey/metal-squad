using System;
using UnityEngine;

public class BaseGrenade : CachingMonoBehaviour
{
	public void TryAwake()
	{
		this.transform = base.GetComponent<Transform>();
		this.rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	public virtual void OnInit(PlayerMain player, bool FlipX, bool hasDamage = true)
	{
	}

	public void OnUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		this.spriteMain.transform.Rotate(0f, 0f, 5f * Time.deltaTime * 50f);
	}

	public void OnPause()
	{
		this.savedVelocity = this.rigidbody2D.velocity;
		this.savedAngularVelocity = this.rigidbody2D.angularVelocity;
		this.rigidbody2D.velocity = Vector2.zero;
		this.rigidbody2D.isKinematic = true;
	}

	public void OnResume()
	{
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.AddForce(this.savedVelocity, ForceMode2D.Impulse);
		this.rigidbody2D.AddTorque(this.savedAngularVelocity, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.CompareTag("Enemy") || coll.CompareTag("Tank") || coll.CompareTag("Boss") || coll.CompareTag("Ground"))
		{
			this.rigidbody2D.isKinematic = true;
			base.gameObject.SetActive(false);
			if (this.OnTrigerEnter != null)
			{
				this.OnTrigerEnter();
				this.OnTrigerEnter = null;
			}
		}
	}

	protected void SetSpine(int level)
	{
		int num = level / 10;
		this.spriteMain.sprite = this.spriteGrenades[num];
	}

	protected virtual void OnDisable()
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		}
		this.isInit = false;
		this.rigidbody2D.isKinematic = false;
	}

	private Vector3 savedVelocity;

	private float savedAngularVelocity;

	protected bool isInit;

	[SerializeField]
	protected LayerMask layerMask;

	protected float Damaged;

	protected float Radius;

	protected EWeapon weaponCurrent;

	protected Action OnTrigerEnter;

	public SpriteRenderer spriteMain;

	public Sprite[] spriteGrenades;

	public PlayerMain player;
}

using System;
using UnityEngine;

public class RocketJetpack : CachingMonoBehaviour
{
	private void OnDisable()
	{
		this.isShoot = false;
		int type = this.Type;
		if (type != 0)
		{
			if (type == 1)
			{
				try
				{
				}
				catch
				{
				}
			}
		}
		else
		{
			try
			{
				JetpackManager.Instance.RocketPool.Store(this);
			}
			catch
			{
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Rambo"))
		{
			return;
		}
		try
		{
			IHealth component = other.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-70f * GameMode.Instance.GetMode(), EWeapon.ROCKET);
				GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
				base.gameObject.SetActive(false);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(base.name + "|" + ex.Message);
		}
	}

	public void Shoot(float y)
	{
		Vector2 v = default(Vector2);
		v.y = y;
		v.x = CameraController.Instance.transform.position.x + 7f;
		this.isShoot = true;
		Vector3 position = v;
		this.transform.position = position;
		this.pos = position;
		this.axis = this.transform.up;
		this.mAudio.Play();
		this.isRight = true;
	}

	public void Shoot(Vector2 vt2, bool isRight)
	{
		this.transform.position = vt2;
		this.isShoot = true;
		this.pos = this.transform.position;
		this.axis = this.transform.up;
		this.mAudio.Play();
		this.isRight = isRight;
		this.transform.localScale = new Vector3((float)((!isRight) ? -1 : 1), 1f, 1f);
	}

	public override void UpdateObject()
	{
		if (!this.isShoot)
		{
			return;
		}
		if (this.isRight)
		{
			this.pos -= this.transform.right * Time.deltaTime * this.MoveSpeed;
		}
		else
		{
			this.pos += this.transform.right * Time.deltaTime * this.MoveSpeed;
		}
		this.transform.position = this.pos + this.axis * Mathf.Sin(Time.time * this.frequency) * this.magnitude;
		if (this.transform.position.x < CameraController.Instance.transform.position.x - 8f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private bool isShoot;

	private const float DAMAGE = 70f;

	private WaitForSeconds TIME_WAIT = new WaitForSeconds(1f);

	public float MoveSpeed = 5f;

	public float frequency = 20f;

	public float magnitude = 0.5f;

	private Vector3 axis;

	private Vector3 pos;

	public AudioSource mAudio;

	public int Type;

	private bool isRight;
}

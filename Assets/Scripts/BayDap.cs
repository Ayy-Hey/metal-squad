using System;
using System.Collections;
using UnityEngine;

public class BayDap : CachingMonoBehaviour
{
	public override void InitObject()
	{
		this.startY = this.transform.position.y;
		this.colliderBay.enabled = false;
		this.Stops[0].SetActive(false);
		this.Stops[1].SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.BAY_DAP);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
		if (collision.CompareTag("Ground"))
		{
			this.particle.Play();
			float time = (this.startY - this.transform.position.y) / 2f;
			this.rigidbody2D.gravityScale = 0f;
			this.rigidbody2D.velocity = Vector2.zero;
			this.colliderBay.enabled = false;
			if (ProfileManager.settingProfile.IsSound)
			{
				this.mAudio.Play();
			}
			base.StartCoroutine(this.GoUp(time));
			this.Stops[0].SetActive(true);
			this.Stops[1].SetActive(true);
		}
	}

	private IEnumerator Falling(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		this.rigidbody2D.gravityScale = 1f;
		this.colliderBay.enabled = true;
		yield break;
	}

	private IEnumerator GoUp(float time)
	{
		yield return new WaitForSeconds(1f);
		this.rigidbody2D.gravityScale = 0f;
		this.Stops[0].SetActive(false);
		this.Stops[1].SetActive(false);
		float upspeed = 0f;
		while (this.transform.position.y < this.startY)
		{
			yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
			this.transform.Translate(0f, upspeed * Time.fixedDeltaTime, 0f);
			upspeed += Time.fixedDeltaTime * 5f;
			yield return 0;
		}
		base.StartCoroutine(this.Falling(this.timeDelay));
		yield break;
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private void OnBecameVisible()
	{
		if (this.isInit)
		{
			return;
		}
		this.isInit = true;
		base.StartCoroutine(this.Falling(this.startTimeDelay));
	}

	private void OnBecameInvisible()
	{
		try
		{
			if (CameraController.Instance.transform.position.x > this.transform.position.x)
			{
				base.StopAllCoroutines();
				base.gameObject.SetActive(false);
			}
		}
		catch (Exception ex)
		{
		}
	}

	[SerializeField]
	private Collider2D colliderBay;

	[SerializeField]
	private ParticleSystem particle;

	[SerializeField]
	private float startTimeDelay;

	[SerializeField]
	private float timeDelay;

	[SerializeField]
	private float damage;

	[SerializeField]
	[Header("Khoảng cách phát hiện rambo để kích hoạt(theo trục x)")]
	private float range;

	private float startY;

	private bool isInit;

	private float distance;

	[SerializeField]
	private AudioSource mAudio;

	public GameObject[] Stops;
}

using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class SkillDuoiBoss15 : CachingMonoBehaviour
{
	private Action Done { get; set; }

	private void OnValidate()
	{
		try
		{
			if (!this.skeletonAnimation)
			{
				this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
			}
			if (this.skeletonAnimation)
			{
				Spine.Animation[] items = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
				this.anims = new string[items.Length];
				for (int i = 0; i < items.Length; i++)
				{
					this.anims[i] = items[i].Name;
				}
			}
			if (!this.coll)
			{
				this.coll = base.GetComponent<Collider2D>();
			}
		}
		catch
		{
		}
	}

	public void Init()
	{
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		base.gameObject.SetActive(false);
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (trackEntry.ToString().Contains(this.anims[0]))
		{
			this.coll.enabled = true;
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		if (trackEntry.ToString().Contains(this.anims[0]))
		{
			this.skeletonAnimation.state.SetAnimation(0, this.anims[1], false);
			this.coll.enabled = false;
		}
		else
		{
			try
			{
				this.Done();
			}
			catch
			{
				UnityEngine.Debug.Log("not completed skill duoi");
			}
		}
	}

	public void Attack(float damage, Vector3 pos, Action done = null)
	{
		this.Done = done;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this._damage = damage;
		this.skeletonAnimation.state.SetAnimation(0, this.anims[0], false);
		this.coll.enabled = false;
	}

	public void Run(float damage, Vector3 pos, Action done = null)
	{
		this.Done = done;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this._damage = damage;
		this.halfLength = this.rectTransform.sizeDelta.y / (360f / Camera.main.orthographicSize) / 2f * this.rectTransform.localScale.y;
		this.posY = this.rectTransform.anchoredPosition.y;
		this.isHit = false;
		this.rigidbody2D.gravityScale = 3f;
	}

	public override void UpdateObject()
	{
		this.hit = Physics2D.Raycast(this.transform.position, Vector2.down, 10f, this.mask);
		RaycastHit2D[] array = Physics2D.RaycastAll(this.transform.position, Vector2.down, 10f, this.mask);
		if (this.hit.collider)
		{
			this.img.fillAmount = (this.hit.distance + this.halfLength) / this.halfLength / 2f;
			if (!this.isHit && this.img.fillAmount < 1f)
			{
				this.isHit = true;
				this.par.gameObject.transform.position = this.hit.point;
				this.par.Play();
			}
		}
		if (this.rectTransform.anchoredPosition.y <= 20f)
		{
			this.CheckStop();
		}
		if (this.rigidbody2D.gravityScale < 0f && this.rectTransform.anchoredPosition.y >= this.posY)
		{
			if (this.Done != null)
			{
				this.Done();
			}
			this.rigidbody2D.gravityScale = 0f;
			this.rigidbody2D.velocity = Vector2.zero;
			base.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					collision.GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
				}
			}
			catch
			{
				UnityEngine.Debug.Log("don't attack rambo");
			}
		}
	}

	private void CheckStop()
	{
		if (this.rigidbody2D)
		{
			this.rigidbody2D.velocity = Vector2.zero;
			this.rigidbody2D.gravityScale = -3f;
		}
		try
		{
			for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
			{
				float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.ListRambo[i].transform.position.x);
				if (num <= 1f)
				{
					IHealth component = GameManager.Instance.ListRambo[i].GetComponent<IHealth>();
					if (component != null)
					{
						component.AddHealthPoint(-this._damage, EWeapon.NONE);
					}
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private Image img;

	[SerializeField]
	private ParticleSystem par;

	[SerializeField]
	private Collider2D coll;

	private float distanceGround;

	private float halfLength;

	private RaycastHit2D hit;

	private bool isHit;

	private float _damage;

	private float posY;
}

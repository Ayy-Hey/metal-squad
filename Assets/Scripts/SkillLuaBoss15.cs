using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkillLuaBoss15 : CachingMonoBehaviour
{
	public void Run(float damage, float veloX, Vector3 pos, Action<SkillLuaBoss15> hide)
	{
		if (this.hideAction == null)
		{
			this.hideAction = hide;
		}
		this._damage = damage;
		this._veloX = veloX;
		base.gameObject.SetActive(true);
		if (this.anims == null)
		{
			this.anims = this.ske.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		}
		this.transform.position = pos;
		this._scale = new Vector3(0.3f, 0.3f, 0.3f);
		this.ske.skeleton.FlipX = (veloX < 0f);
		this.ske.timeScale = 2f;
		this.ske.state.SetAnimation(0, this.anims[0], true);
		this.transform.localScale = this._scale;
		this.rigidbody2D.gravityScale = 1f;
		this.coolDown = 1f;
		this.isInit = true;
	}

	public override void UpdateObject()
	{
		float deltaTime = Time.deltaTime;
		if (!this.mesh.isVisible)
		{
			if (this.coolDown > 0f)
			{
				this.coolDown -= deltaTime;
			}
			else
			{
				this.Hide();
			}
		}
		if (this._scale.x < 1f)
		{
			this._scale.x = (this._scale.y = (this._scale.z = Mathf.MoveTowards(this._scale.z, 1f, deltaTime)));
			this.transform.localScale = this._scale;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			this.rigidbody2D.gravityScale = 0f;
			this.rigidbody2D.velocity = new Vector2(this._veloX * 0.75f, 0f);
		}
		if (collision.gameObject.CompareTag("Rambo"))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					IHealth component2 = collision.GetComponent<IHealth>();
					component2.AddHealthPoint(-this._damage, EWeapon.NONE);
					this.ske.timeScale = 1f;
					this.ske.state.SetAnimation(0, this.anims[1], false);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
	}

	private void Hide()
	{
		try
		{
			this.rigidbody2D.velocity = Vector2.zero;
			this.isInit = false;
			base.gameObject.SetActive(false);
			this.hideAction(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	internal bool isInit;

	[SerializeField]
	private SkeletonAnimation ske;

	[SerializeField]
	private MeshRenderer mesh;

	private Spine.Animation[] anims;

	private Action<SkillLuaBoss15> hideAction;

	private float _damage;

	private float _veloX;

	private Vector3 _scale;

	private float coolDown;
}

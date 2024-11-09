using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BombSpider : MonoBehaviour
{
	private void OnValidate()
	{
		try
		{
			if (!this.skeletonAnimation)
			{
				this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
			}
			Spine.Animation[] items = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.anims = new string[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				this.anims[i] = items[i].Name;
			}
		}
		catch
		{
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			this.state = BombSpider.EState.Attack;
			this.PlayAnim(false);
		}
	}

	public void Init(bool flip, float damage, Vector3 pos, Action<BombSpider> onHide)
	{
		this._damage = damage;
		this.hide = onHide;
		base.gameObject.SetActive(true);
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.skeletonAnimation.skeleton.FlipX = flip;
		this.meshRenderer.sortingOrder = 0;
		base.transform.position = pos;
		this.skeletonAnimation.state.ClearTracks();
		this._countIdle = 0;
		this.coll.enabled = false;
		this.state = BombSpider.EState.Idle2;
		this.PlayAnim(false);
	}

	public void Pause(bool pause)
	{
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		BombSpider.EState estate = this.state;
		if (estate != BombSpider.EState.Idle2)
		{
			if (estate != BombSpider.EState.Idle1)
			{
				if (estate == BombSpider.EState.Attack)
				{
					try
					{
						for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
						{
							float num = Vector3.Distance(base.transform.position, GameManager.Instance.ListRambo[i].tfOrigin.transform.position);
							if (num <= 1.5f)
							{
								GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
							}
						}
						this.Hide();
					}
					catch
					{
					}
				}
			}
			else
			{
				this._countIdle++;
				if (this._countIdle >= 8)
				{
					this.state = BombSpider.EState.Attack;
					this.PlayAnim(false);
				}
			}
		}
		else
		{
			this.coll.enabled = true;
			this.meshRenderer.sortingOrder = 2;
			this.state = BombSpider.EState.Idle1;
			this.PlayAnim(true);
		}
	}

	private void Hide()
	{
		this.skeletonAnimation.state.Complete -= this.OnComplete;
		Vector3 position = base.transform.position;
		position.y += 1f;
		GameManager.Instance.fxManager.ShowFxNo01(position, 1f);
		base.gameObject.SetActive(false);
		this.hide(this);
	}

	private void PlayAnim(bool loop = true)
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this.state], loop);
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private MeshRenderer meshRenderer;

	[SerializeField]
	private Collider2D coll;

	private float _damage;

	private Action<BombSpider> hide;

	private BombSpider.EState state;

	private int _countIdle;

	private enum EState
	{
		Attack,
		Idle1,
		Idle2
	}
}

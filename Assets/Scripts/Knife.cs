using System;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Knife : MonoBehaviour
{
	public void Show(PlayerMain _player, TrackEntry entry)
	{
		this.player = _player;
		this.isInit = true;
		base.gameObject.SetActive(true);
		Vector2 offset = this.circle.offset;
		offset.x = ((!this.player._PlayerSpine.FlipX) ? 0f : this.XOffset);
		this.circle.offset = offset;
		base.StartCoroutine(this.WaitCompleted(entry));
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.timeClearStuck += deltaTime;
		if (this.timeClearStuck >= 2f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		this.isInit = false;
		this.timeClearStuck = 0f;
		base.StopAllCoroutines();
	}

	private IEnumerator WaitCompleted(TrackEntry entry)
	{
		yield return new WaitForSpineAnimationComplete(entry);
		base.gameObject.SetActive(false);
		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		IHealth component = other.GetComponent<IHealth>();
		float damage = ProfileManager.melesProfile[this.player._PlayerData.IDKnife].Damage;
		if (component != null && !other.CompareTag("Tank"))
		{
			GameManager.Instance.fxManager.ShowEffect(8, other.transform.position, Vector3.one, true, true);
			int idknife = this.player._PlayerData.IDKnife;
			if (idknife != 0)
			{
				if (idknife == 1)
				{
					component.AddHealthPoint(-damage, EWeapon.AXE);
					base.gameObject.SetActive(false);
				}
			}
			else
			{
				component.AddHealthPoint(-damage, EWeapon.HUMMER);
				base.gameObject.SetActive(false);
			}
		}
	}

	public CircleCollider2D circle;

	private float timeClearStuck;

	private bool isInit;

	private float XOffset = -0.7f;

	private PlayerMain player;
}

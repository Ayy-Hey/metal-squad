using System;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

public class KnifeSword : MonoBehaviour
{
	public void Show(PlayerMain _player, TrackEntry entry)
	{
		this.player = _player;
		this.isInit = true;
		base.StopAllCoroutines();
		base.gameObject.SetActive(true);
		this.timeClearStuck = 0f;
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

	private void OnTriggerStay2D(Collider2D other)
	{
		IHealth component = other.GetComponent<IHealth>();
		float num = ProfileManager.melesProfile[this.player._PlayerData.IDKnife].Damage;
		num /= 10f;
		if (component != null && !other.CompareTag("Tank") && Time.timeSinceLevelLoad - this.timecounter > 0.2f)
		{
			GameManager.Instance.fxManager.ShowEffect(8, other.transform.position, Vector3.one, true, true);
			component.AddHealthPoint(-num, EWeapon.SWORD);
			this.timecounter = Time.timeSinceLevelLoad;
			this.AddHPPlayer();
		}
	}

	private void AddHPPlayer()
	{
	}

	private float timecounter;

	private float timeClearStuck;

	private bool isInit;

	private PlayerMain player;
}

using System;
using System.Collections;
using UnityEngine;

public class AirplaneRambo : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.BEGIN);
		yield return new WaitUntil(() => GameManager.Instance.player != null);
		yield return new WaitUntil(() => GameManager.Instance.player.IsInit);
		this.isinit = true;
		if (ProfileManager.settingProfile.IsMusic)
		{
			this.mAudio.volume = 0.3f;
			this.mAudio.Play();
		}
		yield break;
	}

	private void LateUpdate()
	{
		if (!this.isinit)
		{
			return;
		}
		if (base.transform.position.x > CameraController.Instance.RightCamera())
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!object.ReferenceEquals(GameManager.Instance.player, null) && GameManager.Instance.player.gameObject.activeSelf && this.isTarget && Time.timeSinceLevelLoad - this.timeCounter >= 0.5f)
		{
			base.transform.Translate(Vector3.right * this.SPEED * Time.deltaTime);
		}
		else
		{
			Vector3 target = Vector3.zero;
			if (this.tfRamboStart != null)
			{
				target = this.tfRamboStart.position;
			}
			target.y = base.transform.position.y;
			base.transform.position = Vector3.MoveTowards(base.transform.position, target, 2f * Time.deltaTime);
			float num = Mathf.Abs(base.transform.position.x - target.x);
			if (num < 0.1f && !this.isCreate && CameraController.Instance.isCompleted)
			{
				this.isCreate = true;
				this.isTarget = true;
				this.timeCounter = Time.timeSinceLevelLoad;
				GameManager.Instance.player._PlayerSpine.SetParachute(base.transform.position);
				this.SPEED = 10f;
			}
		}
	}

	private void OnBecameInvisible()
	{
		if (this.isCreate)
		{
			base.gameObject.SetActive(false);
		}
	}

	public Transform tfRamboStart;

	private bool isTarget;

	private float timeCounter;

	public AudioSource mAudio;

	private bool isCreate;

	private float SPEED = 6f;

	private bool isinit;
}

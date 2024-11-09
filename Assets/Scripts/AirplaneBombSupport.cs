using System;
using UnityEngine;

public class AirplaneBombSupport : MonoBehaviour
{
	public void CallSupport()
	{
		this.isInit = true;
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			this.count = 3;
		}
		else
		{
			this.count = ProfileManager.bombAirplaneSkillProfile.NumberTurn;
		}
		this.timeCreateBomb = 0f;
		this.countNodeBomb = 0;
		Vector3 position = base.transform.position;
		position.x = CameraController.Instance.transform.position.x - 9f;
		position.y = CameraController.Instance.transform.position.y + 2f;
		base.transform.position = position;
		if (ProfileManager.settingProfile.IsSound)
		{
			this.mAudio.Play();
		}
	}

	private void Update()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		Vector3 position = base.transform.position;
		position.y = CameraController.Instance.transform.position.y + 2f;
		base.transform.position = position;
		this.timeCreateBomb += Time.deltaTime;
		base.transform.Translate(Vector3.right * Time.deltaTime * 6f);
		if (this.timeCreateBomb >= 0.604f)
		{
			this.countNodeBomb++;
			this.timeCreateBomb = 0f;
			if (this.countNodeBomb < 5)
			{
			}
		}
		if (base.transform.position.x > CameraController.Instance.transform.position.x + 10f)
		{
			this.mAudio.Stop();
			this.timeCreateBomb = 0f;
			this.countNodeBomb = 0;
			this.count--;
			Vector3 position2 = base.transform.position;
			position2.x = CameraController.Instance.transform.position.x - 9f;
			base.transform.position = position2;
			if (ProfileManager.settingProfile.IsSound)
			{
				this.mAudio.Play();
			}
			if (this.count <= 0)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	private bool isInit;

	private int count = 3;

	private float timeCreateBomb;

	private int countNodeBomb;

	public AudioSource mAudio;
}

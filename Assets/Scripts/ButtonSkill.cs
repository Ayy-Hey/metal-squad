using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkill : MonoBehaviour
{
	public bool IsActive { get; set; }

	public void OnInit(float cooldown)
	{
		base.gameObject.SetActive(true);
		cooldown -= cooldown * GameManager.Instance.player._PlayerBooster.skillBooster;
		this._time = (this.cooldown = cooldown);
		this.slider.value = 0f;
		this.anim.gameObject.SetActive(false);
		this.IsActive = false;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit || this.IsActive)
		{
			return;
		}
		this._time -= Time.deltaTime;
		this._time = Mathf.Max(0f, this._time);
		if (this._time <= 0f)
		{
			this.anim.SetActive(true);
			this.IsActive = true;
		}
		this.slider.value = Mathf.Clamp01(1f - this._time / this.cooldown);
	}

	public void Reset()
	{
		if (this.callback != null)
		{
			this.callback();
		}
		this._time = this.cooldown;
		this.slider.value = 0f;
		this.IsActive = false;
		this.anim.gameObject.SetActive(false);
	}

	public Slider slider;

	public GameObject anim;

	private Action callback;

	private float cooldown;

	private bool isInit;

	private float _time;
}

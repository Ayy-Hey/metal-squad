using System;
using Player;
using UnityEngine;

public class BaseRambo : BaseCharactor, ISpeed, ISkill
{
	public float GetSpeedScale()
	{
		return this.ScaleSpeed;
	}

	public void NormalSpeed()
	{
		this.ScaleSpeed = 1f;
	}

	public void DownSpeed(float down_percent)
	{
		this.ScaleSpeed = 1f - down_percent;
	}

	public void UpSpeed(float up_percent)
	{
		this.ScaleSpeed = 1f + up_percent;
	}

	private void UpdateBaseRambo(float deltaTime)
	{
		this.TimeTrungDoc += deltaTime;
		if (this.TimeTrungDoc >= 3f && this.AnimTrungDoc != null && this.AnimTrungDoc.gameObject.activeSelf)
		{
			if (Time.timeSinceLevelLoad - this.TimeTrungDoc2 >= 0.5f)
			{
				IHealth component = this.transform.GetComponent<IHealth>();
				if (component != null)
				{
					component.AddHealthPoint(-20f, EWeapon.NONE);
				}
				this.TimeTrungDoc2 = Time.timeSinceLevelLoad;
			}
			if (this.AnimTrungDoc != null)
			{
				this.AnimTrungDoc.gameObject.SetActive(false);
			}
			this.NormalSpeed();
		}
	}

	public void TrungDoc()
	{
		this.TimeTrungDoc = 0f;
		this.ScaleSpeed = 0.8f;
		if (this.AnimTrungDoc != null)
		{
			this.AnimTrungDoc.OnPlay();
		}
	}

	private void OnParticleCollision(GameObject other)
	{
		if (other.name.Equals("Smoke"))
		{
			this.TrungDoc();
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		this.UpdateBaseRambo(deltaTime);
	}

	public override void DownSpeed()
	{
		base.DownSpeed();
		this.DownSpeed(0.5f);
	}

	public override void Normal_Speed()
	{
		base.Normal_Speed();
		this.NormalSpeed();
	}

	public bool IsInVisible()
	{
		return this.isInVisible;
	}

	private float ScaleSpeed = 1f;

	[SerializeField]
	private PlayerEffectTrungDoc AnimTrungDoc;

	private float TimeTrungDoc = float.MaxValue;

	private float TimeTrungDoc2;

	public bool isInVisible;
}

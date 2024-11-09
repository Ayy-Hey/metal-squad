using System;
using UnityEngine;

public class BombSupport : CachingMonoBehaviour
{
	public void Create(Vector3 pos)
	{
		this.transform.position = pos;
		this.isInit = true;
		this.box2D.enabled = true;
	}

	public void UpdateBomb(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.transform.Translate(Vector3.down * deltaTime * 5f);
	}

	private void OnDisable()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 3f, this.layermask);
		float num = ProfileManager.bombAirplaneSkillProfile.Damaged;
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			num = 70f;
		}
		for (int i = 0; i < array.Length; i++)
		{
			BaseEnemy component = array[i].GetComponent<BaseEnemy>();
			if (component != null && component.isInCamera)
			{
				component.AddHealthPoint(-num, EWeapon.BOMB);
			}
		}
		this.isInit = false;
		try
		{
		}
		catch (Exception ex)
		{
		}
	}

	public bool isInit;

	public BoxCollider2D box2D;

	[SerializeField]
	private LayerMask layermask;
}

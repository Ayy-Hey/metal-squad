using System;
using System.Collections;
using UnityEngine;

public class Box : CachingMonoBehaviour, IHealth
{
	private void Start()
	{
		this.InitObject();
	}

	public override void InitObject()
	{
		this.hp = 5f;
		for (int i = 0; i < this.boxCollider2D.Length; i++)
		{
			this.boxCollider2D[i].isTrigger = false;
		}
		this.rigidbody2D.isKinematic = false;
		if (this.timehide == null)
		{
			this.timehide = new WaitForSeconds(0.5f);
		}
	}

	public override void UpdateObject()
	{
	}

	public void AddHealthPoint(float hp, EWeapon lastWeapon)
	{
		this.hp += hp;
		if (this.hp <= 0f)
		{
			this.rigidbody2D.isKinematic = true;
			for (int i = 0; i < this.boxCollider2D.Length; i++)
			{
				this.boxCollider2D[i].isTrigger = true;
			}
			this.animator.SetTrigger("die");
			base.StartCoroutine(this.Hide());
		}
	}

	public float GetHealthPoint()
	{
		return this.hp;
	}

	private IEnumerator Hide()
	{
		yield return this.timehide;
		base.gameObject.SetActive(false);
		yield break;
	}

	private const string KEY_DIE = "die";

	private float hp;

	private WaitForSeconds timehide;

	public Animator animator;

	public BoxCollider2D[] boxCollider2D;
}

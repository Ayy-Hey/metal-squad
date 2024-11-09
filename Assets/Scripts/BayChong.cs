using System;
using UnityEngine;

public class BayChong : MonoBehaviour
{
	public void OnInit()
	{
		this.isInit = true;
		this.isAttack = false;
		this.timeCounter = 0f;
		this.timeAttack = 0f;
		this.objChong.gameObject.SetActive(false);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.timeCounter += deltaTime;
		if (this.timeCounter >= 1f)
		{
			if (!this.isAttack)
			{
				this.box.enabled = true;
				this.isAttack = true;
				this.objChong.gameObject.SetActive(true);
				this.objChong.Play(0);
			}
			this.timeAttack += deltaTime;
			if (this.timeAttack >= 1.5f)
			{
				this.timeAttack = 0f;
				this.timeCounter = 0f;
				this.isAttack = false;
				this.box.enabled = false;
				this.objChong.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			IHealth component = other.GetComponent<IHealth>();
			if (component != null && this.isAttack)
			{
				component.AddHealthPoint(-this.Damaged, EWeapon.BAYCHONG);
			}
		}
	}

	private float timeCounter;

	private float timeAttack;

	private bool isAttack;

	[SerializeField]
	private BoxCollider2D box;

	[SerializeField]
	private SpriteRenderer sprite;

	[SerializeField]
	private float Damaged = 10f;

	[SerializeField]
	private Animator objChong;

	private bool isInit;
}

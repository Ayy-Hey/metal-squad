using System;
using UnityEngine;

public class TrapJetpack : CachingMonoBehaviour
{
	public void OnInit()
	{
		this.ObjSprite.SetActive(true);
		this.isinit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isinit)
		{
			return;
		}
		if (GameManager.Instance.player.IsProteced)
		{
			this.transform.Translate(Vector3.right * this.Speed2 * deltaTime);
		}
		else
		{
			this.transform.Translate(Vector3.right * this.Speed * deltaTime);
		}
	}

	public void Reset()
	{
		LeanTween.moveX(base.gameObject, CameraController.Instance.transform.position.x - 10f, 1f);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			IHealth component = other.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-10000f, EWeapon.BOMB);
				this.isAttackToPlay = true;
			}
		}
	}

	[SerializeField]
	private GameObject ObjSprite;

	[SerializeField]
	private float Speed = 2f;

	private float Speed2 = 0.2f;

	private bool isinit;

	public bool isAttackToPlay;
}

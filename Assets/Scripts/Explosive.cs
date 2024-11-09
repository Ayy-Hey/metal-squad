using System;
using UnityEngine;

public class Explosive : CachingMonoBehaviour, IHealth
{
	private void Start()
	{
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.isKinematic = true;
		}
	}

	public void AddHealthPoint(float hp, EWeapon lastWeapon)
	{
		this.HP += hp;
		this.timePingPongColor = 0f;
		if (this.HP <= 0f)
		{
			GameManager.Instance.fxManager.ShowEffect(4, this.transform.position, Vector3.one, true, true);
			if (this.isBomb)
			{
				Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 1f, this.layerMask);
				for (int i = 0; i < array.Length; i++)
				{
					IHealth component = array[i].GetComponent<IHealth>();
					if (!object.ReferenceEquals(component, null))
					{
						component.AddHealthPoint(-this.Damage, EWeapon.BOMB);
					}
				}
			}
			if (this.isGift)
			{
				GameManager.Instance.giftManager.Create(this.transform.position, 0);
			}
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}

	public void Update()
	{
		if (!object.ReferenceEquals(GameManager.Instance.player, null))
		{
			float num = Mathf.Abs(GameManager.Instance.player.GetPosition().x - this.transform.position.x);
			if (num <= 8f && this.rigidbody2D != null)
			{
				this.rigidbody2D.isKinematic = false;
			}
		}
		this.OnUpdate();
	}

	public void OnUpdate()
	{
		this.PingPongColor();
	}

	private void PingPongColor()
	{
		this.timePingPongColor += Time.deltaTime;
		if (this.timePingPongColor >= 0.5f)
		{
			this.sprite.color = Color.white;
			return;
		}
		Color color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 10f, 1f));
		this.sprite.color = color;
	}

	[SerializeField]
	private float HP;

	[SerializeField]
	private SpriteRenderer sprite;

	private float timePingPongColor = 1f;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private bool isBomb;

	[SerializeField]
	private float Damage;

	[SerializeField]
	private bool isGift;
}

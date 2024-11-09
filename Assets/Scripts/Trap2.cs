using System;
using UnityEngine;

public class Trap2 : MonoBehaviour
{
	private void Update()
	{
		this.OnUpdate();
	}

	public void OnUpdate()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING || !this.isParrent || this.isStop)
		{
			return;
		}
		float num = Mathf.Abs(base.transform.position.x - GameManager.Instance.player.GetPosition().x);
		if (num <= 3f)
		{
			this.isThrown = true;
		}
		if (this.isThrown)
		{
			base.transform.position = new Vector2(base.transform.position.x, Mathf.MoveTowards(base.transform.position.y, this.Y, 5f * Time.deltaTime));
		}
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (this.isParrent || this.isFirst)
		{
			return;
		}
		if (coll.gameObject.CompareTag("Rambo"))
		{
			IHealth component = coll.gameObject.GetComponent<IHealth>();
			if (!object.ReferenceEquals(component, null))
			{
				this.isFirst = true;
				component.AddHealthPoint(-this.DAMAGE, EWeapon.NONE);
			}
		}
	}

	[SerializeField]
	private float Y;

	[SerializeField]
	private float DAMAGE;

	[SerializeField]
	private bool isParrent;

	private bool isThrown;

	private bool isStop;

	private bool isFirst;
}

using System;
using UnityEngine;

public class FxFire : MonoBehaviour
{
	public void OnInit()
	{
		this.TIME_LIFE = ProfileManager.grenadesProfile[2].GetOption(EGrenadeOption.Effect_Time);
		this.isInit = true;
		this.timelife = 0f;
		this.time_life_attack = 0f;
		Vector3 position = base.transform.position;
		position.y = 0.1f;
		Vector3 v = position;
		position.x += 0.1f;
		RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 100f, this.layerMask);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.up, 100f, this.layerMask);
		RaycastHit2D hit2 = Physics2D.Raycast(v, Vector2.down, 100f, this.layerMask);
		float num = 0f;
		this.DAMAGED = ProfileManager.grenadesProfile[2].GetOption(EGrenadeOption.Damage) / 10f;
		if (hit && hit2)
		{
			Vector2 from = hit.point - hit2.point;
			num = Vector2.Angle(from, Vector2.right) * (float)((hit.point.y > hit2.point.y) ? -1 : 1);
			Vector3 position2 = base.transform.position;
			position2.y = ((Vector2.Distance(base.transform.position, hit.point) >= Vector2.Distance(base.transform.position, raycastHit2D.point)) ? raycastHit2D.point.y : hit.point.y);
			base.transform.position = position2;
		}
		base.transform.localRotation = Quaternion.Euler(0f, 0f, -num);
	}

	public void OnPreview(Vector2 pos)
	{
		this.isInit = true;
		this.timelife = 0f;
		this.time_life_attack = 0f;
		this.TIME_LIFE = 1.5f;
		base.transform.position = pos;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.timelife += deltaTime;
		this.time_life_attack += deltaTime;
		if (this.timelife >= this.TIME_LIFE)
		{
			base.gameObject.SetActive(false);
		}
		if (this.time_life_attack >= this.TIME_LIFE_ATTACK && !this.isPreview)
		{
			this.time_life_attack = 0f;
			for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
			{
				bool flag = false;
				try
				{
					flag = this.box2D.IsTouching(GameManager.Instance.ListEnemy[i].bodyCollider2D);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log(ex.Message);
				}
				if (GameManager.Instance.ListEnemy[i] && flag)
				{
					if (GameManager.Instance.ListEnemy[i].CompareTag("Tank"))
					{
						GameManager.Instance.fxManager.CreateFxFlame01(0, GameManager.Instance.ListEnemy[i].transform, 2f);
					}
					else
					{
						GameManager.Instance.fxManager.CreateFxFlame01(0, GameManager.Instance.ListEnemy[i].transform, 1f);
					}
					GameManager.Instance.ListEnemy[i].AddHealthPoint(-this.DAMAGED, EWeapon.BOMB);
				}
			}
		}
	}

	private void OnDisable()
	{
		try
		{
			if (!this.isPreview)
			{
				GameManager.Instance.fxManager.PoolFxFire.Store(this);
			}
			else
			{
				PreviewWeapon.Instance.PoolFxFire.Store(this);
			}
		}
		catch
		{
		}
	}

	private float TIME_LIFE = 5f;

	private float TIME_LIFE_ATTACK = 0.5f;

	private float DAMAGED = 10f;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private BoxCollider2D box2D;

	private bool isInit;

	private float timelife;

	private float time_life_attack;

	public bool isPreview;
}

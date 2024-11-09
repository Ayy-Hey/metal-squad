using System;
using System.Collections.Generic;
using UnityEngine;

public class FxFire2 : MonoBehaviour
{
	public void OnPreview(Vector2 pos)
	{
		this.anim.Play(0);
		this.isInit = true;
		this.timelife = 0f;
		this.time_life_attack = 0f;
		this.TIME_LIFE = 1.5f;
		base.transform.position = pos;
	}

	public void OnInit()
	{
		this.TIME_LIFE = ProfileManager.grenadesProfile[3].GetOption(EGrenadeOption.Effect_Time);
		this.anim.Play(0);
		this.isInit = true;
		this.timelife = 0f;
		this.time_life_attack = 0f;
		Vector3 position = base.transform.position;
		position.y = 0.1f;
		Vector3 v = position;
		position.x += 0.1f;
		this.DAMAGED = ProfileManager.grenadesProfile[3].GetOption(EGrenadeOption.Damage) / 10f;
		RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 100f, this.layerMask);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.up, 100f, this.layerMask);
		RaycastHit2D hit2 = Physics2D.Raycast(v, Vector2.down, 100f, this.layerMask);
		float num = 0f;
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
				if (GameManager.Instance.ListEnemy[i] != null && flag)
				{
					GameManager.Instance.ListEnemy[i].AddHealthPoint(-this.DAMAGED, EWeapon.BOMB);
					if (GameManager.Instance.ListEnemy[i].CompareTag("Tank"))
					{
						GameManager.Instance.fxManager.CreateFxFlame01(1, GameManager.Instance.ListEnemy[i].transform, 2f);
					}
					else
					{
						GameManager.Instance.fxManager.CreateFxFlame01(1, GameManager.Instance.ListEnemy[i].transform, 1f);
					}
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (this.isPreview)
		{
			return;
		}
		if (other.CompareTag("Enemy") || other.CompareTag("Tank") || other.CompareTag("Boss"))
		{
			IToxic component = other.GetComponent<IToxic>();
			if (component != null)
			{
				component.HitToxic(this.DAMAGED_SLOW);
				this.CachedToxic.Add(component);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (this.isPreview)
		{
			return;
		}
		if (other.CompareTag("Enemy") || other.CompareTag("Tank") || other.CompareTag("Boss"))
		{
			IToxic component = other.GetComponent<IToxic>();
			if (component != null)
			{
				component.ReleaseToxic(this.DAMAGED_SLOW);
				if (this.CachedToxic.Contains(component))
				{
					this.CachedToxic.Remove(component);
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
				for (int i = 0; i < this.CachedToxic.Count; i++)
				{
					this.CachedToxic[i].ReleaseToxic(this.DAMAGED_SLOW);
				}
				this.CachedToxic.Clear();
				GameManager.Instance.fxManager.PoolFxFire2.Store(this);
			}
			else
			{
				PreviewWeapon.Instance.PoolFxFire2.Store(this);
			}
		}
		catch
		{
		}
	}

	private float TIME_LIFE = 5f;

	private float TIME_LIFE_ATTACK = 0.5f;

	private float DAMAGED = 10f;

	private readonly float DAMAGED_SLOW = 0.3f;

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private BoxCollider2D box2D;

	private bool isInit;

	private float timelife;

	private float time_life_attack;

	private List<IToxic> CachedToxic = new List<IToxic>();

	public bool isPreview;
}

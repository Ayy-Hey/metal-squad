using System;
using UnityEngine;

public class BodyLine : MonoBehaviour
{
	private void OnValidate()
	{
	}

	public void Active(float damage, float activeTime, bool oneShot = true)
	{
		this.damage = damage;
		this.oneShot = oneShot;
		this.activeTime = activeTime;
		this.polygon.enabled = true;
		this.lineRenderer.enabled = true;
		this.ActiveParticle(true);
		this.lineRenderer.positionCount = this.transformBodyPoins.Length;
		this.polygonPath = new Vector2[this.transformBodyPoins.Length];
		this.polygonPath[0] = Vector2.zero;
		this.lineRenderer.sortingLayerName = "Gameplay";
		this.lineRenderer.sortingOrder = 2;
		this.isActive = true;
	}

	public void Deactive()
	{
		this.isActive = false;
		Behaviour behaviour = this.polygon;
		bool enabled = false;
		this.lineRenderer.enabled = enabled;
		behaviour.enabled = enabled;
		this.ActiveParticle(false);
		try
		{
			this.hideAction(this.oneShot);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isActive)
		{
			return;
		}
		if (this.activeTime > 0f)
		{
			this.activeTime -= deltaTime;
			if (this.activeTime <= 0f)
			{
				this.Deactive();
				return;
			}
		}
		this.SetPositionLineRenderer();
		this.SetPolygonPath();
		this.ChangeOffset(deltaTime);
	}

	private void SetPositionLineRenderer()
	{
		if (this.transformBodyPoins == null)
		{
			return;
		}
		for (int i = 0; i < this.transformBodyPoins.Length; i++)
		{
			this.lineRenderer.SetPosition(i, this.transformBodyPoins[i].position);
		}
	}

	private void SetPolygonPath()
	{
		for (int i = 1; i < this.transformBodyPoins.Length; i++)
		{
			this.polygonPath[i] = this.transformBodyPoins[i].position - this.transformBodyPoins[0].position;
		}
		this.polygon.SetPath(0, this.polygonPath);
	}

	private void ChangeOffset(float deltaTime)
	{
		this.offset = this.lineRenderer.materials[0].mainTextureOffset;
		this.offset.x = this.offset.x + deltaTime;
		this.lineRenderer.materials[0].mainTextureOffset = this.offset;
	}

	private void ActiveParticle(bool active)
	{
		for (int i = 0; i < this.objParticles.Length; i++)
		{
			this.objParticles[i].SetActive(active);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!this.oneShot)
		{
			return;
		}
		try
		{
			if (collision.CompareTag("Rambo"))
			{
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
			}
		}
		catch (Exception ex)
		{
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		try
		{
			if (collision.CompareTag("Rambo"))
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					GameManager.Instance.player.AddHealthPoint(-this.damage, EWeapon.NONE);
				}
			}
		}
		catch (Exception ex)
		{
		}
	}

	public Action<bool> hideAction;

	[NonSerialized]
	public bool isActive;

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private PolygonCollider2D polygon;

	[SerializeField]
	private Transform[] transformBodyPoins;

	[SerializeField]
	private GameObject[] objParticles;

	private Vector2 offset;

	private Vector2[] polygonPath;

	private float activeTime;

	private float damage;

	private bool oneShot;

	private bool ramboIsGround;
}

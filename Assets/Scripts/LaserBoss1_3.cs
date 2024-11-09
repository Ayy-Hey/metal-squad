using System;
using UnityEngine;

public class LaserBoss1_3 : MonoBehaviour
{
	public void Init(float damage, float timeDelay, float timeActive, Vector2 direction, Action hideAction)
	{
		this.isStart = false;
		this.isFire = false;
		this.hideAction = hideAction;
		this.damage = damage;
		this.timeDelay = timeDelay;
		this.timeActive = timeActive;
		this.direction = direction;
		this.lineRenderer.sortingLayerName = "Gameplay";
		this.lineRenderer.sortingOrder = 50;
		this.lineRenderer.materials[0].mainTextureOffset = Vector2.zero;
		this.lineRenderer.materials[0] = null;
		this.lineRenderer.SetPosition(0, Vector3.zero);
		this.lineRenderer.SetPosition(1, Vector3.zero);
	}

	public void UpdateObject(float delta)
	{
		if (!this.isStart)
		{
			this.isStart = true;
			if (this.timeDelay > 0f)
			{
				this.lineRenderer.material = this.materialWarning;
				LineRenderer lineRenderer = this.lineRenderer;
				Color color = this.colorWarning;
				this.lineRenderer.endColor = color;
				lineRenderer.startColor = color;
			}
			else
			{
				this.isFire = true;
				this.lineRenderer.material = this.materialLaser;
				LineRenderer lineRenderer2 = this.lineRenderer;
				Color color = this.colorLaser;
				this.lineRenderer.endColor = color;
				lineRenderer2.startColor = color;
			}
			Vector3 position = base.transform.position;
			this.lineRenderer.SetPosition(0, position);
			this.hit = Physics2D.Raycast(position, this.direction, 12f, this.maskGround);
			this.lineRenderer.SetPosition(1, this.hit.point);
		}
		this.ChangeOffset(delta);
		if (this.timeDelay > 0f)
		{
			this.timeDelay -= delta;
			if (this.timeDelay <= 0f)
			{
				this.isFire = true;
				this.lineRenderer.material = this.materialLaser;
				LineRenderer lineRenderer3 = this.lineRenderer;
				Color color = this.colorLaser;
				this.lineRenderer.endColor = color;
				lineRenderer3.startColor = color;
				this.objEffEnd.transform.position = this.lineRenderer.GetPosition(1);
				this.ActiveEff(true);
			}
		}
		if (this.isFire)
		{
			this.hit = Physics2D.Raycast(base.transform.position, this.direction, 12f, this.maskTarget);
			if (this.hit.collider != null)
			{
				try
				{
					this.hit.transform.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log(ex.Message);
				}
			}
			if (this.timeActive > 0f)
			{
				this.timeActive -= delta;
			}
			else
			{
				try
				{
					this.hideAction();
					this.ActiveEff(false);
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.Log(ex2.Message);
				}
			}
		}
	}

	private void ChangeOffset(float deltaTime)
	{
		this.offset = this.lineRenderer.materials[0].mainTextureOffset;
		this.offset.x = this.offset.x - deltaTime;
		this.lineRenderer.materials[0].mainTextureOffset = this.offset;
	}

	private void ActiveEff(bool enable)
	{
		this.objEffStart.SetActive(enable);
		this.objEffEnd.SetActive(enable);
	}

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private Material materialWarning;

	[SerializeField]
	private Material materialLaser;

	[SerializeField]
	private Color colorWarning;

	[SerializeField]
	private Color colorLaser;

	[SerializeField]
	private LayerMask maskGround;

	[SerializeField]
	private LayerMask maskTarget;

	[SerializeField]
	private GameObject objEffStart;

	[SerializeField]
	private GameObject objEffEnd;

	private Action hideAction;

	private float timeDelay;

	private float damage;

	private bool isStart;

	private bool isFire;

	private RaycastHit2D hit;

	private Vector2 offset;

	private Vector3 direction;

	private float timeActive;
}

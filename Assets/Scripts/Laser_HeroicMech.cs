using System;
using UnityEngine;

public class Laser_HeroicMech : MonoBehaviour
{
	private void OnValidate()
	{
		try
		{
			this.lineRenderer.sortingLayerName = "Gameplay";
			this.lineRenderer.sortingOrder = 2;
		}
		catch
		{
		}
	}

	public void InitObject(float damage, float timeActive, float timeDelay, Vector2 direction, Action hideAction)
	{
		this.damage = damage;
		this.timeActive = timeActive;
		this.timeDelay = timeDelay;
		this.direction = direction;
		this.hideAction = hideAction;
		Vector3 position = CameraController.Instance.transform.position;
		Vector3 position2 = base.transform.position;
		this.endParticle.transform.position = position2;
		base.gameObject.SetActive(true);
		this.bornParticle.SetActive(true);
		this.startParticle.SetActive(false);
		this.endParticle.SetActive(false);
		this.lineRenderer.enabled = false;
		this.lineRenderer.sortingLayerName = "Gameplay";
		this.lineRenderer.sortingOrder = 2;
		this.scale = Vector2.up;
		this.offset = Vector2.zero;
		this.laserLenght = 0f;
		this.isInit = true;
	}

	public void UpdateObject(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.timeDelay > 0f)
		{
			this.timeDelay -= deltaTime;
			if (this.timeDelay <= 0f)
			{
				this.startParticle.SetActive(true);
				this.endParticle.SetActive(true);
				this.lineRenderer.enabled = true;
				this.lineRenderer.SetPosition(0, base.transform.position);
			}
			return;
		}
		Vector3 position = this.endParticle.transform.position;
		position.x += 30f * deltaTime * this.direction.x;
		this.endParticle.transform.position = position;
		this.laserLenght = Vector3.Distance(base.transform.position, position);
		this.lineRenderer.SetPosition(1, this.endParticle.transform.position);
		this.hit = Physics2D.Raycast(base.transform.position, this.direction, this.laserLenght, this.mask);
		if (this.hit.collider != null)
		{
			try
			{
				this.hit.collider.GetComponent<IHealth>().AddHealthPoint(-this.damage * deltaTime, EWeapon.NONE);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
		if (this.timeActive > 0f)
		{
			this.timeActive -= deltaTime;
			if (this.timeActive <= 0f)
			{
				try
				{
					this.isInit = false;
					base.gameObject.SetActive(false);
					this.hideAction();
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.Log(ex2.Message);
				}
			}
		}
		this.scale.x = this.laserLenght / this.maxLenght;
		this.offset.x = this.offset.x + deltaTime;
		this.lineRenderer.material.mainTextureScale = this.scale;
		this.lineRenderer.material.mainTextureOffset = this.offset;
	}

	[NonSerialized]
	public bool isInit;

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private GameObject bornParticle;

	[SerializeField]
	private GameObject startParticle;

	[SerializeField]
	private GameObject endParticle;

	[SerializeField]
	private LayerMask mask;

	private Action hideAction;

	private float damage;

	private float timeActive;

	private float timeDelay;

	private Vector2 direction;

	private float laserLenght;

	private float maxLenght;

	private RaycastHit2D hit;

	private Vector2 offset;

	private Vector2 scale;
}

using System;
using UnityEngine;

public class LaserBoss1_6 : MonoBehaviour
{
	public void ActiveLaser(int sortingOrder, float time, float endLeftX, float endRightX)
	{
		float x = this.startPoin.position.x;
		this.endPoinLeftX = endLeftX + x;
		this.endPoinRightX = endRightX + x;
		this.speedMoveEndPoin = Mathf.Abs(endLeftX - endRightX) / time;
		this.endPos = this.endPoin.position;
		this.endPos.x = this.endPoinLeftX;
		this.endPoin.position = this.endPos;
		this.startPoin.gameObject.SetActive(true);
		this.endPoin.gameObject.SetActive(true);
		this.line.SetPosition(0, this.startPoin.position);
		this.line.SetPosition(1, this.endPos);
		this.isActive = true;
	}

	public void InActive()
	{
		this.isActive = false;
		this.startPoin.gameObject.SetActive(false);
		this.endPoin.gameObject.SetActive(false);
		this.line.SetPosition(0, Vector3.zero);
		this.line.SetPosition(1, Vector3.zero);
		this._ramboName = string.Empty;
	}

	public void UpdateObject(float deltaTime)
	{
		if (this.isActive)
		{
			this._offset = this.line.material.mainTextureOffset;
			this._offset.x = this._offset.x + deltaTime * this.speed;
			this.line.material.mainTextureOffset = this._offset;
			this.endPos.x = Mathf.MoveTowards(this.endPos.x, this.endPoinRightX, this.speedMoveEndPoin * deltaTime);
			this.endPoin.position = this.endPos;
			this.direc = this.endPos - this.startPoin.position;
			this.hit = Physics2D.Raycast(this.startPoin.position, this.direc, 30f, this.mask);
			if (this.hit.collider != null && this.hit.collider.tag.Equals("Rambo"))
			{
				try
				{
					ISkill component = this.hit.collider.GetComponent<ISkill>();
					if (component != null && component.IsInVisible())
					{
						return;
					}
					this.hit.collider.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
					this.ActionAttackToRambo();
				}
				catch
				{
					UnityEngine.Debug.Log("loi khong chay");
				}
			}
			this.line.SetPosition(0, this.startPoin.position);
			this.line.SetPosition(1, this.endPoin.position);
			if (Mathf.Abs(this.endPoinRightX - this.endPos.x) < 0.001f)
			{
				this.InActive();
			}
		}
	}

	public Action ActionAttackToRambo;

	public float damage;

	public LayerMask mask;

	public LineRenderer line;

	public Transform startPoin;

	public Transform endPoin;

	public float speed = 4f;

	[HideInInspector]
	public bool isActive;

	private float endPoinLeftX;

	private float endPoinRightX;

	private Vector3 endPos;

	private float speedMoveEndPoin;

	private RaycastHit2D hit;

	private Vector2 direc;

	private string _ramboName;

	private Vector2 _offset;
}

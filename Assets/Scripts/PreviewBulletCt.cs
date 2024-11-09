using System;
using UnityEngine;

public class PreviewBulletCt : MonoBehaviour
{
	private void OnDisable()
	{
		this.isInit = false;
		try
		{
			this.hideCallback();
		}
		catch
		{
		}
	}

	public void OnInit(int rank, Vector3 pos, Vector2 Direction, Action hideCallback)
	{
		this.hideCallback = hideCallback;
		base.gameObject.SetActive(true);
		base.transform.position = pos;
		this.childScale = 0.1f;
		this.Direction = Direction.normalized;
		base.transform.rotation = Quaternion.FromToRotation(Vector3.up, Direction);
		this.pingpongRange = 1f;
		this.tfChild.transform.localScale = Vector3.one * this.childScale;
		this.tfChild.transform.rotation = Quaternion.identity;
		this.rank = rank;
		if (rank != 0)
		{
			if (rank != 1)
			{
				if (rank == 2)
				{
					this.units[0].Init(new Action<Vector3, bool>(this.OnAttack), true);
					this.units[1].Init(new Action<Vector3, bool>(this.OnAttack), false);
					this.units[2].Init(new Action<Vector3, bool>(this.OnAttack), false);
				}
			}
			else
			{
				this.units[0].Init(new Action<Vector3, bool>(this.OnAttack), true);
				this.units[1].Init(new Action<Vector3, bool>(this.OnAttack), false);
			}
		}
		else
		{
			this.units[0].Init(new Action<Vector3, bool>(this.OnAttack), true);
		}
		this.counter_time_hide = 0f;
		this.timeHide = 5f / this.Speed;
		this.isInit = true;
	}

	private void OnAttack(Vector3 point, bool isMain)
	{
		if (isMain)
		{
			PreviewWeapon.Instance.CreateFxMainCt(point, 1f);
			PreviewWeapon.Instance.CreateFxNo2(0, 1, point, 1.5f);
		}
		else
		{
			PreviewWeapon.Instance.CreateFxNo2(0, 1, point, 0.8f);
		}
		for (int i = 0; i <= this.rank; i++)
		{
			if (this.units[i].isInit)
			{
				return;
			}
		}
		base.gameObject.SetActive(false);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.childScale < 0.99f)
		{
			this.childScale = Mathf.SmoothDamp(this.childScale, 1f, ref this.veloChildScale, 0.2f);
			if (this.childScale >= 0.99f)
			{
				this.childScale = 1f;
			}
			this.tfChild.transform.localScale = this.childScale * Vector3.one;
		}
		this.time += deltaTime * 2f;
		float t = Mathf.PingPong(this.time, 1f);
		float num = Mathf.SmoothStep(-this.pingpongRange, this.pingpongRange, t);
		if (this.units[1].isInit)
		{
			this.unitPos = this.units[1].transform.localPosition;
			this.unitPos.y = num;
			this.units[1].transform.localPosition = this.unitPos;
		}
		if (this.units[2].isInit)
		{
			this.unitPos = this.units[2].transform.localPosition;
			this.unitPos.y = -num;
			this.units[2].transform.localPosition = this.unitPos;
		}
		this.counter_time_hide += deltaTime;
		if (this.counter_time_hide >= this.timeHide)
		{
			base.gameObject.SetActive(false);
		}
		base.transform.Translate(this.Speed * deltaTime * this.Direction, Space.World);
	}

	public bool isInit;

	[SerializeField]
	private Transform tfChild;

	[SerializeField]
	private PreviewBulletCtUnit[] units;

	public float Speed = 10f;

	private float childScale;

	private Vector2 Direction;

	private float pingpongRange;

	private float counter_time_hide;

	private Action hideCallback;

	private float veloChildScale;

	private float time;

	private Vector3 unitPos;

	private int rank;

	private float timeHide;
}

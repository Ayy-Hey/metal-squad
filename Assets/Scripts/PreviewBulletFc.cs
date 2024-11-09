using System;
using UnityEngine;

public class PreviewBulletFc : MonoBehaviour
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

	public void OnInit(int rank, Vector3 pos, Vector3 direction, Action hideCallback, float angle = 0f, float scale = 1f, bool isMain = true, Collider2D igroneColl = null)
	{
		this.igroneColl = igroneColl;
		this.isMain = isMain;
		this.hideCallback = hideCallback;
		this.rank = rank;
		base.gameObject.SetActive(true);
		base.transform.position = pos;
		base.transform.localScale = Vector3.one * scale;
		Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles;
		eulerAngles.z += angle;
		base.transform.eulerAngles = eulerAngles;
		this.coolDownHide = 0f;
		this.timeHide = ((!isMain) ? (2f / this.speed) : (5f / this.speed));
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		base.transform.Translate(this.speed * deltaTime * base.transform.up, Space.World);
		this.coolDownHide += deltaTime;
		if (this.coolDownHide >= this.timeHide)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (this.isMain)
		{
			base.gameObject.SetActive(false);
			PreviewWeapon.Instance.CreateFxNo1(base.transform.position, 0.8f);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			int num = this.rank;
			if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						float num2 = -30f;
						for (int i = 0; i < 6; i++)
						{
							PreviewWeapon.Instance.CreateBulletFc(this.rank, position, up, num2, 0.7f, false, coll);
							num2 += 60f;
						}
					}
				}
				else
				{
					float num2 = 0f;
					for (int j = 0; j < 5; j++)
					{
						PreviewWeapon.Instance.CreateBulletFc(this.rank, position, up, num2, 0.7f, false, coll);
						num2 += 72f;
					}
				}
			}
			else
			{
				float num2 = -45f;
				for (int k = 0; k < 4; k++)
				{
					PreviewWeapon.Instance.CreateBulletFc(this.rank, position, up, num2, 0.7f, false, coll);
					num2 += 90f;
				}
			}
		}
		else
		{
			if (coll == this.igroneColl)
			{
				return;
			}
			base.gameObject.SetActive(false);
			PreviewWeapon.Instance.CreateFxNo2(1, 2, base.transform.position, 0.8f);
		}
	}

	public bool isInit;

	public float speed = 10f;

	private Action hideCallback;

	private int rank;

	private Collider2D igroneColl;

	private bool isMain;

	private float coolDownHide;

	private float timeHide;
}

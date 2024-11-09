using System;
using UnityEngine;

public class CallManSupport : MonoBehaviour
{
	public void CallSupport()
	{
		if (this.isStart)
		{
			return;
		}
		Vector2 v = new Vector2(CameraController.Instance.transform.position.x - 12.6f, CameraController.Instance.transform.position.y + 1f);
		base.transform.position = v;
		this.isStart = true;
		this.isCreateManSupporter = false;
		this.timedelay = 0f;
		this.step = 0;
	}

	private void Update()
	{
		if (!this.isStart || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		int num = this.step;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					base.transform.Translate(Vector2.right * Time.deltaTime * 5f);
					if (base.transform.position.x > CameraController.Instance.transform.position.x + 8f)
					{
						base.gameObject.SetActive(false);
						this.isStart = false;
					}
				}
			}
			else
			{
				this.timedelay += Time.deltaTime;
				if (!this.isCreateManSupporter)
				{
					this.CreateMan();
					this.isCreateManSupporter = true;
				}
				if (this.timedelay >= 1f)
				{
					this.step = 2;
				}
			}
		}
		else
		{
			base.transform.Translate(Vector2.right * Time.deltaTime * 4f);
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.posRambo.position, Vector2.down, 1000f, this.layerGround);
			if (base.transform.position.x >= CameraController.Instance.transform.position.x - 2f && raycastHit2D.collider != null)
			{
				this.timedelay = 0f;
				this.step = 1;
			}
		}
	}

	private void CreateMan()
	{
	}

	private bool isStart;

	private int step;

	private float timedelay;

	private bool isCreateManSupporter;

	public Transform posRambo;

	public LayerMask layerGround;
}

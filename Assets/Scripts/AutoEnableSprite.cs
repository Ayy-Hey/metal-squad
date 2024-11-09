using System;
using UnityEngine;

public class AutoEnableSprite : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.canControlSpriteVisible || Application.isPlaying)
		{
			return;
		}
		this.listSprite = base.GetComponentsInChildren<SpriteRenderer>();
		this.listViewPos = new ViewPos[this.listSprite.Length];
		for (int i = 0; i < this.listSprite.Length; i++)
		{
			this.CaculatorViewPos(i);
		}
	}

	private void Update()
	{
		if (!GameManager.Instance && !CameraController.Instance.isInit)
		{
			return;
		}
		if (this.canControlSpriteVisible && this.listSprite != null && this.listSprite.Length > 0)
		{
			for (int i = 0; i < this.listSprite.Length; i++)
			{
				if (this.listSprite[i])
				{
					if (this.canMove)
					{
						this.CaculatorViewPos(i);
					}
					bool flag = this.listViewPos[i].minX <= CameraController.Instance.viewPos.maxX && this.listViewPos[i].maxX >= CameraController.Instance.viewPos.minX && this.listViewPos[i].minY <= CameraController.Instance.viewPos.maxY && this.listViewPos[i].maxY >= CameraController.Instance.viewPos.minY;
					if (this.listSprite[i].enabled != flag)
					{
						this.listSprite[i].enabled = flag;
					}
				}
			}
		}
	}

	private void CaculatorViewPos(int spriteId)
	{
		if (!this.listSprite[spriteId].sprite)
		{
			return;
		}
		this.listViewPos[spriteId].minX = this.listSprite[spriteId].bounds.min.x;
		this.listViewPos[spriteId].maxX = this.listSprite[spriteId].bounds.max.x;
		this.listViewPos[spriteId].minY = this.listSprite[spriteId].bounds.min.y;
		this.listViewPos[spriteId].maxY = this.listSprite[spriteId].bounds.max.y;
	}

	[Header(" Sprite visible control:")]
	public bool canControlSpriteVisible = true;

	public bool canMove;

	public SpriteRenderer[] listSprite;

	public ViewPos[] listViewPos;

	private Vector3 camPos;
}

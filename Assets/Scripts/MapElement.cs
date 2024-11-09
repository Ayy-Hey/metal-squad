using System;
using UnityEngine;

public class MapElement : MonoBehaviour
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

	public void OnUpdate(Vector2 SizeOffset)
	{
		Vector2 size = this.Size;
		size.x = (float)Screen.width / 1.75f * 0.01f;
		size.y = (float)Screen.height / 1.75f * 0.01f;
		Vector3 position = base.transform.position;
		Vector3 position2 = CameraController.Instance.transform.position;
		Vector2 vector = CameraController.Instance.Size();
		bool flag = position.x - (size.x + SizeOffset.x) > position2.x + vector.x;
		bool flag2 = position.x + (size.x + SizeOffset.x) < position2.x - vector.x;
		bool flag3 = position.y - (size.y + SizeOffset.y) > position2.y + vector.y;
		bool flag4 = position.y + (size.y + SizeOffset.y) < position2.y - vector.y;
		bool flag5 = true;
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				flag5 = (position2.y < base.transform.position.y);
			}
		}
		else
		{
			flag5 = (position2.x < base.transform.position.x);
		}
		if (flag || flag2 || flag3 || flag4)
		{
			if (!this.isPreview)
			{
				base.gameObject.SetActive(false);
			}
			else if (!flag5)
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(true);
		}
	}

	public void CheckMapView()
	{
		if (this.canControlSpriteVisible && base.gameObject.activeSelf && this.listSprite != null && this.listSprite.Length > 0)
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

	[SerializeField]
	private Vector2 Size = new Vector2(6.4f, 3.6f);

	[SerializeField]
	private bool isPreview;

	public bool getData;

	[Header(" Sprite visible control:")]
	public bool canControlSpriteVisible;

	public bool canMove;

	public SpriteRenderer[] listSprite;

	public ViewPos[] listViewPos;
}

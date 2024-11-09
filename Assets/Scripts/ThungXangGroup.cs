using System;
using System.Collections.Generic;
using UnityEngine;

public class ThungXangGroup : MonoBehaviour
{
	public void RemoveObject(ThungXang thungxang)
	{
		this.ListThungXang.Remove(thungxang);
		for (int i = 0; i < this.ListThungXang.Count; i++)
		{
			if (!this.ListThungXang[i].isMain)
			{
				this.ListThungXang[i].rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
			}
		}
		if (this.GroupJump != null)
		{
			this.GroupJump.SetActive(this.ListThungXang.Count == 1);
		}
		this.GroupWall.SetActive(this.ListThungXang.Count > 1);
	}

	public List<ThungXang> ListThungXang;

	public GameObject GroupJump;

	public GameObject GroupWall;
}

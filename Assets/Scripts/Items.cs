using System;
using UnityEngine;

public class Items : MonoBehaviour
{
	private void Start()
	{
		base.name = this.ID.ToString();
	}

	public int ID;
}

using System;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
	public void OnUpdate(float speed)
	{
		base.transform.Translate(speed * Time.deltaTime * Vector2.left);
	}

	public int ID;

	[SerializeField]
	private GameObject[] arrSprites;
}

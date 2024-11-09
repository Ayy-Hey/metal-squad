using System;
using UnityEngine;

public class CargoShip : MonoBehaviour
{
	private void Update()
	{
		this.OnUpdate();
	}

	public void OnUpdate()
	{
		base.transform.position = new Vector3(base.transform.position.x, Mathf.PingPong(Time.time * 0.2f, 0.3f), base.transform.position.z);
	}
}

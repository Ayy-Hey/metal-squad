using System;
using UnityEngine;

public class StopGateCampaign : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Rambo"))
		{
			return;
		}
        Debug.Log("StopGateCampaign");
		this.Gate.ShowNewStart();
	}

	[SerializeField]
	private GateCampaign Gate;
}

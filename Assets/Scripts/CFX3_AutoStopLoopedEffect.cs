using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CFX3_AutoStopLoopedEffect : MonoBehaviour
{
	private void OnEnable()
	{
		this.d = this.effectDuration;
	}

	private void Update()
	{
		if (this.d > 0f)
		{
			this.d -= Time.deltaTime;
			if (this.d <= 0f)
			{
				base.GetComponent<ParticleSystem>().Stop(true);
				CFX3_Demo_Translate component = base.gameObject.GetComponent<CFX3_Demo_Translate>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}

	public float effectDuration = 2.5f;

	private float d;
}

using System;
using System.Collections;
using UnityEngine;

public class CFX_ShurikenThreadFix : MonoBehaviour
{
	private void OnEnable()
	{
		this.systems = base.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in this.systems)
		{
			particleSystem.enableEmission = false;
		}
		base.StartCoroutine("WaitFrame");
	}

	private IEnumerator WaitFrame()
	{
		yield return null;
		foreach (ParticleSystem particleSystem in this.systems)
		{
			particleSystem.enableEmission = true;
			particleSystem.Play(true);
		}
		yield break;
	}

	private ParticleSystem[] systems;
}

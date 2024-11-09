using System;
using UnityEngine;

public class TestingRigidbodyCS : MonoBehaviour
{
	private void Start()
	{
		this.ball1 = GameObject.Find("Sphere1");
		LeanTween.rotateAround(this.ball1, Vector3.forward, -90f, 1f);
		LeanTween.move(this.ball1, new Vector3(2f, 0f, 7f), 1f).setDelay(1f).setRepeat(-1);
	}

	private void Update()
	{
	}

	private GameObject ball1;
}

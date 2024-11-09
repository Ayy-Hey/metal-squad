using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwoDLaserPack
{
	public class DemoFollowScript : MonoBehaviour
	{
		private void Start()
		{
			this.isHomeAndShouldDeactivate = false;
			this.movingToDeactivationTarget = false;
			this.acquiredTargets = new List<Transform>();
			if (this.target == null)
			{
				UnityEngine.Debug.Log("No target found for the FollowScript on: " + base.gameObject.name);
			}
		}

		private void OnEnable()
		{
		}

		private void Update()
		{
			if (this.shouldFollow && this.target != null)
			{
				this.newPosition = Vector2.Lerp(base.transform.position, this.target.position, Time.deltaTime * this.speed);
				base.transform.position = new Vector3(this.newPosition.x, this.newPosition.y, base.transform.position.z);
			}
		}

		private void OnDisable()
		{
		}

		public Transform target;

		public float speed;

		public bool shouldFollow;

		public bool isHomeAndShouldDeactivate;

		public bool movingToDeactivationTarget;

		private Vector3 newPosition;

		public List<Transform> acquiredTargets;
	}
}

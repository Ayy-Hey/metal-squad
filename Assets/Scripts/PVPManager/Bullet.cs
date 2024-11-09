using System;
using Photon.Realtime;
using UnityEngine;

namespace PVPManager
{
	public class Bullet : MonoBehaviour
	{
		public Photon.Realtime.Player Owner { get; private set; }

		public void Start()
		{
			UnityEngine.Object.Destroy(base.gameObject, 3f);
		}

		public void OnCollisionEnter(Collision collision)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void InitializeBullet(Photon.Realtime.Player owner, Vector3 originalDirection, float lag)
		{
			this.Owner = owner;
			base.transform.forward = originalDirection;
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.velocity = originalDirection * 200f;
			component.position += component.velocity * lag;
		}
	}
}

using System;
using System.Collections;
using UnityEngine;

namespace UltimateJoystickExample.Spaceship
{
	public class AsteroidController : MonoBehaviour
	{
		public void Setup(Vector3 force, Vector3 torque)
		{
			this.myRigidbody = base.GetComponent<Rigidbody>();
			this.myRigidbody.AddForce(force);
			this.myRigidbody.AddTorque(torque);
			base.StartCoroutine(this.DelayInitialDestruction((!this.isDebris) ? 1f : 0.25f));
		}

		private IEnumerator DelayInitialDestruction(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			this.canDestroy = true;
			yield break;
		}

		private void Update()
		{
			if ((Mathf.Abs(base.transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect * 1.3f || Mathf.Abs(base.transform.position.z) > Camera.main.orthographicSize * 1.3f) && this.canDestroy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnCollisionEnter(Collision theCollision)
		{
			if (theCollision.gameObject.name == "Bullet")
			{
				UnityEngine.Object.Destroy(theCollision.gameObject);
				this.asteroidManager.ModifyScore(this.isDebris);
				if (!this.isDebris)
				{
					this.Explode();
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			else if (theCollision.gameObject.name == "Player")
			{
				this.asteroidManager.SpawnExplosion(theCollision.transform.position);
				UnityEngine.Object.Destroy(theCollision.gameObject);
				if (!this.isDebris)
				{
					this.Explode();
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				this.asteroidManager.ShowDeathScreen();
			}
			else if (!this.isDebris && this.canDestroy)
			{
				this.Explode();
			}
			else if (this.isDebris && this.canDestroy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.asteroidManager.SpawnExplosion(base.transform.position);
		}

		private void Explode()
		{
			if (this.isDestroyed)
			{
				return;
			}
			this.isDestroyed = true;
			this.asteroidManager.SpawnDebris(base.transform.position);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public GameManager asteroidManager;

		private Rigidbody myRigidbody;

		private bool canDestroy;

		private bool isDestroyed;

		public bool isDebris;
	}
}

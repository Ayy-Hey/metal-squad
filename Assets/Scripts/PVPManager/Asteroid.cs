using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace PVPManager
{
	public class Asteroid : MonoBehaviour
	{
		public void Awake()
		{
			this.photonView = base.GetComponent<PhotonView>();
			this.rigidbody = base.GetComponent<Rigidbody>();
			if (this.photonView.InstantiationData != null)
			{
				this.rigidbody.AddForce((Vector3)this.photonView.InstantiationData[0]);
				this.rigidbody.AddTorque((Vector3)this.photonView.InstantiationData[1]);
				this.isLargeAsteroid = (bool)this.photonView.InstantiationData[2];
			}
		}

		public void Update()
		{
			if (!this.photonView.IsMine)
			{
				return;
			}
			if (Mathf.Abs(base.transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect || Mathf.Abs(base.transform.position.z) > Camera.main.orthographicSize)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
			if (this.isDestroyed)
			{
				return;
			}
			if (collision.gameObject.CompareTag("Bullet"))
			{
				if (this.photonView.IsMine)
				{
					Bullet component = collision.gameObject.GetComponent<Bullet>();
					component.Owner.AddScore((!this.isLargeAsteroid) ? 1 : 2);
					this.DestroyAsteroidGlobally();
				}
				else
				{
					this.DestroyAsteroidLocally();
				}
			}
			else if (collision.gameObject.CompareTag("Player") && this.photonView.IsMine)
			{
				collision.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All, new object[0]);
				this.DestroyAsteroidGlobally();
			}
		}

		private void DestroyAsteroidGlobally()
		{
			this.isDestroyed = true;
			if (this.isLargeAsteroid)
			{
				int num = UnityEngine.Random.Range(3, 6);
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = Quaternion.Euler(0f, (float)i * 360f / (float)num, 0f) * Vector3.forward * UnityEngine.Random.Range(0.5f, 1.5f) * 300f;
					Vector3 vector2 = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(500f, 1500f);
					object[] data = new object[]
					{
						vector,
						vector2,
						false,
						PhotonNetwork.Time
					};
					PhotonNetwork.InstantiateSceneObject("SmallAsteroid", base.transform.position + vector.normalized * 10f, Quaternion.Euler(0f, UnityEngine.Random.value * 180f, 0f), 0, data);
				}
			}
			PhotonNetwork.Destroy(base.gameObject);
		}

		private void DestroyAsteroidLocally()
		{
			this.isDestroyed = true;
			base.GetComponent<Renderer>().enabled = false;
		}

		public bool isLargeAsteroid;

		private bool isDestroyed;

		private PhotonView photonView;

		private Rigidbody rigidbody;
	}
}

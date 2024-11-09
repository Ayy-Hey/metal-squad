using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace PVPManager
{
	public class Spaceship : MonoBehaviour
	{
		public void Awake()
		{
			this.photonView = base.GetComponent<PhotonView>();
			this.rigidbody = base.GetComponent<Rigidbody>();
			this.collider = base.GetComponent<Collider>();
			this.renderer = base.GetComponent<Renderer>();
		}

		public void Start()
		{
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
			{
				renderer.material.color = AsteroidsGame.GetColor(this.photonView.Owner.GetPlayerNumber());
			}
		}

		public void Update()
		{
			if (!this.photonView.IsMine || !this.controllable)
			{
				return;
			}
			this.rotation = UnityEngine.Input.GetAxis("Horizontal");
			this.acceleration = UnityEngine.Input.GetAxis("Vertical");
			if (Input.GetButton("Jump") && (double)this.shootingTimer <= 0.0)
			{
				this.shootingTimer = 0.2f;
				this.photonView.RPC("Fire", RpcTarget.AllViaServer, new object[]
				{
					this.rigidbody.position,
					this.rigidbody.rotation
				});
			}
			if (this.shootingTimer > 0f)
			{
				this.shootingTimer -= Time.deltaTime;
			}
		}

		public void FixedUpdate()
		{
			if (!this.photonView.IsMine)
			{
				return;
			}
			if (!this.controllable)
			{
				return;
			}
			Quaternion rot = this.rigidbody.rotation * Quaternion.Euler(0f, this.rotation * this.RotationSpeed * Time.fixedDeltaTime, 0f);
			this.rigidbody.MoveRotation(rot);
			Vector3 force = rot * Vector3.forward * this.acceleration * 1000f * this.MovementSpeed * Time.fixedDeltaTime;
			this.rigidbody.AddForce(force);
			if (this.rigidbody.velocity.magnitude > this.MaxSpeed * 1000f)
			{
				this.rigidbody.velocity = this.rigidbody.velocity.normalized * this.MaxSpeed * 1000f;
			}
			this.CheckExitScreen();
		}

		private IEnumerator WaitForRespawn()
		{
			yield return new WaitForSeconds(4f);
			this.photonView.RPC("RespawnSpaceship", RpcTarget.AllViaServer, new object[0]);
			yield break;
		}

		[PunRPC]
		public void DestroySpaceship()
		{
			this.rigidbody.velocity = Vector3.zero;
			this.rigidbody.angularVelocity = Vector3.zero;
			this.collider.enabled = false;
			this.renderer.enabled = false;
			this.controllable = false;
			this.EngineTrail.SetActive(false);
			this.Destruction.Play();
			object obj;
			if (this.photonView.IsMine && PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerLives", out obj))
			{
				PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
				{
					{
						"PlayerLives",
						((int)obj > 1) ? ((int)obj - 1) : 0
					}
				}, null, null);
				if ((int)obj > 1)
				{
					base.StartCoroutine("WaitForRespawn");
				}
			}
		}

		[PunRPC]
		public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
		{
			float f = (float)(PhotonNetwork.Time - info.timestamp);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BulletPrefab, this.rigidbody.position, Quaternion.identity);
			gameObject.GetComponent<Bullet>().InitializeBullet(this.photonView.Owner, rotation * Vector3.forward, Mathf.Abs(f));
		}

		[PunRPC]
		public void RespawnSpaceship()
		{
			this.collider.enabled = true;
			this.renderer.enabled = true;
			this.controllable = true;
			this.EngineTrail.SetActive(true);
			this.Destruction.Stop();
		}

		private void CheckExitScreen()
		{
			if (Camera.main == null)
			{
				return;
			}
			if (Mathf.Abs(this.rigidbody.position.x) > Camera.main.orthographicSize * Camera.main.aspect)
			{
				this.rigidbody.position = new Vector3(-Mathf.Sign(this.rigidbody.position.x) * Camera.main.orthographicSize * Camera.main.aspect, 0f, this.rigidbody.position.z);
				this.rigidbody.position -= this.rigidbody.position.normalized * 0.1f;
			}
			if (Mathf.Abs(this.rigidbody.position.z) > Camera.main.orthographicSize)
			{
				this.rigidbody.position = new Vector3(this.rigidbody.position.x, this.rigidbody.position.y, -Mathf.Sign(this.rigidbody.position.z) * Camera.main.orthographicSize);
				this.rigidbody.position -= this.rigidbody.position.normalized * 0.1f;
			}
		}

		public float RotationSpeed = 90f;

		public float MovementSpeed = 2f;

		public float MaxSpeed = 0.2f;

		public ParticleSystem Destruction;

		public GameObject EngineTrail;

		public GameObject BulletPrefab;

		private PhotonView photonView;

		private Rigidbody rigidbody;

		private Collider collider;

		private Renderer renderer;

		private float rotation;

		private float acceleration;

		private float shootingTimer;

		private bool controllable = true;
	}
}

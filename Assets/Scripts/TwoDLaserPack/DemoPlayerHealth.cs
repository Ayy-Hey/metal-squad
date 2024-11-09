using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	public class DemoPlayerHealth : MonoBehaviour
	{
		public int HealthPoints
		{
			get
			{
				return this._healthPoints;
			}
			set
			{
				this._healthPoints = value;
				if (this._healthPoints <= 0)
				{
					if (this.bloodSplatPrefab != null)
					{
						UnityEngine.Object.Instantiate<GameObject>(this.bloodSplatPrefab, base.transform.position, Quaternion.identity);
					}
					this.healthText.text = "Health: 0";
					Renderer component = base.gameObject.GetComponent<Renderer>();
					component.enabled = false;
					PlayerMovement component2 = base.gameObject.GetComponent<PlayerMovement>();
					component2.enabled = false;
					this.restartButton.gameObject.SetActive(true);
					foreach (LineBasedLaser lineBasedLaser in this.allLasersInScene)
					{
						lineBasedLaser.OnLaserHitTriggered -= this.LaserOnOnLaserHitTriggered;
						lineBasedLaser.SetLaserState(false);
					}
				}
				else
				{
					this.healthText.text = "Health: " + this._healthPoints;
				}
			}
		}

		private void Start()
		{
			this._healthPoints = 10;
			if (this.restartButton == null)
			{
				this.restartButton = UnityEngine.Object.FindObjectsOfType<Button>().FirstOrDefault((Button b) => b.name == "ButtonReplay");
			}
			this.healthText = UnityEngine.Object.FindObjectsOfType<Text>().FirstOrDefault((Text t) => t.name == "TextHealth");
			this.healthText.text = "Health: 10";
			this.allLasersInScene = UnityEngine.Object.FindObjectsOfType<LineBasedLaser>();
			this.restartButton.onClick.RemoveAllListeners();
			this.restartButton.onClick.AddListener(new UnityAction(this.OnRestartButtonClick));
			if (this.allLasersInScene.Any<LineBasedLaser>())
			{
				foreach (LineBasedLaser lineBasedLaser in this.allLasersInScene)
				{
					lineBasedLaser.OnLaserHitTriggered += this.LaserOnOnLaserHitTriggered;
					lineBasedLaser.SetLaserState(true);
					lineBasedLaser.targetGo = base.gameObject;
				}
			}
			PlayerMovement component = base.gameObject.GetComponent<PlayerMovement>();
			component.enabled = true;
			Renderer component2 = base.gameObject.GetComponent<Renderer>();
			component2.enabled = true;
			this.restartButton.gameObject.SetActive(false);
		}

		private void OnRestartButtonClick()
		{
			this.CreateNewPlayer();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void CreateNewPlayer()
		{
			GameObject targetGo = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, new Vector2(6.26f, -2.8f), Quaternion.identity);
			foreach (LineBasedLaser lineBasedLaser in this.allLasersInScene)
			{
				lineBasedLaser.targetGo = targetGo;
			}
		}

		private void LaserOnOnLaserHitTriggered(RaycastHit2D hitInfo)
		{
			if (hitInfo.collider.gameObject == base.gameObject && this.bloodParticleSystem != null)
			{
				this.bloodParticleSystem.Play();
				this.HealthPoints--;
			}
		}

		private void Update()
		{
		}

		public GameObject bloodSplatPrefab;

		public GameObject playerPrefab;

		public Button restartButton;

		public Text healthText;

		private LineBasedLaser[] allLasersInScene;

		public ParticleSystem bloodParticleSystem;

		[SerializeField]
		private int _healthPoints;
	}
}

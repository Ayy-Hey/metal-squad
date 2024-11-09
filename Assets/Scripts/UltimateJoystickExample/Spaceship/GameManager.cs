using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UltimateJoystickExample.Spaceship
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance
		{
			get
			{
				return GameManager.instance;
			}
		}

		private void Awake()
		{
			if (GameManager.instance != null)
			{
				if (GameManager.instance.gameObject.activeInHierarchy)
				{
					UnityEngine.Debug.LogWarning("There are multiple instances of the Game Manager script. Removing the old manager from the scene.");
					UnityEngine.Object.Destroy(GameManager.instance.gameObject);
				}
				GameManager.instance = null;
			}
			GameManager.instance = base.GetComponent<GameManager>();
		}

		private void Start()
		{
			base.StartCoroutine("SpawnTimer");
			this.UpdateScoreText();
		}

		private IEnumerator SpawnTimer()
		{
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < this.startingAsteroids; i++)
			{
				this.SpawnAsteroid();
			}
			while (this.spawning)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(this.spawnTimeMin, this.spawnTimeMax));
				this.SpawnAsteroid();
			}
			yield break;
		}

		private void SpawnAsteroid()
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			Vector3 zero = Vector3.zero;
			if (Mathf.Abs(insideUnitCircle.x) > Mathf.Abs(insideUnitCircle.y))
			{
				zero = new Vector3(Mathf.Sign(insideUnitCircle.x) * Camera.main.orthographicSize * Camera.main.aspect * 1.3f, 0f, insideUnitCircle.y * Camera.main.orthographicSize);
			}
			else
			{
				zero = new Vector3(insideUnitCircle.x * Camera.main.orthographicSize * Camera.main.aspect * 1.3f, 0f, Mathf.Sign(insideUnitCircle.y) * Camera.main.orthographicSize);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.astroidPrefab, zero, Quaternion.Euler(UnityEngine.Random.value * 360f, UnityEngine.Random.value * 360f, UnityEngine.Random.value * 360f));
			gameObject.GetComponent<AsteroidController>().Setup(-zero.normalized * 1000f, UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(500f, 1500f));
			gameObject.GetComponent<AsteroidController>().asteroidManager = GameManager.instance;
		}

		public void SpawnDebris(Vector3 pos)
		{
			int num = UnityEngine.Random.Range(3, 6);
			for (int i = 0; i < num; i++)
			{
				Vector3 a = Quaternion.Euler(0f, (float)i * 360f / (float)num, 0f) * Vector3.forward * 5f * 300f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.debrisPrefab, pos + a.normalized * UnityEngine.Random.Range(0f, 5f), Quaternion.Euler(0f, UnityEngine.Random.value * 180f, 0f));
				gameObject.transform.localScale = new Vector3(UnityEngine.Random.Range(0.25f, 0.5f), UnityEngine.Random.Range(0.25f, 0.5f), UnityEngine.Random.Range(0.25f, 0.5f));
				gameObject.GetComponent<AsteroidController>().Setup(a / 2f, UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(500f, 1500f));
				gameObject.GetComponent<AsteroidController>().asteroidManager = GameManager.instance;
			}
		}

		public void SpawnExplosion(Vector3 pos)
		{
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.explosionPrefab, pos, Quaternion.identity);
			UnityEngine.Object.Destroy(obj, 1f);
		}

		public void ShowDeathScreen()
		{
			this.gameOverScreen.gameObject.SetActive(true);
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.playerExplosionPrefab, UnityEngine.Object.FindObjectOfType<PlayerController>().transform.position, Quaternion.identity);
			UnityEngine.Object.Destroy(obj, 2f);
			base.StartCoroutine("ShakeCamera");
			base.StartCoroutine("FadeDeathScreen");
			this.spawning = false;
			UltimateJoystick.GetUltimateJoystick("Movement").UpdatePositioning();
		}

		private IEnumerator FadeDeathScreen()
		{
			yield return new WaitForSeconds(0.5f);
			this.scoreText.text = "Final Score\n" + this.score.ToString();
			Color imageColor = this.gameOverScreen.color;
			Color textColor = this.gameOverText.color;
			for (float t = 0f; t < 1f; t += Time.deltaTime * 3f)
			{
				imageColor.a = Mathf.Lerp(0f, 0.75f, t);
				textColor.a = Mathf.Lerp(0f, 1f, t);
				this.gameOverScreen.color = imageColor;
				this.gameOverText.color = textColor;
				this.scoreText.fontSize = (int)Mathf.Lerp(50f, 100f, t);
				yield return null;
			}
			imageColor.a = 0.75f;
			textColor.a = 1f;
			this.gameOverScreen.color = imageColor;
			this.gameOverText.color = textColor;
			yield break;
		}

		public void ModifyScore(bool isDebris)
		{
			this.score += ((!isDebris) ? this.asteroidPoints : this.debrisPoints);
			this.UpdateScoreText();
		}

		private void UpdateScoreText()
		{
			this.scoreText.text = this.score.ToString();
		}

		private IEnumerator ShakeCamera()
		{
			Vector2 origPos = Camera.main.transform.position;
			for (float t = 0f; t < 1f; t += Time.deltaTime * 2f)
			{
				Vector2 tempVec = origPos + UnityEngine.Random.insideUnitCircle;
				Camera.main.transform.position = tempVec;
				yield return null;
			}
			Camera.main.transform.position = origPos;
			yield break;
		}

		private static GameManager instance;

		[Header("Prefabs")]
		public GameObject astroidPrefab;

		public GameObject debrisPrefab;

		public GameObject explosionPrefab;

		public GameObject playerExplosionPrefab;

		private bool spawning = true;

		[Header("Spawning")]
		public float spawnTimeMin = 5f;

		public float spawnTimeMax = 10f;

		public int startingAsteroids = 2;

		[Header("Score")]
		public Text scoreText;

		private int score;

		public int asteroidPoints = 50;

		public int debrisPoints = 10;

		[Header("Game Over")]
		public Image gameOverScreen;

		public Text gameOverText;
	}
}

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace TwoDLaserPack
{
	public class SpriteBasedLaser : MonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event SpriteBasedLaser.LaserHitTriggerHandler OnLaserHitTriggered;

		private void OnEnable()
		{
			this.gameObjectCached = base.gameObject;
			if (this.laserLineRendererArc != null)
			{
				this.laserLineRendererArc.SetVertexCount(this.laserArcSegments);
			}
		}

		private void Start()
		{
			this.startLaserLength = this.maxLaserLength;
			if (this.laserOscillationPositionerScript != null)
			{
				this.laserOscillationPositionerScript.radius = this.oscillationThreshold;
			}
		}

		private void OscillateLaserParts(float currentLaserDistance)
		{
			if (this.laserOscillationPositionerScript == null)
			{
				return;
			}
			this.lerpYValue = Mathf.Lerp(this.middleGoPiece.transform.localPosition.y, this.laserOscillationPositionerScript.randomPointInCircle.y, Time.deltaTime * this.oscillationSpeed);
			if (this.startGoPiece != null && this.middleGoPiece != null)
			{
				Vector2 b = new Vector2(this.startGoPiece.transform.localPosition.x, this.laserOscillationPositionerScript.randomPointInCircle.y);
				Vector2 v = Vector2.Lerp(this.startGoPiece.transform.localPosition, b, Time.deltaTime * this.oscillationSpeed);
				this.startGoPiece.transform.localPosition = v;
				Vector2 v2 = new Vector2(currentLaserDistance / 2f + this.startSpriteWidth / 4f, this.lerpYValue);
				this.middleGoPiece.transform.localPosition = v2;
			}
			if (this.endGoPiece != null)
			{
				Vector2 v3 = new Vector2(currentLaserDistance + this.startSpriteWidth / 2f, this.lerpYValue);
				this.endGoPiece.transform.localPosition = v3;
			}
		}

		private void SetLaserArcVertices(float distancePoint, bool useHitPoint)
		{
			for (int i = 1; i < this.laserArcSegments; i++)
			{
				float value = Mathf.Sin((float)i + Time.time * UnityEngine.Random.Range(0.5f, 1.3f));
				float y = Mathf.Clamp(value, this.laserArcMaxYDown, this.laserArcMaxYUp);
				Vector2 v = new Vector2((float)i * 1.2f, y);
				if (useHitPoint && i == this.laserArcSegments - 1)
				{
					this.laserLineRendererArc.SetPosition(i, new Vector2(distancePoint, 0f));
				}
				else
				{
					this.laserLineRendererArc.SetPosition(i, v);
				}
			}
		}

		private void Update()
		{
			if (this.gameObjectCached != null && this.laserActive)
			{
				if (this.startGoPiece == null)
				{
					this.InstantiateLaserPart(ref this.startGoPiece, this.laserStartPiece);
					this.startGoPiece.transform.parent = base.transform;
					this.startGoPiece.transform.localPosition = Vector2.zero;
					this.startSpriteWidth = this.laserStartPiece.GetComponent<Renderer>().bounds.size.x;
				}
				if (this.middleGoPiece == null)
				{
					this.InstantiateLaserPart(ref this.middleGoPiece, this.laserMiddlePiece);
					this.middleGoPiece.transform.parent = base.transform;
					this.middleGoPiece.transform.localPosition = Vector2.zero;
				}
				this.middleGoPiece.transform.localScale = new Vector3(this.maxLaserLength - this.startSpriteWidth + 0.2f, this.middleGoPiece.transform.localScale.y, this.middleGoPiece.transform.localScale.z);
				if (this.oscillateLaser)
				{
					this.OscillateLaserParts(this.maxLaserLength);
				}
				else
				{
					if (this.middleGoPiece != null)
					{
						this.middleGoPiece.transform.localPosition = new Vector2(this.maxLaserLength / 2f + this.startSpriteWidth / 4f, this.lerpYValue);
					}
					if (this.endGoPiece != null)
					{
						this.endGoPiece.transform.localPosition = new Vector2(this.maxLaserLength + this.startSpriteWidth / 2f, 0f);
					}
				}
				RaycastHit2D hit;
				if (this.laserRotationEnabled && this.targetGo != null)
				{
					Vector3 v = this.targetGo.transform.position - this.gameObjectCached.transform.position;
					this.laserAngle = Mathf.Atan2(v.y, v.x);
					if (this.laserAngle < 0f)
					{
						this.laserAngle = 6.28318548f + this.laserAngle;
					}
					float angle = this.laserAngle * 57.29578f;
					if (this.lerpLaserRotation)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.AngleAxis(angle, base.transform.forward), Time.deltaTime * this.turningRate);
						Vector3 v2 = base.transform.rotation * Vector3.right;
						hit = Physics2D.Raycast(base.transform.position, v2, this.maxLaserRaycastDistance);
					}
					else
					{
						base.transform.rotation = Quaternion.AngleAxis(angle, base.transform.forward);
						hit = Physics2D.Raycast(base.transform.position, v, this.maxLaserRaycastDistance);
					}
				}
				else
				{
					hit = Physics2D.Raycast(base.transform.position, base.transform.right, this.maxLaserRaycastDistance);
				}
				if (!this.ignoreCollisions)
				{
					if (hit.collider != null)
					{
						this.maxLaserLength = Vector2.Distance(hit.point, base.transform.position) + this.startSpriteWidth / 4f;
						this.InstantiateLaserPart(ref this.endGoPiece, this.laserEndPiece);
						if (this.hitSparkParticleSystem != null)
						{
							this.hitSparkParticleSystem.transform.position = hit.point;
							this.hitSparkParticleSystem.enableEmission = true;
						}
						if (this.useArc)
						{
							if (!this.laserLineRendererArc.enabled)
							{
								this.laserLineRendererArc.enabled = true;
							}
							this.SetLaserArcVertices(this.maxLaserLength, true);
							this.SetLaserArcSegmentLength();
						}
						else if (this.laserLineRendererArc.enabled)
						{
							this.laserLineRendererArc.enabled = false;
						}
						if (!this.waitingForTriggerTime)
						{
							base.StartCoroutine(this.HitTrigger(this.collisionTriggerInterval, hit));
						}
					}
					else
					{
						this.SetLaserBackToDefaults();
						if (this.useArc)
						{
							if (!this.laserLineRendererArc.enabled)
							{
								this.laserLineRendererArc.enabled = true;
							}
							this.SetLaserArcSegmentLength();
							this.SetLaserArcVertices(0f, false);
						}
						else if (this.laserLineRendererArc.enabled)
						{
							this.laserLineRendererArc.enabled = false;
						}
					}
				}
				else
				{
					this.SetLaserBackToDefaults();
					this.SetLaserArcVertices(0f, false);
					this.SetLaserArcSegmentLength();
				}
			}
		}

		private IEnumerator HitTrigger(float triggerInterval, RaycastHit2D hit)
		{
			this.waitingForTriggerTime = true;
			if (this.OnLaserHitTriggered != null)
			{
				this.OnLaserHitTriggered(hit);
			}
			yield return new WaitForSeconds(triggerInterval);
			this.waitingForTriggerTime = false;
			yield break;
		}

		public void SetLaserState(bool enabledStatus)
		{
			this.laserActive = enabledStatus;
			if (this.startGoPiece != null)
			{
				this.startGoPiece.SetActive(enabledStatus);
			}
			if (this.middleGoPiece != null)
			{
				this.middleGoPiece.SetActive(enabledStatus);
			}
			if (this.endGoPiece != null)
			{
				this.endGoPiece.SetActive(enabledStatus);
			}
			if (this.laserLineRendererArc != null)
			{
				this.laserLineRendererArc.enabled = enabledStatus;
			}
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkParticleSystem.enableEmission = enabledStatus;
			}
		}

		private void SetLaserArcSegmentLength()
		{
			int vertexCount = Mathf.Abs((int)this.maxLaserLength);
			this.laserLineRendererArc.SetVertexCount(vertexCount);
			this.laserArcSegments = vertexCount;
		}

		private void SetLaserBackToDefaults()
		{
			UnityEngine.Object.Destroy(this.endGoPiece);
			this.maxLaserLength = this.startLaserLength;
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkParticleSystem.enableEmission = false;
				this.hitSparkParticleSystem.transform.position = new Vector2(this.maxLaserLength, base.transform.position.y);
			}
		}

		private void InstantiateLaserPart(ref GameObject laserComponent, GameObject laserPart)
		{
			if (laserComponent == null)
			{
				laserComponent = UnityEngine.Object.Instantiate<GameObject>(laserPart);
				laserComponent.transform.parent = base.gameObject.transform;
				laserComponent.transform.localPosition = Vector2.zero;
				laserComponent.transform.localEulerAngles = Vector2.zero;
			}
		}

		public void DisableLaserGameObjectComponents()
		{
			UnityEngine.Object.Destroy(this.startGoPiece);
			UnityEngine.Object.Destroy(this.middleGoPiece);
			UnityEngine.Object.Destroy(this.endGoPiece);
		}

		public GameObject laserStartPiece;

		public GameObject laserMiddlePiece;

		public GameObject laserEndPiece;

		public LineRenderer laserLineRendererArc;

		public int laserArcSegments = 20;

		public RandomPositionMover laserOscillationPositionerScript;

		public bool oscillateLaser;

		public float maxLaserLength = 20f;

		public float oscillationSpeed = 1f;

		public bool laserActive;

		public bool ignoreCollisions;

		public GameObject targetGo;

		public ParticleSystem hitSparkParticleSystem;

		public float laserArcMaxYDown;

		public float laserArcMaxYUp;

		public float maxLaserRaycastDistance;

		public bool laserRotationEnabled;

		public bool lerpLaserRotation;

		public float turningRate = 3f;

		public float collisionTriggerInterval = 0.25f;

		public bool useArc;

		public float oscillationThreshold = 0.2f;

		private GameObject gameObjectCached;

		private float laserAngle;

		private float lerpYValue;

		private float startLaserLength;

		private GameObject startGoPiece;

		private GameObject middleGoPiece;

		private GameObject endGoPiece;

		private float startSpriteWidth;

		private bool waitingForTriggerTime;

		public delegate void LaserHitTriggerHandler(RaycastHit2D hitInfo);
	}
}

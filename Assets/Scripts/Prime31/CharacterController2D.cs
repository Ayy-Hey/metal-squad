using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Prime31
{
	[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
	public class CharacterController2D : MonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<RaycastHit2D> onControllerCollidedEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Collider2D> onTriggerEnterEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Collider2D> onTriggerStayEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Collider2D> onTriggerExitEvent;

		public float skinWidth
		{
			get
			{
				return this._skinWidth;
			}
			set
			{
				this._skinWidth = value;
				this.recalculateDistanceBetweenRays();
			}
		}

		public bool isGrounded
		{
			get
			{
				return this.collisionState.below;
			}
		}

		private void Awake()
		{
			this.platformMask |= this.oneWayPlatformMask;
			this.transform = base.GetComponent<Transform>();
			this.boxCollider = base.GetComponent<BoxCollider2D>();
			this.rigidBody2D = base.GetComponent<Rigidbody2D>();
			this.skinWidth = this._skinWidth;
			for (int i = 0; i < 32; i++)
			{
				if ((this.triggerMask.value & 1 << i) == 0)
				{
					Physics2D.IgnoreLayerCollision(base.gameObject.layer, i);
				}
			}
		}

		public void OnTriggerEnter2D(Collider2D col)
		{
			if (this.onTriggerEnterEvent != null)
			{
				this.onTriggerEnterEvent(col);
			}
		}

		public void OnTriggerStay2D(Collider2D col)
		{
			if (this.onTriggerStayEvent != null)
			{
				this.onTriggerStayEvent(col);
			}
		}

		public void OnTriggerExit2D(Collider2D col)
		{
			if (this.onTriggerExitEvent != null)
			{
				this.onTriggerExitEvent(col);
			}
		}

		[Conditional("DEBUG_CC2D_RAYS")]
		private void DrawRay(Vector3 start, Vector3 dir, Color color)
		{
			UnityEngine.Debug.DrawRay(start, dir, color);
		}

		public void move(Vector3 deltaMovement)
		{
			this.collisionState.wasGroundedLastFrame = this.collisionState.below;
			this.collisionState.reset();
			this._raycastHitsThisFrame.Clear();
			this._isGoingUpSlope = false;
			this.primeRaycastOrigins();
			if (deltaMovement.y < 0f && this.collisionState.wasGroundedLastFrame)
			{
				this.handleVerticalSlope(ref deltaMovement);
			}
			if (deltaMovement.x != 0f)
			{
				this.moveHorizontally(ref deltaMovement);
			}
			if (deltaMovement.y != 0f)
			{
				this.moveVertically(ref deltaMovement);
			}
			deltaMovement.z = 0f;
			this.transform.Translate(deltaMovement, Space.World);
			if (Time.deltaTime > 0f)
			{
				this.velocity = deltaMovement / Time.deltaTime;
			}
			if (!this.collisionState.wasGroundedLastFrame && this.collisionState.below)
			{
				this.collisionState.becameGroundedThisFrame = true;
			}
			if (this._isGoingUpSlope)
			{
				this.velocity.y = 0f;
			}
			if (this.onControllerCollidedEvent != null)
			{
				for (int i = 0; i < this._raycastHitsThisFrame.Count; i++)
				{
					this.onControllerCollidedEvent(this._raycastHitsThisFrame[i]);
				}
			}
			this.ignoreOneWayPlatformsThisFrame = false;
		}

		public void warpToGrounded()
		{
			do
			{
				this.move(new Vector3(0f, -1f, 0f));
			}
			while (!this.isGrounded);
		}

		public void recalculateDistanceBetweenRays()
		{
			float num = this.boxCollider.size.y * Mathf.Abs(this.transform.localScale.y) - 2f * this._skinWidth;
			this._verticalDistanceBetweenRays = num / (float)(this.totalHorizontalRays - 1);
			float num2 = this.boxCollider.size.x * Mathf.Abs(this.transform.localScale.x) - 2f * this._skinWidth;
			this._horizontalDistanceBetweenRays = num2 / (float)(this.totalVerticalRays - 1);
		}

		private void primeRaycastOrigins()
		{
			Bounds bounds = this.boxCollider.bounds;
			bounds.Expand(-2f * this._skinWidth);
			this._raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
			this._raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
			this._raycastOrigins.bottomLeft = bounds.min;
		}

		private void moveHorizontally(ref Vector3 deltaMovement)
		{
			bool flag = deltaMovement.x > 0f;
			float num = Mathf.Abs(deltaMovement.x) + this._skinWidth;
			Vector2 vector = (!flag) ? (-Vector2.right) : Vector2.right;
			Vector3 vector2 = (!flag) ? this._raycastOrigins.bottomLeft : this._raycastOrigins.bottomRight;
			for (int i = 0; i < this.totalHorizontalRays; i++)
			{
				Vector2 vector3 = new Vector2(vector2.x, vector2.y + (float)i * this._verticalDistanceBetweenRays);
				this.DrawRay(vector3, vector * num, Color.red);
				if (i == 0 && this.collisionState.wasGroundedLastFrame)
				{
					this._raycastHit = Physics2D.Raycast(vector3, vector, num, this.platformMask);
				}
				else
				{
					this._raycastHit = Physics2D.Raycast(vector3, vector, num, this.platformMask & ~this.oneWayPlatformMask);
				}
				if (this._raycastHit)
				{
					if (i == 0 && this.handleHorizontalSlope(ref deltaMovement, Vector2.Angle(this._raycastHit.normal, Vector2.up)))
					{
						this._raycastHitsThisFrame.Add(this._raycastHit);
						break;
					}
					deltaMovement.x = this._raycastHit.point.x - vector3.x;
					num = Mathf.Abs(deltaMovement.x);
					if (flag)
					{
						deltaMovement.x -= this._skinWidth;
						this.collisionState.right = true;
					}
					else
					{
						deltaMovement.x += this._skinWidth;
						this.collisionState.left = true;
					}
					this._raycastHitsThisFrame.Add(this._raycastHit);
					if (num < this._skinWidth + 0.001f)
					{
						break;
					}
				}
			}
		}

		private bool handleHorizontalSlope(ref Vector3 deltaMovement, float angle)
		{
			if (Mathf.RoundToInt(angle) == 90)
			{
				return false;
			}
			if (angle < this.slopeLimit)
			{
				if (deltaMovement.y < this.jumpingThreshold)
				{
					float num = this.slopeSpeedMultiplier.Evaluate(angle);
					deltaMovement.x *= num;
					deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * 0.0174532924f) * deltaMovement.x);
					bool flag = deltaMovement.x > 0f;
					Vector3 vector = (!flag) ? this._raycastOrigins.bottomLeft : this._raycastOrigins.bottomRight;
					RaycastHit2D hit;
					if (this.collisionState.wasGroundedLastFrame)
					{
						hit = Physics2D.Raycast(vector, deltaMovement.normalized, deltaMovement.magnitude, this.platformMask);
					}
					else
					{
						hit = Physics2D.Raycast(vector, deltaMovement.normalized, deltaMovement.magnitude, this.platformMask & ~this.oneWayPlatformMask);
					}
					if (hit)
					{
						deltaMovement = (Vector3)hit.point - vector;
						if (flag)
						{
							deltaMovement.x -= this._skinWidth;
						}
						else
						{
							deltaMovement.x += this._skinWidth;
						}
					}
					this._isGoingUpSlope = true;
					this.collisionState.below = true;
				}
			}
			else
			{
				deltaMovement.x = 0f;
			}
			return true;
		}

		private void moveVertically(ref Vector3 deltaMovement)
		{
			bool flag = deltaMovement.y > 0f;
			float num = Mathf.Abs(deltaMovement.y) + this._skinWidth;
			Vector2 vector = (!flag) ? (-Vector2.up) : Vector2.up;
			Vector3 vector2 = (!flag) ? this._raycastOrigins.bottomLeft : this._raycastOrigins.topLeft;
			vector2.x += deltaMovement.x;
			LayerMask mask = this.platformMask;
			if ((flag && !this.collisionState.wasGroundedLastFrame) || this.ignoreOneWayPlatformsThisFrame)
			{
				mask &= ~this.oneWayPlatformMask;
			}
			for (int i = 0; i < this.totalVerticalRays; i++)
			{
				Vector2 vector3 = new Vector2(vector2.x + (float)i * this._horizontalDistanceBetweenRays, vector2.y);
				this.DrawRay(vector3, vector * num, Color.red);
				this._raycastHit = Physics2D.Raycast(vector3, vector, num, mask);
				if (this._raycastHit)
				{
					deltaMovement.y = this._raycastHit.point.y - vector3.y;
					num = Mathf.Abs(deltaMovement.y);
					if (flag)
					{
						deltaMovement.y -= this._skinWidth;
						this.collisionState.above = true;
					}
					else
					{
						deltaMovement.y += this._skinWidth;
						this.collisionState.below = true;
					}
					this._raycastHitsThisFrame.Add(this._raycastHit);
					if (!flag && deltaMovement.y > 1E-05f)
					{
						this._isGoingUpSlope = true;
					}
					if (num < this._skinWidth + 0.001f)
					{
						break;
					}
				}
			}
		}

		private void handleVerticalSlope(ref Vector3 deltaMovement)
		{
			float num = (this._raycastOrigins.bottomLeft.x + this._raycastOrigins.bottomRight.x) * 0.5f;
			Vector2 vector = -Vector2.up;
			float num2 = this._slopeLimitTangent * (this._raycastOrigins.bottomRight.x - num);
			Vector2 vector2 = new Vector2(num, this._raycastOrigins.bottomLeft.y);
			this.DrawRay(vector2, vector * num2, Color.yellow);
			this._raycastHit = Physics2D.Raycast(vector2, vector, num2, this.platformMask);
			if (this._raycastHit)
			{
				float num3 = Vector2.Angle(this._raycastHit.normal, Vector2.up);
				if (num3 == 0f)
				{
					return;
				}
				bool flag = Mathf.Sign(this._raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
				if (flag)
				{
					float num4 = this.slopeSpeedMultiplier.Evaluate(-num3);
					deltaMovement.y += this._raycastHit.point.y - vector2.y - this.skinWidth;
					deltaMovement.x *= num4;
					this.collisionState.movingDownSlope = true;
					this.collisionState.slopeAngle = num3;
				}
			}
		}

		public bool ignoreOneWayPlatformsThisFrame;

		[Range(0.001f, 0.3f)]
		[SerializeField]
		private float _skinWidth = 0.02f;

		public LayerMask platformMask = 0;

		public LayerMask triggerMask = 0;

		[SerializeField]
		private LayerMask oneWayPlatformMask = 0;

		[Range(0f, 90f)]
		public float slopeLimit = 30f;

		public float jumpingThreshold = 0.07f;

		public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-90f, 1.5f),
			new Keyframe(0f, 1f),
			new Keyframe(90f, 0f)
		});

		[Range(2f, 20f)]
		public int totalHorizontalRays = 8;

		[Range(2f, 20f)]
		public int totalVerticalRays = 4;

		private float _slopeLimitTangent = Mathf.Tan(1.30899692f);

		[HideInInspector]
		[NonSerialized]
		public new Transform transform;

		[HideInInspector]
		[NonSerialized]
		public BoxCollider2D boxCollider;

		[HideInInspector]
		[NonSerialized]
		public Rigidbody2D rigidBody2D;

		[HideInInspector]
		[NonSerialized]
		public CharacterController2D.CharacterCollisionState2D collisionState = new CharacterController2D.CharacterCollisionState2D();

		[HideInInspector]
		[NonSerialized]
		public Vector3 velocity;

		private const float kSkinWidthFloatFudgeFactor = 0.001f;

		private CharacterController2D.CharacterRaycastOrigins _raycastOrigins;

		private RaycastHit2D _raycastHit;

		private List<RaycastHit2D> _raycastHitsThisFrame = new List<RaycastHit2D>(2);

		private float _verticalDistanceBetweenRays;

		private float _horizontalDistanceBetweenRays;

		private bool _isGoingUpSlope;

		private struct CharacterRaycastOrigins
		{
			public Vector3 topLeft;

			public Vector3 bottomRight;

			public Vector3 bottomLeft;
		}

		public class CharacterCollisionState2D
		{
			public bool hasCollision()
			{
				return this.below || this.right || this.left || this.above;
			}

			public void reset()
			{
				this.right = (this.left = (this.above = (this.below = (this.becameGroundedThisFrame = (this.movingDownSlope = false)))));
				this.slopeAngle = 0f;
			}

			public string ToString()
			{
				return string.Format("[CharacterCollisionState2D] r: {0}, l: {1}, a: {2}, b: {3}, movingDownSlope: {4}, angle: {5}, wasGroundedLastFrame: {6}, becameGroundedThisFrame: {7}", new object[]
				{
					this.right,
					this.left,
					this.above,
					this.below,
					this.movingDownSlope,
					this.slopeAngle,
					this.wasGroundedLastFrame,
					this.becameGroundedThisFrame
				});
			}

			public bool right;

			public bool left;

			public bool above;

			public bool below;

			public bool becameGroundedThisFrame;

			public bool wasGroundedLastFrame;

			public bool movingDownSlope;

			public float slopeAngle;
		}
	}
}

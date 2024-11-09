using System;
using UnityEngine;

namespace Smooth
{
	public class StatePUN2
	{
		public StatePUN2 copyFromState(StatePUN2 state)
		{
			this.ownerTimestamp = state.ownerTimestamp;
			this.position = state.position;
			this.rotation = state.rotation;
			this.scale = state.scale;
			this.velocity = state.velocity;
			this.angularVelocity = state.angularVelocity;
			return this;
		}

		public static StatePUN2 Lerp(StatePUN2 targetTempState, StatePUN2 start, StatePUN2 end, float t)
		{
			targetTempState.position = Vector3.Lerp(start.position, end.position, t);
			targetTempState.rotation = Quaternion.Lerp(start.rotation, end.rotation, t);
			targetTempState.scale = Vector3.Lerp(start.scale, end.scale, t);
			targetTempState.velocity = Vector3.Lerp(start.velocity, end.velocity, t);
			targetTempState.angularVelocity = Vector3.Lerp(start.angularVelocity, end.angularVelocity, t);
			targetTempState.ownerTimestamp = Mathf.Lerp(start.ownerTimestamp, end.ownerTimestamp, t);
			return targetTempState;
		}

		public void resetTheVariables()
		{
			this.ownerTimestamp = 0f;
			this.position = Vector3.zero;
			this.rotation = Quaternion.identity;
			this.scale = Vector3.zero;
			this.velocity = Vector3.zero;
			this.angularVelocity = Vector3.zero;
			this.atPositionalRest = false;
			this.atRotationalRest = false;
			this.teleport = false;
		}

		public void copyFromSmoothSync(SmoothSyncPUN2 smoothSyncScript)
		{
			this.ownerTimestamp = Time.realtimeSinceStartup;
			this.position = smoothSyncScript.getPosition();
			this.rotation = smoothSyncScript.getRotation();
			this.scale = smoothSyncScript.getScale();
			if (smoothSyncScript.hasRigidbody)
			{
				this.velocity = smoothSyncScript.rb.velocity;
				this.angularVelocity = smoothSyncScript.rb.angularVelocity * 57.29578f;
			}
			else if (smoothSyncScript.hasRigidbody2D)
			{
				this.velocity = smoothSyncScript.rb2D.velocity;
				this.angularVelocity.x = 0f;
				this.angularVelocity.y = 0f;
				this.angularVelocity.z = smoothSyncScript.rb2D.angularVelocity;
			}
			else
			{
				this.velocity = Vector3.zero;
				this.angularVelocity = Vector3.zero;
			}
		}

		public float ownerTimestamp;

		public Vector3 position;

		public Quaternion rotation;

		public Vector3 scale;

		public Vector3 velocity;

		public Vector3 angularVelocity;

		public bool teleport;

		public bool atPositionalRest;

		public bool atRotationalRest;

		public float receivedOnServerTimestamp;

		public Vector3 reusableRotationVector;

		public bool serverShouldRelayPosition;

		public bool serverShouldRelayRotation;

		public bool serverShouldRelayScale;

		public bool serverShouldRelayVelocity;

		public bool serverShouldRelayAngularVelocity;

		public bool serverShouldRelayTeleport;
	}
}

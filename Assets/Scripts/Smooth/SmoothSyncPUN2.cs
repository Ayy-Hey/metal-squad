using System;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

namespace Smooth
{
	public class SmoothSyncPUN2 : MonoBehaviourPunCallbacks, IPunObservable
	{
		public SmoothSyncPUN2()
		{
			if (SmoothSyncPUN2._003C_003Ef__mg_0024cache0 == null)
			{
				SmoothSyncPUN2._003C_003Ef__mg_0024cache0 = new SmoothSyncPUN2.validateStateDelegate(SmoothSyncPUN2.validateState);
			}
			this.validateStateMethod = SmoothSyncPUN2._003C_003Ef__mg_0024cache0;
			this.maxPositionDifferenceForVelocitySyncing = 10f;
			this.lastRotationWhenStateWasSent = Quaternion.identity;
			this.childObjectSmoothSyncs = new SmoothSyncPUN2[0];
			this.atRestThresholdCount = 1;
			this.restStatePosition = SmoothSyncPUN2.RestState.MOVING;
			this.restStateRotation = SmoothSyncPUN2.RestState.MOVING;
			
		}

		public static bool validateState(StatePUN2 latestReceivedState, StatePUN2 latestValidatedState)
		{
			return true;
		}

		public void Awake()
		{
			int a = ((int)((float)PhotonNetwork.SerializationRate * this.interpolationBackTime) + 1) * 2;
			this.stateBuffer = new StatePUN2[Mathf.Max(a, 30)];
			if (this.childObjectToSync)
			{
				this.realObjectToSync = this.childObjectToSync;
				this.hasChildObject = true;
				bool flag = false;
				this.childObjectSmoothSyncs = base.GetComponents<SmoothSyncPUN2>();
				for (int i = 0; i < this.childObjectSmoothSyncs.Length; i++)
				{
					if (!this.childObjectSmoothSyncs[i].childObjectToSync)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					UnityEngine.Debug.LogError("You must have one SmoothSync script with unassigned childObjectToSync in order to sync the parent object");
				}
			}
			else
			{
				this.realObjectToSync = base.gameObject;
				int num = 0;
				this.childObjectSmoothSyncs = base.GetComponents<SmoothSyncPUN2>();
				for (int j = 0; j < this.childObjectSmoothSyncs.Length; j++)
				{
					this.childObjectSmoothSyncs[j].syncIndex = num;
					num++;
				}
			}
			this.netID = base.GetComponent<NetworkIdentity>();
			this.rb = this.realObjectToSync.GetComponent<Rigidbody>();
			this.rb2D = this.realObjectToSync.GetComponent<Rigidbody2D>();
			if (this.rb)
			{
				this.hasRigidbody = true;
			}
			else if (this.rb2D)
			{
				this.hasRigidbody2D = true;
				if (this.syncVelocity != SyncMode.NONE)
				{
					this.syncVelocity = SyncMode.XY;
				}
				if (this.syncAngularVelocity != SyncMode.NONE)
				{
					this.syncAngularVelocity = SyncMode.Z;
				}
			}
			if (!this.rb && !this.rb2D)
			{
				this.syncVelocity = SyncMode.NONE;
				this.syncAngularVelocity = SyncMode.NONE;
			}
			if (this.extrapolationMode == SmoothSyncPUN2.ExtrapolationMode.Unlimited)
			{
				this.useExtrapolationDistanceLimit = false;
				this.useExtrapolationTimeLimit = false;
			}
			this.targetTempState = new StatePUN2();
			this.sendingTempState = new NetworkStatePUN2();
		}

		private void Update()
		{
			if (!base.photonView.IsMine && this.whenToUpdateTransform == SmoothSyncPUN2.WhenToUpdateTransform.Update)
			{
				this.adjustOwnerTime();
				this.applyInterpolationOrExtrapolation();
			}
		}

		private void FixedUpdate()
		{
			if (!base.photonView.IsMine && this.whenToUpdateTransform == SmoothSyncPUN2.WhenToUpdateTransform.FixedUpdate)
			{
				this.adjustOwnerTime();
				this.applyInterpolationOrExtrapolation();
			}
		}

		public new void OnEnable()
		{
			if (base.photonView && base.photonView.IsMine)
			{
				this.teleportOwnedObjectFromOwner();
			}
		}

		private void sendState()
		{
			if (!base.photonView.IsMine)
			{
				return;
			}
			if (this.syncPosition != SyncMode.NONE)
			{
				if (this.positionLastAttemptedToSend == this.getPosition())
				{
					if (this.restStatePosition != SmoothSyncPUN2.RestState.AT_REST)
					{
						this.samePositionCount++;
					}
					if (this.samePositionCount == this.atRestThresholdCount)
					{
						this.samePositionCount = 0;
						this.restStatePosition = SmoothSyncPUN2.RestState.AT_REST;
						this.forceStateSendNextOnPhotonSerializeView();
					}
				}
				else if (this.restStatePosition == SmoothSyncPUN2.RestState.AT_REST && !this.almostEqual(this.getPosition(), this.latestTeleportedFromPosition, 0.005f))
				{
					this.restStatePosition = SmoothSyncPUN2.RestState.JUST_STARTED_MOVING;
					this.forceStateSendNextOnPhotonSerializeView();
				}
				else if (this.restStatePosition == SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
				{
					this.restStatePosition = SmoothSyncPUN2.RestState.MOVING;
				}
				else
				{
					this.samePositionCount = 0;
				}
			}
			else
			{
				this.restStatePosition = SmoothSyncPUN2.RestState.AT_REST;
			}
			if (this.syncRotation != SyncMode.NONE)
			{
				if (this.rotationLastAttemptedToSend == this.getRotation())
				{
					if (this.restStateRotation != SmoothSyncPUN2.RestState.AT_REST)
					{
						this.sameRotationCount++;
					}
					if (this.sameRotationCount == this.atRestThresholdCount)
					{
						this.sameRotationCount = 0;
						this.restStateRotation = SmoothSyncPUN2.RestState.AT_REST;
						this.forceStateSendNextOnPhotonSerializeView();
					}
				}
				else if (this.restStateRotation == SmoothSyncPUN2.RestState.AT_REST && this.getRotation() != this.latestTeleportedFromRotation)
				{
					this.restStateRotation = SmoothSyncPUN2.RestState.JUST_STARTED_MOVING;
					this.forceStateSendNextOnPhotonSerializeView();
				}
				else if (this.restStateRotation == SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
				{
					this.restStateRotation = SmoothSyncPUN2.RestState.MOVING;
				}
				else
				{
					this.sameRotationCount = 0;
				}
			}
			else
			{
				this.restStateRotation = SmoothSyncPUN2.RestState.AT_REST;
			}
			this.sendPosition = this.shouldSendPosition();
			this.sendRotation = this.shouldSendRotation();
			this.sendScale = this.shouldSendScale();
			this.sendVelocity = this.shouldSendVelocity();
			this.sendAngularVelocity = this.shouldSendAngularVelocity();
			if (!this.sendPosition && !this.sendRotation && !this.sendScale && !this.sendVelocity && !this.sendAngularVelocity)
			{
				return;
			}
			this.sendingTempState.copyFromSmoothSync(this);
			if (this.restStatePosition == SmoothSyncPUN2.RestState.AT_REST)
			{
				this.sendAtPositionalRestMessage = true;
			}
			if (this.restStateRotation == SmoothSyncPUN2.RestState.AT_REST)
			{
				this.sendAtRotationalRestMessage = true;
			}
			if (this.restStatePosition == SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
			{
				this.sendingTempState.state.position = this.lastPositionWhenStateWasSent;
			}
			if (this.restStateRotation == SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
			{
				this.sendingTempState.state.rotation = this.lastRotationWhenStateWasSent;
			}
			if (this.restStatePosition == SmoothSyncPUN2.RestState.JUST_STARTED_MOVING || this.restStateRotation == SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
			{
				this.sendingTempState.state.ownerTimestamp = this.lastTimeAttemptedToSend;
				if (this.restStatePosition != SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
				{
					this.sendingTempState.state.position = this.positionLastAttemptedToSend;
				}
				if (this.restStateRotation != SmoothSyncPUN2.RestState.JUST_STARTED_MOVING)
				{
					this.sendingTempState.state.rotation = this.rotationLastAttemptedToSend;
				}
			}
			this.lastTimeStateWasSent = Time.realtimeSinceStartup;
			this.shouldSendNextPUNUpdate = true;
		}

		private void applyInterpolationOrExtrapolation()
		{
			if (this.stateCount == 0)
			{
				return;
			}
			if (!this.extrapolatedLastFrame)
			{
				this.targetTempState.resetTheVariables();
			}
			this.triedToExtrapolateTooFar = false;
			float num = this.approximateNetworkTimeOnOwner - this.interpolationBackTime;
			if (this.stateCount > 1 && this.stateBuffer[0].ownerTimestamp > num)
			{
				this.interpolate(num);
				this.extrapolatedLastFrame = false;
			}
			else if (this.stateBuffer[0].atPositionalRest && this.stateBuffer[0].atRotationalRest)
			{
				this.targetTempState.copyFromState(this.stateBuffer[0]);
				this.extrapolatedLastFrame = false;
				if (this.setVelocityInsteadOfPositionOnNonOwners)
				{
					this.triedToExtrapolateTooFar = true;
				}
			}
			else
			{
				bool flag = this.extrapolate(num);
				this.extrapolatedLastFrame = true;
				this.triedToExtrapolateTooFar = !flag;
				if (this.setVelocityInsteadOfPositionOnNonOwners)
				{
					float d = num - this.stateBuffer[0].ownerTimestamp;
					this.targetTempState.velocity = this.stateBuffer[0].velocity;
					this.targetTempState.position = this.stateBuffer[0].position + this.targetTempState.velocity * d;
					Vector3 b = base.transform.position + this.targetTempState.velocity * Time.deltaTime;
					float t = (this.targetTempState.position - b).sqrMagnitude / (this.maxPositionDifferenceForVelocitySyncing * this.maxPositionDifferenceForVelocitySyncing);
					this.targetTempState.velocity = Vector3.Lerp(this.targetTempState.velocity, (this.targetTempState.position - base.transform.position) / Time.deltaTime, t);
				}
			}
			float t2 = this.positionLerpSpeed;
			float t3 = this.rotationLerpSpeed;
			float t4 = this.scaleLerpSpeed;
			if (this.dontLerp)
			{
				t2 = 1f;
				t3 = 1f;
				t4 = 1f;
				this.dontLerp = false;
			}
			if (!this.triedToExtrapolateTooFar)
			{
				bool flag2 = false;
				float num2 = 0f;
				if (this.getPosition() != this.targetTempState.position && (this.snapPositionThreshold != 0f || this.receivedPositionThreshold != 0f))
				{
					num2 = Vector3.Distance(this.getPosition(), this.targetTempState.position);
				}
				if (this.receivedPositionThreshold != 0f)
				{
					if (num2 > this.receivedPositionThreshold)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = false;
				float num3 = 0f;
				if (this.getRotation() != this.targetTempState.rotation && (this.snapRotationThreshold != 0f || this.receivedRotationThreshold != 0f))
				{
					num3 = Quaternion.Angle(this.getRotation(), this.targetTempState.rotation);
				}
				if (this.receivedRotationThreshold != 0f)
				{
					if (num3 > this.receivedRotationThreshold)
					{
						flag3 = true;
					}
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = false;
				float num4 = 0f;
				if (this.getScale() != this.targetTempState.scale)
				{
					flag4 = true;
					if (this.snapScaleThreshold != 0f)
					{
						num4 = Vector3.Distance(this.getScale(), this.targetTempState.scale);
					}
				}
				if (this.hasRigidbody && !this.rb.isKinematic)
				{
					this.rb.velocity = Vector3.zero;
					this.rb.angularVelocity = Vector3.zero;
				}
				else if (this.hasRigidbody2D && !this.rb2D.isKinematic)
				{
					this.rb2D.velocity = Vector2.zero;
					this.rb2D.angularVelocity = 0f;
				}
				if (this.syncPosition != SyncMode.NONE && flag2)
				{
					bool isTeleporting = false;
					if (num2 > this.snapPositionThreshold)
					{
						t2 = 1f;
						isTeleporting = true;
					}
					Vector3 position = this.getPosition();
					if (this.isSyncingXPosition)
					{
						position.x = this.targetTempState.position.x;
					}
					if (this.isSyncingYPosition)
					{
						position.y = this.targetTempState.position.y;
					}
					if (this.isSyncingZPosition)
					{
						position.z = this.targetTempState.position.z;
					}
					if (this.setVelocityInsteadOfPositionOnNonOwners)
					{
						if (this.hasRigidbody)
						{
							this.rb.velocity = this.targetTempState.velocity;
						}
						if (this.hasRigidbody2D)
						{
							this.rb2D.velocity = this.targetTempState.velocity;
						}
					}
					else
					{
						this.setPosition(Vector3.Lerp(this.getPosition(), position, t2), isTeleporting);
					}
				}
				if (this.syncRotation != SyncMode.NONE && flag3)
				{
					bool isTeleporting2 = false;
					if (num3 > this.snapRotationThreshold)
					{
						t3 = 1f;
						isTeleporting2 = true;
					}
					Vector3 eulerAngles = this.getRotation().eulerAngles;
					if (this.isSyncingXRotation)
					{
						eulerAngles.x = this.targetTempState.rotation.eulerAngles.x;
					}
					if (this.isSyncingYRotation)
					{
						eulerAngles.y = this.targetTempState.rotation.eulerAngles.y;
					}
					if (this.isSyncingZRotation)
					{
						eulerAngles.z = this.targetTempState.rotation.eulerAngles.z;
					}
					Quaternion b2 = Quaternion.Euler(eulerAngles);
					this.setRotation(Quaternion.Lerp(this.getRotation(), b2, t3), isTeleporting2);
				}
				if (this.syncScale != SyncMode.NONE && flag4)
				{
					bool isTeleporting3 = false;
					if (num4 > this.snapScaleThreshold)
					{
						t4 = 1f;
						isTeleporting3 = true;
					}
					Vector3 scale = this.getScale();
					if (this.isSyncingXScale)
					{
						scale.x = this.targetTempState.scale.x;
					}
					if (this.isSyncingYScale)
					{
						scale.y = this.targetTempState.scale.y;
					}
					if (this.isSyncingZScale)
					{
						scale.z = this.targetTempState.scale.z;
					}
					this.setScale(Vector3.Lerp(this.getScale(), scale, t4), isTeleporting3);
				}
			}
			else if (this.triedToExtrapolateTooFar)
			{
				if (this.hasRigidbody)
				{
					this.rb.velocity = Vector3.zero;
					this.rb.angularVelocity = Vector3.zero;
				}
				if (this.hasRigidbody2D)
				{
					this.rb2D.velocity = Vector2.zero;
					this.rb2D.angularVelocity = 0f;
				}
			}
		}

		private void interpolate(float interpolationTime)
		{
			int i;
			for (i = 0; i < this.stateCount; i++)
			{
				if (this.stateBuffer[i].ownerTimestamp <= interpolationTime)
				{
					break;
				}
			}
			if (i == this.stateCount)
			{
				i--;
			}
			StatePUN2 statePUN = this.stateBuffer[Mathf.Max(i - 1, 0)];
			StatePUN2 statePUN2 = this.stateBuffer[i];
			float t = (interpolationTime - statePUN2.ownerTimestamp) / (statePUN.ownerTimestamp - statePUN2.ownerTimestamp);
			this.shouldTeleport(statePUN2, ref statePUN, interpolationTime, ref t);
			this.targetTempState = StatePUN2.Lerp(this.targetTempState, statePUN2, statePUN, t);
			if (this.setVelocityInsteadOfPositionOnNonOwners)
			{
				Vector3 b = base.transform.position + this.targetTempState.velocity * Time.deltaTime;
				float t2 = (this.targetTempState.position - b).sqrMagnitude / (this.maxPositionDifferenceForVelocitySyncing * this.maxPositionDifferenceForVelocitySyncing);
				this.targetTempState.velocity = Vector3.Lerp(this.targetTempState.velocity, (this.targetTempState.position - base.transform.position) / Time.deltaTime, t2);
			}
		}

		private bool extrapolate(float interpolationTime)
		{
			if (!this.extrapolatedLastFrame || this.targetTempState.ownerTimestamp < this.stateBuffer[0].ownerTimestamp)
			{
				this.targetTempState.copyFromState(this.stateBuffer[0]);
				this.timeSpentExtrapolating = 0f;
			}
			if (this.extrapolationMode != SmoothSyncPUN2.ExtrapolationMode.None && this.stateCount >= 2)
			{
				if (this.syncVelocity == SyncMode.NONE && !this.stateBuffer[0].atPositionalRest)
				{
					this.targetTempState.velocity = (this.stateBuffer[0].position - this.stateBuffer[1].position) / (this.stateBuffer[0].ownerTimestamp - this.stateBuffer[1].ownerTimestamp);
				}
				if (this.syncAngularVelocity == SyncMode.NONE && !this.stateBuffer[0].atRotationalRest)
				{
					Quaternion quaternion = this.stateBuffer[0].rotation * Quaternion.Inverse(this.stateBuffer[1].rotation);
					Vector3 a = new Vector3(Mathf.DeltaAngle(0f, quaternion.eulerAngles.x), Mathf.DeltaAngle(0f, quaternion.eulerAngles.y), Mathf.DeltaAngle(0f, quaternion.eulerAngles.z));
					Vector3 angularVelocity = a / (this.stateBuffer[0].ownerTimestamp - this.stateBuffer[1].ownerTimestamp);
					this.targetTempState.angularVelocity = angularVelocity;
				}
			}
			if (this.extrapolationMode == SmoothSyncPUN2.ExtrapolationMode.None)
			{
				return false;
			}
			if (this.useExtrapolationTimeLimit && this.timeSpentExtrapolating > this.extrapolationTimeLimit)
			{
				return false;
			}
			bool flag = Mathf.Abs(this.targetTempState.velocity.x) >= 0.01f || Mathf.Abs(this.targetTempState.velocity.y) >= 0.01f || Mathf.Abs(this.targetTempState.velocity.z) >= 0.01f;
			bool flag2 = Mathf.Abs(this.targetTempState.angularVelocity.x) >= 0.01f || Mathf.Abs(this.targetTempState.angularVelocity.y) >= 0.01f || Mathf.Abs(this.targetTempState.angularVelocity.z) >= 0.01f;
			if (!flag && !flag2)
			{
				return false;
			}
			float num;
			if (this.timeSpentExtrapolating == 0f)
			{
				num = interpolationTime - this.targetTempState.ownerTimestamp;
			}
			else
			{
				num = Time.deltaTime;
			}
			this.timeSpentExtrapolating += num;
			if (flag)
			{
				this.targetTempState.position += this.targetTempState.velocity * num;
				if (Mathf.Abs(this.targetTempState.velocity.y) >= 0.01f)
				{
					if (this.hasRigidbody && this.rb.useGravity)
					{
						this.targetTempState.velocity += Physics.gravity * num;
					}
					else if (this.hasRigidbody2D)
					{
						this.targetTempState.velocity += Physics.gravity * this.rb2D.gravityScale * num;
					}
				}
				if (this.hasRigidbody)
				{
					this.targetTempState.velocity -= this.targetTempState.velocity * num * this.rb.drag;
				}
				else if (this.hasRigidbody2D)
				{
					this.targetTempState.velocity -= this.targetTempState.velocity * num * this.rb2D.drag;
				}
			}
			if (flag2)
			{
				float angle = num * this.targetTempState.angularVelocity.magnitude;
				Quaternion lhs = Quaternion.AngleAxis(angle, this.targetTempState.angularVelocity);
				this.targetTempState.rotation = lhs * this.targetTempState.rotation;
				float num2 = 0f;
				if (this.hasRigidbody)
				{
					num2 = this.rb.angularDrag;
				}
				if (this.hasRigidbody2D)
				{
					num2 = this.rb2D.angularDrag;
				}
				if ((this.hasRigidbody || this.hasRigidbody2D) && num2 > 0f)
				{
					this.targetTempState.angularVelocity -= this.targetTempState.angularVelocity * num * num2;
				}
			}
			return !this.useExtrapolationDistanceLimit || Vector3.Distance(this.stateBuffer[0].position, this.targetTempState.position) < this.extrapolationDistanceLimit;
		}

		private void shouldTeleport(StatePUN2 start, ref StatePUN2 end, float interpolationTime, ref float t)
		{
			if (start.ownerTimestamp > interpolationTime && start.teleport && this.stateCount == 2)
			{
				end = start;
				t = 1f;
				this.stopLerping();
			}
			for (int i = 0; i < this.stateCount; i++)
			{
				if (this.stateBuffer[i] == this.latestEndStateUsed && this.latestEndStateUsed != end && this.latestEndStateUsed != start)
				{
					for (int j = i - 1; j >= 0; j--)
					{
						if (this.stateBuffer[j].teleport)
						{
							t = 1f;
							this.stopLerping();
						}
						if (this.stateBuffer[j] == start)
						{
							break;
						}
					}
					break;
				}
			}
			this.latestEndStateUsed = end;
			if (end.teleport)
			{
				t = 1f;
				this.stopLerping();
			}
		}

		public Vector3 getPosition()
		{
			if (this.hasChildObject)
			{
				return this.realObjectToSync.transform.localPosition;
			}
			return this.realObjectToSync.transform.position;
		}

		public Quaternion getRotation()
		{
			if (this.hasChildObject)
			{
				return this.realObjectToSync.transform.localRotation;
			}
			return this.realObjectToSync.transform.rotation;
		}

		public Vector3 getScale()
		{
			return this.realObjectToSync.transform.localScale;
		}

		public void setPosition(Vector3 position, bool isTeleporting)
		{
			if (this.hasChildObject)
			{
				this.realObjectToSync.transform.localPosition = position;
			}
			else if (this.hasRigidbody && !isTeleporting && this.whenToUpdateTransform == SmoothSyncPUN2.WhenToUpdateTransform.FixedUpdate)
			{
				this.rb.MovePosition(position);
			}
			else if (this.hasRigidbody2D && !isTeleporting && this.whenToUpdateTransform == SmoothSyncPUN2.WhenToUpdateTransform.FixedUpdate)
			{
				this.rb2D.MovePosition(position);
			}
			else
			{
				this.realObjectToSync.transform.position = position;
			}
		}

		public void setRotation(Quaternion rotation, bool isTeleporting)
		{
			if (this.hasChildObject)
			{
				this.realObjectToSync.transform.localRotation = rotation;
			}
			else if (this.hasRigidbody && !isTeleporting && this.whenToUpdateTransform == SmoothSyncPUN2.WhenToUpdateTransform.FixedUpdate)
			{
				this.rb.MoveRotation(rotation);
			}
			else if (this.hasRigidbody2D && !isTeleporting && this.whenToUpdateTransform == SmoothSyncPUN2.WhenToUpdateTransform.FixedUpdate)
			{
				this.rb2D.MoveRotation(rotation.eulerAngles.z);
			}
			else
			{
				this.realObjectToSync.transform.rotation = rotation;
			}
		}

		public void setScale(Vector3 scale, bool isTeleporting)
		{
			this.realObjectToSync.transform.localScale = scale;
		}

		private void resetFlags()
		{
			this.forceStateSend = false;
			this.sendAtPositionalRestMessage = false;
			this.sendAtRotationalRestMessage = false;
		}

		private bool almostEqual(Vector3 v1, Vector3 v2, float precision)
		{
			bool result = true;
			if (Mathf.Abs(v1.x - v2.x) > precision)
			{
				result = false;
			}
			if (Mathf.Abs(v1.y - v2.y) > precision)
			{
				result = false;
			}
			if (Mathf.Abs(v1.z - v2.z) > precision)
			{
				result = false;
			}
			return result;
		}

		public void addState(StatePUN2 state)
		{
			if (this.stateCount > 1 && state.ownerTimestamp <= this.stateBuffer[0].ownerTimestamp)
			{
				return;
			}
			for (int i = this.stateBuffer.Length - 1; i >= 1; i--)
			{
				this.stateBuffer[i] = this.stateBuffer[i - 1];
			}
			this.stateBuffer[0] = state;
			this.stateCount = Mathf.Min(this.stateCount + 1, this.stateBuffer.Length);
		}

		public void stopLerping()
		{
			this.dontLerp = true;
		}

		public void clearBuffer()
		{
			base.photonView.RPC("RpcClearBuffer", RpcTarget.All, new object[0]);
		}

		[PunRPC]
		public void RpcClearBuffer()
		{
			this.stateCount = 0;
			this.firstReceivedMessageZeroTime = 0f;
			this.restStatePosition = SmoothSyncPUN2.RestState.MOVING;
			this.restStateRotation = SmoothSyncPUN2.RestState.MOVING;
		}

		public void teleport()
		{
			this.teleportOwnedObjectFromOwner();
		}

		public void teleportOwnedObjectFromOwner()
		{
			if (!base.photonView.IsMine)
			{
				UnityEngine.Debug.LogWarning("Should only call teleportOwnedObjectFromOwner() on owned objects.");
				return;
			}
			this.latestTeleportedFromPosition = this.getPosition();
			this.latestTeleportedFromRotation = this.getRotation();
			base.photonView.RPC("RpcTeleport", RpcTarget.Others, new object[]
			{
				this.getPosition(),
				this.getRotation().eulerAngles,
				this.getScale(),
				Time.realtimeSinceStartup
			});
		}

		public void teleportAnyObject(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
		{
			if (base.photonView.IsMine)
			{
				this.setPosition(newPosition, true);
				this.setRotation(newRotation, true);
				this.setScale(newScale, true);
				this.teleportOwnedObjectFromOwner();
			}
			if (!base.photonView.IsMine)
			{
				base.photonView.RPC("RpcNonServerOwnedTeleportFromServer", RpcTarget.Others, new object[]
				{
					newPosition,
					newRotation.eulerAngles,
					newScale
				});
			}
		}

		[PunRPC]
		public void RpcNonServerOwnedTeleportFromServer(Vector3 newPosition, Vector3 newRotation, Vector3 newScale)
		{
			if (base.photonView.IsMine)
			{
				this.setPosition(newPosition, true);
				this.setRotation(Quaternion.Euler(newRotation), true);
				this.setScale(newScale, true);
				this.teleportOwnedObjectFromOwner();
			}
		}

		[PunRPC]
		public void RpcTeleport(Vector3 position, Vector3 rotation, Vector3 scale, float tempOwnerTime)
		{
			StatePUN2 statePUN = new StatePUN2();
			statePUN.copyFromSmoothSync(this);
			statePUN.position = position;
			statePUN.rotation = Quaternion.Euler(rotation);
			statePUN.ownerTimestamp = tempOwnerTime;
			statePUN.teleport = true;
			this.addTeleportState(statePUN);
		}

		private void addTeleportState(StatePUN2 teleportState)
		{
			if (this.stateCount == 0)
			{
				this.approximateNetworkTimeOnOwner = teleportState.ownerTimestamp;
			}
			if (this.stateCount == 0 || teleportState.ownerTimestamp >= this.stateBuffer[0].ownerTimestamp)
			{
				for (int i = this.stateBuffer.Length - 1; i >= 1; i--)
				{
					this.stateBuffer[i] = this.stateBuffer[i - 1];
				}
				this.stateBuffer[0] = teleportState;
			}
			else
			{
				for (int j = this.stateBuffer.Length - 2; j >= 0; j--)
				{
					if (this.stateBuffer[j].ownerTimestamp > teleportState.ownerTimestamp)
					{
						int num = this.stateBuffer.Length - 1;
						while (j >= 1)
						{
							if (num == j)
							{
								break;
							}
							this.stateBuffer[num] = this.stateBuffer[num - 1];
							j--;
						}
						this.stateBuffer[j + 1] = teleportState;
						break;
					}
				}
			}
			this.stateCount = Mathf.Min(this.stateCount + 1, this.stateBuffer.Length);
		}

		public void forceStateSendNextOnPhotonSerializeView()
		{
			this.forceStateSend = true;
		}

		public bool shouldSendPosition()
		{
			return this.syncPosition != SyncMode.NONE && (this.forceStateSend || (this.getPosition() != this.lastPositionWhenStateWasSent && (this.sendPositionThreshold == 0f || Vector3.Distance(this.lastPositionWhenStateWasSent, this.getPosition()) > this.sendPositionThreshold)));
		}

		public bool shouldSendRotation()
		{
			return this.syncRotation != SyncMode.NONE && (this.forceStateSend || (this.getRotation() != this.lastRotationWhenStateWasSent && (this.sendRotationThreshold == 0f || Quaternion.Angle(this.lastRotationWhenStateWasSent, this.getRotation()) > this.sendRotationThreshold)));
		}

		public bool shouldSendScale()
		{
			return this.syncScale != SyncMode.NONE && (this.forceStateSend || (this.getScale() != this.lastScaleWhenStateWasSent && (this.sendScaleThreshold == 0f || Vector3.Distance(this.lastScaleWhenStateWasSent, this.getScale()) > this.sendScaleThreshold)));
		}

		public bool shouldSendVelocity()
		{
			if (this.hasRigidbody)
			{
				return this.syncVelocity != SyncMode.NONE && (this.forceStateSend || (this.rb.velocity != this.lastVelocityWhenStateWasSent && (this.sendVelocityThreshold == 0f || Vector3.Distance(this.lastVelocityWhenStateWasSent, this.rb.velocity) > this.sendVelocityThreshold)));
			}
			return this.hasRigidbody2D && (this.syncVelocity != SyncMode.NONE && (this.forceStateSend || ((this.rb2D.velocity.x != this.lastVelocityWhenStateWasSent.x || this.rb2D.velocity.y != this.lastVelocityWhenStateWasSent.y) && (this.sendVelocityThreshold == 0f || Vector2.Distance(this.lastVelocityWhenStateWasSent, this.rb2D.velocity) > this.sendVelocityThreshold))));
		}

		public bool shouldSendAngularVelocity()
		{
			if (this.hasRigidbody)
			{
				return this.syncAngularVelocity != SyncMode.NONE && (this.forceStateSend || (this.rb.angularVelocity != this.lastAngularVelocityWhenStateWasSent && (this.sendAngularVelocityThreshold == 0f || Vector3.Distance(this.lastAngularVelocityWhenStateWasSent, this.rb.angularVelocity * 57.29578f) > this.sendAngularVelocityThreshold)));
			}
			return this.hasRigidbody2D && (this.syncAngularVelocity != SyncMode.NONE && (this.forceStateSend || (this.rb2D.angularVelocity != this.lastAngularVelocityWhenStateWasSent.z && (this.sendAngularVelocityThreshold == 0f || Mathf.Abs(this.lastAngularVelocityWhenStateWasSent.z - this.rb2D.angularVelocity) > this.sendAngularVelocityThreshold))));
		}

		public bool isSyncingXPosition
		{
			get
			{
				return this.syncPosition == SyncMode.XYZ || this.syncPosition == SyncMode.XY || this.syncPosition == SyncMode.XZ || this.syncPosition == SyncMode.X;
			}
		}

		public bool isSyncingYPosition
		{
			get
			{
				return this.syncPosition == SyncMode.XYZ || this.syncPosition == SyncMode.XY || this.syncPosition == SyncMode.YZ || this.syncPosition == SyncMode.Y;
			}
		}

		public bool isSyncingZPosition
		{
			get
			{
				return this.syncPosition == SyncMode.XYZ || this.syncPosition == SyncMode.XZ || this.syncPosition == SyncMode.YZ || this.syncPosition == SyncMode.Z;
			}
		}

		public bool isSyncingXRotation
		{
			get
			{
				return this.syncRotation == SyncMode.XYZ || this.syncRotation == SyncMode.XY || this.syncRotation == SyncMode.XZ || this.syncRotation == SyncMode.X;
			}
		}

		public bool isSyncingYRotation
		{
			get
			{
				return this.syncRotation == SyncMode.XYZ || this.syncRotation == SyncMode.XY || this.syncRotation == SyncMode.YZ || this.syncRotation == SyncMode.Y;
			}
		}

		public bool isSyncingZRotation
		{
			get
			{
				return this.syncRotation == SyncMode.XYZ || this.syncRotation == SyncMode.XZ || this.syncRotation == SyncMode.YZ || this.syncRotation == SyncMode.Z;
			}
		}

		public bool isSyncingXScale
		{
			get
			{
				return this.syncScale == SyncMode.XYZ || this.syncScale == SyncMode.XY || this.syncScale == SyncMode.XZ || this.syncScale == SyncMode.X;
			}
		}

		public bool isSyncingYScale
		{
			get
			{
				return this.syncScale == SyncMode.XYZ || this.syncScale == SyncMode.XY || this.syncScale == SyncMode.YZ || this.syncScale == SyncMode.Y;
			}
		}

		public bool isSyncingZScale
		{
			get
			{
				return this.syncScale == SyncMode.XYZ || this.syncScale == SyncMode.XZ || this.syncScale == SyncMode.YZ || this.syncScale == SyncMode.Z;
			}
		}

		public bool isSyncingXVelocity
		{
			get
			{
				return this.syncVelocity == SyncMode.XYZ || this.syncVelocity == SyncMode.XY || this.syncVelocity == SyncMode.XZ || this.syncVelocity == SyncMode.X;
			}
		}

		public bool isSyncingYVelocity
		{
			get
			{
				return this.syncVelocity == SyncMode.XYZ || this.syncVelocity == SyncMode.XY || this.syncVelocity == SyncMode.YZ || this.syncVelocity == SyncMode.Y;
			}
		}

		public bool isSyncingZVelocity
		{
			get
			{
				return this.syncVelocity == SyncMode.XYZ || this.syncVelocity == SyncMode.XZ || this.syncVelocity == SyncMode.YZ || this.syncVelocity == SyncMode.Z;
			}
		}

		public bool isSyncingXAngularVelocity
		{
			get
			{
				return this.syncAngularVelocity == SyncMode.XYZ || this.syncAngularVelocity == SyncMode.XY || this.syncAngularVelocity == SyncMode.XZ || this.syncAngularVelocity == SyncMode.X;
			}
		}

		public bool isSyncingYAngularVelocity
		{
			get
			{
				return this.syncAngularVelocity == SyncMode.XYZ || this.syncAngularVelocity == SyncMode.XY || this.syncAngularVelocity == SyncMode.YZ || this.syncAngularVelocity == SyncMode.Y;
			}
		}

		public bool isSyncingZAngularVelocity
		{
			get
			{
				return this.syncAngularVelocity == SyncMode.XYZ || this.syncAngularVelocity == SyncMode.XZ || this.syncAngularVelocity == SyncMode.YZ || this.syncAngularVelocity == SyncMode.Z;
			}
		}

		private bool isObservedByConnection(NetworkConnection conn)
		{
			for (int i = 0; i < this.netID.observers.Count; i++)
			{
				if (this.netID.observers[i] == conn)
				{
					return true;
				}
			}
			return false;
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				this.sendState();
				if (this.shouldSendNextPUNUpdate)
				{
					NetworkWriter networkWriter = new NetworkWriter();
					this.sendingTempState.Serialize(networkWriter);
					stream.SendNext(this.shouldSendNextPUNUpdate);
					stream.SendNext(networkWriter.AsArray());
					this.shouldSendNextPUNUpdate = false;
				}
				else
				{
					stream.SendNext(this.shouldSendNextPUNUpdate);
				}
				this.positionLastAttemptedToSend = this.getPosition();
				this.rotationLastAttemptedToSend = this.getRotation();
				this.lastTimeAttemptedToSend = Time.realtimeSinceStartup;
				this.resetFlags();
			}
			else
			{
				bool flag = (bool)stream.ReceiveNext();
				if (flag)
				{
					NetworkStatePUN2 networkStatePUN = new NetworkStatePUN2(this);
					object obj = stream.ReceiveNext();
					byte[] buffer = obj as byte[];
					NetworkReader reader = new NetworkReader(buffer);
					networkStatePUN.Deserialize(reader, this);
					if (networkStatePUN != null && !base.photonView.IsMine)
					{
						networkStatePUN.smoothSync.addState(networkStatePUN.state);
					}
				}
			}
		}

		public float approximateNetworkTimeOnOwner
		{
			get
			{
				return this._ownerTime + (Time.realtimeSinceStartup - this.lastTimeOwnerTimeWasSet);
			}
			set
			{
				this._ownerTime = value;
				this.lastTimeOwnerTimeWasSet = Time.realtimeSinceStartup;
			}
		}

		private void adjustOwnerTime()
		{
			if (this.stateBuffer[0] == null || (this.stateBuffer[0].atPositionalRest && this.stateBuffer[0].atRotationalRest))
			{
				return;
			}
			float ownerTimestamp = this.stateBuffer[0].ownerTimestamp;
			float num = this.timeCorrectionSpeed * Time.deltaTime;
			if (this.firstReceivedMessageZeroTime == 0f)
			{
				this.firstReceivedMessageZeroTime = Time.realtimeSinceStartup;
			}
			float num2 = Mathf.Abs(this.approximateNetworkTimeOnOwner - ownerTimestamp);
			if (this.receivedStatesCounter < PhotonNetwork.SerializationRate || num2 < num || num2 > this.snapTimeThreshold)
			{
				this.approximateNetworkTimeOnOwner = ownerTimestamp;
			}
			else if (this.approximateNetworkTimeOnOwner < ownerTimestamp)
			{
				this.approximateNetworkTimeOnOwner += num;
			}
			else
			{
				this.approximateNetworkTimeOnOwner -= num;
			}
		}

		public float interpolationBackTime = 0.12f;

		public SmoothSyncPUN2.ExtrapolationMode extrapolationMode = SmoothSyncPUN2.ExtrapolationMode.Limited;

		public bool useExtrapolationTimeLimit = true;

		public float extrapolationTimeLimit = 5f;

		public bool useExtrapolationDistanceLimit;

		public float extrapolationDistanceLimit = 20f;

		public float sendPositionThreshold;

		public float sendRotationThreshold;

		public float sendScaleThreshold;

		public float sendVelocityThreshold;

		public float sendAngularVelocityThreshold;

		public float receivedPositionThreshold;

		public float receivedRotationThreshold;

		public float snapPositionThreshold;

		public float snapRotationThreshold;

		public float snapScaleThreshold;

		[Range(0f, 1f)]
		public float positionLerpSpeed = 0.85f;

		[Range(0f, 1f)]
		public float rotationLerpSpeed = 0.85f;

		[Range(0f, 1f)]
		public float scaleLerpSpeed = 0.85f;

		[Range(0f, 5f)]
		public float timeCorrectionSpeed = 0.1f;

		public float snapTimeThreshold = 3f;

		public SyncMode syncPosition;

		public SyncMode syncRotation;

		public SyncMode syncScale;

		public SyncMode syncVelocity;

		public SyncMode syncAngularVelocity;

		public bool isPositionCompressed;

		public bool isRotationCompressed;

		public bool isScaleCompressed;

		public bool isVelocityCompressed;

		public bool isAngularVelocityCompressed;

		public SmoothSyncPUN2.WhenToUpdateTransform whenToUpdateTransform;

		public GameObject childObjectToSync;

		[NonSerialized]
		public bool hasChildObject;

		[NonSerialized]
		public SmoothSyncPUN2.validateStateDelegate validateStateMethod;

		private StatePUN2 latestValidatedState;

		public bool setVelocityInsteadOfPositionOnNonOwners;

		public float maxPositionDifferenceForVelocitySyncing;

		[NonSerialized]
		public StatePUN2[] stateBuffer;

		[NonSerialized]
		public int stateCount;

		[NonSerialized]
		public Rigidbody rb;

		[NonSerialized]
		public bool hasRigidbody;

		[NonSerialized]
		public Rigidbody2D rb2D;

		[NonSerialized]
		public bool hasRigidbody2D;

		private bool dontLerp;

		private float firstReceivedMessageZeroTime;

		[NonSerialized]
		public float lastTimeStateWasSent;

		[NonSerialized]
		public Vector3 lastPositionWhenStateWasSent;

		[NonSerialized]
		public Quaternion lastRotationWhenStateWasSent;

		[NonSerialized]
		public Vector3 lastScaleWhenStateWasSent;

		[NonSerialized]
		public Vector3 lastVelocityWhenStateWasSent;

		[NonSerialized]
		public Vector3 lastAngularVelocityWhenStateWasSent;

		[NonSerialized]
		public NetworkIdentity netID;

		[NonSerialized]
		public GameObject realObjectToSync;

		[NonSerialized]
		public int syncIndex;

		[NonSerialized]
		public SmoothSyncPUN2[] childObjectSmoothSyncs;

		[NonSerialized]
		public bool forceStateSend;

		[NonSerialized]
		public bool sendAtPositionalRestMessage;

		[NonSerialized]
		public bool sendAtRotationalRestMessage;

		[NonSerialized]
		public bool sendPosition;

		[NonSerialized]
		public bool sendRotation;

		[NonSerialized]
		public bool sendScale;

		[NonSerialized]
		public bool sendVelocity;

		[NonSerialized]
		public bool sendAngularVelocity;

		private StatePUN2 targetTempState;

		private NetworkStatePUN2 sendingTempState;

		[NonSerialized]
		public Vector3 latestReceivedVelocity;

		[NonSerialized]
		public Vector3 latestReceivedAngularVelocity;

		private float timeSpentExtrapolating;

		private bool extrapolatedLastFrame;

		private Vector3 positionLastAttemptedToSend;

		private bool changedPositionLastFrame;

		private Quaternion rotationLastAttemptedToSend;

		private bool changedRotationLastFrame;

		private int atRestThresholdCount;

		private int samePositionCount;

		private int sameRotationCount;

		private SmoothSyncPUN2.RestState restStatePosition;

		private SmoothSyncPUN2.RestState restStateRotation;

		private bool hadAuthorityLastFrame;

		private StatePUN2 latestEndStateUsed;

		private bool shouldSendNextPUNUpdate;

		private Vector3 latestTeleportedFromPosition;

		private Quaternion latestTeleportedFromRotation;

		private bool triedToExtrapolateTooFar;

		private float lastTimeAttemptedToSend;

		private float _ownerTime;

		private float lastTimeOwnerTimeWasSet;

		public int receivedStatesCounter;

		[CompilerGenerated]
		private static SmoothSyncPUN2.validateStateDelegate _003C_003Ef__mg_0024cache0;

		public enum ExtrapolationMode
		{
			None,
			Limited,
			Unlimited
		}

		public enum WhenToUpdateTransform
		{
			Update,
			FixedUpdate
		}

		public delegate bool validateStateDelegate(StatePUN2 receivedState, StatePUN2 latestVerifiedState);

		private enum RestState
		{
			AT_REST,
			JUST_STARTED_MOVING,
			MOVING
		}
	}
}

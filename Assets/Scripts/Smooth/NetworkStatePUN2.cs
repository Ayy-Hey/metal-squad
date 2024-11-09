using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

namespace Smooth
{
	public class NetworkStatePUN2 : MessageBase
	{
		public NetworkStatePUN2()
		{
		}

		public NetworkStatePUN2(SmoothSyncPUN2 smoothSyncScript)
		{
			this.smoothSync = smoothSyncScript;
			this.state.copyFromSmoothSync(smoothSyncScript);
		}

		public void copyFromSmoothSync(SmoothSyncPUN2 smoothSyncScript)
		{
			this.smoothSync = smoothSyncScript;
			this.state.copyFromSmoothSync(smoothSyncScript);
		}

		public override void Serialize(NetworkWriter writer)
		{
			bool sendPosition = this.smoothSync.sendPosition;
			bool sendRotation = this.smoothSync.sendRotation;
			bool sendScale = this.smoothSync.sendScale;
			bool sendVelocity = this.smoothSync.sendVelocity;
			bool sendAngularVelocity = this.smoothSync.sendAngularVelocity;
			bool sendAtPositionalRestMessage = this.smoothSync.sendAtPositionalRestMessage;
			bool sendAtRotationalRestMessage = this.smoothSync.sendAtRotationalRestMessage;
			if (sendPosition)
			{
				this.smoothSync.lastPositionWhenStateWasSent = this.state.position;
			}
			if (sendRotation)
			{
				this.smoothSync.lastRotationWhenStateWasSent = this.state.rotation;
			}
			if (sendScale)
			{
				this.smoothSync.lastScaleWhenStateWasSent = this.state.scale;
			}
			if (sendVelocity)
			{
				this.smoothSync.lastVelocityWhenStateWasSent = this.state.velocity;
			}
			if (sendAngularVelocity)
			{
				this.smoothSync.lastAngularVelocityWhenStateWasSent = this.state.angularVelocity;
			}
			writer.Write(this.encodeSyncInformation(sendPosition, sendRotation, sendScale, sendVelocity, sendAngularVelocity, sendAtPositionalRestMessage, sendAtRotationalRestMessage));
			writer.WritePackedUInt32((uint)this.smoothSync.syncIndex);
			writer.Write(this.state.ownerTimestamp);
			if (sendPosition)
			{
				if (this.smoothSync.isPositionCompressed)
				{
					if (this.smoothSync.isSyncingXPosition)
					{
						writer.Write(HalfHelper.Compress(this.state.position.x));
					}
					if (this.smoothSync.isSyncingYPosition)
					{
						writer.Write(HalfHelper.Compress(this.state.position.y));
					}
					if (this.smoothSync.isSyncingZPosition)
					{
						writer.Write(HalfHelper.Compress(this.state.position.z));
					}
				}
				else
				{
					if (this.smoothSync.isSyncingXPosition)
					{
						writer.Write(this.state.position.x);
					}
					if (this.smoothSync.isSyncingYPosition)
					{
						writer.Write(this.state.position.y);
					}
					if (this.smoothSync.isSyncingZPosition)
					{
						writer.Write(this.state.position.z);
					}
				}
			}
			if (sendRotation)
			{
				Vector3 eulerAngles = this.state.rotation.eulerAngles;
				if (this.smoothSync.isRotationCompressed)
				{
					if (this.smoothSync.isSyncingXRotation)
					{
						writer.Write(HalfHelper.Compress(eulerAngles.x * 0.0174532924f));
					}
					if (this.smoothSync.isSyncingYRotation)
					{
						writer.Write(HalfHelper.Compress(eulerAngles.y * 0.0174532924f));
					}
					if (this.smoothSync.isSyncingZRotation)
					{
						writer.Write(HalfHelper.Compress(eulerAngles.z * 0.0174532924f));
					}
				}
				else
				{
					if (this.smoothSync.isSyncingXRotation)
					{
						writer.Write(eulerAngles.x);
					}
					if (this.smoothSync.isSyncingYRotation)
					{
						writer.Write(eulerAngles.y);
					}
					if (this.smoothSync.isSyncingZRotation)
					{
						writer.Write(eulerAngles.z);
					}
				}
			}
			if (sendScale)
			{
				if (this.smoothSync.isScaleCompressed)
				{
					if (this.smoothSync.isSyncingXScale)
					{
						writer.Write(HalfHelper.Compress(this.state.scale.x));
					}
					if (this.smoothSync.isSyncingYScale)
					{
						writer.Write(HalfHelper.Compress(this.state.scale.y));
					}
					if (this.smoothSync.isSyncingZScale)
					{
						writer.Write(HalfHelper.Compress(this.state.scale.z));
					}
				}
				else
				{
					if (this.smoothSync.isSyncingXScale)
					{
						writer.Write(this.state.scale.x);
					}
					if (this.smoothSync.isSyncingYScale)
					{
						writer.Write(this.state.scale.y);
					}
					if (this.smoothSync.isSyncingZScale)
					{
						writer.Write(this.state.scale.z);
					}
				}
			}
			if (sendVelocity)
			{
				if (this.smoothSync.isVelocityCompressed)
				{
					if (this.smoothSync.isSyncingXVelocity)
					{
						writer.Write(HalfHelper.Compress(this.state.velocity.x));
					}
					if (this.smoothSync.isSyncingYVelocity)
					{
						writer.Write(HalfHelper.Compress(this.state.velocity.y));
					}
					if (this.smoothSync.isSyncingZVelocity)
					{
						writer.Write(HalfHelper.Compress(this.state.velocity.z));
					}
				}
				else
				{
					if (this.smoothSync.isSyncingXVelocity)
					{
						writer.Write(this.state.velocity.x);
					}
					if (this.smoothSync.isSyncingYVelocity)
					{
						writer.Write(this.state.velocity.y);
					}
					if (this.smoothSync.isSyncingZVelocity)
					{
						writer.Write(this.state.velocity.z);
					}
				}
			}
			if (sendAngularVelocity)
			{
				if (this.smoothSync.isAngularVelocityCompressed)
				{
					if (this.smoothSync.isSyncingXAngularVelocity)
					{
						writer.Write(HalfHelper.Compress(this.state.angularVelocity.x * 0.0174532924f));
					}
					if (this.smoothSync.isSyncingYAngularVelocity)
					{
						writer.Write(HalfHelper.Compress(this.state.angularVelocity.y * 0.0174532924f));
					}
					if (this.smoothSync.isSyncingZAngularVelocity)
					{
						writer.Write(HalfHelper.Compress(this.state.angularVelocity.z * 0.0174532924f));
					}
				}
				else
				{
					if (this.smoothSync.isSyncingXAngularVelocity)
					{
						writer.Write(this.state.angularVelocity.x);
					}
					if (this.smoothSync.isSyncingYAngularVelocity)
					{
						writer.Write(this.state.angularVelocity.y);
					}
					if (this.smoothSync.isSyncingZAngularVelocity)
					{
						writer.Write(this.state.angularVelocity.z);
					}
				}
			}
		}

		public void Deserialize(NetworkReader reader, SmoothSyncPUN2 smoothSync)
		{
			byte syncInformation = reader.ReadByte();
			bool flag = this.shouldSyncPosition(syncInformation);
			bool flag2 = this.shouldSyncRotation(syncInformation);
			bool flag3 = this.shouldSyncScale(syncInformation);
			bool flag4 = this.shouldSyncVelocity(syncInformation);
			bool flag5 = this.shouldSyncAngularVelocity(syncInformation);
			this.state.atPositionalRest = this.shouldBeAtPositionalRest(syncInformation);
			this.state.atRotationalRest = this.shouldBeAtRotationalRest(syncInformation);
			int num = (int)reader.ReadPackedUInt32();
			this.state.ownerTimestamp = reader.ReadSingle();
			if (NetworkServer.active && !smoothSync.photonView.IsMine)
			{
				this.state.serverShouldRelayPosition = flag;
				this.state.serverShouldRelayRotation = flag2;
				this.state.serverShouldRelayScale = flag3;
				this.state.serverShouldRelayVelocity = flag4;
				this.state.serverShouldRelayAngularVelocity = flag5;
			}
			for (int i = 0; i < smoothSync.childObjectSmoothSyncs.Length; i++)
			{
				if (smoothSync.childObjectSmoothSyncs[i].syncIndex == num)
				{
					smoothSync = smoothSync.childObjectSmoothSyncs[i];
				}
			}
			if (!smoothSync)
			{
				UnityEngine.Debug.LogWarning("Could not find target for network state message.");
				return;
			}
			if (smoothSync.receivedStatesCounter < PhotonNetwork.SerializationRate)
			{
				smoothSync.receivedStatesCounter++;
			}
			if (flag)
			{
				if (smoothSync.isPositionCompressed)
				{
					if (smoothSync.isSyncingXPosition)
					{
						this.state.position.x = HalfHelper.Decompress(reader.ReadUInt16());
					}
					if (smoothSync.isSyncingYPosition)
					{
						this.state.position.y = HalfHelper.Decompress(reader.ReadUInt16());
					}
					if (smoothSync.isSyncingZPosition)
					{
						this.state.position.z = HalfHelper.Decompress(reader.ReadUInt16());
					}
				}
				else
				{
					if (smoothSync.isSyncingXPosition)
					{
						this.state.position.x = reader.ReadSingle();
					}
					if (smoothSync.isSyncingYPosition)
					{
						this.state.position.y = reader.ReadSingle();
					}
					if (smoothSync.isSyncingZPosition)
					{
						this.state.position.z = reader.ReadSingle();
					}
				}
			}
			else if (smoothSync.stateCount > 0)
			{
				this.state.position = smoothSync.stateBuffer[0].position;
			}
			else
			{
				this.state.position = smoothSync.getPosition();
			}
			if (flag2)
			{
				this.state.reusableRotationVector = Vector3.zero;
				if (smoothSync.isRotationCompressed)
				{
					if (smoothSync.isSyncingXRotation)
					{
						this.state.reusableRotationVector.x = HalfHelper.Decompress(reader.ReadUInt16());
						StatePUN2 statePUN = this.state;
						statePUN.reusableRotationVector.x = statePUN.reusableRotationVector.x * 57.29578f;
					}
					if (smoothSync.isSyncingYRotation)
					{
						this.state.reusableRotationVector.y = HalfHelper.Decompress(reader.ReadUInt16());
						StatePUN2 statePUN2 = this.state;
						statePUN2.reusableRotationVector.y = statePUN2.reusableRotationVector.y * 57.29578f;
					}
					if (smoothSync.isSyncingZRotation)
					{
						this.state.reusableRotationVector.z = HalfHelper.Decompress(reader.ReadUInt16());
						StatePUN2 statePUN3 = this.state;
						statePUN3.reusableRotationVector.z = statePUN3.reusableRotationVector.z * 57.29578f;
					}
					this.state.rotation = Quaternion.Euler(this.state.reusableRotationVector);
				}
				else
				{
					if (smoothSync.isSyncingXRotation)
					{
						this.state.reusableRotationVector.x = reader.ReadSingle();
					}
					if (smoothSync.isSyncingYRotation)
					{
						this.state.reusableRotationVector.y = reader.ReadSingle();
					}
					if (smoothSync.isSyncingZRotation)
					{
						this.state.reusableRotationVector.z = reader.ReadSingle();
					}
					this.state.rotation = Quaternion.Euler(this.state.reusableRotationVector);
				}
			}
			else if (smoothSync.stateCount > 0)
			{
				this.state.rotation = smoothSync.stateBuffer[0].rotation;
			}
			else
			{
				this.state.rotation = smoothSync.getRotation();
			}
			if (flag3)
			{
				if (smoothSync.isScaleCompressed)
				{
					if (smoothSync.isSyncingXScale)
					{
						this.state.scale.x = HalfHelper.Decompress(reader.ReadUInt16());
					}
					if (smoothSync.isSyncingYScale)
					{
						this.state.scale.y = HalfHelper.Decompress(reader.ReadUInt16());
					}
					if (smoothSync.isSyncingZScale)
					{
						this.state.scale.z = HalfHelper.Decompress(reader.ReadUInt16());
					}
				}
				else
				{
					if (smoothSync.isSyncingXScale)
					{
						this.state.scale.x = reader.ReadSingle();
					}
					if (smoothSync.isSyncingYScale)
					{
						this.state.scale.y = reader.ReadSingle();
					}
					if (smoothSync.isSyncingZScale)
					{
						this.state.scale.z = reader.ReadSingle();
					}
				}
			}
			else if (smoothSync.stateCount > 0)
			{
				this.state.scale = smoothSync.stateBuffer[0].scale;
			}
			else
			{
				this.state.scale = smoothSync.getScale();
			}
			if (flag4)
			{
				if (smoothSync.isVelocityCompressed)
				{
					if (smoothSync.isSyncingXVelocity)
					{
						this.state.velocity.x = HalfHelper.Decompress(reader.ReadUInt16());
					}
					if (smoothSync.isSyncingYVelocity)
					{
						this.state.velocity.y = HalfHelper.Decompress(reader.ReadUInt16());
					}
					if (smoothSync.isSyncingZVelocity)
					{
						this.state.velocity.z = HalfHelper.Decompress(reader.ReadUInt16());
					}
				}
				else
				{
					if (smoothSync.isSyncingXVelocity)
					{
						this.state.velocity.x = reader.ReadSingle();
					}
					if (smoothSync.isSyncingYVelocity)
					{
						this.state.velocity.y = reader.ReadSingle();
					}
					if (smoothSync.isSyncingZVelocity)
					{
						this.state.velocity.z = reader.ReadSingle();
					}
				}
				smoothSync.latestReceivedVelocity = this.state.velocity;
			}
			else
			{
				this.state.velocity = smoothSync.latestReceivedVelocity;
			}
			if (flag5)
			{
				if (smoothSync.isAngularVelocityCompressed)
				{
					this.state.reusableRotationVector = Vector3.zero;
					if (smoothSync.isSyncingXAngularVelocity)
					{
						this.state.reusableRotationVector.x = HalfHelper.Decompress(reader.ReadUInt16());
						StatePUN2 statePUN4 = this.state;
						statePUN4.reusableRotationVector.x = statePUN4.reusableRotationVector.x * 57.29578f;
					}
					if (smoothSync.isSyncingYAngularVelocity)
					{
						this.state.reusableRotationVector.y = HalfHelper.Decompress(reader.ReadUInt16());
						StatePUN2 statePUN5 = this.state;
						statePUN5.reusableRotationVector.y = statePUN5.reusableRotationVector.y * 57.29578f;
					}
					if (smoothSync.isSyncingZAngularVelocity)
					{
						this.state.reusableRotationVector.z = HalfHelper.Decompress(reader.ReadUInt16());
						StatePUN2 statePUN6 = this.state;
						statePUN6.reusableRotationVector.z = statePUN6.reusableRotationVector.z * 57.29578f;
					}
					this.state.angularVelocity = this.state.reusableRotationVector;
				}
				else
				{
					if (smoothSync.isSyncingXAngularVelocity)
					{
						this.state.angularVelocity.x = reader.ReadSingle();
					}
					if (smoothSync.isSyncingYAngularVelocity)
					{
						this.state.angularVelocity.y = reader.ReadSingle();
					}
					if (smoothSync.isSyncingZAngularVelocity)
					{
						this.state.angularVelocity.z = reader.ReadSingle();
					}
				}
				smoothSync.latestReceivedAngularVelocity = this.state.angularVelocity;
			}
			else
			{
				this.state.angularVelocity = smoothSync.latestReceivedAngularVelocity;
			}
		}

		private byte encodeSyncInformation(bool sendPosition, bool sendRotation, bool sendScale, bool sendVelocity, bool sendAngularVelocity, bool atPositionalRest, bool atRotationalRest)
		{
			byte b = 0;
			if (sendPosition)
			{
				b |= this.positionMask;
			}
			if (sendRotation)
			{
				b |= this.rotationMask;
			}
			if (sendScale)
			{
				b |= this.scaleMask;
			}
			if (sendVelocity)
			{
				b |= this.velocityMask;
			}
			if (sendAngularVelocity)
			{
				b |= this.angularVelocityMask;
			}
			if (atPositionalRest)
			{
				b |= this.atPositionalRestMask;
			}
			if (atRotationalRest)
			{
				b |= this.atRotationalRestMask;
			}
			return b;
		}

		private bool shouldSyncPosition(byte syncInformation)
		{
			return (syncInformation & this.positionMask) == this.positionMask;
		}

		private bool shouldSyncRotation(byte syncInformation)
		{
			return (syncInformation & this.rotationMask) == this.rotationMask;
		}

		private bool shouldSyncScale(byte syncInformation)
		{
			return (syncInformation & this.scaleMask) == this.scaleMask;
		}

		private bool shouldSyncVelocity(byte syncInformation)
		{
			return (syncInformation & this.velocityMask) == this.velocityMask;
		}

		private bool shouldSyncAngularVelocity(byte syncInformation)
		{
			return (syncInformation & this.angularVelocityMask) == this.angularVelocityMask;
		}

		private bool shouldBeAtPositionalRest(byte syncInformation)
		{
			return (syncInformation & this.atPositionalRestMask) == this.atPositionalRestMask;
		}

		private bool shouldBeAtRotationalRest(byte syncInformation)
		{
			return (syncInformation & this.atRotationalRestMask) == this.atRotationalRestMask;
		}

		public SmoothSyncPUN2 smoothSync;

		public StatePUN2 state = new StatePUN2();

		private byte positionMask = 1;

		private byte rotationMask = 2;

		private byte scaleMask = 4;

		private byte velocityMask = 8;

		private byte angularVelocityMask = 16;

		private byte atPositionalRestMask = 64;

		private byte atRotationalRestMask = 128;
	}
}

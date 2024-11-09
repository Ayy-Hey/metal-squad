using System;
using System.Collections;
using Photon.Pun;
using Player;
using PVPManager;
using UnityEngine;
using UnityEngine.Networking;

public class SyncRamboState : MonoBehaviourPunCallbacks, IPunObservable
{
	private IEnumerator Start()
	{
		UnityEngine.Debug.Log("+++++++++++++ SyncRamboState Start");
		if (base.photonView.IsMine)
		{
			UnityEngine.Debug.Log("+++++++++++++ Init local rambo: " + PvP_LocalPlayer.Instance.ConstActorNumber);
			this.targetPlayer.IsRemotePlayer = false;
			this.targetPlayer.OnInit(ProfileManager.settingProfile.IDChar, PvP_LocalPlayer.Instance.ConstActorNumber, 0, 0);
			this.IsInit = true;
		}
		else
		{
			this.m_photonView.RPC("RequestInitPlayer", RpcTarget.OthersBuffered, null);
		}
		yield return null;
		yield break;
	}

	[PunRPC]
	public void RequestInitPlayer()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"++++++++++++ Send RPC to init remote rambo: IDChar ",
			ProfileManager.settingProfile.IDChar,
			" IDGUN1 ",
			ProfileManager.rifleGunCurrentId.Data.Value,
			" rankUpgrade ",
			this.targetPlayer.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade,
			" IDKnife ",
			ProfileManager.meleCurrentId.Data.Value,
			" IDGrenades ",
			ProfileManager.grenadeCurrentId.Data.Value
		}));
		this.m_photonView.RPC("InitRemotePlayer", RpcTarget.OthersBuffered, new object[]
		{
			ProfileManager.settingProfile.IDChar,
			ProfileManager.rifleGunCurrentId.Data.Value,
			this.targetPlayer.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade,
			ProfileManager.meleCurrentId.Data.Value,
			ProfileManager.grenadeCurrentId.Data.Value,
			PvP_LocalPlayer.Instance.ConstActorNumber
		});
	}

	[PunRPC]
	public void InitRemotePlayer(int IDChar, int IDGUN1, int rankUpgrade, int IDKnife, int IDGrenades, int actorNumber)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"+++++++++++++ Init remote rambo: IDChar ",
			IDChar,
			" IDGUN1 ",
			IDGUN1,
			" rankUpgrade ",
			rankUpgrade,
			" IDKnife ",
			IDKnife,
			" IDGrenades ",
			IDGrenades,
			" actor number ",
			actorNumber
		}));
		this.targetPlayer.OnInitOnline(IDChar, IDGUN1, IDKnife, IDGrenades, actorNumber, rankUpgrade);
		this.IsInit = true;
	}

	private void Update()
	{
		if (!this.IsInit)
		{
			return;
		}
		if (!this.m_photonView.IsMine)
		{
			if (this.nextFlipX != this.lastFlipX)
			{
				this.targetPlayer._PlayerSpine.FlipX = this.nextFlipX;
				this.lastFlipX = this.nextFlipX;
			}
			this.playerAnimationControl.SetOnlineAnimationTrack0(this.nextOnlineAnimationTrack0);
			this.playerAnimationControl.SetOnlineAnimationTrack1(this.nextOnlineAnimationTrack1);
			this.targetPlayer.tfBone.position = this.nextTfBonePos;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.IsInit)
		{
			return;
		}
		if (stream.IsWriting)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.WritePackedUInt32((uint)this.playerAnimationControl.GetOnlineAnimationTrack0());
			networkWriter.WritePackedUInt32((uint)this.playerAnimationControl.GetOnlineAnimationTrack1());
			this.CompressVector2(networkWriter, this.targetPlayer.tfBone.position);
			networkWriter.Write(this.targetPlayer._PlayerSpine.FlipX);
			stream.SendNext(networkWriter.AsArray());
		}
		else
		{
			NetworkReader networkReader = new NetworkReader(stream.ReceiveNext() as byte[]);
			this.nextOnlineAnimationTrack0 = (CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation)networkReader.ReadPackedUInt32();
			this.nextOnlineAnimationTrack1 = (CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation)networkReader.ReadPackedUInt32();
			this.nextTfBonePos = this.DecompressVector2(networkReader);
			this.nextFlipX = networkReader.ReadBoolean();
		}
	}

	public void CompressVector2(NetworkWriter networkWriter, Vector2 vec)
	{
		networkWriter.Write(HalfHelper.Compress(vec.x));
		networkWriter.Write(HalfHelper.Compress(vec.y));
	}

	public Vector2 DecompressVector2(NetworkReader networkReader)
	{
		return new Vector2
		{
			x = HalfHelper.Decompress(networkReader.ReadUInt16()),
			y = HalfHelper.Decompress(networkReader.ReadUInt16())
		};
	}

	public void CompressVector3(NetworkWriter networkWriter, Vector3 vec)
	{
		networkWriter.Write(HalfHelper.Compress(vec.x));
		networkWriter.Write(HalfHelper.Compress(vec.y));
		networkWriter.Write(HalfHelper.Compress(vec.z));
	}

	public Vector3 DecompressVector3(NetworkReader networkReader)
	{
		return new Vector3
		{
			x = HalfHelper.Decompress(networkReader.ReadUInt16()),
			y = HalfHelper.Decompress(networkReader.ReadUInt16()),
			z = HalfHelper.Decompress(networkReader.ReadUInt16())
		};
	}

	public void SendRpc_CreateM4A1(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateM4A1", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateM4A1(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateM4A1(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_CreateMachine(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateMachine", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateMachine(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateMachine(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_CreateSpread(Vector2 pos, Vector2 Direction, float offsetAngle)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		networkWriter.Write(HalfHelper.Compress(offsetAngle));
		this.m_photonView.RPC("RemotePlayer_CreateSpread", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateSpread(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateSpread(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), HalfHelper.Decompress(networkReader.ReadUInt16()), false);
	}

	public void SendRpc_CreateICE(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateICE", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateICE(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateICE(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_CreateMGL140(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateMGL140", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateMGL140(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateMGL140(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_CreateSniper(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateSniper", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateSniper(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateSniper(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_CreateBulletCt9(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateBulletCt9", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateBulletCt9(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateBulletCt9(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_ThrowGrendeBasic(Vector2 pos, bool FlipX)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		networkWriter.Write(FlipX);
		this.m_photonView.RPC("RemotePlayer_ThrowGrendeBasic", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_ThrowGrendeBasic(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bombManager.ThrowGrendeBasic(this.targetPlayer, this.DecompressVector2(networkReader), networkReader.ReadBoolean(), false);
	}

	public void SendRpc_ThrowGrendeFire(Vector2 pos, bool FlipX)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		networkWriter.Write(FlipX);
		this.m_photonView.RPC("RemotePlayer_ThrowGrendeFire", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_ThrowGrendeFire(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bombManager.ThrowGrendeFire(this.targetPlayer, this.DecompressVector2(networkReader), networkReader.ReadBoolean(), false);
	}

	public void SendRpc_ThrowGrendeIce(Vector2 pos, bool FlipX)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		networkWriter.Write(FlipX);
		this.m_photonView.RPC("RemotePlayer_ThrowGrendeIce", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_ThrowGrendeIce(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bombManager.ThrowGrendeIce(this.targetPlayer, this.DecompressVector2(networkReader), networkReader.ReadBoolean(), false);
	}

	public void SendRpc_ThrowGrendeSmoke(Vector2 pos, bool FlipX)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		networkWriter.Write(FlipX);
		this.m_photonView.RPC("RemotePlayer_ThrowGrendeSmoke", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_ThrowGrendeSmoke(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bombManager.ThrowGrendeSmoke(this.targetPlayer, this.DecompressVector2(networkReader), networkReader.ReadBoolean(), false);
	}

	public void SendRpc_CreateRainBomb(Vector2 pos)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.m_photonView.RPC("RemotePlayer_CreateRainBomb", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateRainBomb(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.skillManager.CreateRainBomb(this.DecompressVector2(networkReader), false);
	}

	public void SendRpc_OnInvisible()
	{
		this.m_photonView.RPC("RemotePlayer_OnInvisible", RpcTarget.Others, null);
	}

	[PunRPC]
	public void RemotePlayer_OnInvisible()
	{
		this.targetPlayer._PlayerSkill.OnInvisible(100f);
	}

	public void SendRpc_OnVisible()
	{
		this.m_photonView.RPC("RemotePlayer_OnVsible", RpcTarget.Others, null);
	}

	[PunRPC]
	public void RemotePlayer_OnVsible()
	{
		this.targetPlayer._PlayerSkill.OnVisible();
	}

	public void SendRpc_CallEyeBotSupport()
	{
		this.m_photonView.RPC("RemotePlayer_CallEyeBotSupport", RpcTarget.Others, null);
	}

	[PunRPC]
	public void RemotePlayer_CallEyeBotSupport()
	{
		GameManager.Instance.skillManager.ShowSkill(this.targetPlayer, 0);
	}

	public void SendRpc_SwitchGun(bool isGunDefault, int IDGun2 = 0)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(isGunDefault);
		networkWriter.WritePackedUInt32((uint)IDGun2);
		this.m_photonView.RPC("RemotePlayer_SwitchGun", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_SwitchGun(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		bool flag = networkReader.ReadBoolean();
		int idgun = (int)networkReader.ReadPackedUInt32();
		if (!flag)
		{
			this.targetPlayer._PlayerData.IDGUN2 = idgun;
		}
		this.targetPlayer._PlayerInput.SwitchGun(flag);
	}

	public void SendRpc_FlameGun_ChangeFireActiveStatus(bool isActive)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(isActive);
		this.m_photonView.RPC("RemotePlayer_FlameGun_ChangeFireActiveStatus", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_FlameGun_ChangeFireActiveStatus(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		bool active = networkReader.ReadBoolean();
		FlameGunPlayer component = this.targetPlayer.GunCurrent.gameObject.GetComponent<FlameGunPlayer>();
		if (component != null)
		{
			component.ObjectFire.SetActive(active);
		}
	}

	public void SendRpc_FlameGun_ChangeFirePos(Vector2 pos, Vector2 directionGun)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, directionGun);
		this.m_photonView.RPC("RemotePlayer_FlameGun_ChangeFirePos", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_FlameGun_ChangeFirePos(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		FlameGunPlayer component = this.targetPlayer.GunCurrent.gameObject.GetComponent<FlameGunPlayer>();
		if (component != null)
		{
			component.ChangeFirePos(this.DecompressVector2(networkReader), this.DecompressVector2(networkReader));
		}
	}

	public void SendRpc_LaserGun_ChangeFireActiveStatus(bool isActive)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(isActive);
		this.m_photonView.RPC("RemotePlayer_LaserGun_ChangeFireActiveStatus", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_LaserGun_ChangeFireActiveStatus(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		bool active = networkReader.ReadBoolean();
		LaserGunPlayer component = this.targetPlayer.GunCurrent.gameObject.GetComponent<LaserGunPlayer>();
		if (component != null)
		{
			component.ObjectFire.SetActive(active);
		}
	}

	public void SendRpc_LaserGun_ChangeFirePos(Vector2 pos, Vector2 directionGun)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, directionGun);
		this.m_photonView.RPC("RemotePlayer_LaserGun_ChangeFirePos", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_LaserGun_ChangeFirePos(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		LaserGunPlayer component = this.targetPlayer.GunCurrent.gameObject.GetComponent<LaserGunPlayer>();
		if (component != null)
		{
			component.ChangeFirePos(this.DecompressVector2(networkReader), this.DecompressVector2(networkReader));
		}
	}

	public void SendRpc_ThunderGun_OnCreateBullet(Vector2 StartPosition, Vector2 EndPosition)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, StartPosition);
		this.CompressVector2(networkWriter, EndPosition);
		this.m_photonView.RPC("RemotePlayer_ThunderGun_OnCreateBullet", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_ThunderGun_OnCreateBullet(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		ThunderGunPlayer component = this.targetPlayer.GunCurrent.gameObject.GetComponent<ThunderGunPlayer>();
		if (component != null)
		{
			component.SetLightningLine(this.DecompressVector2(networkReader), this.DecompressVector2(networkReader));
		}
	}

	public void SendRpc_ThunderGun_OnRelease()
	{
		this.m_photonView.RPC("RemotePlayer_ThunderGun_OnRelease", RpcTarget.Others, null);
	}

	[PunRPC]
	public void RemotePlayer_ThunderGun_OnRelease()
	{
		ThunderGunPlayer component = this.targetPlayer.GunCurrent.gameObject.GetComponent<ThunderGunPlayer>();
		if (component != null)
		{
			component.ReleaseGun();
		}
	}

	public void SendRpc_CreateBulletFc(Vector2 pos, Vector2 Direction)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		this.CompressVector2(networkWriter, pos);
		this.CompressVector2(networkWriter, Direction);
		this.m_photonView.RPC("RemotePlayer_CreateBulletFc", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemotePlayer_CreateBulletFc(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		GameManager.Instance.bulletManager.CreateBulletFc(this.targetPlayer, this.DecompressVector2(networkReader), this.DecompressVector2(networkReader), false);
	}

	public PhotonView m_photonView;

	public PlayerMain targetPlayer;

	public CoOpPlayerMainAnimationControl playerAnimationControl;

	private bool IsInit;

	private CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation nextOnlineAnimationTrack0 = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;

	private CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation nextOnlineAnimationTrack1 = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;

	private Vector2 nextTfBonePos = Vector2.zero;

	private bool lastFlipX;

	private bool nextFlipX;
}

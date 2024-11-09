using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;

public class SyncBossIronMechState : MonoBehaviourPunCallbacks
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.targetBoss != null);
		UnityEngine.Debug.Log("++++++++++++++++ SyncBossIronMechState Start");
		this.m_photonView = PhotonView.Get(this);
		if (base.photonView.IsMine)
		{
			UnityEngine.Debug.Log("Init local Boss Iron Mech");
			this.targetBoss.IsRemoteBoss = false;
		}
		else
		{
			UnityEngine.Debug.Log("Init remote Boss Iron Mech");
			this.targetBoss.IsRemoteBoss = true;
		}
		GameManager.Instance.bossManager.Boss = this.targetBoss;
		this.targetBoss.AddHealthPointAction = new Action<float, EWeapon>(this.SendRpcAddHealthPoint);
		GameManager.Instance.bossManager.CreateBoss();
		this.IsInit = true;
		yield break;
	}

	public void SendRpcChanState(Boss1_6.E_State state, int targetActorNumber)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"+++++++++++++ Send state: ",
			state,
			" targetActorNumber ",
			targetActorNumber
		}));
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.WritePackedUInt32((uint)state);
		networkWriter.WritePackedUInt32((uint)targetActorNumber);
		base.photonView.RPC("RemoteBoss_ChanState", RpcTarget.Others, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemoteBoss_ChanState(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		this.targetBoss._state = (Boss1_6.E_State)networkReader.ReadPackedUInt32();
		int num = (int)networkReader.ReadPackedUInt32();
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"+++++++++++++ Receive state: ",
			this.targetBoss._state,
			" targetActorNumber ",
			num
		}));
		this.targetBoss.ChangeBossTarget(num);
		this.targetBoss.ChanState();
	}

	public void SendRpcAddHealthPoint(float hp, EWeapon lastWeapon)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(HalfHelper.Compress(hp));
		networkWriter.WritePackedUInt32((uint)lastWeapon);
		base.photonView.RPC("RemoteAddHealthPoint", RpcTarget.OthersBuffered, new object[]
		{
			networkWriter.AsArray()
		});
	}

	[PunRPC]
	public void RemoteAddHealthPoint(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		float num = HalfHelper.Decompress(networkReader.ReadUInt16());
		EWeapon eweapon = (EWeapon)networkReader.ReadPackedUInt32();
		this.targetBoss.boxBoss.HP += num;
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.targetBoss.boxBoss.HP, this.targetBoss.cacheEnemy.HP);
	}

	public void SendRpc_Die()
	{
		base.photonView.RPC("RemoteBoss_Die", RpcTarget.OthersBuffered, null);
	}

	[PunRPC]
	public void RemoteBoss_Die()
	{
		this.targetBoss.HP = -999f;
		this.targetBoss.Die();
	}

	public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
	{
		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			this.ActiveDamageLocalBoss();
		}
		else
		{
			this.DeactiveDamageLocalBoss();
		}
	}

	private void ActiveDamageLocalBoss()
	{
		UnityEngine.Debug.Log("+++++++++++ Active Damage Local Boss");
		this.targetBoss._ramboTransform = GameManager.Instance.player.transform;
		this.targetBoss.IsRemoteBoss = false;
	}

	private void DeactiveDamageLocalBoss()
	{
		UnityEngine.Debug.Log("+++++++++++ Deactive Damage Local Boss");
		this.targetBoss.IsRemoteBoss = true;
	}

	private bool IsInit;

	private PhotonView m_photonView;

	public Boss1_6 targetBoss;

	private Boss1_6.E_State lastState = Boss1_6.E_State.Idle;

	private Boss1_6.E_State nextState = Boss1_6.E_State.Idle;
}

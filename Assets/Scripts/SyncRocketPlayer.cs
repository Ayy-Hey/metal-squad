using System;
using System.Collections;
using Photon.Pun;
using PlayerWeapon;
using UnityEngine;
using UnityEngine.Networking;

public class SyncRocketPlayer : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.targetRocket != null);
		this.m_photonView = PhotonView.Get(this);
		this.IsRemote = !this.m_photonView.IsMine;
		if (this.IsRemote)
		{
			this.targetRocket.Damaged = 0f;
		}
		yield break;
	}

	private void InitOnlineRocket(Vector2 pos)
	{
		this.targetRocket.gameObject.SetActive(false);
		this.targetRocket.rigidbody2D.isKinematic = false;
		this.targetRocket.rigidbody2D.position = pos;
		this.targetRocket.transform.position = pos;
		this.targetRocket.duoiTenLua.SetActive(true);
		this.targetRocket.boxCollider2D.enabled = true;
		for (int i = 0; i < this.targetRocket.bullets.Length; i++)
		{
			this.targetRocket.bullets[i].SetActive(false);
		}
		this.targetRocket.bullets[0].SetActive(true);
		this.targetRocket.spriteRenderer.gameObject.SetActive(true);
		this.targetRocket.gameObject.SetActive(true);
		SingletonGame<AudioController>.Instance.PlaySound_P(this.targetRocket._audio, 0.3f);
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

	public IEnumerator SendRpc_Init(Vector2 pos)
	{
		yield return new WaitUntil(() => this.m_photonView != null);
		NetworkWriter netWriter = new NetworkWriter();
		this.CompressVector2(netWriter, pos);
		this.m_photonView.RPC("RemoteRocket_Init", RpcTarget.Others, new object[]
		{
			netWriter.AsArray()
		});
		yield break;
	}

	[PunRPC]
	public void RemoteRocket_Init(byte[] byteArray)
	{
		NetworkReader networkReader = new NetworkReader(byteArray);
		this.InitOnlineRocket(this.DecompressVector2(networkReader));
	}

	public void SendRpc_ExplosiveBullet()
	{
		this.m_photonView.RPC("RemoteRocket_ExplosiveBullet", RpcTarget.Others, null);
	}

	[PunRPC]
	public void RemoteRocket_ExplosiveBullet()
	{
		this.targetRocket.duoiTenLua.SetActive(false);
		for (int i = 0; i < this.targetRocket.bullets.Length; i++)
		{
			this.targetRocket.bullets[i].SetActive(false);
		}
		this.targetRocket.ShowEffectExplosiveBullet();
	}

	private PhotonView m_photonView;

	public RocketPlayer targetRocket;

	public bool IsRemote;
}

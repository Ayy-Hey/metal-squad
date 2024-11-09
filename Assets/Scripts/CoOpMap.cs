using System;
using Photon.Pun;
using UnityEngine;

public class CoOpMap : MonoBehaviour
{
	public void CreatOnlineBoss()
	{
		EBoss eboss = this.boss;
		if (eboss != EBoss.Iron_Mech)
		{
			if (eboss != EBoss.Ultron)
			{
				if (eboss == EBoss.Heavy_Mech)
				{
					PhotonNetwork.InstantiateSceneObject("GameObject/Boss5_1_Online", this.tfCreateBoss.position, Quaternion.identity, 0, null);
				}
			}
			else
			{
				PhotonNetwork.InstantiateSceneObject("GameObject/Boss_Ultron_Online", this.tfCreateBoss.position, Quaternion.identity, 0, null);
			}
		}
		else
		{
			PhotonNetwork.InstantiateSceneObject("GameObject/Boss1_6_Online", this.tfCreateBoss.position, Quaternion.identity, 0, null);
		}
	}

	private void OnDestroy()
	{
		try
		{
			if (GameManager.Instance.bossManager.Boss)
			{
				UnityEngine.Object.Destroy(GameManager.Instance.bossManager.Boss.gameObject);
				GameManager.Instance.hudManager.LineBlood.gameObject.SetActive(false);
			}
		}
		catch
		{
		}
	}

	[HideInInspector]
	public bool isInit;

	public float cameraStartSize = 3.6f;

	public Vector4 startBoundary;

	public LayerMask maskGround;

	public EBoss boss;

	public Transform tfCreateBoss;
}

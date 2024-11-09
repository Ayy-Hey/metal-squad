using System;
using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
	private void Start()
	{
		this.Instantiate_NPC();
	}

	private void Instantiate_NPC()
	{
		string path = string.Empty;
		int idcharactor = this.IDCharactor;
		if (idcharactor != 0)
		{
			if (idcharactor != 1)
			{
				if (idcharactor == 2)
				{
					path = "GameObject/Player/PlayerBoy2_Rescue";
				}
			}
			else
			{
				path = "GameObject/Player/PlayerGirl_Rescue";
			}
		}
		else
		{
			path = "GameObject/Player/PlayerBoy1_Rescue";
		}
		this.NPC_Player = (UnityEngine.Object.Instantiate(Resources.Load(path, typeof(PlayerMain)), base.transform.position, Quaternion.identity) as PlayerMain);
		this.NPC_Player.OnInitNPC(this.IDCharactor, this.IDGun1, this.IDKnife, this.IDGrenades, this.CharactorLevel, this.Gun1Level);
	}

	public PlayerMain NPC_Player;

	public int IDCharactor;

	public int CharactorLevel;

	public int IDGun1;

	public int Gun1Level;

	public int IDKnife;

	public int IDGrenades;
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelWorldMap : MonoBehaviour
{
	public void OnShow(GameMode.Mode difficultMode)
	{
		switch (this.mode)
		{
		case LevelMode.NORMAL:
		case LevelMode.BOSS:
			this.amountStarActive = new int[3];
			this.ShowStar(difficultMode);
			this.txtLevel.SetActive(this.mode != LevelMode.BOSS);
			if (this.txtLevel.activeSelf)
			{
				this.txtLevel.GetComponent<Text>().text = string.Concat(new object[]
				{
					string.Empty,
					this.Map + 1,
					"-",
					this.Level + 1
				});
			}
			if (ProfileManager._CampaignProfile.MapsProfile[(int)this.eLevel].GetUnlocked(difficultMode))
			{
				this.SetUnLocked();
			}
			else
			{
				this.SetLocked();
			}
			if (this.img_line != null)
			{
				if (ProfileManager._CampaignProfile.MapsProfile[(int)this.eLevel].GetStar(difficultMode) > 0)
				{
					this.img_line.sprite = this.imgLine;
				}
				else
				{
					this.img_line.sprite = this.imgLineLocked;
				}
			}
			break;
		case LevelMode.ENDLESS:
		case LevelMode.BONUSCOIN:
			this.DisableBonusCoin();
			break;
		case LevelMode.SPECIAL:
			this.amountStarActive = new int[3];
			this.ShowStar(difficultMode);
			this.txtLevel.SetActive(true);
			if (ProfileManager._CampaignProfile.MapsProfile[(int)this.eLevel].GetUnlocked(difficultMode))
			{
				this.SetUnLocked();
			}
			else
			{
				this.SetLocked();
			}
			if (this.img_line != null)
			{
				if (ProfileManager._CampaignProfile.MapsProfile[(int)this.eLevel].GetStar(difficultMode) > 0)
				{
					this.img_line.sprite = this.imgLine;
				}
				else
				{
					this.img_line.sprite = this.imgLineLocked;
				}
			}
			break;
		}
	}

	private void ShowStar(GameMode.Mode difficultMode)
	{
		LevelMode levelMode = this.mode;
		if (levelMode != LevelMode.BOSS && levelMode != LevelMode.NORMAL)
		{
			if (levelMode == LevelMode.SPECIAL)
			{
				if (difficultMode != GameMode.Mode.NORMAL)
				{
					if (difficultMode != GameMode.Mode.HARD)
					{
						if (difficultMode == GameMode.Mode.SUPER_HARD)
						{
							this.amountStarActive[2] = 0;
							for (int i = 0; i < 3; i++)
							{
								if (DataLoader.missionDataRoot_SuperHard_S[this.eLevel - ELevel.LEVEL_S0].missionDataLevel.missionData[i].IsCompleted)
								{
									this.Stars[i].sprite = this.imgStar;
									this.amountStarActive[2]++;
								}
								else
								{
									this.Stars[i].sprite = this.imgStar_Locked;
								}
							}
						}
					}
					else
					{
						this.amountStarActive[1] = 0;
						for (int j = 0; j < 3; j++)
						{
							if (DataLoader.missionDataRoot_Hard_S[this.eLevel - ELevel.LEVEL_S0].missionDataLevel.missionData[j].IsCompleted)
							{
								this.Stars[j].sprite = this.imgStar;
								this.amountStarActive[1]++;
							}
							else
							{
								this.Stars[j].sprite = this.imgStar_Locked;
							}
						}
					}
				}
				else
				{
					this.amountStarActive[0] = 0;
					for (int k = 0; k < 3; k++)
					{
						if (DataLoader.missionDataRoot_Normal_S[this.eLevel - ELevel.LEVEL_S0].missionDataLevel.missionData[k].IsCompleted)
						{
							this.Stars[k].sprite = this.imgStar;
							this.amountStarActive[0]++;
						}
						else
						{
							this.Stars[k].sprite = this.imgStar_Locked;
						}
					}
				}
			}
		}
		else if (difficultMode != GameMode.Mode.NORMAL)
		{
			if (difficultMode != GameMode.Mode.HARD)
			{
				if (difficultMode == GameMode.Mode.SUPER_HARD)
				{
					this.amountStarActive[2] = 0;
					for (int l = 0; l < 3; l++)
					{
						if (DataLoader.missionDataRoot_SuperHard[(int)this.eLevel].missionDataLevel.missionData[l].IsCompleted)
						{
							this.Stars[l].sprite = this.imgStar;
							this.amountStarActive[2]++;
						}
						else
						{
							this.Stars[l].sprite = this.imgStar_Locked;
						}
					}
				}
			}
			else
			{
				this.amountStarActive[1] = 0;
				for (int m = 0; m < 3; m++)
				{
					if (DataLoader.missionDataRoot_Hard[(int)this.eLevel].missionDataLevel.missionData[m].IsCompleted)
					{
						this.Stars[m].sprite = this.imgStar;
						this.amountStarActive[1]++;
					}
					else
					{
						this.Stars[m].sprite = this.imgStar_Locked;
					}
				}
			}
		}
		else
		{
			this.amountStarActive[0] = 0;
			for (int n = 0; n < 3; n++)
			{
				if (DataLoader.missionDataRoot_Normal[(int)this.eLevel].missionDataLevel.missionData[n].IsCompleted)
				{
					this.Stars[n].sprite = this.imgStar;
					this.amountStarActive[0]++;
				}
				else
				{
					this.Stars[n].sprite = this.imgStar_Locked;
				}
			}
		}
	}

	private void SetUnLocked()
	{
		this.levelImage.sprite = this.imgLevelUnlock;
	}

	private void SetLocked()
	{
		this.levelImage.sprite = this.imgLevelLocked;
	}

	private void DisableBonusCoin()
	{
		base.gameObject.SetActive(false);
	}

	public LevelMode mode;

	public ELevel eLevel;

	public int Map;

	public int Level;

	public bool isStartLevelOfMap;

	public int idBoss;

	public Image levelImage;

	public Sprite imgLevelUnlock;

	public Sprite imgLevelLocked;

	public Sprite imgStar;

	[SerializeField]
	private Sprite imgStar_Locked;

	public Image[] Stars;

	public GameObject txtLevel;

	public Sprite imgLine;

	[SerializeField]
	private Sprite imgLineLocked;

	public Image img_line;

	public Transform tran_Line;

	public Transform tran_LevelNext;

	public int[] amountStarActive;
}

using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
	public void ShowCombo(EWeapon lastWeapon)
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (lastWeapon == EWeapon.NONE || lastWeapon == EWeapon.EXCEPTION || lastWeapon == (EWeapon)0)
		{
			return;
		}
		this.obj.SetActive(true);
		if (this.time_combo == 3.40282347E+38f)
		{
			this.time_combo = Time.timeSinceLevelLoad;
		}
		if (Time.timeSinceLevelLoad - this.time_combo <= 3f)
		{
			this.animBlood.Play(0);
			this.animTexture.Play(0);
			this.combo++;
			if (this.combo > GameManager.Instance.mMission.CountCombo)
			{
				GameManager.Instance.mMission.CountCombo = this.combo;
			}
			if (this.combo <= 6)
			{
				if (this.combo > 1)
				{
					GameManager.Instance.audioManager.Combo(this.combo - 2);
				}
				else
				{
					GameManager.Instance.audioManager.Combo(GameManager.Instance.audioManager.combo.Length - 1);
				}
			}
			this.time_combo = Time.timeSinceLevelLoad;
			this.strBuilder.Length = 0;
			this.strBuilder.Append("x ");
			this.strBuilder.Append(this.combo.ToString());
			this.txtCombo.text = this.strBuilder.ToString();
			this.count_time_combo = 0f;
		}
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			return;
		}
		int type = 0;
		int id = 0;
		switch (lastWeapon)
		{
		case EWeapon.SPREAD_GUN:
			type = 1;
			id = 0;
			break;
		case EWeapon.FLAME:
			type = 1;
			id = 1;
			break;
		case EWeapon.THUNDER:
			type = 1;
			id = 2;
			break;
		case EWeapon.LASER:
			type = 1;
			id = 3;
			break;
		case EWeapon.ROCKET:
			type = 1;
			id = 4;
			break;
		default:
			switch (lastWeapon)
			{
			case EWeapon.M4A1:
				type = 0;
				id = 0;
				break;
			case EWeapon.MACHINE:
				type = 0;
				id = 1;
				break;
			case EWeapon.ICE:
				type = 0;
				id = 2;
				break;
			case EWeapon.SNIPER:
				type = 0;
				id = 3;
				break;
			case EWeapon.MGL140:
				type = 0;
				id = 4;
				break;
			default:
				switch (lastWeapon)
				{
				case EWeapon.GRENADE_M61:
					type = 2;
					id = 0;
					break;
				case EWeapon.GRENADE_ICE:
					type = 2;
					id = 1;
					break;
				case EWeapon.GRENADE_MOLOYOV:
					type = 2;
					id = 2;
					break;
				case EWeapon.GRENADE_CHEMICAL:
					type = 2;
					id = 3;
					break;
				default:
					switch (lastWeapon)
					{
					case EWeapon.HUMMER:
						type = 3;
						id = 0;
						break;
					case EWeapon.AXE:
						type = 3;
						id = 1;
						break;
					case EWeapon.SWORD:
						type = 3;
						id = 2;
						break;
					}
					break;
				}
				break;
			}
			break;
		}
		DailyQuestManager.Instance.Combo(this.combo, type, id, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
		{
			if (isCompleted)
			{
				UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
			}
		});
		GameManager.Instance.mMission.AddComboToGun();
		GameManager.Instance.mMission.StartCheck();
	}

	private void Update()
	{
		if (!this.obj.activeSelf)
		{
			return;
		}
		this.count_time_combo += Time.deltaTime;
		if (this.count_time_combo > 3f && !this.txtCombo.text.Equals(string.Empty))
		{
			if (this.coroutine != null)
			{
				base.StopCoroutine(this.coroutine);
			}
			this.obj.SetActive(false);
			this.combo = 0;
			this.time_combo = float.MaxValue;
			this.txtCombo.text = string.Empty;
			return;
		}
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private const float TIME_NEXT_COMBO = 3f;

	private float time_combo = float.MaxValue;

	private float count_time_combo = float.MaxValue;

	private int combo;

	public Text txtCombo;

	public Animator animBlood;

	public Animator animTexture;

	public GameObject obj;

	private StringBuilder strBuilder = new StringBuilder();

	private int idTween = int.MinValue;

	private Coroutine coroutine;
}

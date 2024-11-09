using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerSkill
	{
		public PlayerSkill(MonoBehaviour _MonoBehaviour, PlayerMain player)
		{
			this.player = player;
			this._MonoBehaviour = _MonoBehaviour;
			this.player.isInVisible = false;
			this.IsSwordBooster = (player._PlayerData.IDCharacter == 2);
			this.isInit = true;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.isInit || !this.player.isInVisible)
			{
				return;
			}
			this._time -= deltaTime;
			if (this._time <= 0f)
			{
				if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
				{
					this.player.syncRamboState.SendRpc_OnVisible();
				}
				this.OnVisible();
			}
		}

		public void OnInvisible(float duration)
		{
			this.duration = duration;
			this._time = duration;
			this.player.skeletonAnimation.skeleton.A = 0.3f;
			this.player.isInVisible = true;
		}

		public void OnVisible()
		{
			this._time = 0f;
			this.player.isInVisible = false;
			this.player.skeletonAnimation.skeleton.A = 1f;
		}

		public float OnPassiveDamage(ETypeWeapon type, int id)
		{
			return 0f;
		}

		public float OnPassiveCrit(ETypeWeapon type, int id)
		{
			return 0f;
		}

		public void OnSkillActiveRocket()
		{
			this._MonoBehaviour.StartCoroutine(this.IESkillRocket());
		}

		private IEnumerator IESkillRocket()
		{
			int _length = 2;
			try
			{
				_length += DataLoader.characterData[2].skills[0].ShootingRound[ProfileManager.rambos[2].LevelUpgrade];
			}
			catch
			{
			}
			if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
			{
				_length = 3;
			}
			for (int i = 0; i < _length; i++)
			{
				this._MonoBehaviour.StartCoroutine(this.CreateBomb());
				yield return new WaitForSeconds(1.5f);
			}
			yield break;
		}

		private IEnumerator CreateBomb()
		{
			Vector2 posCamera = CameraController.Instance.Position;
			Vector2 sizeCamera = CameraController.Instance.Size();
			float dts = sizeCamera.x / 3f;
			float y = posCamera.y + sizeCamera.y + 1f;
			int i = 0;
			while ((float)i < 7f)
			{
				float x0 = posCamera.x - sizeCamera.x;
				x0 += (float)i * dts;
				Vector2 pos = new Vector2(x0, y);
				if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
				{
					this.player.syncRamboState.SendRpc_CreateRainBomb(pos);
				}
				GameManager.Instance.skillManager.CreateRainBomb(pos, true);
				yield return new WaitForSeconds(0.2f);
				i++;
			}
			yield break;
		}

		private bool isInit;

		public bool IsSwordBooster;

		private float _time;

		private float duration;

		private MonoBehaviour _MonoBehaviour;

		private PlayerMain player;

		private List<BaseEnemy> target = new List<BaseEnemy>();
	}
}

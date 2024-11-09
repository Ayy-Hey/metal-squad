using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

namespace Metal
{
	public class ObjectMove : MonoBehaviour
	{
		private void OnEnable()
		{
			this.triggerCamera.OnEnteredTrigger = delegate()
			{
				if (this.isRunning)
				{
					return;
				}
				if (this.objNextRevival != null)
				{
					this.objNextRevival.SetActive(false);
				}
				GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
				foreach (GameObject gameObject in this.objectsMove)
				{
					float x = gameObject.transform.position.x;
					gameObject.transform.position += Vector3.right * 12.8f * this.offsetDistance;
					gameObject.SetActive(true);
					LeanTween.moveX(gameObject, x, this.duration).setOnComplete(delegate()
					{
						if (this.objNextRevival != null)
						{
							this.objNextRevival.SetActive(true);
						}
					});
				}
				GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
				GameManager.Instance.hudManager.HideControl();
				GameManager.Instance.player.EMovement = BaseCharactor.EMovementBasic.Release;
				if (this.camera2DCinematics)
				{
					GameManager.Instance.StateManager.SetPreview();
					this.camera2DCinematics.Play();
				}
				this.isRunning = true;
			};
		}

		public void Reset()
		{
			GameManager.Instance.StateManager.EState = EGamePlay.RUNNING;
			GameManager.Instance.hudManager.ShowControl(1.1f);
			GameManager.Instance.player._PlayerSpine.OnIdle(true);
		}

		[SerializeField]
		private TriggerCamera triggerCamera;

		[SerializeField]
		private GameObject[] objectsMove;

		[SerializeField]
		private ProCamera2DCinematics camera2DCinematics;

		private bool isRunning;

		[SerializeField]
		private float offsetDistance = 2f;

		private float duration = 5f;

		[SerializeField]
		private GameObject objNextRevival;
	}
}

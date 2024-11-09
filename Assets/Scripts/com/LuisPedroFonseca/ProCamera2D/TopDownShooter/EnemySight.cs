using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class EnemySight : MonoBehaviour
	{
		private void Awake()
		{
			this.RefreshRate += UnityEngine.Random.Range(-this.RefreshRate * 0.2f, this.RefreshRate * 0.2f);
		}

		private IEnumerator Start()
		{
			for (;;)
			{
				Vector3 direction = this.player.position - base.transform.position;
				float angle = Vector3.Angle(direction, base.transform.forward);
				if (angle < this.fieldOfViewAngle * 0.5f && Physics.Raycast(base.transform.position + base.transform.up, direction.normalized, out this._hit, this.ViewDistance, this.LayerMask) && this._hit.collider.transform.GetInstanceID() == this.player.GetInstanceID())
				{
					if (!this.playerInSight)
					{
						this.playerInSight = true;
						if (this.OnPlayerInSight != null)
						{
							this.OnPlayerInSight(this._hit.collider.transform);
						}
					}
				}
				else if (this.playerInSight)
				{
					this.playerInSight = false;
					if (this.OnPlayerOutOfSight != null)
					{
						this.OnPlayerOutOfSight();
					}
				}
				yield return new WaitForSeconds(this.RefreshRate);
			}
			yield break;
		}

		public Action<Transform> OnPlayerInSight;

		public Action OnPlayerOutOfSight;

		public float RefreshRate = 1f;

		public float fieldOfViewAngle = 110f;

		public float ViewDistance = 30f;

		public bool playerInSight;

		public Transform player;

		public LayerMask LayerMask;

		private RaycastHit _hit;
	}
}

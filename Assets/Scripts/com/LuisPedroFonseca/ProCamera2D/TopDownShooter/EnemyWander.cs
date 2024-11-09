using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class EnemyWander : MonoBehaviour
	{
		private void Awake()
		{
			this._navMeshAgent = base.GetComponentInChildren<NavMeshAgent>();
			this._startingPos = this._navMeshAgent.transform.position;
		}

		public void StartWandering()
		{
			this._startingTime = Time.time;
			this.GoToWaypoint();
			base.StartCoroutine(this.CheckAgentPosition());
		}

		public void StopWandering()
		{
			base.StopAllCoroutines();
		}

		private IEnumerator CheckAgentPosition()
		{
			for (;;)
			{
				if (this._navMeshAgent.remainingDistance <= this.WaypointOffset && !this._hasReachedDestination)
				{
					this._hasReachedDestination = true;
					if (Time.time - this._startingTime >= this.WanderDuration && this.WanderDuration > 0f)
					{
						UnityEngine.Debug.Log("Stopped wandering");
					}
					else
					{
						this.GoToWaypoint();
					}
				}
				yield return null;
			}
			yield break;
		}

		private void GoToWaypoint()
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			Vector3 vector = Vector3.zero;
			while (navMeshPath.status == NavMeshPathStatus.PathPartial || navMeshPath.status == NavMeshPathStatus.PathInvalid)
			{
				Vector3 b = UnityEngine.Random.insideUnitSphere * this.WanderRadius;
				b.y = this._startingPos.y;
				vector = this._startingPos + b;
				this._navMeshAgent.CalculatePath(vector, navMeshPath);
			}
			this._navMeshAgent.SetDestination(vector);
			this._hasReachedDestination = false;
		}

		public float WanderDuration = 10f;

		public float WaypointOffset = 0.1f;

		public float WanderRadius = 10f;

		private NavMeshAgent _navMeshAgent;

		private bool _hasReachedDestination;

		private Vector3 _startingPos;

		private float _startingTime;
	}
}

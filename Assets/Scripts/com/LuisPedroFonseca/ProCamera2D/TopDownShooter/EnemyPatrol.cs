using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class EnemyPatrol : MonoBehaviour
	{
		private void Awake()
		{
			this._navMeshAgent = base.GetComponentInChildren<NavMeshAgent>();
			this._path = new List<Transform>();
			if (this.PathContainer != null)
			{
				IEnumerator enumerator = this.PathContainer.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform item = (Transform)obj;
						this._path.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("No path set.");
			}
		}

		private void Update()
		{
			if (this.IsPaused)
			{
				return;
			}
			if (this._navMeshAgent.remainingDistance <= this.WaypointOffset && !this._hasReachedDestination)
			{
				this._hasReachedDestination = true;
				PatrolWaypoint component = this._path[this._currentWaypoint].GetComponent<PatrolWaypoint>();
				if (component != null)
				{
					this._stopTime = UnityEngine.Random.Range(component.StopDuration - component.StopDurationVariation, component.StopDuration + component.StopDurationVariation);
					if (UnityEngine.Random.value >= component.StopProbability)
					{
						this.GoToNextWaypoint();
					}
				}
				else
				{
					this.GoToNextWaypoint();
				}
			}
			if (this._hasReachedDestination)
			{
				this._stopTime -= Time.deltaTime;
				if (this._stopTime <= 0f)
				{
					this.GoToNextWaypoint();
				}
			}
		}

		public void StartPatrol()
		{
			this.GoToWaypoint(0);
		}

		public void PausePatrol()
		{
			this.IsPaused = true;
			this._navMeshAgent.isStopped = true;
		}

		public void ResumePatrol()
		{
			this.GoToWaypoint(this._currentWaypoint);
		}

		private void GoToNextWaypoint()
		{
			if (this._currentWaypoint < this._path.Count - 1)
			{
				this._currentWaypoint++;
			}
			else if (this.Loop)
			{
				this._currentWaypoint = 0;
			}
			else
			{
				UnityEngine.Debug.Log("Path completed");
			}
			this.GoToWaypoint(this._currentWaypoint);
		}

		private void GoToWaypoint(int waypoint)
		{
			this.IsPaused = false;
			this._hasReachedDestination = false;
			this._currentWaypoint = waypoint;
			Vector3 destination = new Vector3(this._path[this._currentWaypoint].position.x, this._navMeshAgent.transform.position.y, this._path[this._currentWaypoint].position.z);
			this._navMeshAgent.SetDestination(destination);
		}

		public Transform PathContainer;

		public float WaypointOffset = 0.1f;

		public bool Loop = true;

		public bool IsPaused;

		private NavMeshAgent _navMeshAgent;

		private List<Transform> _path;

		private int _currentWaypoint;

		private bool _hasReachedDestination;

		private float _stopTime;
	}
}

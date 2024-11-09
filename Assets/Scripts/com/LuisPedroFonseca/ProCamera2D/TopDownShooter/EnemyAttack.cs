using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class EnemyAttack : MonoBehaviour
	{
		private void Awake()
		{
			this._transform = base.transform;
			this._navMeshAgent = base.GetComponentInChildren<NavMeshAgent>();
		}

		public void Attack(Transform target)
		{
			this._navMeshAgent.updateRotation = false;
			this._target = target;
			this._hasTarget = true;
			base.StartCoroutine(this.LookAtTarget());
			base.StartCoroutine(this.FollowTarget());
			base.StartCoroutine(this.Fire());
		}

		public void StopAttack()
		{
			this._navMeshAgent.updateRotation = true;
			this._hasTarget = false;
		}

		private IEnumerator LookAtTarget()
		{
			while (this._hasTarget)
			{
				Vector3 lookAtPos = new Vector3(this._target.position.x, this._transform.position.y, this._target.position.z);
				Vector3 diff = lookAtPos - this._transform.position;
				Quaternion newRotation = Quaternion.LookRotation(diff, Vector3.up);
				this._transform.rotation = Quaternion.Slerp(this._transform.rotation, newRotation, this.RotationSpeed * Time.deltaTime);
				yield return null;
			}
			yield break;
		}

		private IEnumerator FollowTarget()
		{
			while (this._hasTarget)
			{
				Vector2 rnd = UnityEngine.Random.insideUnitCircle;
				this._navMeshAgent.destination = this._target.position - (this._target.position - this._transform.position).normalized * 5f + new Vector3(rnd.x, 0f, rnd.y);
				yield return null;
			}
			yield break;
		}

		private IEnumerator Fire()
		{
			while (this._hasTarget)
			{
				GameObject bullet = this.BulletPool.nextThing;
				bullet.transform.position = this.WeaponTip.position;
				bullet.transform.rotation = this._transform.rotation * Quaternion.Euler(new Vector3(0f, -90f + UnityEngine.Random.Range(-this.FireAngleRandomness, this.FireAngleRandomness), 0f));
				yield return new WaitForSeconds(this.FireRate);
			}
			yield break;
		}

		public static Vector2 RandomOnUnitCircle2(float radius)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			insideUnitCircle.Normalize();
			return insideUnitCircle * radius;
		}

		public float RotationSpeed = 2f;

		public Pool BulletPool;

		public Transform WeaponTip;

		public float FireRate = 0.3f;

		public float FireAngleRandomness = 10f;

		private bool _hasTarget;

		private Transform _target;

		private NavMeshAgent _navMeshAgent;

		private Transform _transform;
	}
}

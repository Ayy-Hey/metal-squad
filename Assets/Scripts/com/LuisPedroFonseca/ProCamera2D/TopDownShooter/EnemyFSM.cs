using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	[RequireComponent(typeof(EnemySight))]
	[RequireComponent(typeof(EnemyWander))]
	[RequireComponent(typeof(EnemyAttack))]
	public class EnemyFSM : MonoBehaviour
	{
		private void Awake()
		{
			this._sight = base.GetComponent<EnemySight>();
			this._attack = base.GetComponent<EnemyAttack>();
			this._wander = base.GetComponent<EnemyWander>();
			this._renderers = base.GetComponentsInChildren<Renderer>();
			this._originalColor = this._renderers[0].material.color;
			this._currentColor = this._originalColor;
			EnemySight sight = this._sight;
			sight.OnPlayerInSight = (Action<Transform>)Delegate.Combine(sight.OnPlayerInSight, new Action<Transform>(this.OnPlayerInSight));
			EnemySight sight2 = this._sight;
			sight2.OnPlayerOutOfSight = (Action)Delegate.Combine(sight2.OnPlayerOutOfSight, new Action(this.OnPlayerOutOfSight));
			if (this.Key != null)
			{
				this.Key.gameObject.SetActive(false);
			}
		}

		private void Start()
		{
			this._wander.StartWandering();
		}

		private void OnDestroy()
		{
			EnemySight sight = this._sight;
			sight.OnPlayerInSight = (Action<Transform>)Delegate.Remove(sight.OnPlayerInSight, new Action<Transform>(this.OnPlayerInSight));
			EnemySight sight2 = this._sight;
			sight2.OnPlayerOutOfSight = (Action)Delegate.Remove(sight2.OnPlayerOutOfSight, new Action(this.OnPlayerOutOfSight));
		}

		private void Hit(int damage)
		{
			if (this.Health <= 0)
			{
				return;
			}
			this.Health -= damage;
			base.StartCoroutine(this.HitAnim());
			if (this.Health <= 0)
			{
				this.Die();
			}
		}

		private IEnumerator HitAnim()
		{
			this.Colorize(Color.white);
			yield return new WaitForSeconds(0.05f);
			this.Colorize(this._currentColor);
			yield break;
		}

		private void OnPlayerInSight(Transform obj)
		{
			this._wander.StopWandering();
			this._attack.Attack(obj);
			ProCamera2D.Instance.AddCameraTarget(base.transform, 1f, 1f, 0f, default(Vector2));
			this._currentColor = this.AttackColor;
			this.Colorize(this._currentColor);
		}

		private void OnPlayerOutOfSight()
		{
			this._wander.StartWandering();
			this._attack.StopAttack();
			ProCamera2D.Instance.RemoveCameraTarget(base.transform, 2f);
			this._currentColor = this._originalColor;
			this.Colorize(this._currentColor);
		}

		private void Colorize(Color color)
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].material.color = color;
			}
		}

		private void DropLoot()
		{
			if (this.Key != null)
			{
				this.Key.gameObject.SetActive(true);
				this.Key.transform.position = base.transform.position + new Vector3(0f, 3f, 0f);
			}
		}

		private void Die()
		{
			ProCamera2DShake.Instance.Shake("SmallExplosion");
			this.OnPlayerOutOfSight();
			this.DropLoot();
			UnityEngine.Object.Destroy(base.gameObject, 0.2f);
		}

		public int Health = 100;

		public Color AttackColor = Color.red;

		public DoorKey Key;

		private EnemySight _sight;

		private EnemyAttack _attack;

		private EnemyWander _wander;

		private Renderer[] _renderers;

		private Color _originalColor;

		private Color _currentColor;
	}
}

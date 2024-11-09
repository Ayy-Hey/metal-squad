using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class Door : MonoBehaviour
	{
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		private void Awake()
		{
			this._origPos = base.transform.position;
		}

		public void OpenDoor(float openDelay = -1f)
		{
			if (openDelay == -1f)
			{
				openDelay = this.OpenDelay;
			}
			this._isOpen = true;
			switch (this.DoorDirection)
			{
			case DoorDirection.Left:
				this.Move(this._origPos - new Vector3(this.MovementRange, 0f, 0f), this.AnimDuration, openDelay);
				break;
			case DoorDirection.Right:
				this.Move(this._origPos + new Vector3(this.MovementRange, 0f, 0f), this.AnimDuration, openDelay);
				break;
			case DoorDirection.Up:
				this.Move(this._origPos + new Vector3(0f, 0f, this.MovementRange), this.AnimDuration, openDelay);
				break;
			case DoorDirection.Down:
				this.Move(this._origPos - new Vector3(0f, 0f, this.MovementRange), this.AnimDuration, openDelay);
				break;
			}
		}

		public void CloseDoor()
		{
			this._isOpen = false;
			this.Move(this._origPos, this.AnimDuration, this.CloseDelay);
		}

		private void Move(Vector3 newPos, float duration, float delay)
		{
			if (this._moveCoroutine != null)
			{
				base.StopCoroutine(this._moveCoroutine);
			}
			this._moveCoroutine = base.StartCoroutine(this.MoveRoutine(newPos, duration, delay));
		}

		private IEnumerator MoveRoutine(Vector3 newPos, float duration, float delay)
		{
			yield return new WaitForSeconds(delay);
			Vector3 origPos = base.transform.position;
			float t = 0f;
			while (t <= 1f)
			{
				t += Time.deltaTime / duration;
				base.transform.position = new Vector3(Utils.EaseFromTo(origPos.x, newPos.x, t, EaseType.EaseOut), Utils.EaseFromTo(origPos.y, newPos.y, t, EaseType.EaseOut), Utils.EaseFromTo(origPos.z, newPos.z, t, EaseType.EaseOut));
				yield return null;
			}
			yield break;
		}

		private bool _isOpen;

		public DoorDirection DoorDirection;

		public float MovementRange = 5f;

		public float AnimDuration = 1f;

		public float OpenDelay;

		public float CloseDelay;

		private Vector3 _origPos;

		private Coroutine _moveCoroutine;
	}
}

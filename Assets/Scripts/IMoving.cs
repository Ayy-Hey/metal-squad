using System;
using UnityEngine;

public interface IMoving
{
	void UnFreeze();

	void Freeze(Vector3 pos, float speed, float time_end_slow, float speed_down);

	void FreezeNoSlow(float timeFreeze);

	void UnFreezeNoSlow();
}

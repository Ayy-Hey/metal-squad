using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class PoolMember : MonoBehaviour
	{
		private void OnDisable()
		{
			this.pool.nextThing = base.gameObject;
		}

		public Pool pool;
	}
}

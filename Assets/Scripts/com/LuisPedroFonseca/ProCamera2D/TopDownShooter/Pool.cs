using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class Pool : MonoBehaviour
	{
		public GameObject nextThing
		{
			get
			{
				if (this.things.Count < 1)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.thing);
					gameObject.transform.parent = base.transform;
					gameObject.SetActive(false);
					this.things.Add(gameObject);
					PoolMember poolMember = gameObject.AddComponent<PoolMember>();
					poolMember.pool = this;
				}
				GameObject gameObject2 = this.things[0];
				this.things.RemoveAt(0);
				gameObject2.SetActive(true);
				return gameObject2;
			}
			set
			{
				value.SetActive(false);
				this.things.Add(value);
			}
		}

		public GameObject thing;

		private List<GameObject> things = new List<GameObject>();
	}
}

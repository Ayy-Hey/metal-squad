using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreateLevel
{
	[ExecuteInEditMode]
	public class CreateLevelLevel : MonoBehaviour
	{
		public void GetPointPos()
		{
			this.level = new level();
			this.level.points = new List<CheckPoint>();
			this.level.isReaded = this.isReaded;
			for (int i = 0; i < this.points.Count; i++)
			{
				this.points[i].GetPos();
				this.level.points.Add(this.points[i].point);
			}
		}

		public void AddPoint(CreateLevelPoint point)
		{
			if (!this.points.Contains(point))
			{
				this.points.Add(point);
			}
		}

		public void RemovePoint(CreateLevelPoint point)
		{
			if (this.points.Contains(point))
			{
				this.points.Remove(point);
			}
		}

		public bool isReaded = true;

		public List<CreateLevelPoint> points;

		public level level;
	}
}

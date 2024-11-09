using System;
using com.dev.util.SecurityHelper;
using UnityEngine;

namespace StarMission
{
	public class MissionData
	{
		public string desc { get; set; }

		public int idDesc { get; set; }

		public string[] valueDesc { get; set; }

		public int type { get; set; }

		public string require { get; set; }

		public int gold { get; set; }

		public int Exp { get; set; }

		public int MS { get; set; }

		public void InitData(int number, string mode)
		{
			this.completed = new BoolProfileData("com.metal.squad.MissionData" + number + mode, false);
			this.gold_security = new SecuredInt((int)((double)this.gold * RemoteConfigFirebase.Instance.GetDoubleValue(RemoteConfigFirebase.DROP_GOLD_MISSION, 0.5)));
			this.exp_security = new SecuredInt(this.Exp);
			this.gem_security = new SecuredInt(Mathf.Min(this.MS, 2));
		}

		public bool IsCompleted
		{
			get
			{
				return this.completed.Data;
			}
			set
			{
				this.completed.setValue(value);
			}
		}

		private BoolProfileData completed;

		public SecuredInt gold_security;

		public SecuredInt exp_security;

		public SecuredInt gem_security;
	}
}

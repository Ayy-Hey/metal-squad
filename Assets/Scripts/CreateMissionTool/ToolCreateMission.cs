using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CreateMissionTool
{
	public class ToolCreateMission : MonoBehaviour
	{
		private void OnValidate()
		{
			this.missionRoot.missionDataLevel.Level = (int)this.level;
			if (this.write)
			{
				this.write = false;
				string contents = JsonConvert.SerializeObject(this.missionRoot);
				GameMode.Mode mode = this.mode;
				if (mode != GameMode.Mode.NORMAL)
				{
					if (mode != GameMode.Mode.HARD)
					{
						if (mode == GameMode.Mode.SUPER_HARD)
						{
							File.WriteAllText(string.Concat(new object[]
							{
								Application.dataPath,
								"/Resources/Mission/SuperHard/Decrypt/Level",
								this.missionRoot.missionDataLevel.Level,
								".txt"
							}), contents);
						}
					}
					else
					{
						File.WriteAllText(string.Concat(new object[]
						{
							Application.dataPath,
							"/Resources/Mission/Hard/Decrypt/Level",
							this.missionRoot.missionDataLevel.Level,
							".txt"
						}), contents);
					}
				}
				else
				{
					File.WriteAllText(string.Concat(new object[]
					{
						Application.dataPath,
						"/Resources/Mission/Normal/Decrypt/Level",
						this.missionRoot.missionDataLevel.Level,
						".txt"
					}), contents);
				}
			}
			if (this.read)
			{
				this.read = false;
				string path = string.Empty;
				GameMode.Mode mode2 = this.mode;
				if (mode2 != GameMode.Mode.NORMAL)
				{
					if (mode2 != GameMode.Mode.HARD)
					{
						if (mode2 == GameMode.Mode.SUPER_HARD)
						{
							path = "Mission/SuperHard/Decrypt/Level" + this.missionRoot.missionDataLevel.Level;
						}
					}
					else
					{
						path = "Mission/Hard/Decrypt/Level" + this.missionRoot.missionDataLevel.Level;
					}
				}
				else
				{
					path = "Mission/Normal/Decrypt/Level" + this.missionRoot.missionDataLevel.Level;
				}
				TextAsset textAsset = Resources.Load<TextAsset>(path);
				if (textAsset)
				{
					this.missionRoot = JsonConvert.DeserializeObject<MissionRoot>(textAsset.text);
				}
			}
		}

		public GameMode.Mode mode;

		public ELevel level;

		public MissionRoot missionRoot;

		public bool write;

		public bool read;
	}
}

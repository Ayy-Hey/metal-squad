using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace RankInfoTool
{
	public class CreateRankInfoTool : MonoBehaviour
	{
		private void OnValidate()
		{
			this.rankInfor.Level = this.Level;
			if (this.save)
			{
				this.save = false;
				string contents = JsonConvert.SerializeObject(this.rankInfor);
				File.WriteAllText(string.Concat(new object[]
				{
					Application.dataPath,
					"/Resources/Rank/Decrypt/Rank",
					this.Level,
					".txt"
				}), contents);
			}
			if (this.load)
			{
				this.load = false;
				TextAsset textAsset = Resources.Load<TextAsset>("Rank/Decrypt/Rank" + this.Level);
				this.rankInfor = JsonConvert.DeserializeObject<RankInfor>(textAsset.text);
			}
			if (this.getNameNExp)
			{
				this.getNameNExp = false;
				TextAsset textAsset2 = Resources.Load<TextAsset>("Rank/Decrypt/Rank" + this.Level);
				this.rankOld = JsonConvert.DeserializeObject<RankInfor0>(textAsset2.text);
				this.rankInfor.Name = this.rankOld.Name;
				this.rankInfor.Exp = this.rankOld.Exp;
				this.rankInfor.Gold = (this.rankInfor.Gem = (this.rankInfor.Gacha_A = (this.rankInfor.Gacha_B = (this.rankInfor.Gacha_C = (this.rankInfor.Lucky_A = (this.rankInfor.Lucky_B = (this.rankInfor.Lucky_C = 0)))))));
			}
		}

		[Header("Rank level:")]
		public int Level;

		[Header("Save rank infor:")]
		public bool save;

		[Header("Load rank infor:")]
		public bool load;

		[Header("Get name and exp:")]
		public bool getNameNExp;

		[Header("Info rank:")]
		public RankInfor rankInfor;

		private RankInfor0 rankOld;
	}
}

using System;
using System.Collections.Generic;

namespace ABI
{
	[Serializable]
	public class GetRankResult
	{
		public List<RankData> ranks { get; set; }
	}
}

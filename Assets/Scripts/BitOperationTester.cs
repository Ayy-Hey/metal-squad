using System;
using System.Collections.Generic;
using UnityEngine;

public class BitOperationTester : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log(BitOperationUtils.GetBitSize(-0.85f, 100));
		UnityEngine.Debug.Log(BitOperationUtils.GetStringRepresentation(-1));
		List<BitOperationTester.Data> list = new List<BitOperationTester.Data>();
		for (int i = 0; i < this.numberTest; i++)
		{
			BitOperationTester.Data data = new BitOperationTester.Data();
			list.Add(data);
			UnityEngine.Debug.Log("dkm" + data.ToString());
		}
		DataCompactor dataCompactor = new DataCompactor();
		for (int j = 0; j < list.Count; j++)
		{
			BitOperationTester.DataType dataType = list[j].dataType;
			if (dataType != BitOperationTester.DataType.INT)
			{
				if (dataType != BitOperationTester.DataType.FLOAT)
				{
					if (dataType == BitOperationTester.DataType.BOOL)
					{
						dataCompactor.PutBool(list[j].boolData);
					}
				}
				else
				{
					dataCompactor.PutFloat(list[j].floatData, list[j].bitSize);
				}
			}
			else
			{
				dataCompactor.PutInt(list[j].intData, list[j].bitSize);
			}
		}
		string text = dataCompactor.CompactData();
		UnityEngine.Debug.Log("____" + text);
		DataDecompactor dataDecompactor = new DataDecompactor();
		dataDecompactor.DecompactData(text);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"1: ",
			list.Count,
			"___ 2",
			dataDecompactor.data.Count
		}));
		for (int k = 0; k < list.Count; k++)
		{
			BitOperationTester.DataType dataType2 = list[k].dataType;
			if (dataType2 != BitOperationTester.DataType.INT)
			{
				if (dataType2 != BitOperationTester.DataType.FLOAT)
				{
					if (dataType2 == BitOperationTester.DataType.BOOL)
					{
						bool @bool = dataDecompactor.GetBool();
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Wrong Bool: ",
							@bool,
							" --- ",
							list[k].boolData
						}));
						if (@bool != list[k].boolData)
						{
							UnityEngine.Debug.Log(string.Concat(new object[]
							{
								"Wrong Bool: ",
								@bool,
								" --- ",
								list[k].boolData
							}));
						}
					}
				}
				else
				{
					float @float = dataDecompactor.GetFloat(list[k].bitSize);
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Wrong Float: ",
						@float,
						" --- ",
						list[k].floatData
					}));
					if (@float != list[k].floatData)
					{
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Wrong Float: ",
							@float,
							" --- ",
							list[k].floatData
						}));
					}
				}
			}
			else
			{
				int @int = dataDecompactor.GetInt(list[k].bitSize);
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Wrong Int: ",
					@int,
					" --- ",
					list[k].intData
				}));
				if (@int != list[k].intData)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Wrong Int: ",
						@int,
						" --- ",
						list[k].intData
					}));
				}
			}
		}
	}

	public int numberTest = 100;

	public enum DataType
	{
		INT,
		FLOAT,
		BOOL
	}

	private class Data
	{
		public Data()
		{
			this.dataType = (BitOperationTester.DataType)UnityEngine.Random.Range(0, 3);
			BitOperationTester.DataType dataType = this.dataType;
			if (dataType != BitOperationTester.DataType.INT)
			{
				if (dataType != BitOperationTester.DataType.FLOAT)
				{
					if (dataType == BitOperationTester.DataType.BOOL)
					{
						if (UnityEngine.Random.Range(0, 2) == 0)
						{
							this.boolData = false;
						}
						else
						{
							this.boolData = true;
						}
					}
				}
				else
				{
					this.floatData = (float)Math.Round((double)UnityEngine.Random.Range(-100000f, 100000f), 2);
					this.bitSize = BitOperationUtils.GetBitSize(this.floatData, 100);
				}
			}
			else
			{
				this.intData = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				this.bitSize = BitOperationUtils.GetBitSize(this.intData);
			}
		}

		public string ToString()
		{
			string result = string.Empty;
			BitOperationTester.DataType dataType = this.dataType;
			if (dataType != BitOperationTester.DataType.INT)
			{
				if (dataType != BitOperationTester.DataType.FLOAT)
				{
					if (dataType == BitOperationTester.DataType.BOOL)
					{
						result = this.dataType.ToString() + ":" + this.boolData.ToString();
					}
				}
				else
				{
					result = this.dataType.ToString() + ":" + this.floatData.ToString();
				}
			}
			else
			{
				result = this.dataType.ToString() + ":" + this.intData;
			}
			return result;
		}

		public BitOperationTester.DataType dataType;

		public float floatData;

		public int intData;

		public bool boolData;

		public int bitSize;
	}
}

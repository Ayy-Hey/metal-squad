using System;
using System.Collections.Generic;
using UnityEngine;

public class DataDecompactor
{
	public DataDecompactor()
	{
		this.data = new List<int>();
		this.data.Add(0);
		this.digitIndex = 0;
		this.numberIndex = 0;
	}

	public DataDecompactor(List<int> value)
	{
		this.data = value;
		this.digitIndex = 0;
		this.numberIndex = 0;
	}

	public float GetFloat(int bitSize)
	{
		int @int = this.GetInt(bitSize);
		return (float)@int / 100f;
	}

	public float GetFloat(int bitSize, int precision)
	{
		int @int = this.GetInt(bitSize);
		return (float)@int / Mathf.Pow(10f, (float)precision);
	}

	public int GetInt(int bitSize)
	{
		int num = 0;
		if (this.digitIndex + bitSize < 32)
		{
			num = BitOperationUtils.GetPart(this.data[this.numberIndex], this.digitIndex, this.digitIndex + bitSize) >> this.digitIndex;
			this.digitIndex += bitSize;
		}
		else
		{
			int num2 = 32 - this.digitIndex;
			int num3 = bitSize - num2;
			int part = BitOperationUtils.GetPart(this.data[this.numberIndex], this.digitIndex, 32);
			this.numberIndex++;
			if (this.numberIndex >= this.data.Count)
			{
				this.data.Add(0);
			}
			int part2 = BitOperationUtils.GetPart(this.data[this.numberIndex], 0, num3);
			num = BitOperationUtils.CopyBit(part, num, this.digitIndex, 32, 0);
			num = BitOperationUtils.CopyBit(part2, num, 0, num3, num2);
			this.digitIndex = num3;
		}
		if (!this.GetBool())
		{
			num = -num;
		}
		return num;
	}

	public bool GetBool()
	{
		bool result = BitOperationUtils.IsOn(this.data[this.numberIndex], this.digitIndex);
		this.digitIndex++;
		if (this.digitIndex > 31)
		{
			this.digitIndex = 32 - this.digitIndex;
			this.numberIndex++;
			if (this.numberIndex >= this.data.Count)
			{
				this.data.Add(0);
			}
		}
		return result;
	}

	public void DecompactData(string textData)
	{
		int i = 0;
		int num = 0;
		while (i < textData.Length)
		{
			int num2 = num;
			num += 6;
			int fromData = Number64Set.ConvertToNumber(textData[i]);
			if (num < 32)
			{
				this.data[this.numberIndex] = BitOperationUtils.CopyBit(fromData, this.data[this.numberIndex], 0, 6, num2);
			}
			else
			{
				int num3 = 32 - num2;
				num -= 32;
				this.data[this.numberIndex] = BitOperationUtils.CopyBit(fromData, this.data[this.numberIndex], 0, num3, num2);
				this.numberIndex++;
				if (this.numberIndex >= this.data.Count)
				{
					this.data.Add(0);
				}
				int part = BitOperationUtils.GetPart(fromData, num3, 6);
				this.data[this.numberIndex] = BitOperationUtils.CopyBit(part, this.data[this.numberIndex], num3, 6, 0);
			}
			i++;
		}
		this.numberIndex = 0;
	}

	public void Clear()
	{
		this.data.Clear();
		this.data.Add(0);
		this.digitIndex = 0;
		this.numberIndex = 0;
	}

	public void PrintData()
	{
		UnityEngine.Debug.Log("Decompact Data:");
		for (int i = 0; i < this.data.Count; i++)
		{
			UnityEngine.Debug.Log(BitOperationUtils.GetStringRepresentation(this.data[i]));
		}
	}

	public void PrintNumber64Data()
	{
		UnityEngine.Debug.Log("Print Number 64 Data:");
		List<int> number64IntSet = Number64Set.GetNumber64IntSet(this.data);
		for (int i = 0; i < number64IntSet.Count; i++)
		{
			UnityEngine.Debug.Log(number64IntSet[i]);
		}
	}

	private const float DEFAULT_PRECISION = 100f;

	public List<int> data;

	private int digitIndex;

	private int numberIndex;
}

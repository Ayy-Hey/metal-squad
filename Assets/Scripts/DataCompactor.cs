using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DataCompactor
{
	public DataCompactor()
	{
		this.data = new List<int>();
		this.data.Add(0);
		this.digitIndex = 0;
	}

	public void PutFloat(float value, int bitSize)
	{
		int value2 = (int)Mathf.Round(value * 100f);
		this.PutInt(value2, bitSize);
	}

	public void PutFloat(float value, int bitSize, int precision)
	{
		int value2 = (int)Mathf.Round(value * Mathf.Pow(10f, (float)precision));
		this.PutInt(value2, bitSize);
	}

	public void PutInt(int value, int bitSize)
	{
		int num = Mathf.Abs(value);
		int part = BitOperationUtils.GetPart(num, 0, bitSize);
		if (this.digitIndex + bitSize < 32)
		{
			this.data[this.data.Count - 1] = BitOperationUtils.CopyBit(part, this.data[this.data.Count - 1], 0, bitSize, this.digitIndex);
			this.digitIndex += bitSize;
		}
		else
		{
			int num2 = 32 - this.digitIndex;
			int num3 = bitSize - num2;
			int part2 = BitOperationUtils.GetPart(part, 0, num2);
			int part3 = BitOperationUtils.GetPart(part, num2, bitSize);
			this.data[this.data.Count - 1] = BitOperationUtils.CopyBit(part2, this.data[this.data.Count - 1], 0, num2, this.digitIndex);
			this.data.Add(0);
			this.data[this.data.Count - 1] = BitOperationUtils.CopyBit(part3, this.data[this.data.Count - 1], num2, bitSize, 0);
			this.digitIndex = num3;
		}
		this.PutBool(value > 0);
	}

	public void PutBool(bool value)
	{
		if (value)
		{
			this.data[this.data.Count - 1] = BitOperationUtils.TurnOn(this.data[this.data.Count - 1], this.digitIndex);
		}
		else
		{
			this.data[this.data.Count - 1] = BitOperationUtils.TurnOff(this.data[this.data.Count - 1], this.digitIndex);
		}
		this.digitIndex++;
		if (this.digitIndex > 31)
		{
			this.data.Add(0);
			this.digitIndex = 32 - this.digitIndex;
		}
	}

	public string CompactData()
	{
		for (int i = this.data.Count - 1; i > -1; i--)
		{
			if (this.data[i] != 0)
			{
				break;
			}
			this.data.RemoveAt(i);
		}
		List<int> number64IntSet = Number64Set.GetNumber64IntSet(this.data);
		StringBuilder stringBuilder = new StringBuilder(string.Empty);
		for (int j = 0; j < number64IntSet.Count; j++)
		{
			stringBuilder.Append(Number64Set.ConvertToCharacter(number64IntSet[j]));
		}
		return stringBuilder.ToString();
	}

	public void Clear()
	{
		this.data.Clear();
		this.data.Add(0);
		this.digitIndex = 0;
	}

	public void PrintData()
	{
		UnityEngine.Debug.Log("Compact Data:");
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
}

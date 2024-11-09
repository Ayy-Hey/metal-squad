using System;
using UnityEngine;

public class BitOperationUtils
{
	public static int TurnOn(int data, int bit)
	{
		return data | 1 << bit;
	}

	public static int TurnOff(int data, int bit)
	{
		return data & ~(1 << bit);
	}

	public static bool IsOn(int data, int bit)
	{
		return (data >> bit & 1) != 0;
	}

	public static int CopyBit(int fromData, int toData, int startFrom, int endFrom, int startTo)
	{
		int num = 0;
		for (int i = startFrom; i < endFrom; i++)
		{
			if (BitOperationUtils.IsOn(fromData, i))
			{
				toData = BitOperationUtils.TurnOn(toData, num + startTo);
			}
			num++;
		}
		return toData;
	}

	public static int GetPart(int data, int startIndex, int endIndex)
	{
		int num = 0;
		for (int i = startIndex; i < endIndex; i++)
		{
			if (BitOperationUtils.IsOn(data, i))
			{
				num = BitOperationUtils.TurnOn(num, i);
			}
		}
		return num;
	}

	public static int GetBitSize(float data, int precision)
	{
		int data2 = (int)Mathf.Round(data * (float)precision);
		return BitOperationUtils.GetBitSize(data2);
	}

	public static int GetBitSize(int data)
	{
		int result = 31;
		int data2 = Mathf.Abs(data);
		for (int i = 31; i > -1; i--)
		{
			if (BitOperationUtils.IsOn(data2, i))
			{
				result = i + 1;
				break;
			}
		}
		return result;
	}

	public static string GetStringRepresentation(int data)
	{
		char[] array = new char[32];
		int num = 31;
		for (int i = 0; i < 32; i++)
		{
			if ((data & 1 << i) != 0)
			{
				array[num] = '1';
			}
			else
			{
				array[num] = '0';
			}
			num--;
		}
		return new string(array);
	}
}

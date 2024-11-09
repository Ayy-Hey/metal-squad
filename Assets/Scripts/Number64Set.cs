using System;
using System.Collections.Generic;

public class Number64Set
{
	public static int ConvertToNumber(char value)
	{
		int result;
		if (value == '-')
		{
			result = 10;
		}
		else if (value < ':')
		{
			result = (int)(value - '0');
		}
		else if (value == '_')
		{
			result = 11;
		}
		else if (value < '[')
		{
			result = (int)(value - '5');
		}
		else
		{
			result = (int)(value - ';');
		}
		return result;
	}

	public static char ConvertToCharacter(int value)
	{
		char result;
		if (value < 10)
		{
			result = value.ToString()[0];
		}
		else if (value == 10)
		{
			result = '-';
		}
		else if (value == 11)
		{
			result = '_';
		}
		else if (value < 38)
		{
			result = (char)(value + 53);
		}
		else
		{
			result = (char)(value + 59);
		}
		return result;
	}

	public static List<int> GetNumber64IntSet(List<int> data)
	{
		List<int> list = new List<int>();
		int i = 0;
		int num = 0;
		while (i < data.Count)
		{
			int num2 = 0;
			int num3 = num;
			num += 6;
			if (num < 32)
			{
				num2 = BitOperationUtils.GetPart(data[i], num3, num);
				num2 >>= num3;
			}
			else
			{
				i++;
				if (i < data.Count)
				{
					num -= 32;
					int part = BitOperationUtils.GetPart(data[i - 1], num3, 32);
					int num4 = BitOperationUtils.CopyBit(data[i - 1], part, num3, 32, 0);
					int num5 = BitOperationUtils.GetPart(data[i], 0, num) << 32 - num3;
					num2 = BitOperationUtils.CopyBit(num4 | num5, num2, 0, 6, 0);
				}
				else
				{
					num2 = BitOperationUtils.GetPart(data[i - 1], num3, 32) >> num3;
				}
			}
			list.Add(num2);
		}
		for (int j = list.Count - 1; j > -1; j--)
		{
			if (list[j] != 0)
			{
				break;
			}
			list.RemoveAt(j);
		}
		return list;
	}
}

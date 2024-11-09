using System;
using System.Runtime.InteropServices;

namespace System
{
	public static class HalfHelper
	{
		private static uint FloatToUInt(float v)
		{
			HalfHelper.floatToIntConverter.FloatValue = v;
			return HalfHelper.floatToIntConverter.UIntValue;
		}

		private static float UIntToFloat(uint v)
		{
			HalfHelper.floatToIntConverter.UIntValue = v;
			return HalfHelper.floatToIntConverter.FloatValue;
		}

		private static uint ConvertMantissa(int i)
		{
			uint num = (uint)((uint)i << 13);
			uint num2 = 0u;
			while ((num & 8388608u) == 0u)
			{
				num2 -= 8388608u;
				num <<= 1;
			}
			num &= 4286578687u;
			num2 += 947912704u;
			return num | num2;
		}

		private static uint[] GenerateMantissaTable()
		{
			uint[] array = new uint[2048];
			array[0] = 0u;
			for (int i = 1; i < 1024; i++)
			{
				array[i] = HalfHelper.ConvertMantissa(i);
			}
			for (int j = 1024; j < 2048; j++)
			{
				array[j] = (uint)(939524096 + (j - 1024 << 13));
			}
			return array;
		}

		private static uint[] GenerateExponentTable()
		{
			uint[] array = new uint[64];
			array[0] = 0u;
			for (int i = 1; i < 31; i++)
			{
				array[i] = (uint)((uint)i << 23);
			}
			array[31] = 1199570944u;
			array[32] = 2147483648u;
			for (int j = 33; j < 63; j++)
			{
				array[j] = (uint)((ulong)0x80000000 + (ulong)((long)((long)(j - 32) << 23)));
			}
			array[63] = 3347054592u;
			return array;
		}

		private static ushort[] GenerateOffsetTable()
		{
			ushort[] array = new ushort[64];
			array[0] = 0;
			for (int i = 1; i < 32; i++)
			{
				array[i] = 1024;
			}
			array[32] = 0;
			for (int j = 33; j < 64; j++)
			{
				array[j] = 1024;
			}
			return array;
		}

		private static ushort[] GenerateBaseTable()
		{
			ushort[] array = new ushort[512];
			for (int i = 0; i < 256; i++)
			{
				sbyte b = (sbyte)(127 - i);
				if ((int)b > 24)
				{
					array[i | 0] = 0;
					array[i | 256] = 32768;
				}
				else if ((int)b > 14)
				{
					array[i | 0] = (ushort)(1024 >> 18 + (int)b);
					array[i | 256] = (ushort)(1024 >> 18 + (int)b | 32768);
				}
				else if ((int)b >= -15)
				{
					array[i | 0] = (ushort)(15 - (int)b << 10);
					array[i | 256] = (ushort)(15 - (int)b << 10 | 32768);
				}
				else if ((int)b > -128)
				{
					array[i | 0] = 31744;
					array[i | 256] = 64512;
				}
				else
				{
					array[i | 0] = 31744;
					array[i | 256] = 64512;
				}
			}
			return array;
		}

		private static sbyte[] GenerateShiftTable()
		{
			sbyte[] array = new sbyte[512];
			for (int i = 0; i < 256; i++)
			{
				sbyte b = (sbyte)(127 - i);
				if ((int)b > 24)
				{
					array[i | 0] = 24;
					array[i | 256] = 24;
				}
				else if ((int)b > 14)
				{
					array[i | 0] = (sbyte)((int)b - 1);
					array[i | 256] = (sbyte)((int)b - 1);
				}
				else if ((int)b >= -15)
				{
					array[i | 0] = 13;
					array[i | 256] = 13;
				}
				else if ((int)b > -128)
				{
					array[i | 0] = 24;
					array[i | 256] = 24;
				}
				else
				{
					array[i | 0] = 13;
					array[i | 256] = 13;
				}
			}
			return array;
		}

		public static float HalfToSingle(Half half)
		{
			uint v = HalfHelper.mantissaTable[(int)(HalfHelper.offsetTable[half.internalValue >> 10] + (half.internalValue & 1023))] + HalfHelper.exponentTable[half.internalValue >> 10];
			return HalfHelper.UIntToFloat(v);
		}

		public static Half SingleToHalf(float single)
		{
			uint num = HalfHelper.FloatToUInt(single);
			ushort bits = (ushort)((uint)HalfHelper.baseTable[(int)((UIntPtr)(num >> 23 & 511u))] + ((num & 8388607u) >> (int)HalfHelper.shiftTable[(int)((UIntPtr)(num >> 23))]));
			return Half.ToHalf(bits);
		}

		public static float Decompress(ushort compressedFloat)
		{
			uint v = HalfHelper.mantissaTable[(int)(HalfHelper.offsetTable[compressedFloat >> 10] + (compressedFloat & 1023))] + HalfHelper.exponentTable[compressedFloat >> 10];
			return HalfHelper.UIntToFloat(v);
		}

		public static ushort Compress(float uncompressedFloat)
		{
			uint num = HalfHelper.FloatToUInt(uncompressedFloat);
			return (ushort)((uint)HalfHelper.baseTable[(int)((UIntPtr)(num >> 23 & 511u))] + ((num & 8388607u) >> (int)HalfHelper.shiftTable[(int)((UIntPtr)(num >> 23))]));
		}

		public static Half Negate(Half half)
		{
			return Half.ToHalf((ushort)(half.internalValue ^ 32768));
		}

		public static Half Abs(Half half)
		{
			return Half.ToHalf((ushort)(half.internalValue & 32767));
		}

		public static bool IsNaN(Half half)
		{
			return (half.internalValue & 32767) > 31744;
		}

		public static bool IsInfinity(Half half)
		{
			return (half.internalValue & 32767) == 31744;
		}

		public static bool IsPositiveInfinity(Half half)
		{
			return half.internalValue == 31744;
		}

		public static bool IsNegativeInfinity(Half half)
		{
			return half.internalValue == 64512;
		}

		private static uint[] mantissaTable = HalfHelper.GenerateMantissaTable();

		private static uint[] exponentTable = HalfHelper.GenerateExponentTable();

		private static ushort[] offsetTable = HalfHelper.GenerateOffsetTable();

		private static ushort[] baseTable = HalfHelper.GenerateBaseTable();

		private static sbyte[] shiftTable = HalfHelper.GenerateShiftTable();

		private static HalfHelper.UIntFloat floatToIntConverter = new HalfHelper.UIntFloat
		{
			FloatValue = 0f
		};

		[StructLayout(LayoutKind.Explicit)]
		private struct UIntFloat
		{
			[FieldOffset(0)]
			public uint UIntValue;

			[FieldOffset(0)]
			public float FloatValue;
		}
	}
}

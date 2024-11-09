using System;

namespace com.dev.util.SecurityHelper
{
	public sealed class MD5Helper
	{
		private MD5Helper()
		{
		}

		public static uint RotateLeft(uint uiNumber, ushort shift)
		{
			return uiNumber >> (int)(32 - shift) | uiNumber << (int)shift;
		}

		public static uint ReverseByte(uint uiNumber)
		{
			return (uiNumber & 255u) << 24 | uiNumber >> 24 | (uiNumber & 16711680u) >> 8 | (uiNumber & 65280u) << 8;
		}
	}
}

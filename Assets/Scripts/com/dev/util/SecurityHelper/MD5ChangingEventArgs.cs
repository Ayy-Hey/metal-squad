using System;

namespace com.dev.util.SecurityHelper
{
	public class MD5ChangingEventArgs : EventArgs
	{
		public MD5ChangingEventArgs(byte[] data)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = data[i];
			}
		}

		public MD5ChangingEventArgs(string data)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = (byte)data[i];
			}
		}

		public readonly byte[] NewData;
	}
}

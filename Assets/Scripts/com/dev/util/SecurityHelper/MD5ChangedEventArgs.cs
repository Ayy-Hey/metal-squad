using System;

namespace com.dev.util.SecurityHelper
{
	public class MD5ChangedEventArgs : EventArgs
	{
		public MD5ChangedEventArgs(byte[] data, string HashedValue)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = data[i];
			}
			this.FingerPrint = HashedValue;
		}

		public MD5ChangedEventArgs(string data, string HashedValue)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = (byte)data[i];
			}
			this.FingerPrint = HashedValue;
		}

		public readonly byte[] NewData;

		public readonly string FingerPrint;
	}
}

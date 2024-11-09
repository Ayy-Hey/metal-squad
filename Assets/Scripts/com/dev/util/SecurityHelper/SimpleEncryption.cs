using System;
using System.Text;

namespace com.dev.util.SecurityHelper
{
	public class SimpleEncryption
	{
		public SimpleEncryption(string pass, int key, DataEncryption.ENCRYPTION_MODE mode)
		{
			this.password = pass;
			this.xorKey = key;
			this.encryptionMode = mode;
			this.delimiters = new char[]
			{
				';',
				','
			};
			this.delimitersString = new string(this.delimiters);
		}

		public string encrypt(string plain)
		{
			if (plain != null && plain.Length > 0)
			{
				int[] array = new int[plain.Length];
				for (int i = 0; i < plain.Length; i++)
				{
					array[i] = ((int)plain[i] ^ this.xorKey);
				}
				int num = 0;
				int num2 = array[0];
				for (int i = 1; i < array.Length; i++)
				{
					if (array[i] < num2)
					{
						num = i;
						num2 = array[i];
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (i != num)
					{
						array[i] -= num2;
					}
				}
				StringBuilder stringBuilder = new StringBuilder(DataEncryption.hash(plain + this.password + this.xorKey, this.encryptionMode));
				stringBuilder.Append(this.delimitersString);
				stringBuilder.Append(num + 2);
				stringBuilder.Append(this.delimitersString);
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i]);
					stringBuilder.Append(this.delimitersString);
				}
				return stringBuilder.ToString();
			}
			return plain;
		}

		public string decrypt(string encrypted)
		{
			if (encrypted != null && encrypted.Length > 0)
			{
				try
				{
					string[] array = encrypted.Split(this.delimiters, StringSplitOptions.RemoveEmptyEntries);
					string b = array[0];
					int num = int.Parse(array[1]);
					int num2 = int.Parse(array[num]);
					int[] array2 = new int[array.Length - 2];
					for (int i = 2; i < array.Length; i++)
					{
						if (i != num)
						{
							array2[i - 2] = int.Parse(array[i]) + num2;
						}
						else
						{
							array2[i - 2] = num2;
						}
					}
					StringBuilder stringBuilder = new StringBuilder(string.Empty);
					for (int i = 0; i < array2.Length; i++)
					{
						stringBuilder.Append((char)(array2[i] ^ this.xorKey));
					}
					if (DataEncryption.hash(stringBuilder.ToString() + this.password + this.xorKey, this.encryptionMode) == b)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				catch
				{
					return null;
				}
				return encrypted;
			}
			return encrypted;
		}

		private DataEncryption.ENCRYPTION_MODE encryptionMode;

		private int xorKey = 9573485;

		private string password;

		private char[] delimiters;

		private string delimitersString;
	}
}

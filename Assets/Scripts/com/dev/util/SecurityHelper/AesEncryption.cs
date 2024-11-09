using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace com.dev.util.SecurityHelper
{
	public class AesEncryption
	{
		public AesEncryption(string password, string saltKey, DataEncryption.ENCRYPTION_MODE mode)
		{
			int iterations = 10;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(saltKey), iterations);
			this.aes = new AesManaged();
			this.aes.Key = rfc2898DeriveBytes.GetBytes(32);
			this.aes.IV = rfc2898DeriveBytes.GetBytes(16);
			this.encryptionMode = mode;
		}

		public string encrypt(string plain)
		{
			if (plain != null && plain.Length > 0)
			{
				try
				{
					string str = string.Empty;
					string str2 = DataEncryption.hash(plain, this.encryptionMode);
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.aes.CreateEncryptor(), CryptoStreamMode.Write))
						{
							byte[] bytes = Encoding.UTF8.GetBytes(plain);
							cryptoStream.Write(bytes, 0, bytes.Length);
							cryptoStream.FlushFinalBlock();
							str = Convert.ToBase64String(memoryStream.ToArray());
						}
					}
					return str + ";" + str2;
				}
				catch
				{
					return null;
				}
				return plain;
			}
			return plain;
		}

		public string decrypt2(string encrypted)
		{
			if (encrypted != null && encrypted.Length > 0)
			{
				try
				{
					string result = string.Empty;
					int num = encrypted.IndexOf(';');
					string text = encrypted.Substring(num + 1);
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.aes.CreateDecryptor(), CryptoStreamMode.Write))
						{
							byte[] array = Convert.FromBase64String(encrypted.Substring(0, num));
							cryptoStream.Write(array, 0, array.Length);
							cryptoStream.FlushFinalBlock();
							byte[] array2 = memoryStream.ToArray();
							result = Encoding.UTF8.GetString(array2, 0, array2.Length);
						}
					}
					return result;
				}
				catch
				{
					return null;
				}
				return encrypted;
			}
			return encrypted;
		}

		public string decrypt(string encrypted)
		{
			if (encrypted != null && encrypted.Length > 0)
			{
				try
				{
					string text = string.Empty;
					int num = encrypted.IndexOf(';');
					string a = encrypted.Substring(num + 1);
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.aes.CreateDecryptor(), CryptoStreamMode.Write))
						{
							byte[] array = Convert.FromBase64String(encrypted.Substring(0, num));
							cryptoStream.Write(array, 0, array.Length);
							cryptoStream.FlushFinalBlock();
							byte[] array2 = memoryStream.ToArray();
							text = Encoding.UTF8.GetString(array2, 0, array2.Length);
						}
					}
					string b = DataEncryption.hash(text, this.encryptionMode);
					if (a == b)
					{
						return text;
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

		private AesManaged aes;
	}
}

using System;
using System.Security.Cryptography;
using System.Text;

namespace com.dev.util.SecurityHelper
{
	public class HashHelper
	{
		public static string md5(string password)
		{
			HashHelper.md5Helper.Value = password;
			return HashHelper.md5Helper.FingerPrint.ToLower();
		}

		public static string sha256(string password)
		{
			SHA256Managed sha256Managed = new SHA256Managed();
			byte[] array = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString(HashHelper.stringFormat));
			}
			return stringBuilder.ToString().ToLower();
		}

		private static MD5 md5Helper = new MD5();

		private static readonly string stringFormat = "x2";
	}
}

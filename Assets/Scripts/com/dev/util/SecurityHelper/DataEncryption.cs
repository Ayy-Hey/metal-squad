using System;

namespace com.dev.util.SecurityHelper
{
	public class DataEncryption
	{
		public DataEncryption(string password, string saltKey, int xorKey, DataEncryption.ENCRYPTION_MODE mode)
		{
			switch (this.encryptionMode)
			{
			case DataEncryption.ENCRYPTION_MODE.MODE_1_MAXIMIZE_SPEED:
				this.simpleEncryption = new SimpleEncryption(password, xorKey, mode);
				break;
			case DataEncryption.ENCRYPTION_MODE.MODE_2_BALANCE:
				this.aesEncryption = new AesEncryption(password, saltKey, mode);
				break;
			case DataEncryption.ENCRYPTION_MODE.MODE_3_MAXIMIZE_SECURITY:
				this.aesEncryption = new AesEncryption(password, saltKey, mode);
				break;
			}
			this.encryptionMode = mode;
		}

		public string encrypt(string plain)
		{
			switch (this.encryptionMode)
			{
			case DataEncryption.ENCRYPTION_MODE.MODE_0_NO_ENCRYPTION:
				return plain;
			case DataEncryption.ENCRYPTION_MODE.MODE_1_MAXIMIZE_SPEED:
				return this.simpleEncryption.encrypt(plain);
			case DataEncryption.ENCRYPTION_MODE.MODE_2_BALANCE:
				return this.aesEncryption.encrypt(plain);
			case DataEncryption.ENCRYPTION_MODE.MODE_3_MAXIMIZE_SECURITY:
				return this.aesEncryption.encrypt(plain);
			default:
				return this.simpleEncryption.encrypt(plain);
			}
		}

		public string decrypt(string encrypted)
		{
			switch (this.encryptionMode)
			{
			case DataEncryption.ENCRYPTION_MODE.MODE_0_NO_ENCRYPTION:
				return encrypted;
			case DataEncryption.ENCRYPTION_MODE.MODE_1_MAXIMIZE_SPEED:
				return this.simpleEncryption.decrypt(encrypted);
			case DataEncryption.ENCRYPTION_MODE.MODE_2_BALANCE:
				return this.aesEncryption.decrypt(encrypted);
			case DataEncryption.ENCRYPTION_MODE.MODE_3_MAXIMIZE_SECURITY:
				return this.aesEncryption.decrypt(encrypted);
			default:
				return this.simpleEncryption.decrypt(encrypted);
			}
		}

		public string decrypt2(string encrypted)
		{
			return this.aesEncryption.decrypt2(encrypted);
		}

		public static string hash(string plain, DataEncryption.ENCRYPTION_MODE encryptionMode)
		{
			switch (encryptionMode)
			{
			case DataEncryption.ENCRYPTION_MODE.MODE_0_NO_ENCRYPTION:
				return string.Empty;
			case DataEncryption.ENCRYPTION_MODE.MODE_1_MAXIMIZE_SPEED:
				return HashHelper.md5(plain);
			case DataEncryption.ENCRYPTION_MODE.MODE_2_BALANCE:
				return HashHelper.md5(plain);
			case DataEncryption.ENCRYPTION_MODE.MODE_3_MAXIMIZE_SECURITY:
				return HashHelper.sha256(plain);
			default:
				return string.Empty;
			}
		}

		private DataEncryption.ENCRYPTION_MODE encryptionMode = DataEncryption.ENCRYPTION_MODE.MODE_2_BALANCE;

		private AesEncryption aesEncryption;

		private SimpleEncryption simpleEncryption;

		public enum ENCRYPTION_MODE
		{
			MODE_0_NO_ENCRYPTION,
			MODE_1_MAXIMIZE_SPEED,
			MODE_2_BALANCE,
			MODE_3_MAXIMIZE_SECURITY
		}
	}
}

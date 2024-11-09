using System;
using UnityEngine;

namespace com.dev.util.SecurityHelper
{
	public class SecuredLong
	{
		public SecuredLong(long value)
		{
			this.Value = value;
		}

		public SecuredLong()
		{
		}

		public long Value
		{
			get
			{
				return this.internalValue ^ SecuredLong.XOR_KEY;
			}
			set
			{
				this.internalValue = (value ^ SecuredLong.XOR_KEY);
			}
		}

		public static implicit operator long(SecuredLong c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredLong c)
		{
			return c.Value.ToString();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}

		private static long XOR_KEY = (long)UnityEngine.Random.Range(-1000000000, 1000000000);

		private long internalValue;
	}
}

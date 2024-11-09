using System;
using UnityEngine;

namespace com.dev.util.SecurityHelper
{
	public class SecuredInt
	{
		public SecuredInt(int value)
		{
			this.Value = value;
		}

		public SecuredInt()
		{
		}

		public int Value
		{
			get
			{
				return this.internalValue ^ SecuredInt.XOR_KEY;
			}
			set
			{
				this.internalValue = (value ^ SecuredInt.XOR_KEY);
			}
		}

		public static implicit operator int(SecuredInt c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredInt c)
		{
			return c.Value.ToString();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}

		private static int XOR_KEY = UnityEngine.Random.Range(-1000000000, 1000000000);

		private int internalValue;
	}
}

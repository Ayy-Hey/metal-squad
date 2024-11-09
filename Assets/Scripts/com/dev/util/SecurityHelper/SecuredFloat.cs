using System;
using UnityEngine;

namespace com.dev.util.SecurityHelper
{
	public class SecuredFloat
	{
		public SecuredFloat(float value)
		{
			this.Value = value;
		}

		public SecuredFloat()
		{
		}

		public float Value
		{
			get
			{
				return TypeConverter.IntToFloat(this.internalValue ^ SecuredFloat.XOR_KEY);
			}
			set
			{
				this.internalValue = (TypeConverter.FloatToInt(value) ^ SecuredFloat.XOR_KEY);
			}
		}

		public static implicit operator float(SecuredFloat c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredFloat c)
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

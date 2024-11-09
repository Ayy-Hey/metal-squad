using System;
using System.Diagnostics;

namespace com.dev.util.SecurityHelper
{
	public class MD5
	{
		public MD5()
		{
			this.Value = string.Empty;
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event MD5.ValueChanging OnValueChanging;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event MD5.ValueChanged OnValueChanged;

		public string Value
		{
			get
			{
				char[] array = new char[this.m_byteInput.Length];
				for (int i = 0; i < this.m_byteInput.Length; i++)
				{
					array[i] = (char)this.m_byteInput[i];
				}
				return new string(array);
			}
			set
			{
				if (this.OnValueChanging != null)
				{
					this.OnValueChanging(this, new MD5ChangingEventArgs(value));
				}
				this.m_byteInput = new byte[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					this.m_byteInput[i] = (byte)value[i];
				}
				this.dgFingerPrint = this.CalculateMD5Value();
				if (this.OnValueChanged != null)
				{
					this.OnValueChanged(this, new MD5ChangedEventArgs(value, this.dgFingerPrint.ToString()));
				}
			}
		}

		public byte[] ValueAsByte
		{
			get
			{
				byte[] array = new byte[this.m_byteInput.Length];
				for (int i = 0; i < this.m_byteInput.Length; i++)
				{
					array[i] = this.m_byteInput[i];
				}
				return array;
			}
			set
			{
				if (this.OnValueChanging != null)
				{
					this.OnValueChanging(this, new MD5ChangingEventArgs(value));
				}
				this.m_byteInput = new byte[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					this.m_byteInput[i] = value[i];
				}
				this.dgFingerPrint = this.CalculateMD5Value();
				if (this.OnValueChanged != null)
				{
					this.OnValueChanged(this, new MD5ChangedEventArgs(value, this.dgFingerPrint.ToString()));
				}
			}
		}

		public string FingerPrint
		{
			get
			{
				return this.dgFingerPrint.ToString();
			}
		}

		protected Digest CalculateMD5Value()
		{
			Digest digest = new Digest();
			byte[] array = this.CreatePaddedBuffer();
			uint num = (uint)(array.Length * 8 / 32);
			for (uint num2 = 0u; num2 < num / 16u; num2 += 1u)
			{
				this.CopyBlock(array, num2);
				this.PerformTransformation(ref digest.A, ref digest.B, ref digest.C, ref digest.D);
			}
			return digest;
		}

		protected void TransF(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + MD5Helper.RotateLeft(a + ((b & c) | (~b & d)) + this.X[(int)((UIntPtr)k)] + MD5.T[(int)((UIntPtr)(i - 1u))], s);
		}

		protected void TransG(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + MD5Helper.RotateLeft(a + ((b & d) | (c & ~d)) + this.X[(int)((UIntPtr)k)] + MD5.T[(int)((UIntPtr)(i - 1u))], s);
		}

		protected void TransH(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + MD5Helper.RotateLeft(a + (b ^ c ^ d) + this.X[(int)((UIntPtr)k)] + MD5.T[(int)((UIntPtr)(i - 1u))], s);
		}

		protected void TransI(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + MD5Helper.RotateLeft(a + (c ^ (b | ~d)) + this.X[(int)((UIntPtr)k)] + MD5.T[(int)((UIntPtr)(i - 1u))], s);
		}

		protected void PerformTransformation(ref uint A, ref uint B, ref uint C, ref uint D)
		{
			uint num = A;
			uint num2 = B;
			uint num3 = C;
			uint num4 = D;
			this.TransF(ref A, B, C, D, 0u, 7, 1u);
			this.TransF(ref D, A, B, C, 1u, 12, 2u);
			this.TransF(ref C, D, A, B, 2u, 17, 3u);
			this.TransF(ref B, C, D, A, 3u, 22, 4u);
			this.TransF(ref A, B, C, D, 4u, 7, 5u);
			this.TransF(ref D, A, B, C, 5u, 12, 6u);
			this.TransF(ref C, D, A, B, 6u, 17, 7u);
			this.TransF(ref B, C, D, A, 7u, 22, 8u);
			this.TransF(ref A, B, C, D, 8u, 7, 9u);
			this.TransF(ref D, A, B, C, 9u, 12, 10u);
			this.TransF(ref C, D, A, B, 10u, 17, 11u);
			this.TransF(ref B, C, D, A, 11u, 22, 12u);
			this.TransF(ref A, B, C, D, 12u, 7, 13u);
			this.TransF(ref D, A, B, C, 13u, 12, 14u);
			this.TransF(ref C, D, A, B, 14u, 17, 15u);
			this.TransF(ref B, C, D, A, 15u, 22, 16u);
			this.TransG(ref A, B, C, D, 1u, 5, 17u);
			this.TransG(ref D, A, B, C, 6u, 9, 18u);
			this.TransG(ref C, D, A, B, 11u, 14, 19u);
			this.TransG(ref B, C, D, A, 0u, 20, 20u);
			this.TransG(ref A, B, C, D, 5u, 5, 21u);
			this.TransG(ref D, A, B, C, 10u, 9, 22u);
			this.TransG(ref C, D, A, B, 15u, 14, 23u);
			this.TransG(ref B, C, D, A, 4u, 20, 24u);
			this.TransG(ref A, B, C, D, 9u, 5, 25u);
			this.TransG(ref D, A, B, C, 14u, 9, 26u);
			this.TransG(ref C, D, A, B, 3u, 14, 27u);
			this.TransG(ref B, C, D, A, 8u, 20, 28u);
			this.TransG(ref A, B, C, D, 13u, 5, 29u);
			this.TransG(ref D, A, B, C, 2u, 9, 30u);
			this.TransG(ref C, D, A, B, 7u, 14, 31u);
			this.TransG(ref B, C, D, A, 12u, 20, 32u);
			this.TransH(ref A, B, C, D, 5u, 4, 33u);
			this.TransH(ref D, A, B, C, 8u, 11, 34u);
			this.TransH(ref C, D, A, B, 11u, 16, 35u);
			this.TransH(ref B, C, D, A, 14u, 23, 36u);
			this.TransH(ref A, B, C, D, 1u, 4, 37u);
			this.TransH(ref D, A, B, C, 4u, 11, 38u);
			this.TransH(ref C, D, A, B, 7u, 16, 39u);
			this.TransH(ref B, C, D, A, 10u, 23, 40u);
			this.TransH(ref A, B, C, D, 13u, 4, 41u);
			this.TransH(ref D, A, B, C, 0u, 11, 42u);
			this.TransH(ref C, D, A, B, 3u, 16, 43u);
			this.TransH(ref B, C, D, A, 6u, 23, 44u);
			this.TransH(ref A, B, C, D, 9u, 4, 45u);
			this.TransH(ref D, A, B, C, 12u, 11, 46u);
			this.TransH(ref C, D, A, B, 15u, 16, 47u);
			this.TransH(ref B, C, D, A, 2u, 23, 48u);
			this.TransI(ref A, B, C, D, 0u, 6, 49u);
			this.TransI(ref D, A, B, C, 7u, 10, 50u);
			this.TransI(ref C, D, A, B, 14u, 15, 51u);
			this.TransI(ref B, C, D, A, 5u, 21, 52u);
			this.TransI(ref A, B, C, D, 12u, 6, 53u);
			this.TransI(ref D, A, B, C, 3u, 10, 54u);
			this.TransI(ref C, D, A, B, 10u, 15, 55u);
			this.TransI(ref B, C, D, A, 1u, 21, 56u);
			this.TransI(ref A, B, C, D, 8u, 6, 57u);
			this.TransI(ref D, A, B, C, 15u, 10, 58u);
			this.TransI(ref C, D, A, B, 6u, 15, 59u);
			this.TransI(ref B, C, D, A, 13u, 21, 60u);
			this.TransI(ref A, B, C, D, 4u, 6, 61u);
			this.TransI(ref D, A, B, C, 11u, 10, 62u);
			this.TransI(ref C, D, A, B, 2u, 15, 63u);
			this.TransI(ref B, C, D, A, 9u, 21, 64u);
			A += num;
			B += num2;
			C += num3;
			D += num4;
		}

		protected byte[] CreatePaddedBuffer()
		{
			int num = 448 - this.m_byteInput.Length * 8 % 512;
			uint num2 = (uint)((num + 512) % 512);
			if (num2 == 0u)
			{
				num2 = 512u;
			}
			uint num3 = (uint)((long)this.m_byteInput.Length + (long)((ulong)(num2 / 8u)) + 8L);
			ulong num4 = (ulong)((long)this.m_byteInput.Length * 8L);
			byte[] array = new byte[num3];
			for (int i = 0; i < this.m_byteInput.Length; i++)
			{
				array[i] = this.m_byteInput[i];
			}
			byte[] array2 = array;
			int num5 = this.m_byteInput.Length;
			array2[num5] |= 128;
			for (int j = 8; j > 0; j--)
			{
				array[(int)(checked((IntPtr)(unchecked((ulong)num3 - (ulong)((long)j)))))] = (byte)(num4 >> (8 - j) * 8 & 255UL);
			}
			return array;
		}

		protected void CopyBlock(byte[] bMsg, uint block)
		{
			block <<= 6;
			for (uint num = 0u; num < 61u; num += 4u)
			{
				this.X[(int)((UIntPtr)(num >> 2))] = (uint)((int)bMsg[(int)((UIntPtr)(block + (num + 3u)))] << 24 | (int)bMsg[(int)((UIntPtr)(block + (num + 2u)))] << 16 | (int)bMsg[(int)((UIntPtr)(block + (num + 1u)))] << 8 | (int)bMsg[(int)((UIntPtr)(block + num))]);
			}
		}

		protected static readonly uint[] T = new uint[]
		{
			3614090360u,
			3905402710u,
			606105819u,
			3250441966u,
			4118548399u,
			1200080426u,
			2821735955u,
			4249261313u,
			1770035416u,
			2336552879u,
			4294925233u,
			2304563134u,
			1804603682u,
			4254626195u,
			2792965006u,
			1236535329u,
			4129170786u,
			3225465664u,
			643717713u,
			3921069994u,
			3593408605u,
			38016083u,
			3634488961u,
			3889429448u,
			568446438u,
			3275163606u,
			4107603335u,
			1163531501u,
			2850285829u,
			4243563512u,
			1735328473u,
			2368359562u,
			4294588738u,
			2272392833u,
			1839030562u,
			4259657740u,
			2763975236u,
			1272893353u,
			4139469664u,
			3200236656u,
			681279174u,
			3936430074u,
			3572445317u,
			76029189u,
			3654602809u,
			3873151461u,
			530742520u,
			3299628645u,
			4096336452u,
			1126891415u,
			2878612391u,
			4237533241u,
			1700485571u,
			2399980690u,
			4293915773u,
			2240044497u,
			1873313359u,
			4264355552u,
			2734768916u,
			1309151649u,
			4149444226u,
			3174756917u,
			718787259u,
			3951481745u
		};

		protected uint[] X = new uint[16];

		protected Digest dgFingerPrint;

		protected byte[] m_byteInput;

		public delegate void ValueChanging(object sender, MD5ChangingEventArgs Changing);

		public delegate void ValueChanged(object sender, MD5ChangedEventArgs Changed);
	}
}

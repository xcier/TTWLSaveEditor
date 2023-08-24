using System;
namespace IOTools.Algorithms
{
	public class Adler32
	{
		public Adler32()
		{
			this.a = 1U;
		}
		public byte[] GetHashBytes
		{
			get
			{
				return Converters.UInt32ToBytes(this.GetHashUInt32, true);
			}
		}
		public uint GetHashUInt32
		{
			get
			{
				return this.b << 16 | this.a;
			}
		}
		public void TransformBlock(byte[] Buffer, uint Index, uint Length)
		{
			checked
			{
				uint num = (uint)(unchecked((ulong)Length) - 1UL);
				for (uint num2 = Index; num2 <= num; num2 += 1U)
				{
					this.a = (uint)(unchecked((ulong)(checked(this.a + (uint)Buffer[(int)num2]))) % 65521UL);
					this.b = (uint)(unchecked((ulong)(checked(this.b + this.a))) % 65521UL);
				}
			}
		}
		public void TransformBlock(byte[] Buffer, uint Length)
		{
			this.TransformBlock(Buffer, 0U, Length);
		}
		public void TransformBlock(byte[] Buffer)
		{
			this.TransformBlock(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public uint TransformFinalBlock(byte[] Buffer, uint Index, uint Length)
		{
			this.TransformBlock(Buffer, Index, Length);
			return this.GetHashUInt32;
		}
		public uint TransformFinalBlock(byte[] Buffer, uint Length)
		{
			return this.TransformFinalBlock(Buffer, 0U, Length);
		}
		public uint TransformFinalBlock(byte[] Buffer)
		{
			return this.TransformFinalBlock(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public byte[] Compute(byte[] Buffer, uint Index, uint Length)
		{
			this.TransformBlock(Buffer, Index, Length);
			return this.GetHashBytes;
		}
		public byte[] Compute(byte[] Buffer, uint Length)
		{
			return this.Compute(Buffer, 0U, Length);
		}
		public byte[] Compute(byte[] Buffer)
		{
			return this.Compute(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public void Clear()
		{
			this.a = 1U;
			this.b = 0U;
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		private uint a;
		private uint b;
	}
}

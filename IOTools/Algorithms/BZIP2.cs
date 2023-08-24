using System;
namespace IOTools.Algorithms
{
	public class BZIP2
	{
		public BZIP2()
		{
			this.Table = null;
			this.Poly = 79764919U;
			this.InitVal = uint.MaxValue;
			this.Hash = 0U;
			this.Hash = this.InitVal;
			this.GenerateTable();
		}
		public BZIP2(uint Polynomial, uint InitialValue)
		{
			this.Table = null;
			this.Poly = 79764919U;
			this.InitVal = uint.MaxValue;
			this.Hash = 0U;
			this.Polynomial = Polynomial;
			this.InitialValue = InitialValue;
			this.GenerateTable();
		}
		public uint Polynomial
		{
			get
			{
				return this.Poly;
			}
			set
			{
				if (value > 4294967295U || value < 0U)
				{
					throw new Exception("Value cannot be represented as UInt32");
				}
				this.Poly = value;
			}
		}
		public uint InitialValue
		{
			get
			{
				return this.InitVal;
			}
			set
			{
				if (value > 4294967295U || value < 0U)
				{
					throw new Exception("Value cannot be represented as UInt32");
				}
				this.InitVal = value;
				this.Hash = this.InitVal;
			}
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
				return ~this.Hash;
			}
		}
		private void GenerateTable()
		{
			if (this.Table == null || this.Table.Length != 256)
			{
				this.Table = new uint[256];
			}
			uint num = 0U;
			checked
			{
				uint num2 = (uint)(this.Table.Length - 1);
				for (uint num3 = num; num3 <= num2; num3 += 1U)
				{
					uint num4 = num3 << 24;
					sbyte b = 0;
					unchecked
					{
						do
						{
							if ((num4 & 2147483648U) > 0U)
							{
								num4 = (num4 << 1 ^ this.Polynomial);
							}
							else
							{
								num4 <<= 1;
							}
							b += 1;
						}
						while (b <= 7);
					}
					this.Table[(int)num3] = num4;
				}
			}
		}
		public void TransformBlock(byte[] Buffer, uint Index, uint Length)
		{
			if (this.Table == null || this.Table.Length != 256)
			{
				this.GenerateTable();
			}
			checked
			{
				uint num = (uint)(unchecked((ulong)Length) - 1UL);
				for (uint num2 = Index; num2 <= num; num2 += 1U)
				{
					this.Hash = (this.Hash << 8 ^ this.Table[(int)((byte)(unchecked(((ulong)(this.Hash >> 24) & 255UL) ^ (ulong)Buffer[checked((int)num2)])))]);
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
			this.Hash = this.InitVal;
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		private uint[] Table;
		private uint Poly;
		private uint InitVal;
		private uint Hash;
	}
}

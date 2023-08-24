using System;
namespace IOTools.Algorithms
{
	public class Checksum32
	{
		public uint Compute(byte[] Buffer, uint Index, uint Length)
		{
			ulong num = 0UL;
			checked
			{
				uint num2 = (uint)(unchecked((ulong)Length) - 1UL);
				for (uint num3 = Index; num3 <= num2; num3 += 1U)
				{
					num = (num + unchecked((ulong)Buffer[checked((int)num3)]) & unchecked((ulong)-1));
				}
				return Convert.ToUInt32(num);
			}
		}
		public uint Compute(byte[] Buffer, uint Length)
		{
			return this.Compute(Buffer, 0U, Length);
		}
		public uint Compute(byte[] Buffer)
		{
			return this.Compute(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}

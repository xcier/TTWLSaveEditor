using System;
namespace IOTools.Algorithms
{
	public class Checksum64
	{
		public ulong Compute(byte[] Buffer, uint Index, uint Length)
		{
			ulong num = 0UL;
			checked
			{
				uint num2 = (uint)(unchecked((ulong)Length) - 1UL);
				for (uint num3 = Index; num3 <= num2; num3 += 1U)
				{
					num = (num + unchecked((ulong)Buffer[checked((int)num3)]) & ulong.MaxValue);
				}
				return Convert.ToUInt64(num);
			}
		}
		public ulong Compute(byte[] Buffer, uint Length)
		{
			return this.Compute(Buffer, 0U, Length);
		}
		public ulong Compute(byte[] Buffer)
		{
			return this.Compute(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}

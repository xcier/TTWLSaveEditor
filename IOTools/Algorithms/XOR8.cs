using System;
namespace IOTools.Algorithms
{
	public class XOR8
	{
		public byte Compute(byte[] Buffer, uint Index, uint Length)
		{
			ulong num = 0UL;
			checked
			{
				uint num2 = (uint)(unchecked((ulong)Length) - 1UL);
				for (uint num3 = Index; num3 <= num2; num3 += 1U)
				{
					num = (ulong)Math.Round(Math.Pow(num + unchecked((ulong)Buffer[checked((int)num3)]), 255.0));
				}
				return Convert.ToByte(num);
			}
		}
		public byte Compute(byte[] Buffer, uint Length)
		{
			return this.Compute(Buffer, 0U, Length);
		}
		public byte Compute(byte[] Buffer)
		{
			return this.Compute(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}

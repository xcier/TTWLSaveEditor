using System;
using System.Security.Cryptography;
namespace IOTools.Algorithms
{
	public class SHA2
	{
		public SHA2()
		{
			this.SHA = new SHA256CryptoServiceProvider();
			this.Hash = new byte[32];
		}
		public byte[] GetHashBytes
		{
			get
			{
				return this.Hash;
			}
		}
		public void TransformBlock(byte[] Buffer, uint Index, uint Length)
		{
			checked
			{
				this.SHA.TransformBlock(Buffer, (int)Index, (int)Length, this.Hash, 0);
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
		public byte[] TransformFinalBlock(byte[] Buffer, uint Index, uint Length)
		{
			return checked(this.SHA.TransformFinalBlock(Buffer, (int)Index, (int)Length));
		}
		public byte[] TransformFinalBlock(byte[] Buffer, uint Length)
		{
			return this.TransformFinalBlock(Buffer, 0U, Length);
		}
		public byte[] TransformFinalBlock(byte[] Buffer)
		{
			return this.TransformFinalBlock(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public byte[] Compute(byte[] Buffer, uint Index, uint Length)
		{
			return checked(this.SHA.ComputeHash(Buffer, (int)Index, (int)Length));
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
			this.SHA = new SHA256CryptoServiceProvider();
			this.Hash = new byte[32];
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		private SHA256CryptoServiceProvider SHA;
		private byte[] Hash;
	}
}

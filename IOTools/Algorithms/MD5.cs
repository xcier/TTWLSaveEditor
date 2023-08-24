using System;
using System.Security.Cryptography;
namespace IOTools.Algorithms
{
	public class MD5
	{
		public MD5()
		{
			MD5Provider = new MD5CryptoServiceProvider();
			Hash = new byte[16];
		}
		public byte[] GetHashBytes
		{
			get
			{
				return Hash;
			}
		}
		public void TransformBlock(byte[] Buffer, uint Index, uint Length)
		{
			checked
			{
				MD5Provider.TransformBlock(Buffer, (int)Index, (int)Length, Hash, 0);
			}
		}
		public void TransformBlock(byte[] Buffer, uint Length)
		{
			TransformBlock(Buffer, 0U, Length);
		}
		public void TransformBlock(byte[] Buffer)
		{
			TransformBlock(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public byte[] TransformFinalBlock(byte[] Buffer, uint Index, uint Length)
		{
			return checked(MD5Provider.TransformFinalBlock(Buffer, (int)Index, (int)Length));
		}
		public byte[] TransformFinalBlock(byte[] Buffer, uint Length)
		{
			return TransformFinalBlock(Buffer, 0U, Length);
		}
		public byte[] TransformFinalBlock(byte[] Buffer)
		{
			return TransformFinalBlock(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public byte[] Compute(byte[] Buffer, uint Index, uint Length)
		{
			return checked(MD5Provider.ComputeHash(Buffer, (int)Index, (int)Length));
		}
		public byte[] Compute(byte[] Buffer, uint Length)
		{
			return Compute(Buffer, 0U, Length);
		}
		public byte[] Compute(byte[] Buffer)
		{
			return Compute(Buffer, 0U, checked((uint)Buffer.Length));
		}
		public void Clear()
		{
			MD5Provider = new MD5CryptoServiceProvider();
			Hash = new byte[16];
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		private MD5CryptoServiceProvider MD5Provider;
		private byte[] Hash;
	}
}

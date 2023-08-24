using System;
using System.Security.Cryptography;
namespace IOTools.Encryption
{
	public class RC4
	{
		public RC4(byte[] Key)
		{
			this.CryptoKey = null;
			this.Key = Key;
		}
		public byte[] Key
		{
			get
			{
				return this.CryptoKey;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					throw new CryptographicException("Key must not be null");
				}
				if (value.Length < 1 || value.Length > 256)
				{
					throw new CryptographicException("A valid key must be between 1 to 256 bytes in length");
				}
				this.CryptoKey = value;
			}
		}
		private byte[] Core(byte[] Buffer, int Index, int Length)
		{
			if (this.Key == null || this.Key.Length == 0)
			{
				throw new CryptographicException("Key must not be null");
			}
			if (Buffer == null || Buffer.Length == 0)
			{
				throw new CryptographicException("Input data must not be null");
			}
			byte[] array = new byte[256];
			byte[] array2 = new byte[256];
			int num = 0;
			checked
			{
				do
				{
					array[num] = (byte)num;
					array2[num] = this.CryptoKey[num % this.CryptoKey.Length];
					num++;
				}
				while (num <= 255);
				int num2 = 0;
				int num3 = 0;
				do
				{
					num2 = (num2 + (int)array[num3] + (int)array2[num3]) % 256;
					byte b = array[num3];
					array[num3] = array[num2];
					array[num2] = b;
					num3++;
				}
				while (num3 <= 255);
				int num4 = num2;
				int num5 = 0;
				int num6 = Buffer.Length - 1;
				for (int i = num5; i <= num6; i++)
				{
					num4 = (num4 + 1) % 256;
					num2 = (num2 + (int)array[num4]) % 256;
					byte b = array[num4];
					array[num4] = array[num2];
					array[num2] = b;
					byte b2 = (byte)((int)(unchecked(array[num4] + array[num2])) % 256);
					Buffer[i] ^= array[(int)b2];
				}
				return Buffer;
			}
		}
		public byte[] Encrypt(byte[] Buffer, int Index, int Length)
		{
			return this.Core(Buffer, Index, Length);
		}
		public byte[] Encrypt(byte[] Buffer, int Length)
		{
			return this.Encrypt(Buffer, 0, Length);
		}
		public byte[] Encrypt(byte[] Buffer)
		{
			return this.Encrypt(Buffer, Buffer.Length);
		}
		public byte[] Decrypt(byte[] Buffer, int Index, int Length)
		{
			return this.Core(Buffer, Index, Length);
		}
		public byte[] Decrypt(byte[] Buffer, int Length)
		{
			return this.Decrypt(Buffer, 0, Length);
		}
		public byte[] Decrypt(byte[] Buffer)
		{
			return this.Decrypt(Buffer, Buffer.Length);
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		private byte[] CryptoKey;
	}
}

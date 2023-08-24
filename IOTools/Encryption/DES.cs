using System;
using System.Security.Cryptography;
namespace IOTools.Encryption
{
	public class DES
	{
		public DES()
		{
			this.Provider = new DESCryptoServiceProvider();
			this.Provider.GenerateKey();
			this.Provider.GenerateIV();
			this.Encryptor = this.Provider.CreateEncryptor(this.Provider.Key, this.Provider.IV);
			this.Decryptor = this.Provider.CreateDecryptor(this.Provider.Key, this.Provider.IV);
		}
		public DES(byte[] Key, byte[] IV)
		{
			this.Provider = new DESCryptoServiceProvider();
			this.Provider.Key = Key;
			this.Provider.Key = IV;
			this.Encryptor = this.Provider.CreateEncryptor(this.Provider.Key, this.Provider.IV);
			this.Decryptor = this.Provider.CreateDecryptor(this.Provider.Key, this.Provider.IV);
		}
		public bool ValidKeySize(int Size)
		{
			return this.Provider.ValidKeySize(Size);
		}
		public byte[] Key
		{
			get
			{
				return this.Provider.Key;
			}
			set
			{
				this.Provider.Key = value;
			}
		}
		public byte[] InitializationVector
		{
			get
			{
				return this.Provider.IV;
			}
			set
			{
				this.Provider.IV = value;
			}
		}
		public int KeySize
		{
			get
			{
				return this.Provider.KeySize;
			}
			set
			{
				this.Provider.KeySize = value;
			}
		}
		public KeySizes[] LegalBlockSizes
		{
			get
			{
				return this.Provider.LegalBlockSizes;
			}
		}
		public KeySizes[] LegalKeySizes
		{
			get
			{
				return this.Provider.LegalKeySizes;
			}
		}
		public CipherMode CipherMode
		{
			get
			{
				return this.Provider.Mode;
			}
			set
			{
				this.Provider.Mode = value;
			}
		}
		public byte[] TransformBlock(byte[] Buffer, uint Index, uint Length, Mode Mode)
		{
			checked
			{
				this.Block = new byte[(int)(unchecked((ulong)(checked(Length - Index))) - 1UL) + 1];
				if (Mode == Mode.Decrypt)
				{
					this.Decryptor.TransformBlock(Buffer, (int)Index, (int)Length, this.Block, 0);
				}
				else
				{
					this.Encryptor.TransformBlock(Buffer, (int)Index, (int)Length, this.Block, 0);
				}
				return this.Block;
			}
		}
		public byte[] TransformBlock(byte[] Buffer, uint Length, Mode Mode)
		{
			return this.TransformBlock(Buffer, 0U, Length, Mode);
		}
		public byte[] TransformBlock(byte[] Buffer, Mode Mode)
		{
			return this.TransformBlock(Buffer, 0U, checked((uint)Buffer.Length), Mode);
		}
		public byte[] TransformFinalBlock(byte[] Buffer, uint Index, uint Length, Mode Mode)
		{
			checked
			{
				if (Mode == Mode.Decrypt)
				{
					return this.Decryptor.TransformFinalBlock(Buffer, (int)Index, (int)Length);
				}
				return this.Encryptor.TransformFinalBlock(Buffer, (int)Index, (int)Length);
			}
		}
		public byte[] TransformFinalBlock(byte[] Buffer, uint Length, Mode Mode)
		{
			return this.TransformFinalBlock(Buffer, 0U, Length, Mode);
		}
		public byte[] TransformFinalBlock(byte[] Buffer, Mode Mode)
		{
			return this.TransformFinalBlock(Buffer, 0U, checked((uint)Buffer.Length), Mode);
		}
		public void Dispose()
		{
			this.Encryptor.Dispose();
			this.Decryptor.Dispose();
			GC.SuppressFinalize(this);
		}
		private DESCryptoServiceProvider Provider;
		private ICryptoTransform Encryptor;
		private ICryptoTransform Decryptor;
		private byte[] Block;
	}
}

using System;
using System.IO;
using System.IO.Compression;
namespace IOTools.Compression
{
	public class GZip
	{
		public byte[] TransformBlock(byte[] Buffer, uint Index, uint Length, Mode Mode)
		{
			if (Buffer == null)
			{
				return null;
			}
			Stream stream = new MemoryStream(Buffer);
			Stream stream2 = new MemoryStream();
			if (Mode == Mode.Compress)
			{
				this.Stream = new GZipStream(stream2, CompressionMode.Compress, true);
			}
			else
			{
				this.Stream = new GZipStream(stream2, CompressionMode.Decompress, true);
			}
			checked
			{
				uint num = (uint)(unchecked((ulong)Length) - 1UL);
				for (uint num2 = Index; num2 <= num; num2 += 1U)
				{
					stream2.WriteByte((byte)stream.ReadByte());
					stream2.Flush();
				}
				stream.Close();
				stream.Dispose();
				Buffer = new byte[(int)(stream2.Length - 1L) + 1];
				stream2.Read(Buffer, 0, Buffer.Length);
				stream2.Close();
				this.Stream.Close();
				this.Stream.Dispose();
				return Buffer;
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
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		private GZipStream Stream;
	}
}

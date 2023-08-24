using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
namespace IOTools
{
	public class WriterWrapper : IDisposable, ICloneable, IEquatable<WriterWrapper>
	{
		public WriterWrapper(byte[] Buffer, Endian EndianType = Endian.Little, long StartingPosition = 0L)
		{
			OpenStream = null;
			binWriter = null;
			LastPosition = 0L;
			IsOwner = false;
			StreamType = StreamType.MemoryStream;
			OpenStream = new MemoryStream(Buffer, true);
			binWriter = new BinaryWriter(OpenStream);
			IsOwner = true;
			Position = StartingPosition;
			CurrentEndian = EndianType;
		}
		public WriterWrapper(Stream Stream, Endian EndianType = Endian.Little, long StartingPosition = 0L)
		{
			OpenStream = null;
			binWriter = null;
			LastPosition = 0L;
			IsOwner = false;
			if (!Stream.CanWrite) throw new InvalidDataException("Unable to write to stream");
			StreamType = StreamType.GenericStream;
			OpenStream = Stream;
			binWriter = new BinaryWriter(OpenStream);
			Position = StartingPosition;
			CurrentEndian = EndianType;
        }
		public Stream CurrentStream
		{
			get
			{
				return OpenStream;
			}
		}
        public StreamType StreamType { get; }
        public bool IsOwner { get; }
        public long Position
		{
			get
			{
				return OpenStream.Position;
			}
			set
			{
				if (LastPosition != OpenStream.Position)
				{
					LastPosition = OpenStream.Position;
				}
				OpenStream.Position = value;
			}
		}
		public void JumpBack()
		{
			Position = LastPosition;
		}
		public void Seek(long Offset)
		{
			Seek(Offset, SeekOrigin.Begin);
		}
		public void Seek(long Offset, SeekOrigin SeekOrigin)
		{
			if (LastPosition != OpenStream.Position)
			{
				LastPosition = OpenStream.Position;
			}
			binWriter.BaseStream.Seek(Offset, SeekOrigin);
		}
		public long Length
		{
			get
			{
				return OpenStream.Length;
			}
		}
		public long LengthToEnd
		{
			get
			{
				return checked(Length - Position);
			}
		}
		public Endian CurrentEndian
		{
			get
			{
				return Endianness;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Endian), value))
				{
					return;
				}
				Endianness = value;
			}
		}
        public long LastPosition { get; private set; }
        public byte[] GetBuffer()
		{
			byte[] result = new byte[0];
			GetBuffer(ref result);
			return result;
		}
		public void GetBuffer([In] [Out] ref byte[] OutBuffer)
		{
			Functions.StreamToBuffer(ref OpenStream, ref OutBuffer);
		}
		public void Write(byte[] Buffer)
		{
			Write(Buffer, Buffer.Length, CurrentEndian);
		}
		public void Write(byte[] Buffer, int Length)
		{
			Write(Buffer, Length, CurrentEndian);
		}
		public void Write(byte[] Buffer, int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			if (EndianType == Endian.Big)
			{
				Functions.SwapEndian(ref Buffer);
			}
			binWriter.Write(Buffer, 0, Length);
		}
		public void WriteBytes(byte[] Buffer)
		{
			Write(Buffer);
		}
		public void WriteBytes(byte[] Buffer, int Length)
		{
			Write(Buffer, Length);
		}
		public void WriteBytes(byte[] Buffer, int Length, Endian EndianType)
		{
			Write(Buffer, Length, EndianType);
		}
		public void Insert(byte[] Buffer)
		{
			Insert(Buffer, Buffer.Length);
		}
		public void Insert(byte[] Buffer, int Length)
		{
			Insert(Buffer, Length, CurrentEndian);
		}
		public void Insert(byte[] Buffer, int Length, Endian EndianType)
		{
			checked
			{
				if (Buffer != null)
				{
					if (Buffer.Length == 0)
					{
						return;
					}
					if (Length == 0)
					{
						return;
					}
					if (StreamType != StreamType.FileStream || OpenStream.CanRead)
					{
						if (OpenStream.CanWrite)
						{
							if (LengthToEnd != 0L) {
								long position = Position;
								byte[] array = new byte[(int)(LengthToEnd - 1L) + 1];
								OpenStream.Read(array, 0, (int)LengthToEnd);
								Position = position;
								Write(Buffer, Length, EndianType);
								Write(array, array.Length, Endian.Little);
								Position = position + unchecked((long)Length);
								Array.Clear(array, 0, array.Length);
							}
							else {
								Write(Buffer, Length, EndianType);
							}
						}
					}
				}
			}
		}
		public void InsertBytes(byte[] Buffer)
		{
			Insert(Buffer);
		}
		public void InsertBytes(byte[] Buffer, int Length)
		{
			Insert(Buffer, Length);
		}
		public void InsertBytes(byte[] Buffer, int Length, Endian EndianType)
		{
			Insert(Buffer, Length, EndianType);
		}
		public void Delete(int Length)
		{
			checked
			{
				if (Length != 0)
				{
					if (LengthToEnd == 0L)
					{
						return;
					}
					if (LengthToEnd - unchecked((long)Length) < 0L)
					{
						throw new DataException("Length is longer than the number of bytes left to end of stream. Not enough bytes to delete");
					}
					if (StreamType != StreamType.FileStream || OpenStream.CanRead)
					{
						if (OpenStream.CanWrite)
						{
							byte[] array = new byte[(int)(Length - 1L) + 1];
							long position = Position;
							Position = 0L;
							OpenStream.Flush();
							OpenStream.Read(array, 0, (int)Length);
							OpenStream.Close();
							array = Functions.DeleteBytes(array, (uint)position, (uint)Length);
							OpenStream = new MemoryStream(array, true);
							binWriter = new BinaryWriter(OpenStream);
							binWriter.Write(array, 0, array.Length);
							binWriter.Flush();
							Position = position;
						}
					}
				}
			}
		}
		public void DeleteBytes(int Length)
		{
			Delete(Length);
		}
		public void WriteNull(int Length)
		{
			byte[] buffer = new byte[checked(Length - 1 + 1)];
			Write(buffer);
		}
		public void WriteByte(byte Value)
		{
			Write(new byte[]
			{
				Value
			});
		}
		public void WriteSByte(sbyte Value)
		{
			Write(new byte[]
			{
				checked((byte)Value)
			});
		}
		public void WriteInt8(sbyte Value)
		{
			WriteSByte(Value);
		}
		public void WriteUInt8(byte Value)
		{
			WriteByte(Value);
		}
		public void WriteInt16(short Value)
		{
			WriteInt16(Value, CurrentEndian);
		}
		public void WriteInt16(short Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 2, EndianType);
		}
		public void WriteUInt16(ushort Value)
		{
			WriteUInt16(Value, CurrentEndian);
		}
		public void WriteUInt16(ushort Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 2, EndianType);
		}
		public void WriteInt24(int Value)
		{
			WriteInt24(Value, CurrentEndian);
		}
		public void WriteInt24(int Value, Endian EndianType)
		{
			Write(Converters.Int24ToBytes(Value, Conversions.ToBoolean(Interaction.IIf(EndianType == Endian.Big, true, false))), 3, Endian.Little);
		}
		public void WriteUInt24(uint Value)
		{
			WriteUInt24(Value, CurrentEndian);
		}
		public void WriteUInt24(uint Value, Endian EndianType)
		{
			Write(Converters.UInt24ToBytes(Value, Conversions.ToBoolean(Interaction.IIf(EndianType == Endian.Big, true, false))), 3, Endian.Little);
		}
		public void WriteInt32(int Value)
		{
			WriteInt32(Value, CurrentEndian);
		}
		public void WriteInt32(int Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 4, EndianType);
		}
		public void WriteUInt32(uint Value)
		{
			WriteUInt32(Value, CurrentEndian);
		}
		public void WriteUInt32(uint Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 4, EndianType);
		}
		public void WriteInt40(long Value)
		{
			WriteInt40(Value, CurrentEndian);
		}
		public void WriteInt40(long Value, Endian EndianType)
		{
			Write(Converters.Int40ToBytes(Value, false), 5, EndianType);
		}
		public void WriteUInt40(ulong Value)
		{
			WriteUInt40(Value, CurrentEndian);
		}
		public void WriteUInt40(ulong Value, Endian EndianType)
		{
			Write(Converters.UInt40ToBytes(Value, false), 5, EndianType);
		}
		public void WriteInt48(long Value)
		{
			WriteInt48(Value, CurrentEndian);
		}
		public void WriteInt48(long Value, Endian EndianType)
		{
			Write(Converters.Int48ToBytes(Value, false), 6, EndianType);
		}
		public void WriteUInt48(ulong Value)
		{
			WriteUInt48(Value, CurrentEndian);
		}
		public void WriteUInt48(ulong Value, Endian EndianType)
		{
			Write(Converters.UInt48ToBytes(Value, false), 6, EndianType);
		}
		public void WriteInt56(long Value)
		{
			WriteInt56(Value, CurrentEndian);
		}
		public void WriteInt56(long Value, Endian EndianType)
		{
			Write(Converters.Int56ToBytes(Value, false), 7, EndianType);
		}
		public void WriteUInt56(ulong Value)
		{
			WriteUInt56(Value, CurrentEndian);
		}
		public void WriteUInt56(ulong Value, Endian EndianType)
		{
			Write(Converters.UInt56ToBytes(Value, false), 7, EndianType);
		}
		public void WriteInt64(long Value)
		{
			WriteInt64(Value, CurrentEndian);
		}
		public void WriteInt64(long Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 8, EndianType);
		}
		public void WriteUInt64(ulong Value)
		{
			WriteUInt64(Value, CurrentEndian);
		}
		public void WriteUInt64(ulong Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 8, EndianType);
		}
		public void WriteSingle(float Value)
		{
			WriteSingle(Value, CurrentEndian);
		}
		public void WriteSingle(float Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 4, EndianType);
		}
		public void WriteDouble(double Value)
		{
			WriteDouble(Value, CurrentEndian);
		}
		public void WriteDouble(double Value, Endian EndianType)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Write(bytes, 8, EndianType);
		}
		public void WriteChar(char Value)
		{
			LastPosition = OpenStream.Position;
			binWriter.Write(Value);
		}
		public void WriteChars(char[] Value)
		{
			foreach (char value in Value)
			{
				WriteChar(value);
			}
		}
		public void WriteBinary(string Value)
		{
			WriteBinary(Value, CurrentEndian);
		}
		public void WriteBinary(string Value, Endian EndianType)
		{
			Write(Converters.BitStringToBytes(Value), (int)EndianType);
		}
		public void WriteASCII(string Value)
		{
			WriteASCII(Value, Value.Length);
		}
		public void WriteASCII(string Value, int Length)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(Value);
			Write(bytes, Length, Endian.Little);
		}
		public void WriteUnicode(string Value)
		{
			WriteUnicode(Value, Value.Length);
		}
		public void WriteUnicode(string Value, int Length)
		{
			WriteUnicode(Value, Length, CurrentEndian);
		}
		public void WriteUnicode(string Value, int Length, Endian EndianType)
		{
			if (EndianType == Endian.Big)
			{
				Write(Encoding.BigEndianUnicode.GetBytes(Value), Length, Endian.Little);
				return;
			}
			Write(Encoding.Unicode.GetBytes(Value), Length, Endian.Little);
		}
		public void WriteHex(string Value)
		{
			WriteHex(Value, Value.Length);
		}
		public void WriteHex(string Value, int Length)
		{
			WriteHex(Value, Length, CurrentEndian);
		}
		public void WriteHex(string Value, int Length, Endian EndianType) {
			Write(Converters.HexToBytes(Value), checked((int)Math.Round((double)Length / 2.0)), EndianType);
		}
		public void SwapEndian()
		{
			if (CurrentEndian == Endian.Little)
			{
				CurrentEndian = Endian.Big;
			}
			else
			{
				CurrentEndian = Endian.Little;
			}
		}
		public Endian OppositeEndian
		{
			get
			{
				if (CurrentEndian == Endian.Little)
				{
					return Endian.Big;
				}
				return Endian.Little;
			}
		}
		public bool IsOpen
		{
			get
			{
				if (binWriter != null)
				{
					if (OpenStream != null)
					{
						return true;
					}
				}
				return false;
			}
		}
		public void Flush()
		{
			OpenStream.Flush();
		}
		public void Close(bool CloseUnderlying = true, bool Dispose = false)
		{
			if (IsOpen)
			{
				binWriter.Flush();
				binWriter.Close();
				binWriter = null;
				if (CloseUnderlying)
				{
					OpenStream.Close();
					OpenStream = null;
				}
			}
			if (Dispose) this.Dispose();
		}
		public object Clone()
		{
			return (WriterWrapper)MemberwiseClone();
		}
		public void Dispose()
		{
			GC.Collect(GC.GetGeneration(this), GCCollectionMode.Forced);
		}
		public bool Equals(WriterWrapper obj)
		{
			return base.Equals(obj);
		}
		private Stream OpenStream;
        private BinaryWriter binWriter;
        private Endian Endianness;
    }
}

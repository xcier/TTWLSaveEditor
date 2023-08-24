using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
namespace IOTools
{
	public class IOWrapper : IDisposable, ICloneable, IEquatable<IOWrapper>
	{
		public IOWrapper(byte[] Buffer, Endian EndianType = Endian.Little, long StartingPosition = 0L)
		{
			OpenStream = null;
			Reader = null;
			Writer = null;
			LastPosition = 0L;
			Pausable = true;
			IsOwner = false;
			StreamType = StreamType.MemoryStream;
			OpenStream = new MemoryStream(Buffer, true);
			Reader = new ReaderWrapper(OpenStream, Endian.Little, 0L);
			Writer = new WriterWrapper(OpenStream, Endian.Little, 0L);
			IsOwner = true;
			Position = StartingPosition;
			CurrentEndian = EndianType;
			Pausable = false;
		}
		public IOWrapper(Stream Stream, Endian EndianType = Endian.Little, long StartingPosition = 0L)
		{
			OpenStream = null;
			Reader = null;
			Writer = null;
			LastPosition = 0L;
			Pausable = true;
			IsOwner = false;
			StreamType = StreamType.GenericStream;
			OpenStream = Stream;
			if (!OpenStream.CanRead) throw new InvalidDataException("Cannot read from stream");
			if (!OpenStream.CanWrite) throw new InvalidDataException("Cannot write to stream");
			Reader = new ReaderWrapper(OpenStream, Endian.Little, 0L);
			Writer = new WriterWrapper(OpenStream, Endian.Little, 0L);
			Position = StartingPosition;
			CurrentEndian = EndianType;
			Pausable = false;
		}
		public Stream CurrentStream
		{
			get { return OpenStream; }
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
			OpenStream.Seek(Offset, SeekOrigin);
		}
		public long Length
		{
			get { return OpenStream.Length; }
		}
		public long LengthToEnd
		{
			get { return checked(Length - Position); }
		}
		public Endian CurrentEndian
		{
			get { return Endianness; }
			set
			{
				if (!Enum.IsDefined(typeof(Endian), value)) return;
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
		public char PeekChar()
		{
			return Reader.PeekChar();
		}
		public char[] PeekChars(int Length)
		{
			return Reader.PeekChars(Length);
		}
		public bool PeekBoolean()
		{
			return Reader.PeekBoolean();
		}
		public byte PeekByte()
		{
			return Reader.PeekByte();
		}
		public sbyte PeekSByte()
		{
			return Reader.PeekSByte();
		}
		public sbyte PeekInt8()
		{
			return Reader.PeekInt8();
		}
		public byte PeekUInt8()
		{
			return Reader.PeekUInt8();
		}
		public short PeekInt16()
		{
			return Reader.PeekInt16(CurrentEndian);
		}
		public ushort PeekUInt16()
		{
			return Reader.PeekUInt16(CurrentEndian);
		}
		public short PeekInt16(Endian EndianType)
		{
			return Reader.PeekInt16(EndianType);
		}
		public ushort PeekUInt16(Endian EndianType)
		{
			return Reader.PeekUInt16(EndianType);
		}
		public int PeekInt24()
		{
			return Reader.PeekInt24(CurrentEndian);
		}
		public uint PeekUInt24()
		{
			return Reader.PeekUInt24(CurrentEndian);
		}
		public int PeekInt24(Endian EndianType)
		{
			return Reader.PeekInt24(EndianType);
		}
		public uint PeekUInt24(Endian EndianType)
		{
			return Reader.PeekUInt24(EndianType);
		}
		public int PeekInt32()
		{
			return Reader.PeekInt32(CurrentEndian);
		}
		public uint PeekUInt32()
		{
			return Reader.PeekUInt32(CurrentEndian);
		}
		public int PeekInt32(Endian EndianType)
		{
			return Reader.PeekInt32(EndianType);
		}
		public uint PeekUInt32(Endian EndianType)
		{
			return Reader.PeekUInt32(EndianType);
		}
		public long PeekInt40()
		{
			return Reader.PeekInt40(CurrentEndian);
		}
		public ulong PeekUInt40()
		{
			return Reader.PeekUInt40(CurrentEndian);
		}
		public long PeekInt40(Endian EndianType)
		{
			return Reader.PeekInt40(EndianType);
		}
		public ulong PeekUInt40(Endian EndianType)
		{
			return Reader.PeekUInt40(EndianType);
		}
		public long PeekInt48()
		{
			return Reader.PeekInt48(CurrentEndian);
		}
		public ulong PeekUInt48()
		{
			return Reader.PeekUInt48(CurrentEndian);
		}
		public long PeekInt48(Endian EndianType)
		{
			return Reader.PeekInt48(EndianType);
		}
		public ulong PeekUInt48(Endian EndianType)
		{
			return Reader.PeekUInt48(EndianType);
		}
		public long PeekInt56()
		{
			return Reader.PeekInt56(CurrentEndian);
		}
		public ulong PeekUInt56()
		{
			return Reader.PeekUInt56(CurrentEndian);
		}
		public long PeekInt56(Endian EndianType)
		{
			return Reader.PeekInt56(EndianType);
		}
		public ulong PeekUInt56(Endian EndianType)
		{
			return Reader.PeekUInt56(EndianType);
		}
		public long PeekInt64()
		{
			return Reader.PeekInt64(CurrentEndian);
		}
		public ulong PeekUInt64()
		{
			return Reader.PeekUInt64(CurrentEndian);
		}
		public long PeekInt64(Endian EndianType)
		{
			return Reader.PeekInt64(EndianType);
		}
		public ulong PeekUInt64(Endian EndianType)
		{
			return Reader.PeekUInt64(EndianType);
		}
		public float PeekSingle()
		{
			return Reader.PeekSingle(CurrentEndian);
		}
		public double PeekDouble()
		{
			return Reader.PeekDouble(CurrentEndian);
		}
		public float PeekSingle(Endian EndianType)
		{
			return Reader.PeekSingle(EndianType);
		}
		public double PeekDouble(Endian EndianType)
		{
			return Reader.PeekDouble(EndianType);
		}
		public byte[] PeekBytes(int Length)
		{
			return Reader.PeekBytes(Length, CurrentEndian);
		}
		public byte[] PeekBytes(int Length, Endian EndianType)
		{
			return Reader.PeekBytes(Length, EndianType);
		}
		public string PeekBinary(int Length)
		{
			return Reader.PeekBinary(Length, CurrentEndian);
		}
		public string PeekBinary(int Length, Endian EndianType)
		{
			return Reader.PeekBinary(Length, EndianType);
		}
		public string PeekASCII(int Length)
		{
			return Reader.PeekASCII(Length);
		}
		public string PeekUnicode(int Length)
		{
			return Reader.PeekUnicode(Length, CurrentEndian);
		}
		public string PeekUnicode(int Length, Endian EndianType)
		{
			return Reader.PeekUnicode(Length, EndianType);
		}
		public string PeekHex(int Length)
		{
			return Reader.PeekHex(Length, CurrentEndian);
		}
		public string PeekHex(int Length, Endian EndianType)
		{
			return Reader.PeekHex(Length, EndianType);
		}
		public bool ReadBoolean()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadBoolean();
		}
		public byte ReadByte()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadByte();
		}
		public sbyte ReadSByte()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadSByte();
		}
		public byte[] ReadBytes(int Length)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadBytes(Length, CurrentEndian);
		}
		public byte[] ReadBytes(int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadBytes(Length, EndianType);
		}
		public byte[] ReadAll()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadAll(CurrentEndian);
		}
		public byte[] ReadAll(Endian EndianType)
		{
			Position = 0L;
			return Reader.ReadBytes(checked((int)Length), EndianType);
		}
		public byte[] ReadToEnd()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadToEnd(CurrentEndian);
		}
		public byte[] ReadToEnd(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadToEnd(EndianType);
		}
		public sbyte ReadInt8()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt8();
		}
		public byte ReadUInt8()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt8();
		}
		public short ReadInt16()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt16(CurrentEndian);
		}
		public short ReadInt16(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt16(EndianType);
		}
		public ushort ReadUInt16()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt16(CurrentEndian);
		}
		public ushort ReadUInt16(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt16(EndianType);
		}
		public int ReadInt24()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt24(CurrentEndian);
		}
		public int ReadInt24(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt24(EndianType);
		}
		public uint ReadUInt24()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt24(CurrentEndian);
		}
		public uint ReadUInt24(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt24(EndianType);
		}
		public int ReadInt32()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt32(CurrentEndian);
		}
		public int ReadInt32(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt32(EndianType);
		}
		public uint ReadUInt32()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt32(CurrentEndian);
		}
		public uint ReadUInt32(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt32(EndianType);
		}
		public long ReadInt40()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt40(CurrentEndian);
		}
		public long ReadInt40(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt40(EndianType);
		}
		public ulong ReadUInt40()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt40(CurrentEndian);
		}
		public ulong ReadUInt40(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt40(EndianType);
		}
		public long ReadInt48()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt48(CurrentEndian);
		}
		public long ReadInt48(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt48(EndianType);
		}
		public ulong ReadUInt48()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt48(CurrentEndian);
		}
		public ulong ReadUInt48(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt48(EndianType);
		}
		public long ReadInt56()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt56(CurrentEndian);
		}
		public long ReadInt56(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt56(EndianType);
		}
		public ulong ReadUInt56()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt56(CurrentEndian);
		}
		public ulong ReadUInt56(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt56(EndianType);
		}
		public long ReadInt64()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt64(CurrentEndian);
		}
		public long ReadInt64(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadInt64(EndianType);
		}
		public ulong ReadUInt64()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt64(CurrentEndian);
		}
		public ulong ReadUInt64(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUInt64(EndianType);
		}
		public float ReadSingle()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadSingle(CurrentEndian);
		}
		public float ReadSingle(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadSingle(EndianType);
		}
		public double ReadDouble()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadDouble(CurrentEndian);
		}
		public double ReadDouble(Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadDouble(EndianType);
		}
		public char ReadChar()
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadChar();
		}
		public char[] ReadChars(int Length)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadChars(Length);
		}
		public string ReadBinary(int Length)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadBinary(Length, CurrentEndian);
		}
		public string ReadBinary(int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadBinary(Length, EndianType);
		}
		public string ReadASCII(int Length)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadASCII(Length);
		}
		public string ReadUnicode(int Length)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUnicode(Length, CurrentEndian);
		}
		public string ReadUnicode(int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadUnicode(Length, EndianType);
		}
		public string ReadHex(int Length)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadHex(Length, CurrentEndian);
		}
		public string ReadHex(int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			return Reader.ReadHex(Length, EndianType);
		}
		public void Write(byte[] Buffer)
		{
			LastPosition = OpenStream.Position;
			Writer.Write(Buffer, Buffer.Length, CurrentEndian);
		}
		public void Write(byte[] Buffer, int Length)
		{
			LastPosition = OpenStream.Position;
			Writer.Write(Buffer, Length, CurrentEndian);
		}
		public void Write(byte[] Buffer, int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.Write(Buffer, Length, EndianType);
		}
		public void WriteBytes(byte[] Buffer)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteBytes(Buffer, Buffer.Length, CurrentEndian);
		}
		public void WriteBytes(byte[] Buffer, int Length)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteBytes(Buffer, Length, CurrentEndian);
		}
		public void WriteBytes(byte[] Buffer, int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteBytes(Buffer, Length, EndianType);
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
			Writer.Insert(Buffer, Length, EndianType);
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
					long position = Position;
					Position = 0L;
					Flush();
					byte[] array = Functions.DeleteBytes(Reader.ReadAll(Endian.Little), (uint)position, (uint)Length);
					OpenStream = new MemoryStream(array, true);
					Reader = new ReaderWrapper(OpenStream, CurrentEndian, 0L);
					Writer = new WriterWrapper(OpenStream, CurrentEndian, 0L);
					Write(array, array.Length, Endian.Little);
					Flush();
					Position = position;
				}
			}
		}
		public void DeleteBytes(int Length)
		{
			Delete(Length);
		}
		public void WriteNull(int Length)
		{
			Writer.WriteNull(Length);
		}
		public void WriteByte(byte Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteByte(Value);
		}
		public void WriteSByte(sbyte Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteSByte(Value);
		}
		public void WriteInt8(sbyte Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt8(Value);
		}
		public void WriteUInt8(byte Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt8(checked((sbyte)Value));
		}
		public void WriteInt16(short Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt16(Value, CurrentEndian);
		}
		public void WriteInt16(short Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt16(Value, EndianType);
		}
		public void WriteUInt16(ushort Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt16(Value, CurrentEndian);
		}
		public void WriteUInt16(ushort Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt16(Value, EndianType);
		}
		public void WriteInt24(int Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt24(Value, CurrentEndian);
		}
		public void WriteInt24(int Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt24(Value, EndianType);
		}
		public void WriteUInt24(uint Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt24(Value, CurrentEndian);
		}
		public void WriteUInt24(uint Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt24(Value, EndianType);
		}
		public void WriteInt32(int Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt32(Value, CurrentEndian);
		}
		public void WriteInt32(int Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt32(Value, EndianType);
		}
		public void WriteUInt32(uint Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt32(Value, CurrentEndian);
		}
		public void WriteUInt32(uint Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt32(Value, EndianType);
		}
		public void WriteInt40(long Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt40(Value, CurrentEndian);
		}
		public void WriteInt40(long Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt40(Value, EndianType);
		}
		public void WriteUInt40(ulong Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt40(Value, CurrentEndian);
		}
		public void WriteUInt40(ulong Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt40(Value, EndianType);
		}
		public void WriteInt48(long Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt48(Value, CurrentEndian);
		}
		public void WriteInt48(long Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt48(Value, EndianType);
		}
		public void WriteUInt48(ulong Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt48(Value, CurrentEndian);
		}
		public void WriteUInt48(ulong Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt48(Value, EndianType);
		}
		public void WriteInt56(long Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt56(Value, CurrentEndian);
		}
		public void WriteInt56(long Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt56(Value, EndianType);
		}
		public void WriteUInt56(ulong Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt56(Value, CurrentEndian);
		}
		public void WriteUInt56(ulong Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt56(Value, EndianType);
		}
		public void WriteInt64(long Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt64(Value, CurrentEndian);
		}
		public void WriteInt64(long Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteInt64(Value, EndianType);
		}
		public void WriteUInt64(ulong Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt64(Value, CurrentEndian);
		}
		public void WriteUInt64(ulong Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUInt64(Value, EndianType);
		}
		public void WriteSingle(float Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteSingle(Value, CurrentEndian);
		}
		public void WriteSingle(float Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteSingle(Value, EndianType);
		}
		public void WriteDouble(double Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteDouble(Value, CurrentEndian);
		}
		public void WriteDouble(double Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteDouble(Value, EndianType);
		}
		public void WriteChar(char Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteChar(Value);
		}
		public void WriteChars(char[] Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteChars(Value);
		}
		public void WriteBinary(string Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteBinary(Value, CurrentEndian);
		}
		public void WriteBinary(string Value, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteBinary(Value, EndianType);
		}
		public void WriteASCII(string Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteASCII(Value, Value.Length);
		}
		public void WriteASCII(string Value, int Length)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteASCII(Value, Length);
		}
		public void WriteUnicode(string Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUnicode(Value, Value.Length, CurrentEndian);
		}
		public void WriteUnicode(string Value, int Length)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUnicode(Value, Length, CurrentEndian);
		}
		public void WriteUnicode(string Value, int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteUnicode(Value, Length, EndianType);
		}
		public void WriteHex(string Value)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteHex(Value, Value.Length, CurrentEndian);
		}
		public void WriteHex(string Value, int Length)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteHex(Value, Length, CurrentEndian);
		}
		public void WriteHex(string Value, int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			Writer.WriteHex(Value, Length, EndianType);
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
		public bool Pausable { get; }
		public void Flush()
		{
			OpenStream.Flush();
		}
		public void Close(bool CloseUnderlying = true, bool Dispose = false)
		{
            if (OpenStream == null)
                return;
            Flush();
			Reader = null;
			Writer = null;
			if (CloseUnderlying) {
				OpenStream.Close();
				OpenStream = null;
			}
			if (Dispose) this.Dispose();
		}
		public object Clone()
		{
			return (IOWrapper)MemberwiseClone();
		}
		public void Dispose()
		{
			if (Reader != null) Reader.Dispose();
			if (Writer != null) Writer.Dispose();
		}
		public bool Equals(IOWrapper obj)
		{
			return base.Equals(obj);
		}
		private Stream OpenStream;
		private ReaderWrapper Reader;
		private WriterWrapper Writer;
		private Endian Endianness;
	}
}

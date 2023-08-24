using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
namespace IOTools
{
	public class ReaderWrapper : IDisposable, ICloneable, IEquatable<ReaderWrapper>
	{
		public ReaderWrapper(byte[] Buffer, Endian EndianType = Endian.Little, long StartingPosition = 0L)
		{
			OpenStream = null;
			binaryReader = null;
			LastPosition = 0L;
			IsOwner = false;
			StreamType = StreamType.MemoryStream;
			OpenStream = new MemoryStream(Buffer);
			binaryReader = new BinaryReader(OpenStream);
			IsOwner = true;
			Position = Position;
			CurrentEndian = EndianType;
		}
		public ReaderWrapper(Stream Stream, Endian EndianType = Endian.Little, long StartingPosition = 0L)
		{
			OpenStream = null;
			binaryReader = null;
			LastPosition = 0L;
			IsOwner = false;
			if (!Stream.CanRead)
			{
				throw new InvalidDataException("Stream does not have read access");
			}
			StreamType = StreamType.GenericStream;
			OpenStream = Stream;
			binaryReader = new BinaryReader(OpenStream);
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
			binaryReader.BaseStream.Seek(Offset, SeekOrigin);
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
		public char PeekChar()
		{
			char result = ReadChar();
			Position = LastPosition;
			return result;
		}
		public char[] PeekChars(int Length)
		{
			char[] result = ReadChars(Length);
			Position = LastPosition;
			return result;
		}
		public bool PeekBoolean()
		{
			bool result = ReadBoolean();
			Position = LastPosition;
			return result;
		}
		public byte PeekByte()
		{
			return PeekBytes(1)[0];
		}
		public sbyte PeekSByte()
		{
			return checked((sbyte)PeekBytes(1)[0]);
		}
		public sbyte PeekInt8()
		{
			return PeekSByte();
		}
		public byte PeekUInt8()
		{
			return PeekByte();
		}
		public short PeekInt16()
		{
			return PeekInt16(CurrentEndian);
		}
		public ushort PeekUInt16()
		{
			return PeekUInt16(CurrentEndian);
		}
		public short PeekInt16(Endian EndianType)
		{
			return BitConverter.ToInt16(PeekBytes(2, EndianType), 0);
		}
		public ushort PeekUInt16(Endian EndianType)
		{
			return BitConverter.ToUInt16(PeekBytes(2, EndianType), 0);
		}
		public int PeekInt24()
		{
			return PeekInt24(CurrentEndian);
		}
		public uint PeekUInt24()
		{
			return PeekUInt24(CurrentEndian);
		}
		public int PeekInt24(Endian EndianType)
		{
			return Converters.BytesToInt24(PeekBytes(3, EndianType), false);
		}
		public uint PeekUInt24(Endian EndianType)
		{
			return Converters.BytesToUInt24(PeekBytes(3, EndianType), false);
		}
		public int PeekInt32()
		{
			return PeekInt32(CurrentEndian);
		}
		public uint PeekUInt32()
		{
			return PeekUInt32(CurrentEndian);
		}
		public int PeekInt32(Endian EndianType)
		{
			return BitConverter.ToInt32(PeekBytes(4, EndianType), 0);
		}
		public uint PeekUInt32(Endian EndianType)
		{
			return BitConverter.ToUInt32(PeekBytes(4, EndianType), 0);
		}
		public long PeekInt40()
		{
			return PeekInt40(CurrentEndian);
		}
		public ulong PeekUInt40()
		{
			return PeekUInt40(CurrentEndian);
		}
		public long PeekInt40(Endian EndianType)
		{
			return Converters.BytesToInt40(PeekBytes(5, EndianType), false);
		}
		public ulong PeekUInt40(Endian EndianType)
		{
			return Converters.BytesToUInt40(PeekBytes(5, EndianType), false);
		}
		public long PeekInt48()
		{
			return PeekInt48(CurrentEndian);
		}
		public ulong PeekUInt48()
		{
			return PeekUInt48(CurrentEndian);
		}
		public long PeekInt48(Endian EndianType)
		{
			return Converters.BytesToInt48(PeekBytes(6, EndianType), false);
		}
		public ulong PeekUInt48(Endian EndianType)
		{
			return checked((ulong)Converters.BytesToInt48(PeekBytes(6, EndianType), false));
		}
		public long PeekInt56()
		{
			return PeekInt56(CurrentEndian);
		}
		public ulong PeekUInt56()
		{
			return PeekUInt56(CurrentEndian);
		}
		public long PeekInt56(Endian EndianType)
		{
			return Converters.BytesToInt56(PeekBytes(7, EndianType), false);
		}
		public ulong PeekUInt56(Endian EndianType)
		{
			return checked((ulong)Converters.BytesToInt56(PeekBytes(7, EndianType), false));
		}
		public long PeekInt64()
		{
			return PeekInt64(CurrentEndian);
		}
		public ulong PeekUInt64()
		{
			return PeekUInt64(CurrentEndian);
		}
		public long PeekInt64(Endian EndianType)
		{
			return BitConverter.ToInt64(PeekBytes(8, EndianType), 0);
		}
		public ulong PeekUInt64(Endian EndianType)
		{
			return BitConverter.ToUInt64(PeekBytes(8, EndianType), 0);
		}
		public float PeekSingle()
		{
			return PeekSingle(CurrentEndian);
		}
		public double PeekDouble()
		{
			return PeekDouble(CurrentEndian);
		}
		public float PeekSingle(Endian EndianType)
		{
			return BitConverter.ToSingle(PeekBytes(4, EndianType), 0);
		}
		public double PeekDouble(Endian EndianType)
		{
			return BitConverter.ToDouble(PeekBytes((int)EndianType), 0);
		}
		public byte[] PeekBytes(int Length)
		{
			return PeekBytes(Length, CurrentEndian);
		}
		public byte[] PeekBytes(int Length, Endian EndianType)
		{
			byte[] result = ReadBytes(Length, EndianType);
			Position = LastPosition;
			return result;
		}
		public string PeekBinary(int Length)
		{
			return PeekBinary(Length, CurrentEndian);
		}
		public string PeekBinary(int Length, Endian EndianType)
		{
			byte[] bytes = ReadBytes(Length, EndianType);
			Position = LastPosition;
			return Converters.BytesToBitString(bytes);
		}
		public string PeekASCII(int Length)
		{
			string result = ReadASCII(Length);
			Position = LastPosition;
			return result;
		}
		public string PeekUnicode(int Length)
		{
			return PeekUnicode(Length, CurrentEndian);
		}
		public string PeekUnicode(int Length, Endian EndianType)
		{
			string result = ReadUnicode(Length, EndianType);
			Position = LastPosition;
			return result;
		}
		public string PeekHex(int Length)
		{
			return PeekHex(Length, CurrentEndian);
		}
		public string PeekHex(int Length, Endian EndianType)
		{
			string result = ReadHex(Length, EndianType);
			Position = LastPosition;
			return result;
		}
		public bool ReadBoolean()
		{
			LastPosition = OpenStream.Position;
			return binaryReader.ReadBoolean();
		}
		public byte ReadByte()
		{
			LastPosition = OpenStream.Position;
			return binaryReader.ReadByte();
		}
		public sbyte ReadSByte()
		{
			LastPosition = OpenStream.Position;
			return binaryReader.ReadSByte();
		}
		public byte[] ReadBytes(int Length)
		{
			return ReadBytes(Length, CurrentEndian);
		}
		public byte[] ReadBytes(int Length, Endian EndianType)
		{
			LastPosition = OpenStream.Position;
			byte[] result = binaryReader.ReadBytes(Length);
			if (EndianType == Endian.Big)
			{
				Functions.SwapEndian(ref result);
			}
			return result;
		}
		public byte[] ReadAll()
		{
			return ReadAll(CurrentEndian);
		}
		public byte[] ReadAll(Endian EndianType)
		{
			Position = 0L;
			return ReadBytes(checked((int)Length), EndianType);
		}
		public byte[] ReadToEnd()
		{
			return ReadToEnd(CurrentEndian);
		}
		public byte[] ReadToEnd(Endian EndianType)
		{
			return ReadBytes(checked((int)(Length - Position)), EndianType);
		}
		public sbyte ReadInt8()
		{
			return ReadSByte();
		}
		public byte ReadUInt8()
		{
			return ReadByte();
		}
		public short ReadInt16()
		{
			return ReadInt16(CurrentEndian);
		}
		public short ReadInt16(Endian EndianType)
		{
			return BitConverter.ToInt16(ReadBytes(2, EndianType), 0);
		}
		public ushort ReadUInt16()
		{
			return ReadUInt16(CurrentEndian);
		}
		public ushort ReadUInt16(Endian EndianType)
		{
			return BitConverter.ToUInt16(ReadBytes(2, EndianType), 0);
		}
		public int ReadInt24()
		{
			return ReadInt24(CurrentEndian);
		}
		public int ReadInt24(Endian EndianType)
		{
			return Converters.BytesToInt24(ReadBytes(3, Endian.Little), EndianType == Endian.Big ? true : false);
		}
		public uint ReadUInt24()
		{
			return ReadUInt24(CurrentEndian);
		}
		public uint ReadUInt24(Endian EndianType)
		{
			return Converters.BytesToUInt24(ReadBytes(3, Endian.Little), EndianType == Endian.Big ? true : false);
		}
		public int ReadInt32()
		{
			return ReadInt32(CurrentEndian);
		}
		public int ReadInt32(Endian EndianType)
		{
			return BitConverter.ToInt32(ReadBytes(4, EndianType), 0);
		}
		public uint ReadUInt32()
		{
			return ReadUInt32(CurrentEndian);
		}
		public uint ReadUInt32(Endian EndianType)
		{
			return BitConverter.ToUInt32(ReadBytes(4, EndianType), 0);
		}
		public long ReadInt40()
		{
			return ReadInt40(CurrentEndian);
		}
		public long ReadInt40(Endian EndianType)
		{
			return Converters.BytesToInt40(ReadBytes(5, EndianType), false);
		}
		public ulong ReadUInt40()
		{
			return ReadUInt40(CurrentEndian);
		}
		public ulong ReadUInt40(Endian EndianType)
		{
			return Converters.BytesToUInt40(ReadBytes(5, EndianType), false);
		}
		public long ReadInt48()
		{
			return ReadInt48(CurrentEndian);
		}
		public long ReadInt48(Endian EndianType)
		{
			return Converters.BytesToInt48(ReadBytes(6, EndianType), false);
		}
		public ulong ReadUInt48()
		{
			return ReadUInt48(CurrentEndian);
		}
		public ulong ReadUInt48(Endian EndianType)
		{
			return Converters.BytesToUInt48(ReadBytes(6, EndianType), false);
		}
		public long ReadInt56()
		{
			return ReadInt56(CurrentEndian);
		}
		public long ReadInt56(Endian EndianType)
		{
			return Converters.BytesToInt56(ReadBytes(7, EndianType), false);
		}
		public ulong ReadUInt56()
		{
			return ReadUInt56(CurrentEndian);
		}
		public ulong ReadUInt56(Endian EndianType)
		{
			return Converters.BytesToUInt56(ReadBytes(7, EndianType), false);
		}
		public long ReadInt64()
		{
			return ReadInt64(CurrentEndian);
		}
		public long ReadInt64(Endian EndianType)
		{
			return BitConverter.ToInt64(ReadBytes(8, EndianType), 0);
		}
		public ulong ReadUInt64()
		{
			return ReadUInt64(CurrentEndian);
		}
		public ulong ReadUInt64(Endian EndianType)
		{
			return BitConverter.ToUInt64(ReadBytes(8, EndianType), 0);
		}
		public float ReadSingle()
		{
			return ReadSingle(CurrentEndian);
		}
		public float ReadSingle(Endian EndianType)
		{
			return BitConverter.ToSingle(ReadBytes(4, EndianType), 0);
		}
		public double ReadDouble()
		{
			return ReadDouble(CurrentEndian);
		}
		public double ReadDouble(Endian EndianType)
		{
			return BitConverter.ToDouble(ReadBytes(8, EndianType), 0);
		}
		public char ReadChar()
		{
			LastPosition = OpenStream.Position;
			return binaryReader.ReadChar();
		}
		public char[] ReadChars(int Length)
		{
			LastPosition = OpenStream.Position;
			return binaryReader.ReadChars(Length);
		}
		public string ReadBinary(int Length)
		{
			return ReadBinary(Length, CurrentEndian);
		}
		public string ReadBinary(int Length, Endian EndianType)
		{
			byte[] bytes = ReadBytes(Length, CurrentEndian);
			return Converters.BytesToBitString(bytes);
		}
		public string ReadASCII(int Length)
		{
			return Encoding.ASCII.GetString(ReadBytes(Length, Endian.Little));
		}
		public string ReadUnicode(int Length)
		{
			return ReadUnicode(Length, CurrentEndian);
		}
		public string ReadUnicode(int Length, Endian EndianType)
		{
			byte[] bytes = ReadBytes(Length, Endian.Little);
			if (EndianType == Endian.Big)
			{
				return Encoding.BigEndianUnicode.GetString(bytes);
			}
			return Encoding.Unicode.GetString(bytes);
		}
		public string ReadHex(int Length)
		{
			return ReadHex(Length, CurrentEndian);
		}
		public string ReadHex(int Length, Endian EndianType)
		{
			return Converters.ObjectToHex(ReadBytes(Length, EndianType));
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
				return CurrentEndian == Endian.Little ? Endian.Big : Endian.Little;
			}
		}
        public bool IsOpen
		{
			get
			{
				if (binaryReader != null)
				{
					if (OpenStream != null)
					{
						return true;
					}
				}
				return false;
			}
		}
        public void Close(bool CloseUnderlying = true, bool Dispose = false)
		{
			if (IsOpen)
			{
				binaryReader.Close();
				binaryReader = null;
				if (CloseUnderlying)
				{
					OpenStream.Close();
					OpenStream = null;
				}
			}
			if (Dispose)
			{
				this.Dispose();
			}
		}
		public object Clone()
		{
			return (ReaderWrapper)MemberwiseClone();
		}
		public void Dispose()
		{
			GC.Collect(GC.GetGeneration(this), GCCollectionMode.Forced);
		}
		public bool Equals(ReaderWrapper obj)
		{
			return base.Equals(obj);
		}
		private Stream OpenStream;
        private BinaryReader binaryReader;
        private Endian Endianness;
    }
}

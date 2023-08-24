using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace IOTools {
    public class Functions
	{
		public static void FillBuffer([In] [Out] ref byte[] Buffer, ulong Index, byte FillWith = 0)
		{
			Functions.FillBuffer(ref Buffer, Index, (long)Buffer.Length, FillWith);
		}
		public static void FillBuffer([In] [Out] ref byte[] Buffer, ulong Index, long Length, byte FillWith = 0)
		{
			checked
			{
				long num = (long)Index;
				long num2 = Length - 1L;
				for (long num3 = num; num3 <= num2; num3 += 1L)
				{
					Buffer[(int)num3] = FillWith;
				}
			}
		}
		public static byte[] DeleteBytes(byte[] Buffer, uint Index, uint Length)
		{
			Array.Copy(Buffer, (long)((ulong)(checked(Index + Length))), Buffer, (long)((ulong)Index), checked(unchecked((long)Buffer.Length) - (long)(unchecked((ulong)(checked(Index + Length))))));
			Array.Resize<byte>(ref Buffer, checked((int)(unchecked((long)Buffer.Length) - (long)(unchecked((ulong)Length)))));
			return Buffer;
		}
		public static void CombineArrays(ref Array TargetArray, params Array[] SourceArrays)
		{
			checked
			{
				if (SourceArrays != null)
				{
					if (SourceArrays.Length == 0)
					{
						return;
					}
					if (TargetArray == null)
					{
						TargetArray = (Array)new object();
					}
					int num = 0;
					int num2 = SourceArrays.Length - 1;
					for (int i = num; i <= num2; i++)
					{
						if (SourceArrays[i] != null && SourceArrays[i].Length != 0)
						{
							object[] array = (object[])TargetArray;
							Array.Resize<object>(ref array, TargetArray.Length + SourceArrays[i].Length);
							TargetArray = array;
							Array.Copy(SourceArrays[i], 0, TargetArray, TargetArray.Length - SourceArrays[i].Length, SourceArrays[i].Length);
						}
					}
				}
			}
		}
		public static byte[] SwapEndian(ref byte[] Buffer)
		{
			Array array = Buffer;
			Array array2 = ReverseArray(ref array);
			Buffer = (byte[])array;
			return (byte[])array2;
		}
		public static Array ReverseArray(ref Array Buffer)
		{
			Array.Reverse(Buffer);
			return Buffer;
		}
		public static void StreamToBuffer(ref Stream Stream, ref byte[] OutBuffer)
		{
			checked
			{
				if (Stream != null)
				{
					if (Stream.Length == 0L)
					{
						return;
					}
					if (!Stream.CanRead)
					{
						return;
					}
					Array.Resize<byte>(ref OutBuffer, (int)Stream.Length);
					BinaryReader binaryReader = new BinaryReader(Stream);
					long position = Stream.Position;
					binaryReader.BaseStream.Position = 0L;
					long num = 0L;
					long num2 = binaryReader.BaseStream.Length - 1L;
					for (long num3 = num; num3 <= num2; num3 += 1L)
					{
						OutBuffer[(int)num3] = binaryReader.ReadByte();
					}
					Stream.Position = position;
				}
			}
		}
		public static bool IsValidHex(string Hex)
		{
			return new Regex("^[A-Fa-f0-9]*$", RegexOptions.IgnoreCase).IsMatch(Hex);
		}
		public static bool IsValidUnicode(string Unicode)
		{
			return new Regex("^(\\u0009|[\\u0020-\\u007E]|\\u0085|[\\u00A0-\\uD7FF]|[\\uE000-\\uFFFD])+$", RegexOptions.IgnoreCase).IsMatch(Unicode);
		}
		public static bool IsValidASCII(string String)
		{
			return new Regex("^([\\x00-\\xff]*)$", RegexOptions.IgnoreCase).IsMatch(String);
		}
		public static bool IsValidBitString(string BitString)
		{
			return new Regex("^[0-1]+\\d").IsMatch(BitString);
		}
		public static string ConvertBytesToHumanString(double ByteCount, int DecimalPlaces = 2)
		{
			string[] byteExtensions = {
				"bytes",
				"KB",
				"MB",
				"GB",
				"TB",
				"PB",
				"EB",
				"ZB",
				"YB"
			};
			int divAmount = 0;
			do {
				ByteCount = Convert.ToDouble(Math.Round(new decimal(ByteCount / 1024.0), DecimalPlaces));
				divAmount += 1;
			}
			while (ByteCount > 1024.0);
			return byteExtensions[divAmount];
		}
		public static object PowMin(int Bits, bool Signed = true)
		{
			if (!Signed) return 0;
			return checked((long)Math.Round(Math.Round(unchecked(-Math.Pow(2.0, (double)(checked(Bits - 1)))), 1)));
		}
		public static object PowMax(int Bits, bool Signed = true)
		{
			object value = Math.Round(Math.Pow(2.0, Conversions.ToDouble(Interaction.IIf(Signed, checked(Bits - 1), Bits))) - 1.0, 1);
			if (Signed) return Conversions.ToLong(value);
			return Conversions.ToULong(value);
		}
	}
}

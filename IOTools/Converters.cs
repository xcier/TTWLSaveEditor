using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Runtime.CompilerServices;
using System.Text;
namespace IOTools
{
	public class Converters
	{
		public static string BytesToASCII(byte[] Bytes)
		{
			return Encoding.ASCII.GetString(Bytes);
		}
		public static byte[] ASCIIToBytes(string Value)
		{
			if (!Functions.IsValidASCII(Value))
			{
				throw new Exception("Invalid ASCII input");
			}
			return Encoding.ASCII.GetBytes(Value);
		}
		public static string BytesToUnicode(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				return Encoding.BigEndianUnicode.GetString(Bytes);
			}
			return Encoding.Unicode.GetString(Bytes);
		}
		public static byte[] UnicodeToBytes(string Value, bool ReturnBigEndian = false)
		{
			if (!Functions.IsValidUnicode(Value))
			{
				throw new Exception("Invalid unicode input");
			}
			if (ReturnBigEndian)
			{
				return Encoding.BigEndianUnicode.GetBytes(Value);
			}
			return Encoding.Unicode.GetBytes(Value);
		}
		public static string BytesToBase64(byte[] Bytes)
		{
			return Convert.ToBase64String(Bytes);
		}
		public static byte[] Base64ToBytes(string ASCII)
		{
			return Convert.FromBase64String(ASCII);
		}
		public static string Base64ToASCII(string Base64)
		{
			return Converters.BytesToASCII(Converters.Base64ToBytes(Base64));
		}
		public static string ASCIIToBase64(string ASCII)
		{
			return Converters.BytesToBase64(Converters.ASCIIToBytes(ASCII));
		}
		public static byte[] HexToBytes(string Value)
		{
			if (Value == null || Value.Length == 0 || !Functions.IsValidHex(Value)) throw new Exception("Invalid Hex Input");
			checked
			{
				byte[] array = new byte[(int)Math.Round(unchecked((double)Value.Length / 2.0 - 1.0)) + 1];
				int num = 0;
				int num2 = (int)Math.Round(unchecked((double)Value.Length / 2.0 - 1.0));
				for (int i = num; i <= num2; i++)
				{
					array[i] = (byte)Math.Round(Conversion.Val("&h" + Value.Substring(i * 2, 2)));
				}
				return array;
			}
		}
		public static string BytesToHex(byte[] Bytes)
		{
			return ObjectToHex(Bytes);
		}
		public static string ObjectToHex(object Value)
		{
			if(Value.GetType() == typeof(byte[]) || Value.GetType() == typeof(Byte[])) return BitConverter.ToString((byte[])Value).Replace("-", "");
			return Conversion.Hex(RuntimeHelpers.GetObjectValue(Value));
		}
		public static short BytesToInt16(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Bytes = Functions.SwapEndian(ref Bytes);
			}
			return BitConverter.ToInt16(Bytes, 0);
		}
		public static byte[] Int16ToBytes(short Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian)
			{
				result = Functions.SwapEndian(ref result);
			}
			return result;
		}
		public static ushort BytesToUInt16(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Bytes = Functions.SwapEndian(ref Bytes);
			}
			return BitConverter.ToUInt16(Bytes, 0);
		}
		public static byte[] UInt16ToBytes(ushort Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian)
			{
				result = Functions.SwapEndian(ref result);
			}
			return result;
		}
		public static uint BytesToUInt24(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 3)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			uint num = Conversions.ToUInteger(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(24, false), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(24, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 24 bit integer. MaxValue = ", Functions.PowMax(24, false)), "MinValue = "), Functions.PowMin(24, false)))));
			}
			return num;
		}
		public static int BytesToInt24(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 3)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			int num = Conversions.ToInteger(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(24, true), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(24, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 24 bit integer. MaxValue = ", Functions.PowMax(24, true)), "MinValue = "), Functions.PowMin(24, true)))));
			}
			return num;
		}
		public static byte[] Int24ToBytes(int Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(24, true), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(24, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 24 bit integer. MaxValue = ", Functions.PowMax(24, true)), "MinValue = "), Functions.PowMin(24, true)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static byte[] UInt24ToBytes(uint Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(24, false), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(24, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 24 bit integer. MaxValue = ", Functions.PowMax(24, false)), "MinValue = "), Functions.PowMin(24, false)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static int BytesToInt32(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Array.Reverse(Bytes);
			}
			return BitConverter.ToInt32(Bytes, 0);
		}
		public static byte[] Int32ToBytes(int Value, bool ReturnBigEndian = false)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			if (ReturnBigEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}
		public static uint BytesToUInt32(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Bytes = Functions.SwapEndian(ref Bytes);
			}
			return BitConverter.ToUInt32(Bytes, 0);
		}
		public static byte[] UInt32ToBytes(uint Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian)
			{
				result = Functions.SwapEndian(ref result);
			}
			return result;
		}
		public static ulong BytesToUInt40(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 5)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			ulong num = Conversions.ToULong(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(40, false), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(40, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 40 bit integer. MaxValue = ", Functions.PowMax(40, false)), "MinValue = "), Functions.PowMin(40, false)))));
			}
			return num;
		}
		public static long BytesToInt40(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 5)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			long num = Conversions.ToLong(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(40, true), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(40, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 40 bit integer. MaxValue = ", Functions.PowMax(40, true)), "MinValue = "), Functions.PowMin(40, true)))));
			}
			return num;
		}
		public static byte[] Int40ToBytes(long Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(40, true), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(40, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 40 bit integer. MaxValue = ", Functions.PowMax(40, true)), "MinValue = "), Functions.PowMin(40, true)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[4],
				array[3],
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static byte[] UInt40ToBytes(ulong Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(40, false), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(40, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 40 bit integer. MaxValue = ", Functions.PowMax(40, false)), "MinValue = "), Functions.PowMin(40, false)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[4],
				array[3],
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static ulong BytesToUInt48(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 6)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			ulong num = Conversions.ToULong(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(48, false), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(48, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 48 bit integer. MaxValue = ", Functions.PowMax(48, false)), "MinValue = "), Functions.PowMin(48, false)))));
			}
			return num;
		}
		public static long BytesToInt48(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 6)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			long num = Conversions.ToLong(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(48, true), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(48, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 48 bit integer. MaxValue = ", Functions.PowMax(48, true)), "MinValue = "), Functions.PowMin(48, true)))));
			}
			return num;
		}
		public static byte[] Int48ToBytes(long Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(48, true), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(48, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 48 bit integer. MaxValue = ", Functions.PowMax(48, true)), "MinValue = "), Functions.PowMin(48, true)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[5],
				array[4],
				array[3],
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static byte[] UInt48ToBytes(ulong Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(48, false), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(48, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 48 bit integer. MaxValue = ", Functions.PowMax(48, false)), "MinValue = "), Functions.PowMin(48, false)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[5],
				array[4],
				array[3],
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static ulong BytesToUInt56(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 7)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			ulong num = Conversions.ToULong(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(56, false), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(56, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 56 bit integer. MaxValue = ", Functions.PowMax(56, false)), "MinValue = "), Functions.PowMin(56, false)))));
			}
			return num;
		}
		public static long BytesToInt56(byte[] Buffer, bool ReturnBigEndian = false)
		{
			if (Buffer == null || Buffer.Length != 7)
			{
				throw new NullReferenceException("Input byte array is null or does not contain enough bytes to convert");
			}
			long num = Conversions.ToLong(Converters.Bytes2Int(Buffer, ReturnBigEndian));
			if (Operators.ConditionalCompareObjectGreater(num, Functions.PowMax(56, true), false) || Operators.ConditionalCompareObjectLess(num, Functions.PowMin(56, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 56 bit integer. MaxValue = ", Functions.PowMax(56, true)), "MinValue = "), Functions.PowMin(56, true)))));
			}
			return num;
		}
		public static byte[] Int56ToBytes(long Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(56, true), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(56, true), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as a signed 56 bit integer. MaxValue = ", Functions.PowMax(56, true)), "MinValue = "), Functions.PowMin(56, true)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[6],
				array[5],
				array[4],
				array[3],
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian)
			{
				Array.Reverse(array);
			}
			return array;
		}
		public static byte[] UInt56ToBytes(ulong Value, bool ReturnBigEndian = false)
		{
			if (Operators.ConditionalCompareObjectGreater(Value, Functions.PowMax(56, false), false) || Operators.ConditionalCompareObjectLess(Value, Functions.PowMin(56, false), false))
			{
				throw new OverflowException(new string((char[])Conversions.ToCharArrayRankOne(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Value is to low or to high to be represented as an unsigned 56 bit integer. MaxValue = ", Functions.PowMax(56, false)), "MinValue = "), Functions.PowMin(56, false)))));
			}
			byte[] array = BitConverter.GetBytes(Value);
			array = new byte[]
			{
				array[6],
				array[5],
				array[4],
				array[3],
				array[2],
				array[1],
				array[0]
			};
			if (ReturnBigEndian) Array.Reverse(array);
			return array;
		}
		public static long BytesToInt64(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian) Bytes = Functions.SwapEndian(ref Bytes);
			return BitConverter.ToInt64(Bytes, 0);
		}
		public static byte[] Int64ToBytes(long Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian) result = Functions.SwapEndian(ref result);
			return result;
		}
		public static ulong BytesToUInt64(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Bytes = Functions.SwapEndian(ref Bytes);
			}
			return BitConverter.ToUInt64(Bytes, 0);
		}
		public static byte[] UInt64ToBytes(ulong Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian) result = Functions.SwapEndian(ref result);
			return result;
		}
		public static float BytesToSingle(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Bytes = Functions.SwapEndian(ref Bytes);
			}
			return BitConverter.ToSingle(Bytes, 0);
		}
		public static byte[] SingleToBytes(float Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian)
			{
				result = Functions.SwapEndian(ref result);
			}
			return result;
		}
		public static double BytesToDouble(byte[] Bytes, bool ReturnBigEndian = false)
		{
			if (ReturnBigEndian)
			{
				Bytes = Functions.SwapEndian(ref Bytes);
			}
			return BitConverter.ToDouble(Bytes, 0);
		}
		public static byte[] DoubleToBytes(double Value, bool ReturnBigEndian = false)
		{
			byte[] result = BitConverter.GetBytes(Value);
			if (ReturnBigEndian)
			{
				result = Functions.SwapEndian(ref result);
			}
			return result;
		}
		private static object Bytes2Int(byte[] Buffer, bool Big = false)
		{
			if (Big)
			{
				Array.Reverse(Buffer);
			}
			object obj = 0;
			int num = 0;
			checked
			{
				int num2 = Buffer.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					obj = Operators.AddObject(obj, (long)(unchecked((ulong)Buffer[i]) * (ulong)((long)Math.Round(Math.Pow(256.0, (double)i)))));
				}
				return obj;
			}
		}
		public static string Int32ToBitString(int Value)
		{
			string text = null;
			do
			{
				text += Convert.ToString(Value % 2);
				Value /= 2;
			}
			while (Value >= 1);
			return text;
		}
		public static int BitStringToInt32(string Value)
		{
			if (Value == null || Value.Length == 0 || !Functions.IsValidBitString(Value)) throw new Exception("Invalid bitstring");
			if (Value.Length % 8 != 0) throw new Exception("Bitstring must be divisible by 8");
			long num = 0L;
			int num2 = 0;
			checked
			{
				int num3 = Strings.Len(Value) - 1;
				for (int i = num2; i <= num3; i++)
				{
					num = (long)Math.Round(unchecked((double)num + Conversion.Val(Strings.Mid(Value, checked(Strings.Len(Value) - i + 1), 1)) * Math.Pow(2.0, (double)(checked(i - 1)))));
				}
				return (int)num;
			}
		}
		public static string BytesToBitString(byte[] Bytes)
		{
			string text = null;
			int num = 0;
			checked
			{
				int num2 = Bytes.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					text += Convert.ToString(Bytes[i], 2).PadLeft(8, '0');
				}
				return text;
			}
		}
		public static byte[] BitStringToBytes(string Value)
		{
			if (Value == null || Value.Length == 0 || !Functions.IsValidBitString(Value)) throw new Exception("Invalid bitstring");
			if (Value.Length % 8 != 0) throw new Exception("Bitstring must be divisible by 8");
			checked
			{
				byte[] array = new byte[(int)Math.Round((double)Value.Length / 8.0) + 1];
				int num = 0;
				int num2 = (int)Math.Round(unchecked((double)Value.Length / 8.0 - 1.0));
				for (int i = num; i <= num2; i++)
				{
					array[i] = Convert.ToByte(Value.Substring(i * 8, 8), 2);
				}
				return array;
			}
		}
	}
}

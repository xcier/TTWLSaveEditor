using System;
using System.IO;
using System.Linq;

namespace IOTools {

    public class BitReader {
        private readonly IOWrapper stream;

        public int BitLength { get; private set; }
        public int BitPosition { get; private set; }

        public override string ToString() {
            // Read the IO to the end
            long originalPosition = stream.Position;
            // Read the remaining data
            byte[] arr = stream.ReadToEnd();
            Array.Reverse(arr);
            // Seek back to the original position
            stream.Seek(originalPosition);

            // Convert to a binary string
            return Converters.BytesToBitString(arr);
        }

        public BitReader(byte[] buffer) {
            this.stream = new IOWrapper(buffer, Endian.Big, 0);
            this.BitLength = (int)(stream.LengthToEnd * 8);
        }

        public BitReader(IOWrapper str) {
            byte[] data = str.GetBuffer().Skip((int)str.Position).ToArray();

            this.stream = new IOWrapper(data, str.CurrentEndian, 0);
            this.BitLength = (int)(stream.LengthToEnd * 8);
            this.BitPosition = 0;
        }

        public int BitsRemaining() {
            int bitsRemaining = (BitLength - BitPosition);
            return bitsRemaining;
        }

        public byte ReadByte(int bits) {
            return (byte)ReadUInt32(bits);
        }

        public ushort ReadUInt16(int bits) {
            return (ushort)ReadUInt32(bits);
        }

        public int ReadInt32(int bits) {
            return (int)ReadUInt32(bits);
        }

        public uint[] ReadToEnd() {
            int bytesRemaining = BitsRemaining() % 8;
            uint[] arr = new uint[bytesRemaining];
            for(int i = 0; i < bytesRemaining; i++) {
                arr[i] = ReadUInt32(8);
            }

            return arr;
        }

        public uint ReadUInt32(int bits) {
            // All credits to Rick/Gibbed: <https://github.com/gibbed/Gibbed.Gearbox/blob/master/Gibbed.Gearbox.Common/BitReader.cs> for this logic.
            // I have a chronic case of being bad at math so I uhh just wouldn't have

            if (bits < 0 || bits > 32) throw new ArgumentOutOfRangeException(nameof(bits));
            uint result = 0;
            int shift = 0;
            byte[] buffer = stream.GetBuffer();

            while (bits > 0) {
                if (BitPosition >= BitLength) throw new EndOfStreamException();

                int offset = BitPosition % 8;
                int left = Math.Min(8 - offset, bits);

                var mask = (uint)(1 << left) - 1;
                var value = (uint)(buffer[BitPosition >> 3] >> offset);

                bits -= left;

                result |= (mask & value) << shift;
                shift += left;

                BitPosition += left;
            }


            return result;
        }
    }
}

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CustomPipeline
{
    public class KanjiByteConverter
    {
        private const int _ByteSize = 256;

        private static readonly int _KanjiStartChar = 0x4E00;

        private static readonly string[] _ByteValues = new string[_ByteSize];
        private static readonly char[] _KanjiByteValues = new char[_ByteSize];

        static KanjiByteConverter()
        {
            for (int i = 0; i < _ByteSize; i++)
            {
                _ByteValues[i] = i.ToString("X2");
                _KanjiByteValues[i] = (char)(_KanjiStartChar + i);
            }
        }

        public static string ToText(byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                result.Append(_ByteValues[b]);
            }
            return result.ToString();
        }
        public static byte[] FromText(string text)
        {
            byte[] bytes = new byte[text.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ToByte(text, i * 2, 2);
            }
            return bytes;
        }
        private static byte ToByte(string text, int index, int length)
        {
            if (index < 0 || index + length > text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(text));
            }
            return (byte)((ToByte(text[index]) << 4) | ToByte(text[index + 1]));
        }
        private static byte ToByte(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return (byte)(c - '0');
            }
            if (c >= 'A' && c <= 'F')
            {
                return (byte)(c - 'A' + 10);
            }
            if (c >= 'a' && c <= 'f')
            {
                return (byte)(c - 'a' + 10);
            }
            throw new ArgumentOutOfRangeException(nameof(c));
        }

        public static string ToKanjiText(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];
            int index = 0;
            foreach (byte b in bytes)
            {
                chars[index++] = _KanjiByteValues[b];
            }
            return new string(chars);
        }
        public static byte[] FromKanjiText(string text)
        {
            byte[] bytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                int tmp = text[i] - _KanjiStartChar;
                if (tmp < 0 || tmp >= _ByteSize)
                {
                    throw new ArgumentOutOfRangeException(nameof(text));
                }
                bytes[i] = (byte)tmp;
            }
            return bytes;
        }

        public static byte[] Compression(byte[] bytes)
        {
            using var compressedStream = new MemoryStream();
            using var gzipStream = new GZipStream(compressedStream, CompressionLevel.SmallestSize);

            gzipStream.Write(bytes, 0, bytes.Length);
            gzipStream.Flush();

            return compressedStream.ToArray();
        }

        public static byte[] Decompression(byte[] bytes)
        {
            using var compressedStream = new MemoryStream(bytes);
            using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();

            gzipStream.CopyTo(resultStream);
            gzipStream.Flush();

            return resultStream.ToArray();
        }
    }
}

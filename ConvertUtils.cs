using System;
using System.Linq;
using System.Text;

namespace Des
{
    public static class ConvertUtils
    {
        public static byte[] BinaryStringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length / 8];

            int j = 0;
            while (str.Length > 0)
            {
                var result = Convert.ToByte(str.Substring(0, 8), 2);
                bytes[j++] = result;
                if (str.Length >= 8)
                    str = str[8..];
            }

            return bytes;
        }

        public static string ByteToBinaryString(byte b)
        {
            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0, n = Convert.ToInt32(b); n > 0; i++)
            {
                bits[i] = n % 2;
                n = n / 2;
            }

            return string.Join("", bits.Reverse());
        }

        public static string IntegerToBinaryString(int n)
        {
            int i;
            int[] a = { 0, 0, 0, 0, 0, 0, 0, 0 };
            string binary = "";

            for (i = 0; n > 0; i++)
            {
                a[i] = n % 2;
                n = n / 2;
            }
            for (i = 3; i >= 0; i--)
            {
                binary += a[i];
            }
            return binary;
        }

        public static byte[] AsciiStringToByteArray(string asciiString)
        {
            return Encoding.ASCII.GetBytes(asciiString);
        }

        public static string BytesToBinaryString(byte[] bytes)
        {
            return string.Join(
                "", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }
    }
}
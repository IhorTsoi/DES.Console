using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Des
{
    public static class OutputUtils
    {
        public static void PrintBadKeyAlert()
        {
            Console.WriteLine("The key provided is not good enough.");
        }

        public static void PrintInputs(byte[] textBytes, byte[] keyBytes)
        {
            Console.WriteLine(
                @$"[*] Inputs 
                text as ascii string: {OutputUtils.ToAsciiString(textBytes)}
                text as bytes: {OutputUtils.ToBytesString(textBytes)}
                key as ascii string: {OutputUtils.ToAsciiString(keyBytes)}
                key as bytes: {OutputUtils.ToBytesString(keyBytes)}");
            Console.WriteLine();
        }

        public static void PrintEncryptionResult(byte[] encryptedText)
        {
            Console.WriteLine();
            Console.WriteLine(
                @$"[*] Encrypted 
                text as ascii string: {OutputUtils.ToAsciiString(encryptedText)}
                text as bytes: {OutputUtils.ToBytesString(encryptedText)}");
            Console.WriteLine();
        }

        public static void PrintDecryptionResult(byte[] decryptedText)
        {
            Console.WriteLine();
            Console.WriteLine(
                @$"[*] Decrypted 
                text as ascii string: {OutputUtils.ToAsciiString(decryptedText)}
                text as bytes: {OutputUtils.ToBytesString(decryptedText)}");
            Console.WriteLine();
        }

        private static string ToBytesString(byte[] bytes, char separator = ' ') =>
            string.Join(
                separator,
                bytes.Select(b => b.ToString()));

        private static string ToAsciiString(byte[] bytes) =>
            Encoding.ASCII.GetString(bytes);

        internal static void PrintEntropy(IEnumerable<double> entropyMeasurements)
        {
            Console.WriteLine("[ " + string.Join("\t", entropyMeasurements) + " ]");
        }
    }
}
using System;
using System.Linq;

namespace Des
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputText = "qweqweqw";
            const string inputKey = "12345678";

            var textBytes = ConvertUtils.AsciiStringToByteArray(inputText);
            var keyBytes = ConvertUtils.AsciiStringToByteArray(inputKey);

            OutputUtils.PrintInputs(textBytes, keyBytes);

            var encryptedTextBytes = Encrypt(textBytes, keyBytes);
            OutputUtils.PrintEncryptionResult(encryptedTextBytes);

            var decryptedTextBytes = Decrypt(encryptedTextBytes, keyBytes);
            OutputUtils.PrintDecryptionResult(decryptedTextBytes);
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            var afterInitialPermutation = InitialPermutation(data);
            var afterTransformation = FeistelTransformation(afterInitialPermutation, key);
            var afterFinalPermutation = FinalPermutation(afterTransformation);

            return afterFinalPermutation;
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            var afterInitialPermutation = InitialPermutation(data);
            var afterTransformation = ReverseFeistelTransformation(afterInitialPermutation, key);
            var afterFinalPermutation = FinalPermutation(afterTransformation);

            return afterFinalPermutation;
        }

        static byte[] InitialPermutation(byte[] data)
        {
            var initialPermutationTable = DesConstants.InitialPermutationTable;

            var dataBits = ConvertUtils.BytesToBinaryString(data);

            var permutedBits = string.Join(
                "", initialPermutationTable.Select(i => dataBits[i - 1]));

            return ConvertUtils.BinaryStringToBytes(permutedBits);
        }

        public static byte[] FeistelTransformation(byte[] data, byte[] key)
        {
            var roundKeys = KeysGenerator.GetRoundKeys(key, DesConstants.RoundsCount);

            var (leftPart, rightPart) = (data[0..4], data[4..]);

            byte[] temp;
            foreach (var roundKey in roundKeys)
            {
                temp = rightPart;
                rightPart = ExclusiveOr(leftPart, F(rightPart, roundKey));
                leftPart = temp;
            }

            return leftPart.Concat(rightPart).ToArray();
        }

        public static byte[] ReverseFeistelTransformation(byte[] data, byte[] key)
        {
            var roundKeys = KeysGenerator.GetRoundKeys(key, DesConstants.RoundsCount)
                .Reverse();

            var (leftPart, rightPart) = (data[0..4], data[4..]);

            byte[] temp;
            foreach (var roundKey in roundKeys)
            {
                temp = leftPart;
                leftPart = ExclusiveOr(rightPart, F(leftPart, roundKey));
                rightPart = temp;
            }

            return leftPart.Concat(rightPart).ToArray();
        }

        public static byte[] F(byte[] blockPart, byte[] roundKey)
        {
            var sTables = DesConstants.FeistelTransformation.STables;

            var expandedBlockPart = Expand(blockPart);
            var blockPartXorRoundKey = ExclusiveOr(expandedBlockPart, roundKey);

            var xorBits = ConvertUtils.BytesToBinaryString(blockPartXorRoundKey);

            var transformedBits = string.Join(
                "",
                Enumerable.Range(0, 8)
                    .Select(i =>
                    {
                        var bits = xorBits.Substring(i * 6, 6);

                        return S(bits, sTables[i]);
                    }));

            var permutedBits = P(transformedBits);

            return permutedBits;
        }

        public static byte[] ExclusiveOr(byte[] dataBytes, byte[] keyBytes)
        {
            return dataBytes.Zip(keyBytes)
                .Select(pair => (byte)(pair.First ^ pair.Second))
                .ToArray();
        }

        public static byte[] Expand(byte[] blockPart)
        {
            var expansionTable = DesConstants.FeistelTransformation.ExpansionTable;

            var blockPartBits = ConvertUtils.BytesToBinaryString(blockPart);

            var expandedBlockPartBits = string.Join(
                "", expansionTable.Select(i => blockPartBits[i - 1]));

            return ConvertUtils.BinaryStringToBytes(expandedBlockPartBits);
        }

        public static string S(string blockPartBits, int[,] sTable)
        {
            var row = Convert.ToInt32($"{blockPartBits[0]}{blockPartBits[5]}", 2);
            var column = Convert.ToInt32(blockPartBits.Substring(1, 4), 2);

            var value = sTable[row, column];

            return ConvertUtils.IntegerToBinaryString(value);
        }

        public static byte[] P(string bits)
        {
            var pTable = DesConstants.FeistelTransformation.PTable;

            var permutedBits = string.Join(
                "", pTable.Select(i => bits[i - 1]));

            return ConvertUtils.BinaryStringToBytes(permutedBits);
        }

        static byte[] FinalPermutation(byte[] data)
        {
            var finalPermutationTable = DesConstants.FinalPermutationTable;

            var dataBits = ConvertUtils.BytesToBinaryString(data);

            var permutedBits = string.Join(
                "", finalPermutationTable.Select(i => dataBits[i - 1]));

            return ConvertUtils.BinaryStringToBytes(permutedBits);
        }
    }
}

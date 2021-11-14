using System.Linq;

namespace Des
{
    public static class KeysGenerator
    {
        public static byte[][] GetRoundKeys(byte[] key, int roundCount)
        {
            var shiftsTable = DesConstants.KeysGeneration.ShiftsTable;

            var roundKeys = new byte[roundCount][];

            var (cPart, dPart) = GetCPartAndDPart(key);
            for (int roundNumber = 0; roundNumber < roundCount; roundNumber++)
            {
                (cPart, dPart) = Shift(
                    cPart,
                    dPart,
                    shiftsTable[roundNumber % shiftsTable.Length]);

                roundKeys[roundNumber] = GetRoundKey(cPart, dPart);
            }

            return roundKeys;
        }

        static (string cPart, string dPart) GetCPartAndDPart(byte[] key)
        {
            var cTable = DesConstants.KeysGeneration.CTable;
            var dTable = DesConstants.KeysGeneration.DTable;

            var keyAsBinaryString = ConvertUtils.BytesToBinaryString(key);

            var cPart = string.Join(
                "", cTable.Select(cIndex => keyAsBinaryString[cIndex - 1]));
            var dPart = string.Join(
                "", dTable.Select(dIndex => keyAsBinaryString[dIndex - 1]));

            return (cPart, dPart);
        }

        static (string c, string s) Shift(string cPart, string dPart, int shift)
        {
            var cPartShifted = $"{cPart.Substring(shift)}{cPart.Substring(0, shift)}";
            var dPartShifted = $"{dPart.Substring(shift)}{dPart.Substring(0, shift)}";

            return (cPartShifted, dPartShifted);
        }

        static byte[] GetRoundKey(string cPart, string dPart)
        {
            var roundKeyTable = DesConstants.KeysGeneration.RoundKeyTable;

            var cd = $"{cPart}{dPart}";

            var roundKey = string.Join(
                "", roundKeyTable.Select(index => cd[index - 1]));

            return ConvertUtils.BinaryStringToBytes(roundKey);
        }
    }
}
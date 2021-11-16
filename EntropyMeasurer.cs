using System;
using System.Collections.Generic;
using System.Linq;

namespace Des
{
    public static class EntropyMeasurer
    {
        public static IEnumerable<double> MeasureEntropy(byte[] block)
        {
            for (int i = 0; i < DesConstants.BlockSize; i++)
            {
                var onesCount = ConvertUtils.ByteToBinaryString(block[i])
                    .Count(b => b == '1');
                int zerosCount = 8 - onesCount;

                var oneProbability = (double)onesCount / 8;
                var zeroProbability = (double)zerosCount / 8;

                var vOnes = Math.Log2(oneProbability) * oneProbability;
                var vZeros = Math.Log2(zeroProbability) * zeroProbability;

                var entropy = -1 * (
                    (double.IsNaN(vOnes) ? 0 : vOnes) +
                    (double.IsNaN(vZeros) ? 0 : vZeros));

                yield return Math.Round(entropy, 2);
            }
        }
    }
}
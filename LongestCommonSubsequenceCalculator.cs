using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            if (first.Count == 0 && second.Count == 0) return new int[0, 0];
            var result = new int[first.Count + 1, second.Count + 1];

            for (int i = 1; i < first.Count + 1; i++)
            {
                for (int j = 1; j < second.Count + 1; j++)
                {
                    if (first[i - 1] == second[j - 1]) result[i, j] = result[i - 1, j - 1] + 1;
                    else result[i, j] = Math.Max(result[i, j - 1], result[i - 1, j]);
                }
            }
            return result;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var result = new List<string>();

            for (int i = first.Count; i > 0;)
            {
                for (int j = second.Count; j > 0 && i > 0;)
                {
                    if (first[i - 1] == second[j - 1] && opt[i, j] != 0)
                    {
                        result.Add(first[i - 1]);
                        i--;
                        j--;
                    }

                    else if (opt[i, j - 1] >= opt[i - 1, j]) j--;
                    else i--;
                }
                break;
            }

            result.Reverse();
            return result;
        }
    }
}
using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;
using System.Collections;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var result = new List<ComparisonResult>();

            for (int i = 0; i < documents.Count; i++)
            {
                for (int j = i + 1; j < documents.Count; j++)
                {
                    var tokenCouple = new List<DocumentTokens> { documents[i], documents[j] };
                    var opt = new double[tokenCouple[0].Count + 1, tokenCouple[1].Count + 1];
                    for (var t = 0; t <= tokenCouple[0].Count; ++t) opt[t, 0] = t;
                    for (var r = 0; r <= tokenCouple[1].Count; ++r) opt[0, r] = r;
                    for (var q = 1; q <= tokenCouple[0].Count; ++q)
                    {
                        for (var d = 1; d <= tokenCouple[1].Count; ++d)
                        {
                            if (tokenCouple[0][q - 1] == tokenCouple[1][d - 1])
                                opt[q, d] = opt[q - 1, d - 1];
                            else
                                opt[q, d] = Math.Min(Math.Min(opt[q - 1, d] + 1.0, opt[q, d - 1] + 1.0),
                                    TokenDistanceCalculator.GetTokenDistance(tokenCouple[0][q - 1], tokenCouple[1][d - 1]));
                        }
                    }
                    result.Add(new ComparisonResult(tokenCouple[0], tokenCouple[1], opt[tokenCouple[0].Count, tokenCouple[1].Count]));
                }
            }

            //return new List<ComparisonResult> {
            //    new ComparisonResult(
            //        documents[0],
            //        documents[2],
            //        TokenDistanceCalculator.GetTokenDistance(documents[0][0], documents[2][1]))};

            return result;
        }
    }
}

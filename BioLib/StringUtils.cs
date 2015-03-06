using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib
{
    public static class StringUtils
    {
        public static Decimal HammingDistance(string a, string b)
        {
            if (a.Length != b.Length)
            {
                throw new Exception("Cannot compute Hamming distance - strings have different length");
            }

            Decimal distance = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    distance++;
                }
            }
            return distance;
        }

        public static int GlobalAlignmentScore(string a, string b, ScoringMatrix scoringMatrix, ref string verbose, bool printMatrix = true)
        {
            int[,] scoringGrid = new int[b.Length + 1, a.Length + 1];
            int[,] E = new int[b.Length + 1, a.Length + 1];
            int[,] F = new int[b.Length + 1, a.Length + 1];
            char[,] M = new char[b.Length + 1, a.Length + 1];

            for (int x = 0; x <= a.Length; x++)
            {
                scoringGrid[0, x] = E[0, x] = scoringMatrix.GapPenalty + scoringMatrix.ExtendPenalty * (x - 1);
                M[0, x] = 'x';
            }
            for (int y = 0; y <= b.Length; y++)
            {
                scoringGrid[y, 0] = F[y, 0] = scoringMatrix.GapPenalty + scoringMatrix.ExtendPenalty * (y - 1);
                M[y, 0] = 'x';
            }
            scoringGrid[0, 0] = 0;

            for (int x = 1; x <= a.Length; x++)
            {
                for (int y = 1; y <= b.Length; y++)
                {
                    char chA = a[x - 1], chB = b[y - 1];
                    int price = scoringMatrix.Prices[CharIndex(chB, scoringMatrix.Alphabet), CharIndex(chA, scoringMatrix.Alphabet)];

                    E[y, x] = Math.Max(E[y - 1, x] + scoringMatrix.ExtendPenalty, scoringGrid[y - 1, x] + scoringMatrix.GapPenalty);
                    F[y, x] = Math.Max(F[y, x - 1] + scoringMatrix.ExtendPenalty, scoringGrid[y, x - 1] + scoringMatrix.GapPenalty);
                    scoringGrid[y, x] = Utils.Max(E[y, x], F[y, x], scoringGrid[y - 1, x - 1] + price);
                    if (scoringGrid[y, x] == E[y, x])
                        M[y, x] = 'i';
                    else if (scoringGrid[y, x] == F[y, x])
                        M[y, x] = 'd';
                    else
                        M[y, x] = 'm';
                    /*scoringGrid[y, x] = Utils.Max(
                        scoringGrid[y - 1, x - 1] + price,
                        scoringGrid[y, x - 1] + scoringMatrix.GapPenalty,
                        scoringGrid[y - 1, x] + scoringMatrix.GapPenalty);*/
                }
            }

            string resA = a, resB = b;
            WalkBackwards(M, ref resA, ref resB);
            verbose = resA + "\n" + resB + "\n";

            if (printMatrix)
            {
                var sb = new StringBuilder();
                sb.Append("Result matrix:\n");
                PrintMatrix(scoringGrid, a, b, sb);
                sb.Append("\nE matrix:\n");
                PrintMatrix(E, a, b, sb);
                sb.Append("\nF matrix:\n");
                PrintMatrix(F, a, b, sb);
                sb.Append("\nMove matrix:\n");
                PrintMatrix(M, a, b, sb);
                verbose += sb.ToString();
            }

            return scoringGrid[b.Length, a.Length];
        }

        static void PrintMatrix(int[,] matrix, string a, string b, StringBuilder sb)
        {
            sb.Append(String.Format("{0, 4}{0, 4}", "-", "-"));
            for (int x = 0; x < a.Length; x++)
            {
                sb.Append(String.Format("{0, 4}", a[x]));
            }
            sb.Append("\n");

            for (int y = 0; y <= b.Length; y++)
            {
                sb.Append(String.Format("{0, 4}", y != 0 ? b[y - 1] : '-'));
                for (int x = 0; x <= a.Length; x++)
                {
                    sb.Append(String.Format("{0, 4}", matrix[y, x]));
                }
                sb.Append("\n");
            }
        }

        static void PrintMatrix(char[,] matrix, string a, string b, StringBuilder sb)
        {
            sb.Append(String.Format("{0, 4}{0, 4}", "-", "-"));
            for (int x = 0; x < a.Length; x++)
            {
                sb.Append(String.Format("{0, 4}", a[x]));
            }
            sb.Append("\n");

            for (int y = 0; y <= b.Length; y++)
            {
                sb.Append(String.Format("{0, 4}", y != 0 ? b[y - 1] : '-'));
                for (int x = 0; x <= a.Length; x++)
                {
                    sb.Append(String.Format("{0, 4}", matrix[y, x]));
                }
                sb.Append("\n");
            }
        }

        static int CountGaps(string s)
        {
            int res = 0;
            bool prevIsGap = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '-')
                {
                    if (!prevIsGap)
                    {
                        prevIsGap = true;
                        res++;
                    }
                }
                else
                {
                    prevIsGap = false;
                }
            }
            return res;
        }

        static void WalkBackwards(int[,] matrix, ref string resA, ref string resB)
        {
            string a = resA, b = resB;
            resA = "";
            resB = "";
            int pX = a.Length, pY = b.Length;
            while (pX != 0 && pY != 0)
            {
                int max = Utils.Max(matrix[pY - 1, pX - 1], matrix[pY, pX - 1], matrix[pY - 1, pX]);
                if (max == matrix[pY - 1, pX - 1])
                {
                    resA = a[pX - 1] + resA;
                    resB = b[pY - 1] + resB;

                    pY--;
                    pX--;
                }
                else if (max == matrix[pY - 1, pX])
                {
                    resA = "-" + resA;
                    resB = b[pY - 1] + resB;
                    pY--;
                }
                else
                {
                    resA = a[pX - 1] + resA;
                    resB = "-" + resB;
                    pX--;
                }
            }
        }
        static void WalkBackwards(char[,] matrix, ref string resA, ref string resB)
        {
            string a = resA, b = resB;
            resA = "";
            resB = "";
            int pX = a.Length, pY = b.Length;
            while (pX != 0 && pY != 0)
            {
                switch (matrix[pY, pX])
                {
                    case 'd':
                        resA = a[pX - 1] + resA;
                        resB = "-" + resB;
                        pX--;
                        break;
                    case 'i':
                        resA = "-" + resA;
                        resB = b[pY - 1] + resB;
                        pY--;
                        break;
                    default:
                        resA = a[pX - 1] + resA;
                        resB = b[pY - 1] + resB;
                        pY--;
                        pX--;
                        break;
                }
            }
        }
        static int CharIndex(char c, char[] alphabet)
        {
            return Array.IndexOf(alphabet, c);
        }

        public static int[] SubstringPositions(string s, string sub)
        {
            int pos = -1;
            var list = new List<int>();
            do
            {
                pos = s.IndexOf(sub, pos + 1);
                if (pos >= 0)
                    list.Add(pos);

            } while (pos != -1);
            return list.ToArray();
        }

        public static string RemoveSubstrings(string s, string[] substrings)
        {
            foreach (var sub in substrings)
            {
                s = s.Replace(sub, "");
            }
            return s;
        }

        public static int[] GetSubstringIndices(string s, string sub)
        {
            int[] result = new int[sub.Length];
            int pos = 0;
            for (int i = 0; i < s.Length && pos < sub.Length; i++)
            {
                if (s[i] == sub[pos])
                {
                    result[pos++] = i;
                }
            }
            return result;
        }
        public static string[] ReadFasta(string text)
        {
            return ReadFasta(text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string[] ReadFasta(string[] text)
        {
            var result = new List<string>();
            int pos = 0;
            while (pos < text.Length && text[pos][0] == '>')
            {
                string sequence = "";
                int temp = pos + 1;
                while (temp < text.Length && text[temp][0] != '>')
                {
                    sequence += text[temp];
                    temp++;
                }
                result.Add(sequence);
                pos = temp;
            }
            return result.ToArray();
        }

        public static string CommonSubstring(List<string> lines)
        {
            int len = lines[0].Length;
            int pos = 0;
            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i].Length < len)
                {
                    len = lines[i].Length;
                    pos = i;
                }
            }

            for (int size = len; size > 0; size--)
            {
                for (int i = 0; i < len - size + 1; i++)
                {
                    string sub = lines[pos].Substring(i, size);
                    bool ok = true;
                    for (int l = 0; l < lines.Count; l++)
                    {
                        if (l == pos) continue;
                        if (lines[l].IndexOf(sub) < 0)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                        return sub;
                }
            }
            return "ERROR";
            
            
            //return CommonSubstrings(strings).FirstOrDefault();
        }

        public static IEnumerable<string> CommonSubstrings(List<string> strings)
        {
            if (strings == null)
                throw new ArgumentNullException("strings");
            if (!strings.Any() || strings.Any(s => string.IsNullOrEmpty(s)))
                throw new ArgumentException("None string must be empty", "strings");

            var allSubstrings = new List<List<string>>();
            for (int i = 0; i < strings.Count; i++)
            {
                var substrings = new List<string>();
                string str = strings[i];
                for (int c = 0; c < str.Length - 1; c++)
                {
                    for (int cc = 1; c + cc <= str.Length; cc++)
                    {
                        string substr = str.Substring(c, cc);
                        if (allSubstrings.Count < 1 || allSubstrings.Last().Contains(substr))
                            substrings.Add(substr);
                    }
                }
                allSubstrings.Add(substrings);
            }
            if (allSubstrings.Last().Any())
            {
                var mostCommon = allSubstrings.Last()
                    .GroupBy(str => str)
                    .OrderByDescending(g => g.Key.Length)
                    .ThenByDescending(g => g.Count())
                    .Select(g => g.Key);
                return mostCommon;
            }
            return Enumerable.Empty<string>();
        }

        public static string LongestSubsequence(string a, string b, ref string resultMatrix)
        {
            var scoringGrid = new int[b.Length + 1, a.Length + 1];
            for (int x = 0; x <= a.Length; x++)
            {
                scoringGrid[0, x] = 0;
            }
            for (int y = 0; y <= b.Length; y++)
            {
                scoringGrid[y, 0] = 0;
            }

            for (int x = 1; x <= a.Length; x++)
            {
                for (int y = 1; y <= b.Length; y++)
                {
                    if (a[x - 1] == b[y - 1])
                    {
                        scoringGrid[y, x] = scoringGrid[y - 1, x - 1] + 1;
                    }
                    else
                    {
                        scoringGrid[y, x] = Math.Max(scoringGrid[y - 1, x], scoringGrid[y, x - 1]);
                    }
                }
            }

            var sb = new StringBuilder();
            for (int y = 0; y <= b.Length; y++)
            {
                for (int x = 0; x <= a.Length; x++)
                {
                    sb.Append(String.Format("{0, 3}", scoringGrid[y, x]));
                }
                sb.Append("\n");
            }
            resultMatrix = sb.ToString();

            string res = "";
            int pX = a.Length, pY = b.Length;
            while (pX != 0 && pY != 0)
            {
                if (a[pX - 1] == b[pY - 1])
                {
                    res = a[pX - 1] + res;
                    pX--;
                    pY--;
                }
                else
                {
                    if (scoringGrid[pY - 1, pX] > scoringGrid[pY, pX - 1])
                    {
                        pY--;
                    }
                    else
                    {
                        pX--;
                    }
                }
            }
            return res;
        }

        public static double GetHighestSymbolPercentage(string[] strings, char[] symbols, out string s)
        {
            s = "";
            double best = -1;
            foreach (var str in strings)
            {
                double t = GetSymbolsPercentage(str, symbols);
                if (t > best)
                {
                    best = t;
                    s = str;
                }
            }
            return best;
        }

        public static double GetSymbolsPercentage(string s, char[] symbols)
        {
            int total = 0;
            foreach (var c in s)
            {
                if (symbols.Contains(c))
                    total++;
            }
            return (double)total / s.Length;
        }

        // KMP
        public static int FindSubstring(string s, string pattern, out string verbose)
        {
            int pos = 0, i = 0;
            var pF = PrefixFunction(pattern);
            while (pos < s.Length && i < pattern.Length)
            {
                if (s[pos] == pattern[i])
                {
                    pos++;
                    i++;
                }
                else
                {
                    var shift = pF[i];
                    if (shift == 0)
                    {
                        pos++;
                        i = 0;
                    }
                    else
                    {
                        i = shift - 1;
                    }
                }
            }
            verbose = string.Join(" ", pF);
            return i == pattern.Length ? pos - pattern.Length : -1;
        }

        private static int[] PrefixFunction(string pattern)
        {
            int[] result = new int[pattern.Length];
            int prevResult = 0;

            for (int i = 1; i < result.Length; i++)
            {
                result[i] = prevResult = pattern[prevResult] == pattern[i] ? prevResult + 1 : 0;
            }
            return result;
        }
    }        
}

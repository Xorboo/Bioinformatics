using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib
{
    public static class Utils
    {
        public static string Permutation(int n, bool isSinged = false)
        {
            int[] numbers = new int[n];

            int totalCount = 0;
            string result = Permutate(numbers, n, 0, ref totalCount, isSinged);
            return totalCount + "\n" + result;
        }

        static string Permutate(int[] numbers, int n, int pos, ref int totalCount, bool isSinged)
        {
            string result = "";
            for (int i = 1; i <= n; i++)
            {
                if (!HasNumber(numbers, pos, i))
                {

                    numbers[pos] = i;
                    if (pos == numbers.Length - 1)
                    {
                        result += string.Join(" ", numbers) + "\n";
                        totalCount++;
                    }
                    else
                    {
                        result += Permutate(numbers, n, pos + 1, ref totalCount, isSinged);
                    }
                }

                if (isSinged && !HasNumber(numbers, pos, -i))
                {
                    numbers[pos] = -i;
                    if (pos == numbers.Length - 1)
                    {
                        result += string.Join(" ", numbers) + "\n";
                        totalCount++;
                    }
                    else
                    {
                        result += Permutate(numbers, n, pos + 1, ref totalCount, isSinged);
                    }
                }
            }
            numbers[pos] = 0;
            return result;
        }

        static bool HasNumber(int[] numbers, int pos, int i)
        {
            bool hasNumber = false;
            for (int j = 0; j < pos; j++)
            {
                if (numbers[j] == i || numbers[j] == -i)
                {
                    hasNumber = true;
                    break;
                }
            }
            return hasNumber;
        }

        public static string Permutation(char[] symbols, int length, bool allowDifferentLength = true) 
        {
            char[] word = new char[length];

            int totalCount = 0;
            string result = Permutate(word, length, 0, symbols, ref totalCount, allowDifferentLength);
            return totalCount + "\n" + result;
        }

        static string Permutate(char[] word, int n, int pos, char[] symbols, ref int totalCount, bool allowDifferentLength)
        {
            string result = "";
            for (int i = 0; i < symbols.Length; i++)
            {
                word[pos] = symbols[i];
                if (allowDifferentLength || pos == word.Length - 1)
                {
                    result += GetWord(word) + "\n";
                    totalCount++;
                }

                if (pos != word.Length - 1)
                {
                    result += Permutate(word, n, pos + 1, symbols, ref totalCount, allowDifferentLength);
                }
            }
            word[pos] = '\0';
            return result;
        }

        static string GetWord(char[] word)
        {
            string result = "";
            for (int i = 0; i < word.Length && word[i] != '\0'; i++)
            {
                result += word[i];
            }
            return result;
        }

        public static Decimal PartialPermutation(Decimal n, Decimal k, Decimal modula)
        {
            Decimal result = 1;
            for (Decimal i = n - k + 1; i <= n; i++)
            {
                result *= i;
                if (modula > 1)
                {
                    result %= modula;
                }
            }
            return result;
        }

        public static int Max(int x, int y, int z)
        {
            return Math.Max(x, Math.Max(y, z));
        }
        public static int Min(int x, int y, int z)
        {
            return Math.Min(x, Math.Min(y, z));
        }
    }
}

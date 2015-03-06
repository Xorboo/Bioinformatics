using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib
{
    public class ScoringMatrix
    {
        public char[] Alphabet;
        public int[,] Prices;
        public int GapPenalty;
        public int ExtendPenalty;

        public ScoringMatrix()
        {

        }

        public ScoringMatrix(string text, int penalty, int extendPenalty)
        {
            Init(text.Split('\n'), penalty, extendPenalty);
        }

        public ScoringMatrix(string[] lines, int penalty, int extendPenalty)
        {
            Init(lines, penalty, extendPenalty);
        }

        void Init(string[] lines, int penalty, int extendPenalty)
        {
            GapPenalty = penalty;
            ExtendPenalty = extendPenalty;

            string[] alphabet = lines[0].Split(new char[] { ' ', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Alphabet = new char[alphabet.Length];
            for (int i = 0; i < Alphabet.Length; i++)
            {
                Alphabet[i] = alphabet[i][0];
            }

            Prices = new int[Alphabet.Length, Alphabet.Length];
            for (int i = 0; i < Alphabet.Length; i++)
            {
                string[] prices = lines[i + 1].Split(new char[] { ' ', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = i; j < Alphabet.Length; j++)
                {
                    Prices[i, j] = Prices[j, i] = Convert.ToInt32(prices[j + 1]);
                }
            }
        }

        public static ScoringMatrix BasicMatrix(string letters)
        {
            var m = new ScoringMatrix();
            m.GapPenalty = -1;

            string[] alphabet = letters.Split(new char[] { ' ', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            m.Alphabet = new char[alphabet.Length];
            for (int i = 0; i < m.Alphabet.Length; i++)
            {
                m.Alphabet[i] = alphabet[i][0];
            }
            m.Prices = new int[m.Alphabet.Length, m.Alphabet.Length];
            for (int i = 0; i < m.Alphabet.Length; i++)
            {
                for (int j = i; j < m.Alphabet.Length; j++)
                {
                    if (i == j)
                    {
                        m.Prices[i, j] = 0;
                    }
                    else
                    {
                        m.Prices[i, j] = m.Prices[j, i] = -1;
                    }
                }
            }

            return m;
        }
    }
}

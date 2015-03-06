using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioLib
{
    public class DNA: DataString
    {
        public static Dictionary<char, char> Complements = new Dictionary<char, char>()
        {
            { 'A', 'T' },
            { 'C', 'G' },
            { 'G', 'C' },
            { 'T', 'A' }
        };

        public DNA(string data)
            : base(data, DataType.DNA)
        {
        }

        public string ConvertToRNA()
        {
            return Data.Replace('T', 'U');
        }

        public string GetReverseComplement()
        {
            var sb = new StringBuilder();
            for (int i = Data.Length - 1; i >= 0; i--)
            {
                sb.Append(Complements[Data[i]]);
            }
            return sb.ToString();
        }

        public Decimal HammingDistance(DNA dna)
        {
            return StringUtils.HammingDistance(Data, dna.Data);
        }

        public Decimal HammingDistance(string dnaString)
        {
            return StringUtils.HammingDistance(Data, dnaString);
        }

        public double TransitionTraversionRatio(DNA dna)
        {
            return TransitionTraversionRatio(dna.Data);
        }

        public double TransitionTraversionRatio(string dnaString)
        {
            int tran = 0, trav = 0;
            for (int i = 0; i < Data.Length; i++)
            {
                char a = Data[i], b = dnaString[i];
                if (a != b)
                {
                    if (a == 'A' && b == 'G' || a == 'G' && b == 'A' ||
                        a == 'C' && b == 'T' || a == 'T' && b == 'C')
                    {
                        tran++;
                    }
                    else
                    {
                        trav++;
                    }
                }
            }
            return (double)tran / trav;
        }
    }
}

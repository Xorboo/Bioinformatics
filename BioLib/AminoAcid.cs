using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BioLib
{
    public class AminoAcid
    {
        public static char CloseSymbol = '*';

        public char Letter;
        public List<string> Codons;
        public double Mass;

        public AminoAcid(char letter, string[] codons, double mass)
        {
            Letter = letter;
            Codons = new List<string>(codons);
            Mass = mass;
        }

        public bool IsCloseSymbol()
        {
            return Letter == CloseSymbol;
        }

        public override string ToString()
        {
            return Letter + ": (" + string.Join(", ", Codons) + ") - [" + Mass + "]";
        }
    }
    public class AminoAcidString: DataString
    {
        public static List<AminoAcid> Acids;
        
        public static void LoadAcids(string json)
        {
            AminoAcid[] acids = JsonConvert.DeserializeObject<AminoAcid[]>(json);
            Acids = new List<AminoAcid>(acids);
        }

        public static AminoAcid GetAcid(char c)
        {
            return Acids.Where(x => x.Letter == c).FirstOrDefault();
        }

        public AminoAcidString(string data)
            : base(data, DataType.AminoAcid)
        {
        }

        public AminoAcidString(RNA rna)
            : base(rna.GetAminoAcids(), DataType.AminoAcid)
        {
        }

        public static char GetAcidFromCodon(string codon)
        {
            var acid = Acids.Where(x => x.Codons.Contains(codon)).FirstOrDefault();
            if (acid == null)
            {
                throw new Exception("Cannot find aminoacid for codon \"" + codon + "\"");
            }
            return acid.Letter;
        }

        public string GetDataWithoutStops()
        {
            return Data.Replace("*", "");
        }

        public int GetPossibleRNA(int modulo = 10000000)
        {
            string acid = AddCloseSymbol(Data);

            int count = 1;
            foreach (char c in acid)
            {
                count *= GetAcid(c).Codons.Count;
                if (modulo > 1)
                {
                    count %= modulo;
                }
            }
            return count;
        }

        public double GetMass()
        {
            double sum = 0;
            foreach (char c in Data)
            {
                sum += GetAcid(c).Mass;
            }
            return sum;
        }

        public static string AddCloseSymbol(string acid)
        {
            return acid.Last() == AminoAcid.CloseSymbol ? acid : (acid + AminoAcid.CloseSymbol);
        }
    }
}

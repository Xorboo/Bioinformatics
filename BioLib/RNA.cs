using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioLib
{
    public class RNA: DataString
    {
        public RNA(string data): base(data, DataString.DataType.RNA)
        {
        }

        public RNA(DNA dna)
            : base(dna.ConvertToRNA(), DataType.RNA)
        {
        }

        public string GetAminoAcids()
        {
            if (Data.Length % 3 != 0)
            {
                throw new Exception("Can't convert RNA to AminoAcids, wrong elements count: " + Data.Length);
            }

            var sb = new StringBuilder();
            for (int i = 0; i < Data.Length; i += 3)
            {
                sb.Append(AminoAcidString.GetAcidFromCodon(Data.Substring(i, 3)));
            }
            return sb.ToString();
        }
    }
}

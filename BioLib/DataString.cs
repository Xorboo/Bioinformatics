using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioLib
{
    public class DataString
    {
        public static bool CheckStrings = true;

        public enum DataType
        {
            DNA,
            RNA,
            AminoAcid
        }

        public static Dictionary<DataType, char[]> DataSymbols = new Dictionary<DataType, char[]>()
        {
            { DataType.DNA, new char[] { 'A', 'C', 'G', 'T' } },
            { DataType.RNA, new char[] { 'A', 'C', 'G', 'U' } },
            { DataType.AminoAcid, new char[] { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y', '*' } }
        };

        public DataType Type { get; set; }
        public string Data { get; set; }

        public DataString(string data, DataType dataType)
        {
            Type = dataType;
            Data = data;

            if (CheckStrings && !CheckString(Data, dataType))
            {
                Data = "";
                throw new Exception(dataType.ToString() + " string contains wrong symbols");
            }
        }

        public Dictionary<char, int> CountNucleotides()
        {
            var dict = new Dictionary<char, int>();
            foreach (char c in DataString.DataSymbols[DataType.DNA])
            {
                dict.Add(c, 0);
            }
            foreach (char c in Data)
            {
                dict[c]++;
            }
            return dict;
        }

        public static bool SymbolOfType(char c, DataType dataType)
        {
            return DataSymbols[dataType].Contains(c);
        }

        public static bool CheckString(string data, DataType dataType)
        {
            var unknownSymbols = data.Where(a => !SymbolOfType(a, dataType));
            return unknownSymbols.Count() == 0;

            //throw new Exception(dataType.ToString() + " string contains " + unknownSymbols.Count() +
            //    " wrong symbols: \"" + string.Join(", ", unknownSymbols) + "\"");
        }
    }
}

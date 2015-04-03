using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib.Viterbi
{
    public class State
    {
        public string Name;
        public double Chance = 0;

        public State(string name, double chance = -1)
        {
            Name = name;
            Chance = chance;
        }

        public override bool Equals(object obj)
        {
            return Name == ((State)obj).Name;
        }

        public override string ToString()
        {
            return Name + " [" + Chance + "]";
        }

        public static bool operator ==(State a, State b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Name == b.Name;
        }
        
        public static bool operator !=(State a, State b)
        {
            return !(a == b);
        }
    }
}

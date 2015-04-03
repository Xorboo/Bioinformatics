using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib.Viterbi
{
    public class Transition
    {
        public State From;
        public State To;
        public double Chance;

        public Transition(State from, State to, double chance)
        {
            From = from;
            To = to;
            Chance = chance;
        }

        public override string ToString()
        {
            return From + " -> " + To + " : [" + Chance + "]";
        }
    }
}

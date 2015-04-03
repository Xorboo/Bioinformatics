using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using BioLib;
using BioLib.Ukkonen;
using BioLib.Viterbi;

namespace Bioinformatics
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        string output;

        private void button1_Click(object sender, EventArgs e)
        {
            var tree = new SuffixTree(rtb1.Text);
            tree.Message += VerboseMessage;
            output = "";
            tree.Build();
            rtb1.Text += "\n\n" + output + "\n\nRESULT:\n" + tree.ToString();

            var healthy = new State("Healthy");
            var fever = new State("Fever");

            var normal = new State("Normal");
            var cold = new State("Cold");
            var dizzy = new State("Dizzy");

            var states = new List<State>() { healthy, fever };
            var observations = new List<State>() { normal, cold, dizzy };

            var starts = new List<Transition>() { 
                new Transition(null, healthy, 0.6), 
                new Transition(null, fever, 0.4) 
            };
            var transitions = new List<Transition>() { 
                new Transition(healthy, healthy, 0.7), 
                new Transition(healthy, fever, 0.3),
                new Transition(fever, healthy, 0.4),
                new Transition(fever, fever, 0.6)
            };
            var emissions = new List<Transition>() { 
                new Transition(healthy, normal, 0.5), 
                new Transition(healthy, cold, 0.4),
                new Transition(healthy, dizzy, 0.1),
                new Transition(fever, normal, 0.1),
                new Transition(fever, cold, 0.3),
                new Transition(fever, dizzy, 0.6)
            };

            ViterbiSolver viterbi = new ViterbiSolver(states, observations, starts, transitions, emissions);
            viterbi.Solve();
            rtb2.Text = viterbi.ShortResults() + "\n\n===============================\n\n" + viterbi.LongResults();
        }

        void VerboseMessage(string text, object[] obj)
        {
            output += String.Format(text, obj) + "\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string json = File.ReadAllText("../../../Data/AminoAcids.json");
            AminoAcidString.LoadAcids(json);
        }
    }
}

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

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

namespace Bioinformatics
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var strs = rtb1.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            string verbose;
            int pos = StringUtils.FindSubstring(strs[0], strs[1], out verbose);

            rtb1.Text += "\n" + pos + "\n" + string.Join(" ", strs[1].ToCharArray()) + "\n" + verbose;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string json = File.ReadAllText("../../../Data/AminoAcids.json");
            AminoAcidString.LoadAcids(json);
        }
    }
}

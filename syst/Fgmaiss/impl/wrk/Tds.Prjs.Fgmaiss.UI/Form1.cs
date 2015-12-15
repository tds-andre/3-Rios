using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tds.Prjs.Fgmaiss.ParMed;
using Tds.Prjs.Fgmaiss.Trash;
using Tds.Prjs.Fgmaiss.ParDemission;


namespace Tds.Prjs.Fgmaiss.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var x = new ParMed.Manager();
            x.Execute("..\\..\\..\\..\\..\\repo\\Emissão de Medicamentos - Almoxarifado.pdf.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var x = new Trash.Manager();
            x.Execute("..\\..\\..\\..\\..\\repo\\Rotas -  Varrição Equipe.pdf.txt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var x = new ParDemission.Executor();
            x.Execute("..\\..\\..\\..\\..\\repo\\FGMAISS - Relação de Servidores Demitidos - 01_10 a 15_12_2015.pdf");
        }
    }
}

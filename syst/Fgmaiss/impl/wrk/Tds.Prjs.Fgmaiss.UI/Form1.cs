﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tds.Prjs.Fgmaiss.ParMed;


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
            var x = new Manager();
            x.Execute("C:\\_\\Projetos\\3 Rios\\syst\\Fgmaiss\\repo\\Emissão de Medicamentos - Almoxarifado.pdf");
        }
    }
}

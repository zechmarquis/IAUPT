using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IAupt.uptRNA;

namespace EjemploPerceptronMultiCapa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnEntrenar_Click(object sender, EventArgs e)
        {
            PerceptronMultiCapa rna = new PerceptronMultiCapa(@"ejemplo.pml");
            rna.entrenar();
        }

        private void BtnReconocer_Click(object sender, EventArgs e)
        {
            PerceptronMultiCapa rna = new PerceptronMultiCapa(@"ejemplo.ppm");

            double[] x = { 1, 0, 1 };
            rna.reconocer(x);

            double [,] y = rna.y;
        }
    }
}

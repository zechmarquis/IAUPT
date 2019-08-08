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
using IAupt.uptVC;
using System.IO;
using System.Diagnostics;
using IAupt;
using System.Threading;

namespace Ejemplo_reconocimiento_de_objetos
{
    public partial class Form1 : Form
    {
        Vision vision = new Vision(1);
        PerceptronMultiCapa rna;

        bool fondo = false;

        String directorio;

        List<String> nombresObjetos = new List<string>();

        public Form1()
        {
            InitializeComponent();
            nombresObjetos.Add("");
            nombresObjetos.Add("Tornillo");
            nombresObjetos.Add("Rondana");
            nombresObjetos.Add("Armella");
            nombresObjetos.Add("Alcayata");
            nombresObjetos.Add("Cola de milano");
        }

        private void abrirImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String ruta;
            abrir.Title = "Abrir archivo de imagen";
            abrir.Filter = "Archivo de mapa de bits|*.bmp|Archivo jpg|*.jpg|Archivo png|*.png";
            abrir.FileName = "";
            abrir.ShowDialog();

            ruta = abrir.FileName;

            if (ruta != "")
            {
                pbRGB.Image = Image.FromFile(ruta);
                directorio = ruta.Substring(0, ruta.Length - 8);
            }

            else
            {
                MessageBox.Show("No se seleccionó alguna imagen", "Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = pbRGB.CreateGraphics();
            Pen lapiz = new Pen(Color.Red, 2);
            int tamañoMarca = 5;

            Bitmap imagen = new Bitmap(pbRGB.Image);
            vision.escalarCoordenadas(imagen);
            vision.rgbAgris();
            vision.rgbAbinario((byte)hsbUmbral.Value, fondo);
            pbGris.Image = vision.img;
            List<Hu> momentosHu = vision.etiquetarImagenEspaciosEstados(int.Parse(txtArea.Text));

            double factorX = (1.0 * pbRGB.Width / imagen.Width);
            double factorY = (1.0 * pbRGB.Height / imagen.Height);

            foreach (Hu momentos in momentosHu)
            {
                g.DrawLine(lapiz, (float)(momentos.x * factorX - tamañoMarca), (float)(momentos.y * factorY), (float)(momentos.x * factorX + tamañoMarca), (float)(momentos.y * factorY));
                g.DrawLine(lapiz, (float)(momentos.x * factorX), (float)(momentos.y * factorY - tamañoMarca), (float)(momentos.x * factorX), (float)(momentos.y * factorY + tamañoMarca));
                rna.reconocer(momentos.fi);

                Font fuente = new Font("Times New Roman", 10.0f);
                Brush brocha = new SolidBrush(Color.Red);

                g.DrawString(nombresObjetos[(int)rna.y[0, 0]], fuente, brocha, (float)(momentos.x * factorX - tamañoMarca * 4), (float)(momentos.y * factorY - tamañoMarca * 4));
            }
            textBox2.Text = vision.num_objetos.ToString();
        }

        private void hsbUmbral_Scroll(object sender, ScrollEventArgs e)
        {
            Bitmap imagen = new Bitmap(pbRGB.Image);
            vision.escalarCoordenadas(imagen);
            vision.rgbAgris();
            vision.rgbAbinario((byte)hsbUmbral.Value, fondo);
            pbGris.Image = vision.img;
        }
        private void chkFondo_CheckedChanged(object sender, EventArgs e)
        {
            fondo = chkFondo.Checked;
            if (fondo)
            {
                chkFondo.Text = "Fondo negro";
            }
            else
            {
                chkFondo.Text = "Fondo blanco";
            }
        }
        private void abrirArchivoPesosPerceptronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir.Title = "Archivo de tipo perceptrón multicapa";
            abrir.Filter = "Archivo perceptrón multicapa|*.ppm";
            abrir.FileName = "";

            DialogResult resultado = abrir.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                rna = new PerceptronMultiCapa(abrir.FileName);
                MessageBox.Show("Se creo la red neuronal correctamente.");
            }
            else
            {
                MessageBox.Show("No se seleccionó algún archivo.");
            }
        }

        private void ExtraerMomentosHuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = folderBrowserDialog1.ShowDialog();

            if (respuesta == DialogResult.OK)
            {
                StreamWriter escribir = new StreamWriter(folderBrowserDialog1.SelectedPath + "\\patrones.txt", false);
                Image img;

                for (int i = 1; i <= 5; i++)
                {
                    for (int j = 1; j < 20; j++)
                    {
                        if (j < 10)
                            img = Image.FromFile(folderBrowserDialog1.SelectedPath + "\\IMAG" + "0" + i + "0" + j + ".bmp");
                        else
                            img = Image.FromFile(folderBrowserDialog1.SelectedPath + "\\IMAG" + "0" + i + j + ".bmp");

                        Bitmap imagen = new Bitmap(img);
                        vision.escalarCoordenadas(imagen);
                        vision.rgbAgris();
                        vision.rgbAbinario((byte)hsbUmbral.Value, fondo);
                        List<Hu> momentosHu = vision.etiquetarImagenEspaciosEstados(int.Parse(txtArea.Text));
                        foreach (double fi in momentosHu[0].fi)
                        {
                            escribir.Write(fi + "\t");
                        }
                        escribir.WriteLine();
                    }
                }
                escribir.Close();
            }
        }
    }
}

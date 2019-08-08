namespace Ejemplo_reconocimiento_de_objetos
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbRGB = new System.Windows.Forms.PictureBox();
            this.pbGris = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirImagenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirArchivoPesosPerceptronToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraerMomentosHuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrir = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.hsbUmbral = new System.Windows.Forms.HScrollBar();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.chkFondo = new System.Windows.Forms.CheckBox();
            this.txtArea = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbRGB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGris)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbRGB
            // 
            this.pbRGB.Location = new System.Drawing.Point(12, 27);
            this.pbRGB.Name = "pbRGB";
            this.pbRGB.Size = new System.Drawing.Size(640, 480);
            this.pbRGB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRGB.TabIndex = 0;
            this.pbRGB.TabStop = false;
            // 
            // pbGris
            // 
            this.pbGris.Location = new System.Drawing.Point(714, 27);
            this.pbGris.Name = "pbGris";
            this.pbGris.Size = new System.Drawing.Size(640, 480);
            this.pbGris.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbGris.TabIndex = 1;
            this.pbGris.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.herramientasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1362, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirImagenToolStripMenuItem,
            this.abrirArchivoPesosPerceptronToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "&Archivo";
            // 
            // abrirImagenToolStripMenuItem
            // 
            this.abrirImagenToolStripMenuItem.Name = "abrirImagenToolStripMenuItem";
            this.abrirImagenToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.abrirImagenToolStripMenuItem.Text = "&Abrir imagen";
            this.abrirImagenToolStripMenuItem.Click += new System.EventHandler(this.abrirImagenToolStripMenuItem_Click);
            // 
            // abrirArchivoPesosPerceptronToolStripMenuItem
            // 
            this.abrirArchivoPesosPerceptronToolStripMenuItem.Name = "abrirArchivoPesosPerceptronToolStripMenuItem";
            this.abrirArchivoPesosPerceptronToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.abrirArchivoPesosPerceptronToolStripMenuItem.Text = "Abrir archivo pesos perceptron multicapa";
            this.abrirArchivoPesosPerceptronToolStripMenuItem.Click += new System.EventHandler(this.abrirArchivoPesosPerceptronToolStripMenuItem_Click);
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extraerMomentosHuToolStripMenuItem});
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.herramientasToolStripMenuItem.Text = "Herramientas";
            // 
            // extraerMomentosHuToolStripMenuItem
            // 
            this.extraerMomentosHuToolStripMenuItem.Name = "extraerMomentosHuToolStripMenuItem";
            this.extraerMomentosHuToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.extraerMomentosHuToolStripMenuItem.Text = "Extraer Momentos Hu";
            this.extraerMomentosHuToolStripMenuItem.Click += new System.EventHandler(this.ExtraerMomentosHuToolStripMenuItem_Click);
            // 
            // abrir
            // 
            this.abrir.FileName = "openFileDialog1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 528);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Gris";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // hsbUmbral
            // 
            this.hsbUmbral.Location = new System.Drawing.Point(714, 510);
            this.hsbUmbral.Maximum = 255;
            this.hsbUmbral.Name = "hsbUmbral";
            this.hsbUmbral.Size = new System.Drawing.Size(640, 19);
            this.hsbUmbral.TabIndex = 32;
            this.hsbUmbral.Value = 93;
            this.hsbUmbral.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbUmbral_Scroll);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(233, 530);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(67, 20);
            this.textBox2.TabIndex = 38;
            // 
            // chkFondo
            // 
            this.chkFondo.AutoSize = true;
            this.chkFondo.Location = new System.Drawing.Point(561, 532);
            this.chkFondo.Name = "chkFondo";
            this.chkFondo.Size = new System.Drawing.Size(91, 17);
            this.chkFondo.TabIndex = 40;
            this.chkFondo.Text = "Fondo blanco";
            this.chkFondo.UseVisualStyleBackColor = true;
            this.chkFondo.CheckedChanged += new System.EventHandler(this.chkFondo_CheckedChanged);
            // 
            // txtArea
            // 
            this.txtArea.Location = new System.Drawing.Point(431, 531);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(100, 20);
            this.txtArea.TabIndex = 41;
            this.txtArea.Text = "100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(306, 533);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Área mínima en pixeles:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 534);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Núm. objetos encontrados:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 736);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.chkFondo);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.hsbUmbral);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pbGris);
            this.Controls.Add(this.pbRGB);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pbRGB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGris)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbRGB;
        private System.Windows.Forms.PictureBox pbGris;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirImagenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog abrir;
        private System.Windows.Forms.Button button3;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.HScrollBar hsbUmbral;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox chkFondo;
        private System.Windows.Forms.TextBox txtArea;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraerMomentosHuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirArchivoPesosPerceptronToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}


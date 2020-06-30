using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebaForms
{
    public partial class Form1 : Form
    {
        private Bitmap original; //imagen original
        private Bitmap resultante; //imagen procesada
        //private int[] histograma = new int[256];
        private int[,] conv3x3 = new int[3, 3];
        private int factor;
        private int offset;
        //variabales para el double buffer y evitar el flicker
        private int anchoVentana, altoVentana;
        private string pixel;
        private string texto;

        public Form1()
        {
            InitializeComponent();
            //Creamos el bitmap resultante del cuadro
            resultante = new Bitmap(800, 600);
            //Colocamos los valores para el dibujo con scrolls.
            anchoVentana = 800;
            altoVentana = 600;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void abrirImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ofdSeleccionarImagen.ShowDialog() == DialogResult.OK)
            {
                original = (Bitmap)(Bitmap.FromFile(ofdSeleccionarImagen.FileName));
                anchoVentana = original.Width;
                altoVentana = original.Height;

                resultante = original;

                this.Invalidate(); //Forza el evento paint, redibuja la ventna
            }
        }

        private void guardarImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                resultante.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Verificamos que se tenga un bitmap instanciado
            if (resultante != null)
            {
                //Obtener objeto graphics
                Graphics g = e.Graphics;

                //Calculamos el scroll.
                AutoScrollMinSize = new Size(anchoVentana, altoVentana);

                //Copiamos del bitmap a la ventana
                g.DrawImage(resultante, new Rectangle(this.AutoScrollPosition.X, 
                    this.AutoScrollPosition.Y + 30, anchoVentana, altoVentana));

                //liberamos el recurso
                g.Dispose();
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            resultante = new Bitmap(original.Width, original.Height);
            Color rColor = new Color(); //guardara el color resultante del pixel
            Color oColor = new Color(); //guardara el color original del pixel
            Byte oColorR = new Byte();
            Byte oColorB = new Byte();
            Byte oColorG = new Byte();
            decimal suma, r, g, b, cr, cg, cb, porcent;

            for (x = 0; x < original.Width; x++)
            {
                for (y = 0; y< original.Height; y++)
                {
                    pixel = lblPixel.Text;
                    //resultante.SetPixel(x,y,Color.FromArgb(120,200,120)); //set pixel, me da la ubicacion del pixel y edito el color de ese pixel en rgb.

                    //obtiene el color del pixel.
                    oColor = original.GetPixel(x, y);
                    oColorR = oColor.R;
                    oColorG = oColor.G;
                    oColorB = oColor.B;
                    r = Convert.ToDecimal(oColorR);
                    g = Convert.ToDecimal(oColorG);
                    b = Convert.ToDecimal(oColorB);
                    cr = Convert.ToDecimal(0.2126);
                    cg = Convert.ToDecimal(0.7152);
                    cb = Convert.ToDecimal(0.0722);

                    //Colocamos el color en resultante
                    resultante.SetPixel(x, y, oColor); //en vez de oColor sera rColor
                    if((y==50) && (x==41))
                    {
                        suma = ((cr * r) + (cg*g) + (cb*b));
                        
                        //texto = pixel + "," + Convert.ToString(oColor);
                        texto = pixel + "," + Convert.ToString(suma);
                        lblPixel.Text = texto;
                    }
                }
            }

            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            OpenFileDialog getImage = new OpenFileDialog();
            getImage.InitialDirectory = "C:\\";
            getImage.Filter = "Archivos de Imagen (*.jpg)(*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|GIF (*.gif)|*.gif";

            if (getImage.ShowDialog() == DialogResult.OK)
            {
                pictureBoxImagen.ImageLocation = getImage.FileName;
            }
            else
            {
                MessageBox.Show("No se selecciono imagen", "sin seleccion",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
    }
}

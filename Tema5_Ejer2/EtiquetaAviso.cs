using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tema5_Ejer2
{

    public partial class EtiquetaAviso : Control
    {
        int grosor = 0; //Grosor de las líneas de dibujo
        int offsetX = 0; //Desplazamiento a la derecha del texto
        int offsetY = 0; //Desplazamiento hacia abajo del texto
        int width;
        int height;
        public EtiquetaAviso()
        {
            InitializeComponent();
            this.colorIni = Color.White;
            this.colorFin = Color.White;
            this.flag = false;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            offsetX = 0;
            offsetY = 0;
            grosor = 0;

            //Esta propiedad provoca mejoras en la apariencia o en la eficiencia
            // a la hora de dibujar
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
          

            if (flag) //Primero porque pinta el fondo
            {

                LinearGradientBrush l = new LinearGradientBrush(new PointF(0, 0), new PointF(this.Width, this.Height), colorIni, ColorFin);
                g.FillRectangle(l, new RectangleF(0, 0, this.Width, this.Height));
                l.Dispose();

            }
            switch (Marca)
            {
                case eMarca.Circulo:
                    grosor = 20;
                    g.DrawEllipse(new Pen(Color.Green, grosor), grosor, grosor,
                   this.Font.Height, this.Font.Height);

                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor;

                    width = this.FontHeight;
                    height = this.FontHeight;
                    break;
                case eMarca.Cruz:
                    grosor = 5;
                    Pen lapiz = new Pen(Color.Red, grosor);
                    g.DrawLine(lapiz, grosor, grosor, this.Font.Height,
                   this.Font.Height);
                    g.DrawLine(lapiz, this.Font.Height, grosor, grosor,
                   this.Font.Height);

                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor / 2;

                    width = this.FontHeight;
                    height = this.FontHeight;
                    //Es recomendable liberar recursos de dibujo pues se
                    //pueden realizar muchos y cogen memoria
                    lapiz.Dispose();
                    break;
                case eMarca.ImagenDeForma:
                    if (imagenMarca != null)
                    {
                        grosor = 4;
                        width = (imagenMarca.Width * this.Font.Height) / imagenMarca.Height;
                        height = this.FontHeight;

                        offsetX = this.Font.Height + grosor;
                        offsetY = grosor;

                        g.DrawImage(imagenMarca, grosor, grosor, this.Font.Height, this.Font.Height);

                    }
                    break;
            }


            //Finalmente pintamos el Texto; desplazado si fuera necesario
            SolidBrush b = new SolidBrush(this.ForeColor);
            g.DrawString(this.Text, this.Font, b, offsetX + grosor, offsetY);
            Size tam = g.MeasureString(this.Text, this.Font).ToSize();
            this.Size = new Size(tam.Width + offsetX + grosor, tam.Height + offsetY
           * 2);
            b.Dispose();

        }
        [RefreshProperties(RefreshProperties.Repaint)]

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Refresh();
        }
        public enum eMarca
        {
            Nada,
            Cruz,
            Circulo,
            ImagenDeForma,
        }
        private eMarca marca = eMarca.Nada;
        public eMarca Marca
        {
            set
            {
                marca = value;
                this.Refresh();
            }
            get { return marca; }
        }

        private Color colorIni;
        [Category("Varios")]
        [Description("Define el primer color del gradiente de fondo")]
        public Color ColorIni
        {
            set
            {
                colorIni = value;
                this.Refresh();
            }
            get
            {
                return colorIni;
            }
        }

        private Color colorFin;
        [Category("Varios")]
        [Description("Define el ultimo color del gradiente de fondo")]
        public Color ColorFin
        {
            set
            {
                colorFin = value;
                this.Refresh();
            }
            get
            {
                return colorFin;
            }
        }


        private bool flag;
        [Category("Varios")]
        [Description("Permitir gradiente de fondo")]
        public bool Flag
        {
            set
            {
                flag = value;
                this.Refresh();
            }
            get
            {
                return flag;
            }
        }

        private Bitmap imagenMarca;
        [Category("Varios")]
        [Description("Imagen de la marca")]
        public Bitmap ImagenMarca
        {
            set
            {
                try
                {
                    imagenMarca = value;
                    this.Refresh();
                }
                catch (FileNotFoundException)
                {

                    MessageBox.Show("Error de Imagen (Revisa la ruta)");
                }
            }
            get
            {
                return imagenMarca;
            }
        }

        [Category("Click Marca")]
        [Description("Se lanza al hacer click sobre la marca")]
        public event System.EventHandler ClickEnMarca;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.X <offsetX)
            {
                ClickEnMarca?.Invoke(this, EventArgs.Empty);
               
            }
        }
    }
}

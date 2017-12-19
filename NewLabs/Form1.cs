using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewLabs
{
    public partial class Form1 : Form
    {
        float scalePrm = 2;

        public Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
            
        }
        #region//FUTU-U-U-URE

        float Grad = (float)(2 * Math.PI) / 360; //радиан в градусе
        float R = -50;
        bool TrMouseD = false; //нажата ли мышь
        int TrMouseDX; //координата по Х, где нажата мышь
        int TrMouseDY;
        int TrMouseX; //координата по Х, где мышь находится в данный  момент
        int TrMouseY;
        int TrUgolEX; //углы наклона экрана по Х
        int TrUgolEY;
        int TrUgolDEX; //углы наклона экрана по Х в момент нажатия мыши
        int TrUgolDEY;

        #endregion

        Guitar guitar;

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// движение мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            TrMouseX = e.X;
            TrMouseY = e.Y;

            if (TrMouseD)
            {
                TrUgolEX = TrUgolDEX + (TrMouseX - TrMouseDX);
                TrUgolEY = TrUgolDEY + (TrMouseY - TrMouseDY);

            }
        }

        /// <summary>
        /// прокрутка колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0) { scalePrm -= 0.1f; }
            if (e.Delta > 0) { scalePrm += 0.1f; }
            R = (R < -99) ? -99 : R;
        }

        /// <summary>
        /// нажатие мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            TrMouseD = true;
            TrMouseDX = e.X;
            TrMouseDY = e.Y;
            TrUgolDEX = TrUgolEX;
            TrUgolDEY = TrUgolEY;
        }

        /// <summary>
        /// отпустить мышь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            TrMouseD = false;
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            int[] zbuffer = new int[pictureBox1.Width * pictureBox1.Height];
            for (int i = 0; i < zbuffer.Length; i++)
                zbuffer[i] = -100; 

            guitar = new Guitar();
            
            guitar.tune(TrUgolEX, TrUgolEY, R);
            guitar.scale(scalePrm, scalePrm, scalePrm);
            guitar.center(bitmap);
            guitar.DrawLines(bitmap, ref zbuffer);        
            guitar.Draw(bitmap, ref zbuffer);


            pictureBox1.Image = bitmap;
        }
    }
}

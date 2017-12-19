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
    public class GuitarPart
    {
        public Matrix m;
        protected Color color;
        public virtual void DrawLines(Bitmap btm, ref int[] zbuffer)
        {
            for(int i = 0; i < m.Row - 1; i+=2)
            {
                Vec3i A = new Vec3i((int)m.matrix[i, 0], (int)m.matrix[i, 1], (int)m.matrix[i, 2]);
                Vec3i B = new Vec3i((int)m.matrix[i+1, 0], (int)m.matrix[i+1, 1], (int)m.matrix[i+1, 2]);
                DrawMethods.line(A, B, ref zbuffer, btm, color);
            }
        }

        protected void TrUgol(ref float iX, ref float iY, int iU)
        {
            iU = -iU;
            float X, Y, U = ((float)(2 * Math.PI) / 360) * (float)iU;
            X = (float)(iX * Math.Cos(U) - iY * Math.Sin(U));
            Y = (float)(iX * Math.Sin(U) + iY * Math.Cos(U));
            iX = X;
            iY = Y;
        }

        public virtual void tune(int ugolX, int ugolY, float len)
        {
            for (int f = 0; f < m.Row; f++)
            {
                TrUgol(ref m.matrix[f, 1], ref m.matrix[f, 2], ugolY);
                TrUgol(ref m.matrix[f, 0], ref m.matrix[f, 2], ugolX);
            } 
        }

        public virtual void scale(float mx, float my, float mz)
        {
            Matrix t = new Matrix(4, 4);
            t.matrix = new float[4, 4]
            {
                {mx, 0, 0, 0 },
                {0, my, 0, 0 },
                {0, 0, mz, 0 },
                {0, 0, 0, 0 }
            };

            m = m.Multiple(t);
        }

        public virtual void center(Bitmap bitmap)
        {
            m.Add(bitmap.Width / 2, bitmap.Height / 2);
        }
    }
    public class GuitarParts: GuitarPart
    {
        public Matrix m1;

        public override void DrawLines(Bitmap btm, ref int[] zbuffer)
        {
            for (int i = 0; i < m.Row; i++)
            {
                Vec3i A = new Vec3i((int)m.matrix[i, 0], (int)m.matrix[i, 1], (int)m.matrix[i, 2]);
                Vec3i B = new Vec3i((int)m.matrix[(i + 1) % m.Row, 0], (int)m.matrix[(i + 1) % m.Row, 1], (int)m.matrix[(i + 1) % m.Row, 2]);
                DrawMethods.line(A, B, ref zbuffer, btm, Color.Black);
                Vec3i A1 = new Vec3i((int)m1.matrix[i, 0], (int)m1.matrix[i, 1], (int)m1.matrix[i, 2]);
                Vec3i B1 = new Vec3i((int)m1.matrix[(i + 1) % m.Row, 0], (int)m1.matrix[(i + 1) % m.Row, 1], (int)m1.matrix[(i + 1) % m.Row, 2]);
                DrawMethods.line(A1, B1, ref zbuffer, btm, Color.Black);
            }
            for (int i = 0; i < m.Row; i++)
            {
                Vec3i A = new Vec3i((int)m.matrix[i, 0], (int)m.matrix[i, 1], (int)m.matrix[i, 2]);
                Vec3i A1 = new Vec3i((int)m1.matrix[i, 0], (int)m1.matrix[i, 1], (int)m1.matrix[i, 2]);
                DrawMethods.line(A, A1, ref zbuffer, btm, Color.Black);
            }

        }

        public virtual void Draw(Bitmap btm, ref int[] zbuffer)
        {
            for (int i = 0; i < m.Row; i++)
            {
                Triangle tr1 = new Triangle(
                new Vec3i((int)m.matrix[i, 0], (int)m.matrix[i, 1], (int)m.matrix[i, 2]),
                new Vec3i((int)m.matrix[(i + 1) % m.Row, 0], (int)m.matrix[(i + 1) % m.Row, 1], (int)m.matrix[(i + 1) % m.Row, 2]),
                new Vec3i((int)m1.matrix[i, 0], (int)m1.matrix[i, 1], (int)m1.matrix[i, 2]));

                Triangle tr2 = new Triangle(
                new Vec3i((int)m1.matrix[i, 0], (int)m1.matrix[i, 1], (int)m1.matrix[i, 2]),
                new Vec3i((int)m1.matrix[(i + 1) % m.Row, 0], (int)m1.matrix[(i + 1) % m.Row, 1], (int)m1.matrix[(i + 1) % m.Row, 2]),
                new Vec3i((int)m.matrix[(i + 1) % m.Row, 0], (int)m.matrix[(i + 1) % m.Row, 1], (int)m.matrix[(i + 1) % m.Row, 2]));

                DrawMethods.triangle(tr1, ref zbuffer, btm, color);
                DrawMethods.triangle(tr2, ref zbuffer, btm, color);
            }
        }

        public override void tune(int ugolX, int ugolY, float len)
        {
            for (int f = 0; f < m.Row; f++)
            {
                TrUgol(ref m.matrix[f, 1], ref m.matrix[f, 2], ugolY);
                TrUgol(ref m.matrix[f, 0], ref m.matrix[f, 2], ugolX);
                m.matrix[f, 2] -= len;
                TrUgol(ref m1.matrix[f, 1], ref m1.matrix[f, 2], ugolY);
                TrUgol(ref m1.matrix[f, 0], ref m1.matrix[f, 2], ugolX);
                m1.matrix[f, 2] -= len;
            }
        }
        public override void scale(float mx, float my, float mz)
        {
            base.scale(mx, my, mz);
            Matrix t = new Matrix(4, 4);
            t.matrix = new float[4, 4]
            {
                {mx, 0, 0, 0 },
                {0, my, 0, 0 },
                {0, 0, mz, 0 },
                {0, 0, 0, 0 }
            };
            
            m1 = m1.Multiple(t);
        }

        public override void center(Bitmap bitmap)
        {
            base.center(bitmap);
            m1.Add(bitmap.Width / 2, bitmap.Height / 2);
        }
    }
    class GuitarBody : GuitarParts
    {
        public GuitarBody()
        {
            color = Color.Gray;
            m = new Matrix(8, 4);
            m.matrix = new float[8, 4] { 
                {-115f,    40f,     0f, 0},
                {-70,      -5f,     0f, 0},
                {-80,      -35f,    0f, 0},
                {-35f,     -15f,    0f, 0},
                {15f,      -25f,    0f, 0},
                {-10,       05f,    0f, 0},
                {5f,        40,     0f, 0},
                {-45f,      20,     0f, 0} };

            m1 = new Matrix(8, 4);
            m1.matrix = new float[8, 4] {
                {-115f,    40f,     -10f, 0},
                {-70,      -5f,     -10f, 0},
                {-80,      -35f,    -10f, 0},
                {-35f,     -15f,    -10f, 0},
                {15f,      -25f,    -10f, 0},
                {-10,       05f,    -10f, 0},
                {5f,        40,     -10f, 0},
                {-45f,      20,     -10f, 0} };
        }

        public override void Draw(Bitmap btm, ref int[] zbuffer)
        {
            base.Draw(btm, ref zbuffer);
            {
                Triangle tr1 = new Triangle(
                    new Vec3i((int)m.matrix[0, 0], (int)m.matrix[0, 1], (int)m.matrix[0, 2]),
                    new Vec3i((int)m.matrix[1, 0], (int)m.matrix[1, 1], (int)m.matrix[1, 2]),
                    new Vec3i((int)m.matrix[7, 0], (int)m.matrix[7, 1], (int)m.matrix[7, 2]));
                Triangle tr2 = new Triangle(
                    new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]),
                    new Vec3i((int)m.matrix[1, 0], (int)m.matrix[1, 1], (int)m.matrix[1, 2]),
                    new Vec3i((int)m.matrix[3, 0], (int)m.matrix[3, 1], (int)m.matrix[3, 2]));
                Triangle tr3 = new Triangle(
                    new Vec3i((int)m.matrix[3, 0], (int)m.matrix[3, 1], (int)m.matrix[3, 2]),
                    new Vec3i((int)m.matrix[4, 0], (int)m.matrix[4, 1], (int)m.matrix[4, 2]),
                    new Vec3i((int)m.matrix[5, 0], (int)m.matrix[5, 1], (int)m.matrix[5, 2]));
                Triangle tr4 = new Triangle(
                    new Vec3i((int)m.matrix[5, 0], (int)m.matrix[5, 1], (int)m.matrix[5, 2]),
                    new Vec3i((int)m.matrix[6, 0], (int)m.matrix[6, 1], (int)m.matrix[6, 2]),
                    new Vec3i((int)m.matrix[7, 0], (int)m.matrix[7, 1], (int)m.matrix[7, 2]));
                Triangle tr5 = new Triangle(
                    new Vec3i((int)m.matrix[3, 0], (int)m.matrix[3, 1], (int)m.matrix[3, 2]),
                    new Vec3i((int)m.matrix[1, 0], (int)m.matrix[1, 1], (int)m.matrix[1, 2]),
                    new Vec3i((int)m.matrix[7, 0], (int)m.matrix[7, 1], (int)m.matrix[7, 2]));
                Triangle tr6 = new Triangle(
                    new Vec3i((int)m.matrix[3, 0], (int)m.matrix[3, 1], (int)m.matrix[3, 2]),
                    new Vec3i((int)m.matrix[5, 0], (int)m.matrix[5, 1], (int)m.matrix[5, 2]),
                    new Vec3i((int)m.matrix[7, 0], (int)m.matrix[7, 1], (int)m.matrix[7, 2]));
                tr1.ityA = tr1.ityB = tr1.ityC = tr2.ityA = tr2.ityB = tr2.ityC = tr3.ityA = tr3.ityB = tr3.ityC = tr4.ityA = tr4.ityB = tr4.ityC = tr5.ityA = tr5.ityB = tr5.ityC = tr6.ityA = tr6.ityB = tr6.ityC = 1;

                DrawMethods.triangle(tr1, ref zbuffer, btm, color);
                DrawMethods.triangle(tr2, ref zbuffer, btm, color);
                DrawMethods.triangle(tr3, ref zbuffer, btm, color);
                DrawMethods.triangle(tr4, ref zbuffer, btm, color);
                DrawMethods.triangle(tr5, ref zbuffer, btm, color);
                DrawMethods.triangle(tr6, ref zbuffer, btm, color);
            }
            {
             Triangle tr1 = new Triangle(
                new Vec3i((int)m1.matrix[0, 0], (int)m1.matrix[0, 1], (int)m1.matrix[0, 2]),
                new Vec3i((int)m1.matrix[1, 0], (int)m1.matrix[1, 1], (int)m1.matrix[1, 2]),
                new Vec3i((int)m1.matrix[7, 0], (int)m1.matrix[7, 1], (int)m1.matrix[7, 2]));
            Triangle tr2 = new Triangle(
                new Vec3i((int)m1.matrix[2, 0], (int)m1.matrix[2, 1], (int)m1.matrix[2, 2]),
                new Vec3i((int)m1.matrix[1, 0], (int)m1.matrix[1, 1], (int)m1.matrix[1, 2]),
                new Vec3i((int)m1.matrix[3, 0], (int)m1.matrix[3, 1], (int)m1.matrix[3, 2]));
            Triangle tr3 = new Triangle(
                new Vec3i((int)m1.matrix[3, 0], (int)m1.matrix[3, 1], (int)m1.matrix[3, 2]),
                new Vec3i((int)m1.matrix[4, 0], (int)m1.matrix[4, 1], (int)m1.matrix[4, 2]),
                new Vec3i((int)m1.matrix[5, 0], (int)m1.matrix[5, 1], (int)m1.matrix[5, 2]));
            Triangle tr4 = new Triangle(
                new Vec3i((int)m1.matrix[5, 0], (int)m1.matrix[5, 1], (int)m1.matrix[5, 2]),
                new Vec3i((int)m1.matrix[6, 0], (int)m1.matrix[6, 1], (int)m1.matrix[6, 2]),
                new Vec3i((int)m1.matrix[7, 0], (int)m1.matrix[7, 1], (int)m1.matrix[7, 2]));
            Triangle tr5 = new Triangle(
                new Vec3i((int)m1.matrix[3, 0], (int)m1.matrix[3, 1], (int)m1.matrix[3, 2]),
                new Vec3i((int)m1.matrix[1, 0], (int)m1.matrix[1, 1], (int)m1.matrix[1, 2]),
                new Vec3i((int)m1.matrix[7, 0], (int)m1.matrix[7, 1], (int)m1.matrix[7, 2]));
            Triangle tr6 = new Triangle(
                new Vec3i((int)m1.matrix[3, 0], (int)m1.matrix[3, 1], (int)m1.matrix[3, 2]),
                new Vec3i((int)m1.matrix[5, 0], (int)m1.matrix[5, 1], (int)m1.matrix[5, 2]),
                new Vec3i((int)m1.matrix[7, 0], (int)m1.matrix[7, 1], (int)m1.matrix[7, 2]));
                tr1.ityA = tr1.ityB = tr1.ityC = tr2.ityA = tr2.ityB = tr2.ityC = tr3.ityA = tr3.ityB = tr3.ityC = tr4.ityA = tr4.ityB = tr4.ityC = tr5.ityA = tr5.ityB = tr5.ityC = tr6.ityA = tr6.ityB = tr6.ityC = 1;

                DrawMethods.triangle(tr1, ref zbuffer, btm, color);
                DrawMethods.triangle(tr2, ref zbuffer, btm, color);
                DrawMethods.triangle(tr3, ref zbuffer, btm, color);
                DrawMethods.triangle(tr4, ref zbuffer, btm, color);
                DrawMethods.triangle(tr5, ref zbuffer, btm, color);
                DrawMethods.triangle(tr6, ref zbuffer, btm, color);
            }
        }
    }

    class GuitarSaddle: GuitarParts
    {
        public GuitarSaddle()
        {
            color = Color.DarkGray;
            m = new Matrix(4, 4);
            m1 = new Matrix(4, 4);
            m.matrix = new float[4, 4]
            {
                {-60f, 15f, 2f, 0},
                {-65f, 15f, 2f, 0},
                {-65f, -5f, 2f, 0},
                {-60f, -5f, 2f, 0}
                
            };
            m1.matrix = new float[4, 4]
            {
                {-60f, 15f, 0f , 0},
                {-65f, 15f, 0f , 0},
                {-65f, -5f, 0f , 0},
                {-60f, -5f, 0f , 0}
            };
        }

        public override void Draw(Bitmap btm, ref int[] zbuffer)
        {
            base.Draw(btm, ref zbuffer);

            Triangle tr1 = new Triangle(
                   new Vec3i((int)m.matrix[0, 0], (int)m.matrix[0, 1], (int)m.matrix[0, 2]),
                   new Vec3i((int)m.matrix[1, 0], (int)m.matrix[1, 1], (int)m.matrix[1, 2]),
                   new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]));
            Triangle tr2 = new Triangle(
                   new Vec3i((int)m.matrix[0, 0], (int)m.matrix[0, 1], (int)m.matrix[0, 2]),
                   new Vec3i((int)m.matrix[3, 0], (int)m.matrix[3, 1], (int)m.matrix[3, 2]),
                   new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]));

            tr1.ityA = tr1.ityB = tr1.ityC = tr2.ityA = tr2.ityB = tr2.ityC = 1;

            DrawMethods.triangle(tr1, ref zbuffer, btm, color);
            DrawMethods.triangle(tr2, ref zbuffer, btm, color);
        }

    }

    class GuitarNeck: GuitarParts
    {
        public GuitarNeck()
        {
            color = Color.SandyBrown;
            m = new Matrix(7, 4);
            m.matrix = new float[7, 4] {
                {-46f,     0f,   2f, 0},
                {-50f,     10f,   2f, 0},
                {80f,      10f,   2f, 0},
                {85f,      13f,   2f, 0},
                {110f,     0f,   2f, 0},
                {111f,     -10f,  2f, 0},
                {80f,      0f,   2f, 0},
                };

            m1 = new Matrix(7, 4);
            m1.matrix = new float[7, 4] {
                {-46f,     0f,   0, 0},
                {-50f,     10f,   0, 0},
                {80f,      10f,   0, 0},
                {85f,      13f,   0, 0},
                {110f,     0f,   0, 0},
                {111f,     -10f,  0, 0},
                {80f,      0f,   0, 0},
                };
        }

        public override void Draw(Bitmap bitmap, ref int[] zbuffer)
        {
            base.Draw(bitmap, ref zbuffer);
            {
                Triangle tr1 = new Triangle(
                    new Vec3i((int)m.matrix[0, 0], (int)m.matrix[0, 1], (int)m.matrix[0, 2]),
                    new Vec3i((int)m.matrix[1, 0], (int)m.matrix[1, 1], (int)m.matrix[1, 2]),
                    new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]));
                Triangle tr2 = new Triangle(
                    new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]),
                    new Vec3i((int)m.matrix[3, 0], (int)m.matrix[3, 1], (int)m.matrix[3, 2]),
                    new Vec3i((int)m.matrix[4, 0], (int)m.matrix[4, 1], (int)m.matrix[4, 2]));
                Triangle tr3 = new Triangle(
                    new Vec3i((int)m.matrix[4, 0], (int)m.matrix[4, 1], (int)m.matrix[4, 2]),
                    new Vec3i((int)m.matrix[5, 0], (int)m.matrix[5, 1], (int)m.matrix[5, 2]),
                    new Vec3i((int)m.matrix[6, 0], (int)m.matrix[6, 1], (int)m.matrix[6, 2]));
                Triangle tr4 = new Triangle(
                    new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]),
                    new Vec3i((int)m.matrix[4, 0], (int)m.matrix[4, 1], (int)m.matrix[4, 2]),
                    new Vec3i((int)m.matrix[6, 0], (int)m.matrix[6, 1], (int)m.matrix[6, 2]));
                Triangle tr5 = new Triangle(
                    new Vec3i((int)m.matrix[0, 0], (int)m.matrix[0, 1], (int)m.matrix[0, 2]),
                    new Vec3i((int)m.matrix[2, 0], (int)m.matrix[2, 1], (int)m.matrix[2, 2]),
                    new Vec3i((int)m.matrix[6, 0], (int)m.matrix[6, 1], (int)m.matrix[6, 2]));

                tr1.ityA = tr1.ityB = tr1.ityC = tr2.ityA = tr2.ityB = tr2.ityC = tr3.ityA = tr3.ityB = tr3.ityC = tr4.ityA = tr4.ityB = tr4.ityC = tr5.ityA = tr5.ityB = tr5.ityC = 1;

                DrawMethods.triangle(tr1, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr2, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr3, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr4, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr5, ref zbuffer, bitmap, color);
            }
            {
                Triangle tr1 = new Triangle(
                    new Vec3i((int)m1.matrix[0, 0], (int)m1.matrix[0, 1], (int)m1.matrix[0, 2]),
                    new Vec3i((int)m1.matrix[1, 0], (int)m1.matrix[1, 1], (int)m1.matrix[1, 2]),
                    new Vec3i((int)m1.matrix[2, 0], (int)m1.matrix[2, 1], (int)m1.matrix[2, 2]));
                Triangle tr2 = new Triangle(
                    new Vec3i((int)m1.matrix[2, 0], (int)m1.matrix[2, 1], (int)m1.matrix[2, 2]),
                    new Vec3i((int)m1.matrix[3, 0], (int)m1.matrix[3, 1], (int)m1.matrix[3, 2]),
                    new Vec3i((int)m1.matrix[4, 0], (int)m1.matrix[4, 1], (int)m1.matrix[4, 2]));
                Triangle tr3 = new Triangle(
                    new Vec3i((int)m1.matrix[4, 0], (int)m1.matrix[4, 1], (int)m1.matrix[4, 2]),
                    new Vec3i((int)m1.matrix[5, 0], (int)m1.matrix[5, 1], (int)m1.matrix[5, 2]),
                    new Vec3i((int)m1.matrix[6, 0], (int)m1.matrix[6, 1], (int)m1.matrix[6, 2]));
                Triangle tr4 = new Triangle(
                    new Vec3i((int)m1.matrix[2, 0], (int)m1.matrix[2, 1], (int)m1.matrix[2, 2]),
                    new Vec3i((int)m1.matrix[4, 0], (int)m1.matrix[4, 1], (int)m1.matrix[4, 2]),
                    new Vec3i((int)m1.matrix[6, 0], (int)m1.matrix[6, 1], (int)m1.matrix[6, 2]));
                Triangle tr5 = new Triangle(
                    new Vec3i((int)m1.matrix[0, 0], (int)m1.matrix[0, 1], (int)m1.matrix[0, 2]),
                    new Vec3i((int)m1.matrix[2, 0], (int)m1.matrix[2, 1], (int)m1.matrix[2, 2]),
                    new Vec3i((int)m1.matrix[6, 0], (int)m1.matrix[6, 1], (int)m1.matrix[6, 2]));

                tr1.ityA = tr1.ityB = tr1.ityC = tr2.ityA = tr2.ityB = tr2.ityC = tr3.ityA = tr3.ityB = tr3.ityC = tr4.ityA = tr4.ityB = tr4.ityC = tr5.ityA = tr5.ityB = tr5.ityC = 1;

                DrawMethods.triangle(tr1, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr2, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr3, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr4, ref zbuffer, bitmap, color);
                DrawMethods.triangle(tr5, ref zbuffer, bitmap, color);
            }
        }
    }

    class GuitarStrings: GuitarPart
    {
        public GuitarStrings()
        {
            color = Color.Black;
            m = new Matrix(14, 4);
            m.matrix = new float[14, 4] {
                {-63f,     8f,   2f, 0},
                {88f,      8f,   2f, 0},
                {-63f,     7f,   2f, 0},
                {90f,      7f,   2f, 0},
                {-63f,     6f,   2f, 0},
                {92f,      6f,   2f, 0},
                {-63f,     5f,   2f, 0},
                {94f,      5f,   2f, 0},
                {-63f,     4f,   2f, 0},
                {96f,      4f,   2f, 0},
                {-63f,     3f,   2f, 0},
                {98f,      3f,   2f, 0},
                {-63f,     2f,   2f, 0},
                {100f,     2f,   2f, 0}
            };
        }
    }

    /// <summary>
    /// Класс, содержащий поля и методы для отрисовки и преобразования гитары
    /// </summary>
    class Guitar
    {
        public GuitarBody body;
        public GuitarNeck neck;
        public GuitarStrings strings;
        public GuitarSaddle saddle;

        public Guitar()
        {
            body = new GuitarBody();
            neck = new GuitarNeck();
            strings = new GuitarStrings();
            saddle = new GuitarSaddle();
        }

        public void DrawLines(Bitmap btm, ref int[] zbuffer)
        {
            body.DrawLines(btm, ref zbuffer);
            neck.DrawLines(btm, ref zbuffer);
            strings.DrawLines(btm, ref zbuffer);
            saddle.DrawLines(btm, ref zbuffer);
        }
        public void Draw( Bitmap btm, ref int[] zbuffer)
        {
            body.Draw(btm, ref zbuffer);
            neck.Draw(btm, ref zbuffer);
            saddle.Draw(btm, ref zbuffer);
        }
        public void tune(int ugolX, int ugolY, float len)
        {
            body.tune(ugolX, ugolY, len);
            neck.tune(ugolX, ugolY, len);
            strings.tune(ugolX, ugolY, len);
            saddle.tune(ugolX, ugolY, len);
        }
        public void scale(float mx, float my, float mz)
        {
            body.scale(mx, my, mz);
            neck.scale(mx, my, mz);
            strings.scale(mx, my, mz);
            saddle.scale(mx, my, mz);
        }
        public void center(Bitmap bitmap)
        {
            body.center(bitmap);
            neck.center(bitmap);
            strings.center(bitmap);
            saddle.center(bitmap);
        }

        public void toDimetrix()
        {
            float Grad = (float)(2 * Math.PI) / 360;
            Matrix transform = new Matrix(4,4);
            float psi = 53.7f * Grad;
            float fi = 7f * Grad;
            transform.matrix = new float[4, 4]
            {
                {(float)Math.Cos(psi), (float)(Math.Sin(fi)*Math.Cos(psi)),     0, 0 },
                {0,                    (float)Math.Cos(psi),                    0, 0 },
                {(float)Math.Sin(psi), -(float)(Math.Sin(psi)*Math.Cos(psi)),   0, 0 },
                {0,                    0,                                       0, 1 }
            };
            //body.m = body.m.Multiple(transform);
            //body.m1 = body.m1.Multiple(transform);


        }

    }

}

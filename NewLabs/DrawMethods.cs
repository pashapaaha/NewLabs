using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace NewLabs
{

    class Vec3i
    {
        public int x, y, z;

        public Vec3i() { }

        public Vec3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3i(Vec3i a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        public Vec3i(Vec3f a)
        {
            x = (int)a.x;
            y = (int)a.y;
            z = (int)a.z;
        }

        public static Vec3i operator +(Vec3i a, Vec3i b)
        {
            Vec3i res = new Vec3i();
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            res.z = a.z + b.z;
            return res;
        }
        public static Vec3i operator -(Vec3i a, Vec3i b)
        {
            Vec3i res = new Vec3i();
            res.x = a.x - b.x;
            res.y = a.y - b.y;
            res.z = a.z - b.z;
            return res;
        }
        public static Vec3i operator *(Vec3i a, Vec3i b)
        {
            Vec3i res = new Vec3i();
            res.x = a.x * b.x;
            res.y = a.y * b.y;
            res.z = a.z * b.z;
            return res;
        }
        public static Vec3i operator +(Vec3i a, Vec3f b)
        {
            Vec3i res = new Vec3i();
            res.x = (int)(a.x + b.x);
            res.y = (int)(a.y + b.y);
            res.z = (int)(a.z + b.z);
            return res;
        }
    }
    class Vec3f
    {
        public float x, y, z;
        public Vec3f() { }
        public Vec3f(Vec3i a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        public Vec3f(Vec3f a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            Vec3f res = new Vec3f();
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            res.z = a.z + b.z;
            return res;
        }
        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            Vec3f res = new Vec3f();
            res.x = a.x - b.x;
            res.y = a.y - b.y;
            res.z = a.z - b.z;
            return res;
        }
        public static Vec3f operator *(Vec3f a, Vec3f b)
        {
            Vec3f res = new Vec3f();
            res.x = a.x * b.x;
            res.y = a.y * b.y;
            res.z = a.z * b.z;
            return res;
        }
        public static Vec3f operator *(Vec3f a, float d)
        {
            Vec3f res = new Vec3f();
            res.x = a.x* d;
            res.y = a.y* d;
            res.z = a.z * d;
            return res;
        }
    }
    class Triangle
    {
        public Vec3i a, b, c;
        public float ityA, ityB, ityC;
        public Triangle() {  }
        public Triangle(Vec3i va, Vec3i vb, Vec3i vc)
        {
            a = va;
            b = vb;
            c = vc;
        }
    }

    class DrawMethods
    {
        static public void triangle(Vec3i t0, Vec3i t1, Vec3i t2, float ity0, float ity1, float ity2,ref int[] zbuffer, Bitmap btm, Color color)
        {
            int width = btm.Width;
            int height = btm.Height;

            if (t0.y == t1.y && t0.y == t2.y) return; 
            if (t0.y > t1.y) { swap(ref t0, ref t1); swap(ref ity0, ref ity1); }
            if (t0.y > t2.y) { swap(ref t0, ref t2); swap(ref ity0, ref ity2); }
            if (t1.y > t2.y) { swap(ref t1, ref t2); swap(ref ity1, ref ity2); }

            int total_height = t2.y - t0.y;
            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.y - t0.y || t1.y == t0.y;
                int segment_height = second_half ? t2.y - t1.y : t1.y - t0.y;
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.y - t0.y : 0)) / segment_height; 
                Vec3i A = t0 + (new Vec3f(t2 - t0)) * alpha;
                Vec3i B = second_half ? t1 + new Vec3f(t2 - t1) * beta : t0 + new Vec3f(t1 - t0) * beta;
                float ityA = ity0 + (ity2 - ity0) * alpha;
                float ityB = second_half ? ity1 + (ity2 - ity1) * beta : ity0 + (ity1 - ity0) * beta;
                if (A.x > B.x) {
                    swap(ref A, ref B);
                    swap(ref ityA, ref ityB);
                }
                for (int j = A.x; j <= B.x; j++)
                {
                    float phi = B.x == A.x ? 1.0f : (float)(j - A.x) / (B.x - A.x);
                    Vec3i P = new Vec3i(new Vec3f(A) + new Vec3f(B - A) * phi);
                    float ityP = ityA + (ityB - ityA) * phi;
                    int idx = P.x + P.y * width;
                    if (P.x >= width || P.y >= height || P.x < 0 || P.y < 0) continue;
                    if (zbuffer[idx] < P.z)
                    {
                        zbuffer[idx] = P.z;
                        //image.set(P.x, P.y, TGAColor(255, 255, 255) * ityP);
                        btm.SetPixel(P.x, P.y, color);
                    }
                }
            }
        }

        static public void triangle(Triangle tr, ref int[] zbuffer, Bitmap btm, Color color)
        {
            Vec3i t0 = tr.a, t1 = tr.b, t2 = tr.c;
            float ity0 = tr.ityA, ity1 = tr.ityB, ity2 = tr.ityC;

            int width = btm.Width;
            int height = btm.Height;

            if (t0.y == t1.y && t0.y == t2.y) return;
            if (t0.y > t1.y) { swap(ref t0, ref t1); swap(ref ity0, ref ity1); }
            if (t0.y > t2.y) { swap(ref t0, ref t2); swap(ref ity0, ref ity2); }
            if (t1.y > t2.y) { swap(ref t1, ref t2); swap(ref ity1, ref ity2); }

            int total_height = t2.y - t0.y;
            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.y - t0.y || t1.y == t0.y;
                int segment_height = second_half ? t2.y - t1.y : t1.y - t0.y;
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.y - t0.y : 0)) / segment_height;
                Vec3i A = t0 + (new Vec3f(t2 - t0)) * alpha;
                Vec3i B = second_half ? t1 + new Vec3f(t2 - t1) * beta : t0 + new Vec3f(t1 - t0) * beta;
                float ityA = ity0 + (ity2 - ity0) * alpha;
                float ityB = second_half ? ity1 + (ity2 - ity1) * beta : ity0 + (ity1 - ity0) * beta;
                if (A.x > B.x)
                {
                    swap(ref A, ref B);
                    swap(ref ityA, ref ityB);
                }
                //for (int j = A.x; j <= B.x; j++)
                //{
                //    float phi = B.x == A.x ? 1.0f : (float)(j - A.x) / (B.x - A.x);
                //    Vec3i P = new Vec3i(new Vec3f(A) + new Vec3f(B - A) * phi);
                //    float ityP = ityA + (ityB - ityA) * phi;
                //    int idx = P.x + P.y * width;
                //    if (P.x >= width || P.y >= height || P.x < 0 || P.y < 0) continue;
                //    if (zbuffer[idx] < P.z)
                //    {
                //        zbuffer[idx] = P.z;
                //        //image.set(P.x, P.y, TGAColor(255, 255, 255) * ityP);
                //        btm.SetPixel(P.x, P.y, color);
                //    }
                //}                
                line(A, B, ref zbuffer, btm, color);
            }
        }

        static public void line(Vec3i A, Vec3i B, ref int[] zbuffer, Bitmap btm, Color color)
        {
            int width = btm.Width;
            int height = btm.Height;

            for (int j = A.x; j <= B.x; j++)
            {
                float phi = B.x == A.x ? 1.0f : (float)(j - A.x) / (B.x - A.x);
                Vec3i P = new Vec3i(new Vec3f(A) + new Vec3f(B - A) * phi);
                //float ityP = ityA + (ityB - ityA) * phi;
                int idx = P.x + P.y * width;
                if (P.x >= width || P.y >= height || P.x < 0 || P.y < 0) continue;
                if (zbuffer[idx] < P.z)
                {
                    zbuffer[idx] = P.z;
                    //image.set(P.x, P.y, TGAColor(255, 255, 255) * ityP);
                    btm.SetPixel(P.x, P.y, color);
                }
            }
        }



        static void swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
    }


}

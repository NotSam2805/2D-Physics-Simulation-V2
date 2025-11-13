using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Physics_Simulation_V2
{
    public class Vector
    {
        private float x;
        private float y;
        private float magnitude;

        public void SetX(float newX)
        {
            x = newX;
            CalcMagnitude();
        }
        public void SetY(float newY)
        {
            y = newY;
            CalcMagnitude();
        }
        public float GetX() { return x; }
        public float GetY() { return y; }
        public float GetMagnitude()
        {
            CalcMagnitude();
            return magnitude;
        }


        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            CalcMagnitude();
        }

        private void CalcMagnitude()
        {
            magnitude = (float)Math.Sqrt((x * x) + (y * y));
        }

        public void Add(Vector vector)
        {
            x += vector.GetX();
            y += vector.GetY();
            CalcMagnitude();
        }

        public void Multiply(float factor)
        {
            x *= factor;
            y *= factor;
            CalcMagnitude();
        }

        public void Normalise()
        {
            x = x / magnitude;
            y = y / magnitude;
        }

        public float AngleToX()
        {
            float angle;
            CalcMagnitude();
            if (x > 0 && y > 0)
            {
                angle = (float)Math.Acos(x / magnitude);
            }
            else if (x < 0 && y > 0)
            {
                angle = (float)Math.PI - (float)Math.Acos(-x / magnitude);
            }
            else if (x < 0 && y < 0)
            {
                angle = (float)Math.PI + (float)Math.Acos(-x / magnitude);
            }
            else
            {
                angle = (1.5f * (float)Math.PI) + (float)Math.Acos(-y / magnitude);
            }
            return angle;
        }

        /*
        public float AngleToY()
        {
            var angle = (float)Math.Acos(y / magnitude);
            if (x <= 0)
            {
                angle += (float)Math.PI / 2f;
            }
            if (y <= 0)
            {
                angle += (float)Math.PI / 2f;
            }
            return angle;
        }
        */

        public float ComponentInDirection(Vector direction)
        {
            return magnitude * (float)Math.Cos(AngleBetween(this, direction));
        }

        public string ToString()
        {
            return x + ", " + y;
        }

        public static Vector GetOrigin() { return new Vector(0, 0); }

        public static Vector Multiply(Vector a, float factor)
        {
            var vec = new Vector(a.GetX(), a.GetY());
            vec.SetX(a.GetX() * factor);
            vec.SetY(a.GetY() * factor);
            return vec;
        }

        public static Vector Sum(Vector a, Vector b)
        {
            var vec = new Vector(a.GetX(), a.GetY());
            vec.Add(b);
            return vec;
        }

        public static float AngleBetween(Vector a, Vector b)
        {
            return Math.Abs(a.AngleToX() - b.AngleToX());
        }

        public static float DotProduct(Vector a, Vector b)
        {
            return (a.GetX() * b.GetX()) + (a.GetY() * b.GetY());
        }
    }
}

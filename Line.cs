using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Physics_Simulation_V2
{
    public class Line
    {
        private float gradient;
        private float yIntercept;
        private float xIntercept;

        private float lowerYbound;
        private float upperYbound;
        private float lowerXbound;
        private float upperXbound;
        private float length;

        public float GetGradient() { return gradient; }
        public float GetYintercept() { return yIntercept; }
        public float GetLength() { return length; }

        public Line(float gradient, float yIntercept, float lowerYbound, float upperYbound)
        {
            this.gradient = gradient;
            this.yIntercept = yIntercept;
            this.xIntercept = (-yIntercept) / gradient;
            if (upperYbound < lowerYbound)
            {
                this.lowerYbound = upperYbound;
                this.upperYbound = lowerYbound;
            }
            else
            {
                this.lowerYbound = lowerYbound;
                this.upperYbound = upperYbound;
            }
            lowerXbound = CalcX(lowerYbound);
            upperXbound = CalcX(upperYbound);

            length = (float)Math.Sqrt(((upperYbound - lowerYbound) * (upperYbound - lowerYbound)) + ((upperXbound - lowerXbound) * (upperXbound - lowerXbound)));
        }

        public Line(float gradient, float yIntercept) : this(gradient, yIntercept, -10000, 10000) { }

        public float CalcX(float y)
        {
            return (y - yIntercept) / gradient;
        }

        public float CalY(float x)
        {
            return (gradient * x) + yIntercept;
        }

        public bool IsOnLine(Vector pos)
        {
            if (pos.GetY() - (gradient * pos.GetX()) == yIntercept)
            {
                return true;
            }
            return false;
        }

        public bool IsInBounds(Vector pos)
        {
            if (pos.GetY() <= upperYbound && pos.GetY() >= lowerYbound)
            {
                return true;
            }
            return false;
        }

        public bool IsOnLineAndInBounds(Vector pos)
        {
            return IsInBounds(pos) && IsOnLine(pos);
        }


        public static bool DoesIntersect(Line a, Line b)
        {
            if (a.GetGradient() == b.GetGradient())
            {
                return false;
            }
            return true;
        }

        public static Vector CalcIntersection(Line a, Line b)
        {
            float x = (b.GetYintercept() - a.GetYintercept()) / (a.GetGradient() - b.GetGradient());
            float y = a.CalY(x);

            return new Vector(x, y);
        }

        public static Line CalcLineBetweenPoints(Vector a, Vector b)
        {
            float gradient = (a.GetY() - b.GetY()) / (a.GetX() - b.GetX());
            float intercept = a.GetY() - (gradient * a.GetX());
            return new Line(gradient, intercept, a.GetY(), b.GetY());
        }
    }
}

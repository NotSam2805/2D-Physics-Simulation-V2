using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace _2D_Physics_Simulation_V2
{
    public class Frame
    {
        private int height;
        private int width;
        private Vector centre;
        public Color background;
        private Simulation sim;
        private Bitmap image;

        //Limits of frame
        private int xUpperLimit;
        private int yUpperLimit;
        private int xLowerLimit;
        private int yLowerLimit;

        public Frame(int height, int width, Color background, Vector centre, Simulation sim)
        {
            this.height = height;
            this.width = width;
            this.background = background;
            this.centre = centre;
            this.sim = sim;

            xUpperLimit = (int)Math.Round(centre.GetX()) + (width / 2);
            yUpperLimit = (int)Math.Round(centre.GetY()) + (height / 2);
            xLowerLimit = (int)Math.Round(centre.GetX()) - (width / 2);
            yLowerLimit = (int)Math.Round(centre.GetY()) - (height / 2);
        }

        public Bitmap GenerateImage()
        {
            image = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    image.SetPixel(x, y, background);
                }
            }

            for (int i = 0; i < sim.GetNumberOfParticles(); i++)
            {
                var pos = sim.GetParticlePosition(i);
                var roundPos = new Vector((float)Math.Round(pos.GetX()), (float)Math.Round(pos.GetY()));

                if (!IsPosInFrame(roundPos))
                {
                    continue;
                }

                roundPos.SetX(roundPos.GetX() + (width / 2));
                roundPos.SetY((height / 2) - roundPos.GetY());
                image.SetPixel((int)roundPos.GetX(), (int)roundPos.GetY(), sim.GetParticleColor(i));
            }

            return image;
        }

        public bool IsPosInFrame(Vector pos)
        {
            return (pos.GetX() < xUpperLimit) && (pos.GetX() > xLowerLimit) && (pos.GetY() > yLowerLimit) && (pos.GetY() < yUpperLimit);
        }

        public void SaveFrame(string fileName)
        {
            image.Save(fileName + ".png");
        }

        public void PrintAllPixels()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Console.WriteLine(image.GetPixel(x, y));
                }
            }
        }
    }
}

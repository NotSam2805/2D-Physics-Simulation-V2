using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _2D_Physics_Simulation_V2
{
    public class Animation
    {
        private Frame[] frames;
        private float fps;
        private float timeBetweenFrames;
        private Simulation sim;
        private float deltaTime;
        private float timeOfLastFrame;
        private int frameCount;

        //Frame info
        private Color background;
        private Vector centre;
        private int height;
        private int width;

        public Animation(float fps, float length, ref Simulation sim, int height, int width, Color background, Vector centre)
        {
            this.fps = fps;
            frames = new Frame[(int)Math.Round(fps * length)];
            timeBetweenFrames = 1f / fps;
            this.sim = sim;
            deltaTime = 0;
            timeOfLastFrame = sim.GetTime();
            frameCount = 0;

            this.centre = centre;
            this.background = background;
            this.width = width;
            this.height = height;

            sim.collisionCalcFreq = fps / 4f;
        }

        public void CheckDelta()
        {
            deltaTime = sim.GetTime() - timeOfLastFrame;
            while (deltaTime >= timeBetweenFrames)
            {
                GenerateNextFrame();
                deltaTime -= timeBetweenFrames;
            }
            timeOfLastFrame = sim.GetTime();
        }

        private void GenerateNextFrame()
        {
            frames[frameCount] = new Frame(height, width, background, centre, sim);
            frames[frameCount].GenerateImage();
            frameCount++;
        }

        public void StartSimulationRecording()
        {
            while (frameCount < frames.Length)
            {
                sim.StepTime(timeBetweenFrames);
                GenerateNextFrame();
            }
        }

        public void SaveFrames(string fileName)
        {
            for (int i = 0; i < frameCount; i++)
            {
                frames[i].SaveFrame(fileName + i);
            }
        }
    }
}

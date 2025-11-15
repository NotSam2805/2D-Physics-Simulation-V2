using System;
using System.Collections.Generic;

namespace _2D_Physics_Simulation_V2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sim = new Simulation(50);

            sim.AddParticle(new Particle("1", 1, new Vector(-10, -10)));
            sim.AddParticle(new Particle("2", 1, new Vector(-10, 10)));
            sim.GetParticle("1").color = System.Drawing.Color.Red;
            sim.GetParticle("2").color = System.Drawing.Color.Blue;
            sim.GetParticle("1").ApplyImpulse(new Vector(10, 10));
            sim.GetParticle("2").ApplyImpulse(new Vector(10, -10));

            using (var window = new GraphicsWindow(800, 600, "LearnOpenTK"))
            {
                window.Run();

                // Define a triangle (3 vertices)
                float[] triangle =
                {
                    0.0f, 0.5f, 0.0f,     // top
                    -0.5f, -0.5f, 0.0f,   // bottom left
                    0.5f, -0.5f, 0.0f     // bottom right
                };

                // Add the triangle to be drawn
                window.AddShape(triangle);
            }
        }
    }
}

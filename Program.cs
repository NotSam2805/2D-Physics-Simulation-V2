using System;

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
                // Example: a triangle
                float[] triangleVerts =
                {
                    0.0f, 0.5f, 0f,
                    -0.5f, -0.5f, 0f,
                    0.5f, -0.5f, 0f
                };

                window.AddShape(triangleVerts);

            }
        }
    }
}

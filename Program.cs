using System;

namespace _2D_Physics_Simulation_V2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sim = new Simulation(50);
            var ani = new Animation(10, 1.5f, ref sim, 100, 100, System.Drawing.Color.Black, Vector.GetOrigin());

            sim.AddParticle(new Particle("1", 1, new Vector(-10, -10)));
            sim.AddParticle(new Particle("2", 1, new Vector(-10, 10)));
            sim.GetParticle("1").color = System.Drawing.Color.Red;
            sim.GetParticle("2").color = System.Drawing.Color.Blue;
            sim.GetParticle("1").ApplyImpulse(new Vector(10, 10));
            sim.GetParticle("2").ApplyImpulse(new Vector(10, -10));

            ani.StartSimulationRecording();
            ani.SaveFrames("Collision test");

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}

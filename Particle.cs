using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace _2D_Physics_Simulation_V2
{
    public class Particle : PhysicsObject
    {
        private float size = 1;//ik particles should not have size, but idc
        public float springK = 0.9f;
        //Constructor
        public Particle(string id, float mass, Vector position)
        {
            this.id = id;
            this.mass = mass;
            this.position = position;
            velocity = new Vector(0, 0);
            resultantForce = new Vector(0, 0);
            size = 1;

            color = Color.White;
        }

        //Check if a point is within the particle bounds (used for collision detection)
        public override bool DoesOverlap(Vector pos)
        {
            pos.Multiply(-1);
            if (Vector.Sum(pos, position).GetMagnitude() <= size)
            {
                return true;
            }
            return false;
        }

        public override void TimeStep(float time)
        {
            ApplyResultantForce(time);
            UpdatePosition(time);
        }
    }
}

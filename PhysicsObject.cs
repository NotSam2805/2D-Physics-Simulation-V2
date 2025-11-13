using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace _2D_Physics_Simulation_V2
{
    public abstract class PhysicsObject
    {
        protected string id;

        protected Vector position;
        protected Vector velocity;
        protected Vector resultantForce;

        protected float mass;

        public Color color;

        //Get methods
        public Vector GetPosition() { return position; }
        public Color GetColor() { return color; }
        public string GetID() { return id; }
        public Vector GetVelocity() { return velocity; }
        public float GetMass() { return mass; }

        //Forces added to the resultant force will be applied with some time
        public void AddForce(Vector force)
        {
            resultantForce.Add(force);
        }

        //Accelerate the particle by some force over some time
        public void ApplyForce(Vector force, float time)
        {
            force.Multiply(1 / mass);
            force.Multiply(time);
            velocity.Add(force);
        }

        //Apply the resultant forces over the specified time
        protected void ApplyResultantForce(float time)
        {
            resultantForce.Multiply(1 / mass);
            resultantForce.Multiply(time);
            velocity.Add(resultantForce);
            resultantForce.SetX(0);
            resultantForce.SetY(0);
        }

        public void ApplyImpulse(Vector impulse)
        {
            impulse.Multiply(1 / mass);
            velocity.Add(impulse);
        }

        //Move the particle based on its current positon and velocity over some time
        protected void UpdatePosition(float time)
        {
            position.Add(Vector.Multiply(velocity, time));
        }

        //Estimate where the particle will be in some future time based on current velocity
        public Vector GetProjection(float forwardTime)
        {
            return Vector.Sum(position, Vector.Multiply(velocity, forwardTime));
        }

        //Check if a point is within the particle bounds (used for collision detection)
        public abstract bool DoesOverlap(Vector pos);

        public abstract void TimeStep(float time);
    }
}

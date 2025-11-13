using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace _2D_Physics_Simulation_V2
{
    public class Simulation
    {
        private Particle[] particles; //Storing all particles in one array (may change to some other data structure for efficiency with large numbers of particles)
        private int numberOfParticles;
        public float collisionCalcFreq = 10;
        private float collisionCheckTime;
        private List<ParticleCollisionEvent> particleCollisionEventsToCome;

        public int GetNumberOfParticles() { return numberOfParticles; }

        private float time;

        public float GetTime() { return time; }

        public Simulation(int maxParticles)
        {
            particles = new Particle[maxParticles];
            numberOfParticles = 0;

            time = 0;
            particleCollisionEventsToCome = new List<ParticleCollisionEvent>();
            collisionCheckTime = 1f / collisionCalcFreq;
        }

        public void AddParticle(Particle newParticle)
        {
            particles[numberOfParticles] = newParticle;
            numberOfParticles++;
        }

        public Vector GetParticlePosition(int particleIndex)
        {
            if (particles[particleIndex] == null)
            {
                throw (new Exception("Particle at index: " + particleIndex + " does not exist."));
            }

            return particles[particleIndex].GetPosition();
        }

        public Color GetParticleColor(int particleIndex)
        {
            if (particles[particleIndex] == null)
            {
                throw (new Exception("Particle at index: " + particleIndex + " does not exist."));
            }

            return particles[particleIndex].GetColor();
        }

        public void StepTime(float moveTime)//Try to move time but check if any events need to happen first
        {
            float lowestTime = moveTime;
            while (moveTime > 0f)
            {
                foreach (ParticleCollisionEvent colEvent in particleCollisionEventsToCome)
                {
                    if (colEvent.timeOfCollision < (time + lowestTime))
                    {
                        lowestTime = colEvent.timeOfCollision - time;
                        //Console.WriteLine("collision at time: " + lowestTime);
                    }
                }

                MoveTime(lowestTime);
                moveTime -= lowestTime;
                lowestTime = moveTime;
            }
        }

        //Step time by some amount, performing collision checks and position calculations for all physics objects
        public void MoveTime(float moveTime)
        {
            Console.WriteLine("Current time: " + time + ", stepping: " + moveTime);
            for (int i = 0; i < numberOfParticles; i++)
            {
                particles[i].TimeStep(moveTime);
                Console.WriteLine("Particle " + particles[i].GetID() + " position: " + particles[i].GetPosition().ToString());
            }
            time += moveTime;


            //Check if any collisions need to happen
            for (int i = 0; i < particleCollisionEventsToCome.Count; i++)
            {
                if (particleCollisionEventsToCome[i].timeOfCollision <= time - moveTime)
                {
                    Collide(particleCollisionEventsToCome[i]);
                    particleCollisionEventsToCome.Remove(particleCollisionEventsToCome[i]);
                }
            }

            collisionCheckTime -= moveTime;
            if (collisionCheckTime <= 0)
            {
                CheckParticleCollisions();
                collisionCheckTime = 1f / collisionCalcFreq;
            }

        }

        public ref Particle GetParticle(string particleID)
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                if (particles[i].GetID() == particleID)
                {
                    return ref particles[i];
                }
            }

            throw (new Exception("Particle with ID: " + particleID + " does not exist."));
        }

        public bool CheckExistingParticles(string checkID)
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                if (particles[i].GetID() == checkID)
                {
                    return true;
                    //throw (new Exception("Particle with ID: " + checkID + " already exists."));
                }
            }
            return false;
        }

        private void CheckParticleCollisions()
        {
            List<Line> courses = new List<Line>();
            List<string> particleIDs = new List<string>();
            for (int i = 0; i < numberOfParticles; i++)
            {
                //Find all lines between all current particle positions and projected positions by next collision check
                courses.Add(Line.CalcLineBetweenPoints(particles[i].GetPosition(), particles[i].GetProjection(1 / collisionCalcFreq)));
                particleIDs.Add(particles[i].GetID());
            }

            //Find any courses that intersect with any other courses
            for (int a = 0; a < courses.Count; a++)
            {
                for (int b = 0; b < courses.Count; b++)
                {
                    if (a == b) { continue; }
                    Vector intersect = Line.CalcIntersection(courses[a], courses[b]);
                    if (courses[a].IsInBounds(intersect) && courses[b].IsInBounds(intersect))
                    {
                        /* Find the time to the collision:
                         * - Find the distance between the particle and the intersect
                         * - Divide by the particle's velocity
                         * Can assume that velocity is in the same direction as the displacement of the intersect
                         */
                        var tempPartPos = GetParticle(particleIDs[a]).GetPosition();
                        tempPartPos.Multiply(-1);//To do vectory subtraction
                        var distToCol = intersect;
                        distToCol.Add(tempPartPos);

                        float timeToCol = distToCol.GetMagnitude() / GetParticle(particleIDs[a]).GetVelocity().GetMagnitude();
                        particleCollisionEventsToCome.Add(new ParticleCollisionEvent(particleIDs[a], particleIDs[b], intersect, timeToCol + time));
                    }
                }
            }

            //Need to remove copies of collision events where particle 1 and particle 2 are swapped
            for (int a = 0; a < particleCollisionEventsToCome.Count; a++)
            {
                for (int b = 0; b < particleCollisionEventsToCome.Count; b++)
                {
                    if (a == b) { continue; }
                    if ((particleCollisionEventsToCome[a].particleID1 == particleCollisionEventsToCome[b].particleID1 && particleCollisionEventsToCome[a].particleID2 == particleCollisionEventsToCome[b].particleID2) || (particleCollisionEventsToCome[a].particleID2 == particleCollisionEventsToCome[b].particleID1 && particleCollisionEventsToCome[a].particleID1 == particleCollisionEventsToCome[b].particleID2))
                    {
                        particleCollisionEventsToCome.RemoveAt(b);
                    }
                }
            }

            foreach (ParticleCollisionEvent col in particleCollisionEventsToCome)
            {
                Console.WriteLine(col.ToString());
            }
        }

        private void Collide(ref Particle a, ref Particle b, Vector positionOfCollision)
        {
            if (!a.DoesOverlap(positionOfCollision) || !b.DoesOverlap(positionOfCollision))
            {
                return;
            }

            var normal = b.GetPosition();
            normal.Multiply(-1);
            normal.Add(a.GetPosition());
            normal.Normalise();

            var relativeV = b.GetVelocity();
            relativeV.Multiply(-1);
            relativeV.Add(a.GetVelocity());

            var elasticity = a.springK * b.springK;

            var impulseMag = -(1 + elasticity) * Vector.DotProduct(relativeV, normal);
            var tempN = normal;
            tempN.Multiply((1 / a.GetMass()) + (1 / b.GetMass()));
            impulseMag = impulseMag / Vector.DotProduct(Vector.Multiply(normal, (1 / a.GetMass()) + (1 / b.GetMass())), normal);

            var impulse = Vector.Multiply(normal, impulseMag);
            a.ApplyImpulse(impulse);
            b.ApplyImpulse(Vector.Multiply(impulse, -1));
        }

        private void Collide(ParticleCollisionEvent particleEvent)
        {
            Collide(ref GetParticle(particleEvent.particleID1), ref GetParticle(particleEvent.particleID2), particleEvent.positionOfCollision);
        }
    }
}

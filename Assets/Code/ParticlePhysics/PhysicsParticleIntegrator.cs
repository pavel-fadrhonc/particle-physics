using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Code
{
    public class PhysicsParticleIntegrator : IPhysicsParticleIntegrator
    {
        private Vector3 gravity;
        private List<PhysicsParticle> particles = new List<PhysicsParticle>();
        private List<Constraint> constraints = new List<Constraint>();

        public int NumberOfInterations { get; set; }
        public List<PhysicsParticle> Particles => particles;
        public Bounds WorldBounds { get; set; }
        public UnityEngine.Vector3 Gravity
        {
            get => gravity;
            set
            {
                gravity = value;
            }
        }

        public void AddParticle(PhysicsParticle particle)
        {
            particles.Add(particle);   
        }

        public void AddConstraint(Constraint constraint)
        {
            constraints.Add(constraint);
        }

        public void AccumulateForces()
        {
            for (int particleIdx = 0; particleIdx < particles.Count; particleIdx++)
            {
                var particle = particles[particleIdx];

                particle.Acceleration = gravity;
            }
        }

        public void Integrate(float deltaTime)
        {
            for (int particleIdx = 0; particleIdx < particles.Count; particleIdx++)
            {
                var particle = particles[particleIdx];
                if (particle.IsStatic)
                    continue;
                
                var newPosition = (2 - particle.Drag) * particle.CurrentPosition - (1 - particle.Drag) * particle.PreviousPosition +
                                  particle.Acceleration * deltaTime * deltaTime;
                particle.PreviousPosition = particle.CurrentPosition;
                particle.CurrentPosition = newPosition;
            }
        }

        public void SatisfyConstraints()
        {
            for (int itIdx = 0; itIdx < NumberOfInterations; itIdx++)
            {
                for (int conIdx = 0; conIdx < constraints.Count; conIdx++)
                {
                    var constraint = constraints[conIdx];
                    
                    var delta = constraint.Particle2.CurrentPosition - constraint.Particle1.CurrentPosition;
                    //var deltaLength = delta.magnitude;
                    //var deltaLength = Mathf.Sqrt(Vector3.Dot(delta,delta));

                    var deltaLengthSq = Vector3.Dot(delta, delta);
                    var restLengthSq = constraint.RestLength * constraint.RestLength;
                    
                    // unwrapped taylor approximation of square root, for good explanation, see https://hackernoon.com/calculating-the-square-root-of-a-number-using-the-newton-raphson-method-a-how-to-guide-yr4e32zo
//                    var guess = constraint.RestLength;
//                    var deltaLength = (guess + deltaLengthSq / guess) * 0.5f;
//                    var diff = (deltaLength - constraint.RestLength) / deltaLength;

                   // var diff = 0.5f - restLengthSq / (restLengthSq + deltaLengthSq);
                    
//                    // Pseudo-code for satisfying (C2) using sqrt approximation
//                    delta = x1-x2;
//                    delta*=restlength*restlength/(delta*delta+restlength*restlength)-0.5;
//                    x1 += delta;
//                    x2 -= delta;          
// optimized
//                    var restLengthSq = constraint.RestLength * constraint.RestLength;
                    //delta *= (constraint.RestLength * constraint.RestLength) / (Vector3.Dot(delta, delta) + constraint.RestLength * constraint.RestLength) - 0.5f;
                    
//                    var d = delta.squaredLength();
//		
//                    var diff = (d - this.squared_rest_length) / ((this.squared_rest_length + d) * (p1_im + p2_im));
//		
//                    if (p1_im != 0){
//                        this.p1.setCurrent(p1.add(delta.multiply(p1_im * diff)));
//                    }
//		
//                    if (p2_im != 0){
//                        this.p2.setCurrent( p2.subtract(delta.multiply(p2_im*diff)) );
//                    }                  

                    var p1InvMass = constraint.Particle1.InvMass;
                    var p2InvMass = constraint.Particle2.InvMass;
                    
                    var diff = (deltaLengthSq - restLengthSq) / ((restLengthSq + deltaLengthSq) * (p1InvMass + p2InvMass));
                    
                    if (!constraint.Particle1.IsStatic)
                        constraint.Particle1.CurrentPosition += diff * p1InvMass * delta;

                    if (!constraint.Particle2.IsStatic)
                        constraint.Particle2.CurrentPosition -= diff * p2InvMass*  delta;
                }
                
//                for (int pIdx = 0; pIdx < particles.Count; pIdx++)
//                {
//                    var particle = particles[pIdx];
//                
//                
//
//                    // world bounds constraint
//                    var pos = Vector3.Min(Vector3.Max(WorldBounds.min, particle.CurrentPosition), WorldBounds.max);
//                    particle.CurrentPosition = pos;
//                }
                
            }
            
            
            
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Code
{
    public interface IPhysicsParticleIntegrator
    {
        Vector3 Gravity { get; set; }
        Bounds WorldBounds { get; set; }
        int NumberOfInterations { get; set; }

        List<PhysicsParticle> Particles { get; }
        
        void AddParticle(PhysicsParticle particle);
        void AddConstraint(Constraint constraint);

        void AccumulateForces();
        void Integrate(float deltaTime);
        void SatisfyConstraints();
    }
}
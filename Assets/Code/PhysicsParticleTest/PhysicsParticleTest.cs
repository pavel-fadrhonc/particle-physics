using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Zenject;

namespace Code.PhysicsParticleTest
{
    public class PhysicsParticleTest : IInitializable, IFixedTickable
    {
        private Dictionary<PhysicsParticleMB, PhysicsParticle> transToParticleDict = new Dictionary<PhysicsParticleMB, PhysicsParticle>();
        
        private readonly ReadOnlyCollection<PhysicsParticleMB> _particles;
        private readonly IPhysicsParticleIntegrator _particleIntegrator;
        private readonly List<ConstraintReference> _constraintReferences;
        private readonly ParticleFactory _particleFactory;
        private readonly Settings _settings;

        public PhysicsParticleTest(
            ReadOnlyCollection<PhysicsParticleMB> particles,
            IPhysicsParticleIntegrator particleIntegrator,
            List<ConstraintReference> constraintReferences,
            ParticleFactory particleFactory,
            Settings settings)
        {
            _particles = particles;
            _particleIntegrator = particleIntegrator;
            _constraintReferences = constraintReferences;
            _particleFactory = particleFactory;
            _settings = settings;
        }

        public void Initialize()
        {
            for (var index = 0; index < _particles.Count; index++)
            {
                var particleMB = _particles[index];
                var particle = _particleFactory.Create();
                particle.CurrentPosition = particleMB.transform.position;
                particle.PreviousPosition = particleMB.transform.position;
                particle.IsStatic = particleMB.IsStatic;
                particle.Mass = particleMB.Mass;
                particle.InvMass = 1 / particleMB.Mass;
                particle.Drag = particleMB.Drag;

                _particleIntegrator.AddParticle(particle);
                transToParticleDict[particleMB] = particle;
            }
            
            foreach (var constraintReference in _constraintReferences)
            {
                _particleIntegrator.AddConstraint(
                    new Constraint()
                    {
                        Particle1 = transToParticleDict[constraintReference.particle1MB],
                        Particle2 = transToParticleDict[constraintReference.particle2MB],
                        RestLength = (constraintReference.particle1MB.transform.position 
                                      - constraintReference.particle2MB.transform.position).magnitude
                    } );
            }

            _particleIntegrator.WorldBounds = _settings.bounds;
            _particleIntegrator.Gravity = _settings.gravity;
            _particleIntegrator.NumberOfInterations = _settings.NumberOfSolverIterations;
        }

        public void FixedTick()
        {
            for (int i = 0; i < _particleIntegrator.Particles.Count; i++)
            {
                var particle = _particleIntegrator.Particles[i];
                if (!particle.IsStatic)
                    continue;

                particle.CurrentPosition = _particles[i].transform.position;
            }

            _particleIntegrator.AccumulateForces();
            _particleIntegrator.Integrate(Time.fixedDeltaTime);
            _particleIntegrator.SatisfyConstraints();

            for (int i = 0; i < _particleIntegrator.Particles.Count; i++)
            {
                var particle = _particleIntegrator.Particles[i];

                _particles[i].transform.position = particle.CurrentPosition;
            }
        }

        public class ParticleFactory : PlaceholderFactory<PhysicsParticle> {}

        [Serializable]
        public class Settings
        {
            public int NumberOfSolverIterations;
            public Vector3 gravity;
            public Bounds bounds;
        }
    }
}
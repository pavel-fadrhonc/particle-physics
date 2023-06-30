using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Code.PhysicsParticleTest
{
    public class PhysicsConstraintsGenerator
    {
        public enum ParticleConstrainType
        {
            Grid
        }

        private readonly Settings _settings;

        public PhysicsConstraintsGenerator(
            Settings settings)
        {
            _settings = settings;
        }
        
        private List<Constraint> _constraints = new List<Constraint>();
        public ReadOnlyCollection<Constraint> Constraints => _constraints.AsReadOnly();

        public void GenerateConstraints(ReadOnlyCollection<PhysicsParticle> particles)
        {
            switch (_settings.constrainType)
            {
                case ParticleConstrainType.Grid:
                    
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SearchAndAddConstraint(ReadOnlyCollection<PhysicsParticle> particles, PhysicsParticle particle)
        {
            // search right 
            var particlesOnRight = particles
                .Where(p => Mathf.Approximately(p.CurrentPosition.y, particle.CurrentPosition.y) &&
                            p.CurrentPosition.x > particle.CurrentPosition.x);

            if (!particlesOnRight.Any())
                return;

            
        }

        [Serializable]
        public class Settings
        {
            public ParticleConstrainType constrainType;
        }
    }
}
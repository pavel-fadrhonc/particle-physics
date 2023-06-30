using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Code.PhysicsParticleTest
{
    [ExecuteInEditMode]
    public class PhysicsParticlesReference : MonoBehaviour
    {
        [SerializeField] private Transform particleParent;

        [SerializeField] [HideInInspector]
        private List<PhysicsParticleMB> particleTransforms = new List<PhysicsParticleMB>();
        public ReadOnlyCollection<PhysicsParticleMB> ParticleTransforms => particleTransforms.AsReadOnly();

        private void OnEnable()
        {
            var trans = new List<PhysicsParticleMB>(particleParent.GetComponentsInChildren<PhysicsParticleMB>());

            if (particleTransforms.Count != trans.Count)
            {
                particleTransforms.Clear();
                particleTransforms.AddRange(trans);
            }
        }
    }
}
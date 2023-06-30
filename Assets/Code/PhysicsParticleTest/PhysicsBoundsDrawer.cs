using System;
using UnityEngine;
using Zenject;

namespace Code.PhysicsParticleTest
{
    public class PhysicsBoundsDrawer : MonoBehaviour
    {
        private Bounds _bounds;
        
        [Inject]
        public void Construct(Bounds bounds)
        {
            _bounds = bounds;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);   
        }
    }
}
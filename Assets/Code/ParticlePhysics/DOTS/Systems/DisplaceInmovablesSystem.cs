using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ParticlePhysics.DOTS
{
    [DisableAutoCreation]
    public class DisplaceInmovablesSystem : SystemBase
    {
        private Vector3 _lastPosition;
        
        private readonly Transform _displacementParent;

        public DisplaceInmovablesSystem(
            Transform displacementParent)
        {
            _displacementParent = displacementParent;
            _lastPosition = displacementParent.position;
        }
        
        protected override void OnUpdate()
        {
            var position = _displacementParent.position;
            
            var displacement = position - _lastPosition;
            _lastPosition = position;
            
            Entities.ForEach(
                (ref ParticleCurrentPosition currentPosition,
                    in ParticleMass particleMass) =>
                {
                    if (particleMass.invMass > 0)
                        return;

                    currentPosition.Value -= (float3) displacement;
                }).ScheduleParallel();
        }
    }
}
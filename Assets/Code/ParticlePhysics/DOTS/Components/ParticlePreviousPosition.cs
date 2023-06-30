using Unity.Entities;
using Unity.Mathematics;

namespace ParticlePhysics.DOTS
{
    public struct ParticlePreviousPosition : IComponentData
    {
        public float3 Value;
    }
}
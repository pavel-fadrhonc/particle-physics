using Unity.Entities;
using Unity.Mathematics;

namespace ParticlePhysics.DOTS
{
    public struct ParticleAcceleration : IComponentData
    {
        public float3 Value;
    }
}
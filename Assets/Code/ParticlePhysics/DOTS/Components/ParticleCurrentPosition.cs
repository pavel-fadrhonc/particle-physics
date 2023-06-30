using Unity.Entities;
using Unity.Mathematics;

namespace ParticlePhysics.DOTS
{
    public struct ParticleCurrentPosition : IComponentData
    {
        public float3 Value;
    }
}
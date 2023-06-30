using Unity.Entities;

namespace ParticlePhysics.DOTS
{
    public struct ParticleMass : IComponentData
    {
        public float invMass;
    }
}
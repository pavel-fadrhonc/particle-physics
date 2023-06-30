using Unity.Entities;

namespace ParticlePhysics.DOTS
{
    public struct ParticleDistanceConstraint : IComponentData
    {
        public Entity particle1;
        public Entity particle2;
        public float restLength;
    }
}
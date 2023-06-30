using Unity.Entities;

namespace ParticlePhysics.DOTS
{
    [UpdateAfter(typeof(AccumulateParticleForcesSystem))]
    public class IntegratePacticleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.fixedDeltaTime;

            Entities
                .ForEach(
                    (   ref ParticlePreviousPosition particlePreviousPosition,
                        ref ParticleCurrentPosition particleCurrentPosition,
                        in ParticleDrag particleDrag,
                        in ParticleAcceleration particleAcceleration,
                        in ParticleMass particleMass) =>
                    {
                        if (particleMass.invMass == 0)
                            return;
                        
                        var newPosition = (2.0f - particleDrag.Value) * particleCurrentPosition.Value 
                                          - (1 - particleDrag.Value) * particlePreviousPosition.Value +
                                          particleAcceleration.Value * dt * dt;
            
                        particlePreviousPosition.Value = particleCurrentPosition.Value;
                        particleCurrentPosition.Value = newPosition;
                    }).ScheduleParallel();
        }
    }
}
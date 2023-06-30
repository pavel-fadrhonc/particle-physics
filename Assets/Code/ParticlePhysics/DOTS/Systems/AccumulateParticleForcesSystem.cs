using Unity.Entities;

namespace ParticlePhysics.DOTS
{
    public class AccumulateParticleForcesSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var gravity = Settings.GRAVITY;

            Entities.ForEach(
                (ref ParticleAcceleration particleAcceleration) =>
                {
                    {
                        particleAcceleration.Value = gravity;
                    }
                }
            ).ScheduleParallel();
        }
    }
}
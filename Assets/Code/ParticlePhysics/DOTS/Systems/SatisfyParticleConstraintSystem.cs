using System.Dynamic;
using Unity.Entities;
using Unity.Mathematics;

namespace ParticlePhysics.DOTS
{
    [UpdateAfter(typeof(IntegratePacticleSystem))]
    public class SatisfyParticleConstraintSystem  : SystemBase
    {
        protected override void OnUpdate()
//        {
// aint gonna work since we already reference iterating over on ParticleCurrentPosition
// and at the same time we want to write to this on some other entity - not possible since there can be race condition
//            Entities.
//                ForEach((ref ParticleCurrentPosition currentPosition,
//                        in ParticleMass mass,
//                        in ParticleDistanceConstraint constraint) =>
//                {
//                    if (!HasComponent<ParticleCurrentPosition>(constraint.otherParticle))
//                        return;
//                    if (!HasComponent<ParticleMass>(constraint.otherParticle))
//                        return;
//
//                    var otherPosition = GetComponent<ParticleCurrentPosition>(constraint.otherParticle);
//                    var otherMass = GetComponent<ParticleMass>(constraint.otherParticle);
//
//                    var delta = otherPosition.Value - currentPosition.Value;
//                    var deltaLengthSq = math.lengthsq(delta);
//                    var restLengthSq = constraint.restLength * constraint.restLength;
//
//                    var diff = (deltaLengthSq - restLengthSq) /
//                               ((restLengthSq + deltaLengthSq) * (mass.invMass + otherMass.invMass));
//
//                    currentPosition.Value += diff * mass.invMass * delta;
//                    otherPosition.Value -= diff * mass.invMass * delta;
//                }).Schedule();
//        }

        {
            for (int i = 0; i < Settings.NUM_ITERATIONS; i++)
            {
                Entities.ForEach((ref ParticleDistanceConstraint distanceConstraint) =>
                {
                    if (!HasComponent<ParticleCurrentPosition>(distanceConstraint.particle1) ||
                        !HasComponent<ParticleMass>(distanceConstraint.particle1) ||
                        !HasComponent<ParticleCurrentPosition>(distanceConstraint.particle2) ||
                        !HasComponent<ParticleMass>(distanceConstraint.particle2))
                        return;            
                
                    var particle1Pos = GetComponent<ParticleCurrentPosition>(distanceConstraint.particle1);
                    var particle1Mass = GetComponent<ParticleMass>(distanceConstraint.particle1);
                
                    var particle2Pos = GetComponent<ParticleCurrentPosition>(distanceConstraint.particle2);
                    var particle2Mass = GetComponent<ParticleMass>(distanceConstraint.particle2);
                
                    var delta = particle2Pos.Value - particle1Pos.Value;
                    var deltaLengthSq = math.lengthsq(delta);
                    var restLengthSq = distanceConstraint.restLength * distanceConstraint.restLength;

                    var diff = (deltaLengthSq - restLengthSq) /
                               ((restLengthSq + deltaLengthSq) * (particle1Mass.invMass + particle2Mass.invMass));

//                    var p1Pos = particle1Pos.Value + diff * particle1Mass.invMass * delta;
//                    var p2Pos = particle1Pos.Value + diff * particle2Mass.invMass * delta;;

                    if (particle1Mass.invMass > 0)
                    {
                        particle1Pos.Value += diff * particle1Mass.invMass * delta;
                        SetComponent(distanceConstraint.particle1, particle1Pos);
                    }

                    if (particle2Mass.invMass > 0)
                    {
                        particle2Pos.Value -= diff * particle2Mass.invMass * delta;
                        SetComponent(distanceConstraint.particle2, particle2Pos);
                    }

//                    particle1Pos.Value += new float3(0, 0.00f, 0);
//                    particle2Pos.Value += new float3(0, 0.01f, 0);

//                    particle1Pos.Value = p1Pos;
//                    particle2Pos.Value = p2Pos;
                    
                    
                    
                
                }).Schedule();                
            }
        }
    }
}
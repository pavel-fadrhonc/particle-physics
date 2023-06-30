using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace Code.PhysicsParticleTest
{
    [RequireComponent(typeof(PhysicsParticlesReference))]
    public class PhysicsParticleTestInstaller : MonoInstaller
    {
        public PhysicsParticleTest.Settings settings;
        //public PhysicsConstraintsGenerator.Settings constraintsSettings;
        public PhysicsConstraintsReference constraints;
        
        public override void InstallBindings()
        {
            Container.BindInstance(settings);
            var allParticles = GetComponent<PhysicsParticlesReference>().ParticleTransforms;
            Container.Bind<IPhysicsParticleIntegrator>().To<PhysicsParticleIntegrator>().AsSingle();
            Container.BindFactory<PhysicsParticle, PhysicsParticleTest.ParticleFactory>();
            
            Container.BindInterfacesTo<PhysicsParticleTest>().AsSingle().WithArguments(
                allParticles,
                constraints.Constraints
                );
            Container.BindInstance(settings.bounds);
            
            Container.BindInterfacesAndSelfTo<PhysicsConstraintsGenerator>().AsSingle();
            //Container.BindInstance(constraintsSettings);
        }
    }
}
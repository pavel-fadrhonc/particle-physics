using System;
using System.Collections.Generic;
using NaughtyAttributes;
using ParticlePhysics.DOTS;
using Unity.Entities;
using UnityEngine;

namespace Code.PhysicsParticleTest
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ParticleMeshGenerator : MonoBehaviour
    {
        public Vector2 clothDimensions;
        public Vector2Int clothResolution;
        public float defaultMass;
        public float defaultDrag;
        public Transform drivingTrans;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        
        private List<Entity> particleEntities = new List<Entity>();
        
        private void Awake()
        {
            Init();

            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = world.EntityManager;

            var invMass = 1 / defaultMass;
            var vertices = _meshFilter.mesh.vertices;
            
            for (int y = 0; y < clothResolution.y; y++)
            {
                for (int x = 0; x < clothResolution.x; x++)
                {
                    var index = GetClothIndex(x, y);
                    var vertex = vertices[index];
                    
                    // particle entities
                    var particleEntity = entityManager.CreateEntity();
                    entityManager.SetName(particleEntity, $"ClothVertex_{index}");
                    entityManager.AddComponentData(particleEntity, new ParticleAcceleration());
                    entityManager.AddComponentData(particleEntity, new ParticleCurrentPosition()
                    {
                        Value = vertex
                    });
                    entityManager.AddComponentData(particleEntity, new ParticlePreviousPosition()
                    {
                        Value = vertex
                    });
                    entityManager.AddComponentData(particleEntity, new ParticleMass()
                    {
                        invMass = y < clothResolution.y - 1 ? invMass : 0
                    });
                    entityManager.AddComponentData(particleEntity, new ParticleDrag()
                    {
                        Value = defaultDrag
                    });
                
                    particleEntities.Add(particleEntity);                    

                    // constraint entities
                    if (x > 0)
                    { // left constraint
                        var otherIndex = GetClothIndex(x - 1, y);
                        var otherVertex = vertices[otherIndex];
                        var otherParticleEntity = particleEntities[otherIndex];

                        var constraintEntity = entityManager.CreateEntity();
                        entityManager.SetName(constraintEntity, $"Constraint_{index}_{otherIndex}");
                        entityManager.AddComponentData(constraintEntity,
                            new ParticleDistanceConstraint()
                            {
                                particle1 = particleEntity,
                                particle2 = otherParticleEntity,
                                restLength = (otherVertex - vertex).magnitude
                            });
                    }

                    if (y > 0)
                    { // up constraint
                        var otherIndex = GetClothIndex(x, y - 1);
                        var otherVertex = vertices[otherIndex];
                        var otherParticleEntity = particleEntities[otherIndex];

                        var constraintEntity = entityManager.CreateEntity();
                        entityManager.SetName(constraintEntity, $"Constraint_{index}_{otherIndex}");
                        entityManager.AddComponentData(constraintEntity,
                            new ParticleDistanceConstraint()
                            {
                                particle1 = particleEntity,
                                particle2 = otherParticleEntity,
                                restLength = (otherVertex - vertex).magnitude
                            });
                    }
                }
            }

            var displaceSystem = world.CreateSystem<DisplaceInmovablesSystem>(drivingTrans);
            world.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(displaceSystem);
        }

        private void Update()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var vertices = _meshFilter.mesh.vertices;

            for (int y = 0; y < clothResolution.y; y++)
            {
                for (int x = 0; x < clothResolution.x; x++)
                {
                    var index = GetClothIndex(x, y);
                    var particleEntity = particleEntities[index];
                    var currentPosition = entityManager.GetComponentData<ParticleCurrentPosition>(particleEntity);
                    vertices[index] = currentPosition.Value;
                }
            }

            _meshFilter.mesh.vertices = vertices;
        }
        
        private int GetClothIndex(int x, int y) =>  y * clothResolution.y + x;

        [Button("Generate Cloth Mesh")]
        private void GenerateClothMesh()
        {
            Init();

            var cellSize = clothDimensions / clothResolution;
            Vector3[] vertices = new Vector3[clothResolution.x * clothResolution.y];
            Vector2[] uvs = new Vector2[clothResolution.x * clothResolution.y];
            //int[] triangles = new int[(clothResolution.x - 1) * (clothResolution.y - 1) * 2];
            List<int> triangles = new List<int>();

            for (int y = 0; y < clothResolution.y; y++)
            {
                for (int x = 0; x < clothResolution.x; x++)
                {
                    var idx = GetClothIndex(x, y);
                    
                    var vertexPos = new Vector3(x * cellSize.x, y * cellSize.y, 0); 
                    vertices[idx] = vertexPos;
                    
                    uvs[idx] = new Vector2((float) x / clothResolution.x, (float) y / clothResolution.y);

                    if (x > 0 && y > 0)
                    {
                        //    A___B
                        //    |\  |
                        //    | \ |
                        //    C   D = this
                        var A = GetClothIndex(x-1, y-1);
                        var B = GetClothIndex(x, y - 1);
                        var C = GetClothIndex(x - 1, y);
                        var D = idx;

                        triangles.Add(C);
                        triangles.Add(A);
                        triangles.Add(D);
                        triangles.Add(A);
                        triangles.Add(B);
                        triangles.Add(D);
                    }
                }
            }  
            
            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles.ToArray();

            _meshFilter.mesh = mesh;
        }

        private void Init()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}
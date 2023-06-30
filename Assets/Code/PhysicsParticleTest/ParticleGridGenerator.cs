using DefaultNamespace;
using NaughtyAttributes;
using UnityEngine;

namespace Code.PhysicsParticleTest
{
    [RequireComponent(typeof(PhysicsConstraintsReference))]
    public class ParticleGridGenerator : MonoBehaviour
    {
        [Tooltip("row / column count")]
        public Vector2Int gridSize;
        public Vector2 cellSize;
        public float defaultMass;
        
        [SerializeField][HideInInspector]
        private PhysicsParticleMB[][] grid = null;

        private PhysicsConstraintsReference constraintsReference;

        [Button("Generate")]
        public void Generate()
        {
            // delete old
            if (grid != null)
            {
                for (int x = 0; x < grid.Length; x++)
                {
                    for (int y = 0; y < grid[x].Length; y++)
                    {
                        if (grid[x][y] != null)
                        {
                            var go = grid[x][y].gameObject;
                            if (go != null)
                                DestroyImmediate(go);
                        }
                    }                
                }                
            }
            
            // create new
            grid = new PhysicsParticleMB[gridSize.x][];
            if (constraintsReference == null)
            {
                constraintsReference = GetComponent<PhysicsConstraintsReference>();
            }
            
            constraintsReference.ClearConstraints();

            for (int x = 0; x < gridSize.x; x++)
            {
                grid[x] = new PhysicsParticleMB[gridSize.y];
                
                for (int y = 0; y < gridSize.y; y++)
                {
                    var pos = new Vector3(x * cellSize.x, y * cellSize.y);
                    var go = new GameObject();
                    go.transform.SetParent(transform);
                    go.transform.localPosition = pos;
                    go.transform.rotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;
                    go.name = $"Particle_{x}_{y}";
                    //IconManager.SetIcon(go, IconManager.Icon.CircleGray);

                    var particleMB = go.AddComponent<PhysicsParticleMB>();
                    particleMB.Mass = defaultMass;

                    grid[x][y] = particleMB;

                    if (x > 0)
                    { // left constraint
                        var constraint = new ConstraintReference();
                        constraint.particle1MB = particleMB;
                        constraint.particle2MB = grid[x - 1][y];
                        constraintsReference.AddConstraint(constraint);
                    }

                    if (y > 0)
                    { // up constraint
                        var constraint = new ConstraintReference();
                        constraint.particle1MB = particleMB;
                        constraint.particle2MB = grid[x][y - 1];
                        constraintsReference.AddConstraint(constraint);
                    }
                }
            }

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    grid[x][y].transform.SetAsLastSibling();
                }
            }
        }
    }
}
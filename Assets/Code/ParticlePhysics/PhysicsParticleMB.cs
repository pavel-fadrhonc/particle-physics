using UnityEngine;

namespace Code
{
    public class PhysicsParticleMB : MonoBehaviour
    {
        [SerializeField]
        private bool isStatic = false;
        
        [SerializeField]
        private float mass;

        [SerializeField]
        private float drag;
        
        public float Mass
        {
            get => mass;
            set => mass = value;
        }

        public bool IsStatic
        {
            get => isStatic;
            set => isStatic = value;
        }

        public float Drag
        {
            get => drag;
            set => drag = value;
        }
    }
}
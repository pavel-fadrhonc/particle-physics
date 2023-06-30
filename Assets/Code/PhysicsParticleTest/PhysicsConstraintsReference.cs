using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Code.PhysicsParticleTest
{
    [Serializable]
    public class ConstraintReference
    {
        public PhysicsParticleMB particle1MB;
        public PhysicsParticleMB particle2MB;

        [SerializeField] [HideInInspector]
        private LineRenderer _lineRenderer;
        public LineRenderer LineRenderer
        {
            get => _lineRenderer;
            set => _lineRenderer = value;
        }
    }
    
    [ExecuteInEditMode]
    public class PhysicsConstraintsReference : MonoBehaviour
    {
        [SerializeField]
        private List<ConstraintReference> constraints = new List<ConstraintReference>();

        [Header("Line Settings")]
        public float width;
        public Material material;

        public List<ConstraintReference> Constraints => constraints;

        [Button("Regenerate Lines")]
        private void RegenerateLines()
        {
            ClearConstraints();

            foreach (var constraint in constraints)
            {
                GenerateLine(constraint);
            }
        }

        [Button("Toggle Lines")]
        private void ToggleLines()
        {
            foreach (var constraint in constraints)
            {
                constraint.LineRenderer.gameObject.SetActive(!constraint.LineRenderer.gameObject.activeSelf);
            }
        }

        public void AddConstraint(ConstraintReference constraintReference)
        {
            constraints.Add(constraintReference);
            GenerateLine(constraintReference);
        }

        public void ClearConstraints()
        {
            foreach (var constraint in constraints)
            {
                if (constraint.LineRenderer == null)
                    continue;

                if (Application.isEditor)
                    DestroyImmediate(constraint.LineRenderer.gameObject);
                else
                    Destroy(constraint.LineRenderer.gameObject);
            }      
            
            constraints.Clear();
        }

        private void LateUpdate()
        {
            foreach (var constraintReference in constraints)
            {
                if (constraintReference.LineRenderer == null)
                    GenerateLine(constraintReference);
                
                constraintReference.LineRenderer.SetPosition(0, constraintReference.particle1MB.transform.position);
                constraintReference.LineRenderer.SetPosition(1, constraintReference.particle2MB.transform.position);
            }
        }

        private void GenerateLine(ConstraintReference constraintReference)
        {
            var lineGo = new GameObject("constraint_" + constraintReference.particle1MB.name + "_" + constraintReference.particle1MB.name);
            lineGo.transform.SetParent(transform);
            lineGo.transform.localPosition = Vector3.zero;
            lineGo.transform.localRotation = Quaternion.identity;
            lineGo.transform.localEulerAngles = Vector3.one;

            var lineRend = lineGo.AddComponent<LineRenderer>();
            lineRend.startWidth = lineRend.endWidth = width;
            lineRend.material = material;

            constraintReference.LineRenderer = lineRend;
        }
    }
}
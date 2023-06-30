using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParticle
{
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private Vector3 acceleration;
    
    public bool IsStatic { get; set; }
    public float Mass { get; set; }
    public float Drag { get; set; }
    public float InvMass { get; set; }

    public Vector3 CurrentPosition
    {
        get => currentPosition;
        set => currentPosition = value;
    }

    public Vector3 PreviousPosition
    {
        get => previousPosition;
        set => previousPosition = value;
    }

    public Vector3 Acceleration
    {
        get => acceleration;
        set => acceleration = value;
    }
}

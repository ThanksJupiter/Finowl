using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementComponent : BaseComponent
{
    public NavMeshAgent agent { get; set; }

    public float walkSpeed = .9f;
    public float runSpeed = 3f;
    public float turnSpeed = .1f;
    public bool useAcceleration = true;
    public float lerpThreshold = .2f;
    public float accelerationSpeed = 1.5f;
    public bool alive = true;
    public bool movementAllowed { get; set; }
    public float blendAlpha { get; set; }

    public Vector3 velocity { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        movementAllowed = true;
    }

    public void SetMovementAllowed(bool value)
    {
        movementAllowed = value;
    }
}

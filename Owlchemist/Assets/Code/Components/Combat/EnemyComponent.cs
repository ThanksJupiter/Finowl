using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyComponent : BaseComponent
{
    [HideInInspector]
    public NavMeshAgent agent;
    public float moveSpeed = 1.5f;
    public float aggroRange = 20f;

    public bool isPursuingTransportable { get; set; }
    public bool isActive { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        isActive = true;
    }

    public void SetIsActive(bool value)
    {
        isActive = value;
    }
}

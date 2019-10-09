using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(NavMeshAgent))]
public class ChickenAIComponent : MonoBehaviour
{
    public AIWaypointSystem waypoints;
    public ChickenStateMachine stateMachine;
    public GameObject[] lightBombs;

    private NavMeshAgent agent;
    private AIPerception perceptionScript;
    private GameObject player;
    private LightComponent playerLightComponent;
    private CombatComponent playerCombatComponent;
    private GameManagerComponent gameManager;
    private GameObject world;
    private World worldSystem;

    public float waitTime = 0f;
    public float huntTime = 0f;
    public float stunTime = 5f;
    public float chargeTime = 1f;
    public float stalkingSpeed = 3.5f;
    public float chargeDistance = 3f;
    public float attackDistance = 1f;
    public float attackCooldown = 2f;
    public float fleeTorchDistance = 5f;
    public float chargeSpeed = 3.5f;
    public float playerSwipeDistance = 4f;

    private float nextPointDistance = 0.5f;
    private float originalWaitTime;
    private float originalHuntTime;
    private float remainingStunTime = 0f;
    [SerializeField]
    private float startChargeTime = 0f;
    private float originalWalkSpeed;
    private float originalAttackCooldown;
    private float originalChargeSpeed;

    public int damage = 1;

    private int currentWaypointIndex = 0;

    #region Animation Events

    public delegate void SetRunningDelegate();
    public SetRunningDelegate OnSetRunning;

    public delegate void SetWalkingDelegate();
    public SetWalkingDelegate OnSetWalking;

    public delegate void SetStalkingDelegate();
    public SetStalkingDelegate OnSetStalking;

    public delegate void TriggerLookAroundDelegate();
    public TriggerLookAroundDelegate OnTriggerLook;

    public delegate void TriggerStaggerDelegate();
    public TriggerStaggerDelegate OnTriggerStagger;

    public delegate void TriggerStompDelegate();
    public TriggerStompDelegate OnTriggerStomp;

    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(player, "Failed to locate GameObject with tag <b>Player</b>");
        agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent, "Failed to locate <b>NavMeshAgent</b> on Enemy GameObject");
        stateMachine = GetComponent<ChickenStateMachine>();
        Assert.IsNotNull(stateMachine, "Failed to locate <b>ChickenStateMachine.cs</b> on Enemy GameObject");
        perceptionScript = GetComponent<AIPerception>();
        Assert.IsNotNull(perceptionScript, "Failed to locate <b>AIPerception.cs</b> on Enemy GameObject");
        playerLightComponent = player.GetComponent<LightComponent>();
        Assert.IsNotNull(playerLightComponent, "Failed to locate <b>LightComponent.cs</b> on Player GameObject");
        playerCombatComponent = player.GetComponent<CombatComponent>();
        Assert.IsNotNull(playerCombatComponent, "Failed to locate <b>CombatComponent.cs</b> on Player GameObject");
        lightBombs = GameObject.FindGameObjectsWithTag("LightBomb");
        Assert.IsNotNull(lightBombs, "Failed to locate <b>LightBombs</b> in array check in Enemy GameObject");
        gameManager = player.GetComponent<GameManagerComponent>();
        Assert.IsNotNull(gameManager, "Failed to locate <b>GameManagerComponent.cs</b> on Player GameObject");
        world = GameObject.FindGameObjectWithTag("World");
        Assert.IsNotNull(world, "Failed to locate <b>World</b> under Main scene in hierarchy");
        worldSystem = world.GetComponent<World>();
        Assert.IsNotNull(worldSystem, "Failed to locate <b>World.cs</b> on World GameObject");

        originalWaitTime = waitTime;
        originalHuntTime = huntTime;
        originalAttackCooldown = attackCooldown;
        originalWalkSpeed = agent.speed;
        originalChargeSpeed = chargeSpeed;
    }

    void Start()
    {
        Assert.IsNotNull(waypoints, "Failed to locate <b>AIWayPointSystem</b> on Enemy GameObject");
        agent.SetDestination(waypoints.wayPoints[currentWaypointIndex]);
        Mathf.Clamp(huntTime, 0f, originalHuntTime);
    }
    
    void Update()
    {
        if (worldSystem.shouldTick)
        {
            agent.enabled = true;
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            remainingStunTime -= Time.deltaTime;
            if (startChargeTime >= chargeTime)
            {
                startChargeTime = 0f;
                stateMachine.SetPreviousStateFalse();
            }


            if (remainingStunTime <= 0)
            {
                stateMachine.stunned = false;
            }

            if (!stateMachine.seeingPlayer)
            {
                attackCooldown -= Time.deltaTime;
                huntTime -= Time.deltaTime;

                if (huntTime <= 0f)
                {
                    if (!agent.pathPending && !stateMachine.stunned && agent.remainingDistance <= nextPointDistance)
                    {
                        if (waitTime == originalWaitTime)
                        {
                            IdleOnSpot();
                        }
                        waitTime -= Time.deltaTime;
                        if (waitTime < 0)
                        {
                            GoToNextWaypoint();
                            stateMachine.SetPreviousStateFalse();
                            stateMachine.patrolling = true;
                            waitTime = originalWaitTime;
                        }
                    }
                }

                if (playerLightComponent.lightSource.enabled && !stateMachine.stunned && playerDistance <= fleeTorchDistance)
                {
                    Retreating();
                    if (agent.remainingDistance <= 0.5)
                    {
                        waitTime = originalWaitTime;
                    }
                }

                for (int i = 0; i < lightBombs.Length; i++)
                {
                     if (lightBombs[i].activeInHierarchy)
                     {
                        Retreating();
                         if (agent.remainingDistance <= 0.5)
                         {
                             waitTime = originalWaitTime;
                         }
                     }
                }


            }
            if (stateMachine.seeingPlayer && !stateMachine.stunned)
            {
                attackCooldown -= Time.deltaTime;
                agent.SetDestination(player.transform.position);
                if (attackCooldown <= 0f)
                {
                    if (agent.remainingDistance > chargeDistance)
                    {
                        StalkPlayer();
                    }
                    if (agent.remainingDistance <= chargeDistance && agent.remainingDistance > attackDistance && stateMachine.stalking && startChargeTime < chargeTime || 
                        agent.remainingDistance <= chargeDistance && agent.remainingDistance > attackDistance && stateMachine.attacking && startChargeTime < chargeTime)
                    {
                        ChargePlayer();
                        startChargeTime += Time.deltaTime;
                    }
                    if (agent.remainingDistance <= attackDistance && stateMachine.charging)
                    {
                        AttackPlayer();
                    }
                }

                if (playerLightComponent.lightSource.enabled && playerDistance <= fleeTorchDistance)
                {
                    agent.speed = originalWalkSpeed;
                    Retreating();
                    if (agent.remainingDistance <= 0.5)
                    {
                        waitTime = originalWaitTime;
                    }
                }

                for (int i = 0; i < lightBombs.Length; i++)
                {
                    if (lightBombs[i].activeInHierarchy)
                    {
                        Retreating();
                        if (agent.remainingDistance <= 0.5)
                        {
                            waitTime = originalWaitTime;
                        }
                    }
                }
            }

            if (playerCombatComponent.isSwiping && playerSwipeDistance >= playerDistance)
            {
                Stunned();
            }
        }
        if (!worldSystem.shouldTick) 
        {
            agent.enabled = false;
        }
    }

    public void GoToNextWaypoint()
    {
        OnSetWalking();
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.wayPoints.Length;
        agent.SetDestination(waypoints.wayPoints[currentWaypointIndex]);
        agent.speed = originalWalkSpeed;
    }

    public void ChargePlayer()
    {
        stateMachine.SetPreviousStateFalse();
        stateMachine.charging = true;
        agent.speed = chargeSpeed;
    }

    public void AttackPlayer()
    {
        OnTriggerStomp();
        stateMachine.SetPreviousStateFalse();
        stateMachine.attacking = true;
        player.GetComponent<HealthComponent>().TakeGranularDamageOverTime(100f, 0.3f, true);
        agent.speed = originalWalkSpeed;
        attackCooldown = originalAttackCooldown;
    }

    public void StalkPlayer()
    {
        OnSetStalking();
        stateMachine.SetPreviousStateFalse();
        stateMachine.stalking = true;
        huntTime = originalHuntTime;
        agent.speed = stalkingSpeed;
    }

    public void IdleOnSpot()
    {
        OnTriggerLook();
        stateMachine.SetPreviousStateFalse();
        stateMachine.idle = true;
    }

    public void Retreating()
    {
        OnSetRunning();
        stateMachine.SetPreviousStateFalse();
        stateMachine.retreating = true;
        Vector3 dirToPlayer = transform.position - player.transform.position;
        Vector3 newPos = transform.position + dirToPlayer;
        agent.SetDestination(newPos);
    }

    public void Stunned()
    {
        OnTriggerStagger();
        stateMachine.SetPreviousStateFalse();
        stateMachine.stunned = true;
        remainingStunTime = stunTime;
    }
}

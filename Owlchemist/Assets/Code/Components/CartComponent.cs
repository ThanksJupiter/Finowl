using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CartComponent : BaseComponent
{
    [HideInInspector]
    public NavMeshAgent agent;

    [Header("Combat")]
    public float attackRate = .5f;
    public LayerMask enemyMask;
    public float attackTimer { get; set; }

    [Header("Travel")]
    public float maxTravelSpeed = 1f;
    public float travelSpeed { get; set; }
    public float travelDistance = 10f;

    public bool isTraveling { get; set; }

    public float maxFuel = 100f;
    public float fuelDrain = 3f;
    public float fuelLevelPerResource = 10f;
    public float currentFuelLevel { get; set; }

    public float maxLight = 3;

    private Light powerLight;

    public void AddFuel(int amount)
    {
        float fuelPercentage = currentFuelLevel / maxFuel;
        currentFuelLevel += amount * fuelLevelPerResource;
        powerLight.intensity = Mathf.Lerp(0, maxLight, fuelPercentage);
        travelSpeed = Mathf.Lerp(0, maxTravelSpeed, fuelPercentage);
    }
    public void ConsumeFuel(float amount)
    {
        float fuelPercentage = currentFuelLevel / maxFuel;
        currentFuelLevel -= amount * fuelDrain;
        powerLight.intensity = Mathf.Lerp(0, maxLight, fuelPercentage);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        powerLight = GetComponentInChildren<Light>();
        currentFuelLevel = 0f;
        attackTimer = 0f;
        isTraveling = true;
    }

    public void SetPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }
}

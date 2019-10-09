using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GOAPComponent : BaseComponent, IGOAP
{
    // list actions & return them in getWorldState
    public FSM stateMachine;

    public FSM.FSMState idleState;
    public FSM.FSMState moveToState;
    public FSM.FSMState performActionState;

    public HashSet<GOAPAction> availableActions;
    public Queue<GOAPAction> currentActions { get; set; }

    public float regenerationRate = 10f;
    public float maxEnergy = 100f;

    private bool isActionInterrupted = false;
    private float energy = 0f;

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("damagePlayer", false));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
        goal.Add(new KeyValuePair<string, object>("stalkPlayer", true));
        return goal;
    }

    public bool CanAttack(float energyCost)
    {
        return energy >= energyCost;
    }

    public void InitializeTarget(Transform target)
    {
        
    }

    public void RegenerateEnergy(float dt)
    {
        energy += regenerationRate * dt;
    }

    public void ConsumeEnergy(float amount)
    {
        energy -= amount;
    }

    public bool HasMaxEnergy()
    {
        return energy >= maxEnergy;
    }

    public void AnnounceIdleState()
    {
        Debug.Log("Idle state");
    }

    public void AnnounceMoveToState()
    {
        Debug.Log("Move state");
    }

    public void AnnouncePerformActionState()
    {
        Debug.Log("Perform action state");
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions)
    {
        
    }

    public void ActionsFinished()
    {
        
    }

    public void PlanAborted(GOAPAction aborter)
    {

    }

    public bool IsAgentInRange(GOAPAction nextAction)
    {
        if (nextAction.IsActionInterrupted(gameObject))
        {
            isActionInterrupted = true;
        }

        float dist = Vector3.Distance(transform.position, nextAction.target.transform.position);
        
        if (dist < nextAction.interactRange)
        {
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }

    public bool IsActionInterrupted(GOAPAction action)
    {
        return isActionInterrupted;
    }
}

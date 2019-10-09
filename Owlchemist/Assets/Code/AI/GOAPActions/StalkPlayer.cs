using UnityEngine;
using UnityEngine.AI;

public class StalkPlayer : GOAPAction
{
    public bool isStalking { get; set; }

    public float distance = 5f;

    public StalkPlayer()
    {
        addEffect("stalkPlayer", true);
        addPrecondition("playerLit", true);
        isStalking = false;
    }

    public override void reset()
    {
        isStalking = false;
        target = null;
    }

    public override bool isDone()
    {
        return isStalking;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool perform(GameObject agent)
    {
        target = constantTarget;
        Vector3 dir = (agent.transform.position - target.transform.position).normalized;
        agent.GetComponent<NavMeshAgent>()?.SetDestination(target.transform.position + dir * distance);
        isStalking = true;
        return isStalking;
    }

    public override bool requiresInRange()
    {
        return false;
    }

    public override bool IsActionInterrupted(GameObject agent)
    {
        return false;
    }
}

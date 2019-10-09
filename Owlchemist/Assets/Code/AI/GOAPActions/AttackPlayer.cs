using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : GOAPAction
{
    public  bool hasAttacked = false;

    public float attackCost = 70;
    public int damage = 5;

    public AttackPlayer()
    {
        addEffect("damagePlayer", true);
        addPrecondition("playerLit", false);
        cost = attackCost;
    }

    public override void reset()
    {
        hasAttacked = false;
        target = null;
    }

    public override bool isDone()
    {
        return hasAttacked;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = constantTarget;

        LightComponent lc = target.GetComponent<LightComponent>();

        bool conditionMet = lc != null;

        return conditionMet;
    }

    public override bool perform(GameObject agent)
    {
        GOAPComponent gc = agent.GetComponent<GOAPComponent>();
        if (gc)
        {
            if (gc.CanAttack(attackCost))
            {
                hasAttacked = true;
                agent.GetComponent<GOAPComponent>()?.ConsumeEnergy(cost);
                target.GetComponent<HealthComponent>()?.TakeGranularDamageOverTime(damage * 50f, 1f, true);
            }
        }
        return hasAttacked;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool IsActionInterrupted(GameObject agent)
    {
        return target.GetComponent<WorldStateComponent>().IsPlayerLit();
        //return target.GetComponent<LightComponent>().IsLightEnabled();
    }
}

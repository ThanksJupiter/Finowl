using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportableSystem : BaseSystem
{
    public Filter[] filters;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(int id,
            GameObject go,
            CartComponent tc,
            CombatComponent cc
            )
        {
            this.id = id;

            gameObject = go;
            transportableComponent = tc;
            combatComponent = cc;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public CartComponent transportableComponent;
        public CombatComponent combatComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            CartComponent tc = objects[i].GetComponent<CartComponent>();
            CombatComponent cc = objects[i].GetComponent<CombatComponent>();

            if (tc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, tc, cc));
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            CartComponent transComp = filter.transportableComponent;
            CombatComponent combComp = filter.combatComponent;

            // ----- logic -----

            /*if (transComp.isTraveling)
            {
                transComp.ConsumeFuel(deltaTime);
                transComp.agent.Move(transComp.GetOwnerGO().transform.TransformDirection(transform.forward) * transComp.travelSpeed * deltaTime);
            }*/
        }
    }

    /*Projectile[] debugProjectiles;
    private void DetectTargets(CartComponent tc, CombatComponent cc)
    {
        Collider[] hits = Physics.OverlapSphere(tc.GetOwnerGO().transform.position, 15f, tc.enemyMask);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyComponent ec = hits[i].transform.GetComponentInParent<EnemyComponent>();
            if (ec)
            {
                if (tc.attackTimer > tc.attackRate)
                {
                    HealthComponent hc = ec.transform.GetComponentInParent<HealthComponent>();
                    LaunchProjectile(hc, cc);
                    tc.attackTimer = 0f;
                }
            }
        }
    }

    private void LaunchProjectile(HealthComponent target, CombatComponent cc)
    {
        cc.projectileList.Add(new Projectile(
            target,
            cc.GetOwnerGO().transform.position,
            target.GetOwnerGO().transform.position,
            cc.damage,
            cc.projectileSpeed
            ));

        
        cc.projectiles = cc.projectileList.ToArray();
        debugProjectiles = cc.projectiles;
    }

    private void MoveProjectiles(Projectile[] projectiles, float deltaTime)
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            Projectile p = projectiles[i];

            p.currentPosition = Vector3.Lerp(p.startPosition, p.target.gameObject.transform.position, p.lerpAlpha);
            p.IncreaseAlpha(deltaTime);
        }
    }

    private void HandleProjectileCollision(Filter filter)
    {
        CombatComponent cc = filter.combatComponent;
        for (int i = 0; i < cc.projectiles.Length; i++)
        {
            Projectile p = cc.projectiles[i];
            if (Vector3.Distance(p.currentPosition, p.target.GetOwnerGO().transform.position) < cc.hitThreshold)
            {
                if (p.target == null)
                {
                    cc.projectileList.Remove(p);
                    continue;
                }

                p.target.TakeDamage(p.damage);
                cc.projectileList.Remove(p);
            }
        }

        cc.projectiles = cc.projectileList.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (debugProjectiles == null) { return; }

        if (debugProjectiles.Length != 0)
        {
            Gizmos.color = new Color(1f, 0f, 0f, .5f);
            for (int i = 0; i < debugProjectiles.Length; i++)
            {
                Gizmos.DrawSphere(debugProjectiles[i].currentPosition, .5f);
            }
        }
    }*/
}

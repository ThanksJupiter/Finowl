using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : BaseSystem
{
    public Filter[] filters;
    EnvironmentComponent environmentComponent;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(int id,
            GameObject go,
            CombatComponent cc,
            InputComponent ic
            )
        {
            this.id = id;

            gameObject = go;
            combatComponent = cc;
            inputComponent = ic;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public CombatComponent combatComponent;
        public InputComponent inputComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            CombatComponent cc = objects[i].GetComponent<CombatComponent>();
            InputComponent ic = objects[i].GetComponent<InputComponent>();

            if (cc && ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, cc, ic));
            }
        }

        filters = tmpFilters.ToArray();

        environmentComponent = GetComponentInChildren<EnvironmentComponent>();
    }

    // for gizmos
    //private Projectile[] debugProjectiles;

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            CombatComponent combComp = filter.combatComponent;
            InputComponent inputComp = filter.inputComponent;


            // ----- logic -----

            /*if (inputComp.xButtonDown || Input.GetKeyDown(KeyCode.X))
            {
                PerformTorchSwing(combComp);
            }*/
            /*if (combComp.projectiles != null)
            {
                MoveProjectiles(combComp.projectiles, deltaTime);
                HandleProjectileCollision(filter);
            }

            if (combComp.isEngagedInCombat)
            {
                if (inputComp.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    inputComp.GetMouseWorldLocation(out hit);
                    HealthComponent hc = hit.transform.GetComponentInParent<HealthComponent>();

                    if (hc)
                    {
                        LaunchProjectile(hc, combComp);
                    }
                }
            }

            debugProjectiles = combComp.projectiles;*/
        }

        HealthComponent hc = player.healthComponent;

        if (hc.isTutorialNoTakeDamage) { return; }

        LightComponent lc = player.lightComponent;

        /*
                if (Input.GetKeyDown(KeyCode.H))
                {
                    hc.RestoreGranularDamageOverTime(100f, 5f, true);
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    hc.TakeGranularDamageOverTime(50f, 1f, true);
                }*/

        if (!hc.isHealing)
        {
            if (hc.isTakingDamage)
            {
                if (hc.healthAlterDuration > hc.currentAlterTime)
                {
                    float healthPercentage = hc.currentGranularHealth / hc.maxGranularHealth;

                    player.uiComponent.healthIndicator.SetImageFillAmount(hc.currentHealth, healthPercentage);
                    player.healthComponent.TakeGranularDamage(hc.alterTickAmount * deltaTime);
                    hc.currentAlterTime += Time.deltaTime;
                    return;
                }
                else
                {
                    hc.isTakingDamage = false;
                }
            }
            if (lc.nearbyLights.Count == 0 && !lc.IsLightEnabled())
            {
                if (hc.currentGranularHealth > 0)
                {
                    // pulse damage
                    hc.currentFogDamageTimer += deltaTime;
                    if (hc.currentFogDamageTimer > hc.fogDamageInterval)
                    {
                        hc.currentFogDamageTimer = 0f;
                        float healthPercentage = hc.currentGranularHealth / hc.maxGranularHealth;

                        player.uiComponent.healthIndicator.SetImageFillAmount(hc.currentHealth, healthPercentage);
                        hc.TakeGranularDamageOverTime(hc.darknessGranularHealthDrain, hc.darknessHealthDrainTime, false);
                        //player.healthComponent.TakeGranularDamage(hc.darknessGranularHealthDrain);
                        player.inputComponent.PulseVibrate(.3f);
                        player.uiComponent.healthIndicator.TriggerHeartDecay(hc.currentHealth);
                    }
                }
            }
            else
            {
                if (hc.currentGranularHealth <= hc.maxGranularHealth && !player.combatComponent.isRunning)
                {
                    float healthPercentage = hc.currentGranularHealth / hc.maxGranularHealth;

                    player.uiComponent.healthIndicator.SetImageFillAmount(hc.currentHealth, healthPercentage);
                    player.healthComponent.RestoreGranularDamage(hc.granularHealthRestorationAmount * deltaTime);
                }
            }
        }
        else
        {
            if (hc.healthAlterDuration > hc.currentAlterTime && hc.currentGranularHealth <= hc.maxGranularHealth)
            {
                float healthPercentage = hc.currentGranularHealth / hc.maxGranularHealth;
                player.uiComponent.healthIndicator.SetImageFillAmount(hc.currentHealth, healthPercentage);
                player.healthComponent.RestoreGranularDamage(hc.alterTickAmount * deltaTime);
                hc.currentAlterTime += Time.deltaTime;
            }
            else
            {
                hc.isHealing = false;
            }
        }
    }

    private void ThrowPotion()
    {
        player.animationComponent.OnPlayThrowAnimation();
    }

    private void PerformTorchSwing(CombatComponent cc)
    {
        player.animationComponent.OnPlayAttackAnimation();
        player.movementComponent.movementAllowed = false;
    }

    /*
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

                    p.target.TakeDamage();
                    cc.projectileList.Remove(p);
                }
            }

            cc.projectiles = cc.projectileList.ToArray();
        }*/

    /*private void OnDrawGizmos()
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

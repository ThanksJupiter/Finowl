using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSystem : BaseSystem
{
    public Filter[] filters;
    public GridComponent gridComp;
    private LightComponent lightyComp;
    
    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            InputComponent ic,
            LightComponent lc
            )
        {
            this.id = id;

            gameObject = go;
            lightComponent = lc;
            inputComponent = ic;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public LightComponent lightComponent;
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
            LightComponent lc = objects[i].GetComponent<LightComponent>();
            InputComponent ic = objects[i].GetComponent<InputComponent>();

            if (lc && ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic, lc));
                lightyComp = lc;
            }
        }

        filters = tmpFilters.ToArray();
        player.uiComponent.torchIndicator.SetImageFillAmount(1, 0f);
        player.uiComponent.torchIndicator.SetImageFillAmount(2, 0f);
        player.uiComponent.torchIndicator.SetImageFillAmount(3, 0f);
        gridComp = GetComponentInChildren<GridComponent>();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        //inputComponent.OnYButtonDown += ToggleLight;
        //inputComponent.OnXButtonDown += RechargeOneTorch;
        player.eventComponent.OnRefillOneTorch += RechargeOneTorch;
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            LightComponent lightComp = filter.lightComponent;
            InputComponent inputComp = filter.inputComponent;


            // ----- logic -----

            if (lightComp.currentCharges != 0)
            {
                if (inputComp.rightShoulderDown || Input.GetKeyDown(KeyCode.R))
                {
                    if (lightComp.currentFuel > 0)
                    {
                        SetLightActive(lightComp, !lightComp.IsLightEnabled(), true);
                    }
                }

                if (lightComp.currentFuel > 0 && lightComp.IsLightEnabled())
                {
                    DecayLight(lightComp, deltaTime);
                }
            }
            else if (inputComp.rightShoulderDown || Input.GetKeyDown(KeyCode.R))
            {
                FailAttemptSetLightActive(lightComp, !lightComp.IsLightEnabled(), true);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                DevRechargeTorches();
            }

            foreach (Light light in player.lightComponent.nearbyLights)
            {
                //Debug.Log(light.transform.parent.gameObject);
                if (!light.transform.parent.gameObject.activeSelf && player.lightComponent.nearbyLights.Contains(light))
                {
                    player.lightComponent.nearbyLights.Remove(light);
                    break;
                }
            }
        }
    }

    private void DecayLight(LightComponent lc, float deltaTime)
    {
        TorchIndicator indica = player.uiComponent.torchIndicator;
        float fuelDecay = player.combatComponent.isRunning ? lc.runningFuelDecay : lc.fuelDecay;
        lc.currentFuel -= fuelDecay * deltaTime;
        float fuelPercentage = lc.currentFuel / lc.maxFuel;
        lc.lightSource.intensity = Mathf.Lerp(lc.minIntensity, lc.maxIntensity, fuelPercentage);
        indica.SetImageFillAmount(lc.currentCharges, fuelPercentage);
        if (lc.lightSource.intensity <= lc.minIntensity)
        {
            SetLightActive(lc, false, false);
            if (lc.currentCharges > 0)
            {
                if (lc.currentCharges != 1)
                {
                    lc.currentFuel = lc.maxFuel;
                }

                lc.currentCharges--;
            }
        }
    }

    private void SetLightActive(LightComponent lc, bool value, bool triggerAnimation)
    {
        if (triggerAnimation)
        {
            if (player.movementComponent.movementAllowed)
            {
                player.animationComponent.OnPlayTorchToggleAnimation();
                player.movementComponent.SetMovementAllowed(false);
            }
        }
        else
        {
            lc.SetLightEnabled(value);
        }
    }

    private void FailAttemptSetLightActive(LightComponent lc, bool value, bool triggerAnimation)
    {
        if (triggerAnimation)
        {
            if (player.movementComponent.movementAllowed)
            {
                player.animationComponent.OnPlayTorchToggleAnimation();
                player.movementComponent.SetMovementAllowed(false);
            }
        }
    }

    private void RechargeOneTorch()
    {
        if (player.lightComponent.currentCharges < player.lightComponent.maxCharges)
        {
            if (player.lightComponent.currentCharges != 0)
            {
                player.uiComponent.torchIndicator.SetImageFillAmount(player.lightComponent.currentCharges, 1f);
            }
            
            player.lightComponent.currentCharges++;
            player.uiComponent.torchIndicator.PlayVfx(player.lightComponent.currentCharges);
            player.lightComponent.currentFuel = player.lightComponent.maxFuel;
            player.uiComponent.torchIndicator.SetImageFillAmount(player.lightComponent.currentCharges, 1f);
        }
        else
        {
            player.uiComponent.torchIndicator.SetImageFillAmount(player.lightComponent.currentCharges, 1f);
            player.lightComponent.currentFuel = player.lightComponent.maxFuel;
        }
    }

    private void DevRechargeTorches()
    {
        player.lightComponent.currentCharges = 3;
        player.lightComponent.currentFuel = player.lightComponent.maxFuel;
        player.uiComponent.torchIndicator.SetImageFillAmount(1, 1f);
        player.uiComponent.torchIndicator.SetImageFillAmount(2, 1f);
        player.uiComponent.torchIndicator.SetImageFillAmount(3, 1f);
    }

    private void OnDrawGizmos()
    {
        if (lightyComp == null) { return; }

        Gizmos.DrawWireSphere(lightyComp.transform.position, lightyComp.lightIntensity);
    }
}

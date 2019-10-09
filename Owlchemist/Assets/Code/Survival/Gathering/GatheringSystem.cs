using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatheringSystem : BaseSystem
{
    public Filter[] filters;
    public GameObject tempUIfix;
    public float interactedTime = 0f;
    public bool isGathering = false;
    public List<CollectibleComponent> pickedUpItem = new List<CollectibleComponent>();
    public BaseResource mushroomResource;
    public BaseResource flowerResource;
    private Vector3 VFXPos;

    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            InventoryComponent ic,
            InputComponent inc,
            InteractorComponent iac
            )
        {
            this.id = id;

            gameObject = go;
            inventoryComponent = ic;
            inputComponent = inc;
            interactorComponent = iac;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public InventoryComponent inventoryComponent;
        public InputComponent inputComponent;
        public InteractorComponent interactorComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here

        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            InventoryComponent ic = objects[i].GetComponent<InventoryComponent>();
            InteractorComponent iac = objects[i].GetComponent<InteractorComponent>();
            InputComponent inc = objects[i].GetComponent<InputComponent>();
            if (ic && iac && inc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic, inc, iac));
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
            InventoryComponent inventoryComp = filter.inventoryComponent;
            InputComponent inputComp = filter.inputComponent;
            InteractorComponent interactComp = filter.interactorComponent;


            // ----- logic -----
            if (inputComp.GetKeyDown(KeyCode.E) || inputComp.GetButtonDown("Fire1"))
            {
                isGathering = true;
            }
            else if (inputComp.GetKeyUp(KeyCode.E) || inputComp.GetButtonUp("Fire1") || isGathering == false)
            {
                //stop gather
                interactedTime = 0f;
                isGathering = false;
                player.uiComponent.gatheringPrompt.ProgressFill(0);
            }

            if (isGathering)
            {
                AttemptWorldInteract(interactComp, player.inventoryComponent, inputComp.worldInteractMask);
                interactComp.SetInteractMode(InteractMode.Object);
            }
            if (player.vfxComponent.gatherEffect.isPlaying)
            {
                player.vfxComponent.gatherEffect.transform.position = VFXPos;
            }
        }
    }
    private void AttemptWorldInteract(InteractorComponent interactorComp, InventoryComponent inventoryComp, LayerMask mask)
    {
        bool DontRemoveItemIfFull = false;
        if (player.gatheringComponent.nearbyCollectibles.Count != 0)
        {
            for (int i = 0; i < player.gatheringComponent.nearbyCollectibles.Count; i++)
            {
                if (player.gatheringComponent.nearbyCollectibles[i].isGatherable)
                {
                    CollectibleComponent c = player.gatheringComponent.nearbyCollectibles[i];

                    // if several items, interact with one most in front of player
                    if (interactedTime >= c.baseCollectible.interactionTime && c.isGatherable)
                    {
                        for (int y = 0; y < player.inventoryComponent.InventoryBag.Count; y++)
                        {

                            if (c.baseCollectible == player.inventoryComponent.InventoryBag[y].itemType.baseCollectible ||
                                player.inventoryComponent.InventoryBag[y].itemType.baseCollectible == player.inventoryComponent.tempColl.itemType.baseCollectible)
                            {
                                DontRemoveItemIfFull = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        //c.baseCollectible.PerformCollectibleEvent(player);

                        if (!c.baseCollectible.PerformCollectibleEvent(player))
                        {
                            if (DontRemoveItemIfFull)
                            {
                                if (c.TypeofCollectible != CollectibleComponent.CollectibleType.UTILITY)
                                {
                                    if (!player.vfxComponent.gatherEffect.isPlaying)
                                    {
                                        player.vfxComponent.gatherEffect.Play();
                                    }
                                    VFXPos = c.transform.position;
                                    GatherResource(c, inventoryComp);
                                }

                                player.animationComponent.OnPlayGatherAnimation();
                                interactedTime = 0f;
                            }
                        }
                        else
                        {
                            /*Light l = c.GetComponentInChildren<Light>();

                            if (l)
                            {
                                Debug.Log(l);
                                if (player.lightComponent.nearbyLights.Contains(l))
                                {
                                    player.lightComponent.nearbyLights.Remove(l);
                                }
                            }*/

                            player.eventComponent.OnHidePreviousGatherPrompt();
                            interactedTime = 0f;
                        }
                        if (DontRemoveItemIfFull)
                        {
                            if (c.baseCollectible.destroyObjectOnPickup)
                            {
                                c.gameObject.SetActive(false);
                                interactedTime = 0f;

                            }
                            player.gatheringComponent.nearbyCollectibles.Remove(c);
                        }
                        interactedTime = 0f;
                        DontRemoveItemIfFull = false;
                    }
                    else if (interactedTime <= c.baseCollectible.interactionTime && c.isGatherable)
                    {
                        interactedTime += Time.deltaTime;
                        player.uiComponent.gatheringPrompt.ProgressFill(interactedTime / c.baseCollectible.interactionTime);
                    }
                    if (!c.isGatherable)
                    {
                        isGathering = false;
                    }
                }
            }
        }
    }

    private void GatherResource(CollectibleComponent resource, InventoryComponent ic)
    {
        player.inventoryComponent.AddtoInventroy(resource, player.inventoryComponent, resource.Quantity);
        player.uiComponent.groceryList.UpdateList();
        pickedUpItem.Add(resource);
        //player.eventComponent.OnHidePreviousGatherPrompt();
        isGathering = false;
        player.eventComponent.OnGatherResource();
        /*
                if (resource == mushroomResource)
                {
                    player.gatheringComponent.mushroomAmount++;
                }
                else if (resource == flowerResource)
                {
                    player.gatheringComponent.flowerAmount++;
                }*/
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(
            player.go.transform.position,
            player.gatheringComponent.viewRadius,
            player.gatheringComponent.targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            CollectibleComponent c = targetsInViewRadius[i].GetComponent<CollectibleComponent>();

            if (c == null) { return; }

            Vector3 dirToTarget = (c.transform.position - player.go.transform.position).normalized;
            if (Vector3.Angle(player.go.transform.forward, dirToTarget) < player.gatheringComponent.viewAngle / 2)
            {
                if (!player.gatheringComponent.nearbyCollectibles.Contains(c))
                {
                    player.gatheringComponent.nearbyCollectibles.Add(c);
                    DisplayLootUIElement(c);
                }
            }
        }

        foreach (CollectibleComponent c in player.gatheringComponent.nearbyCollectibles)
        {
            if (!c.gameObject.activeSelf)
            {
                player.gatheringComponent.nearbyCollectibles.Remove(c);
                player.eventComponent.OnHideUIElement(player.uiComponent.gatheringPrompt.gameObject);
                break;
            }

            /*float dst = Vector3.Distance(
                c.transform.position,
                player.go.transform.position);*/

            Vector3 dirToTarget = (c.transform.position - player.go.transform.position).normalized;
            if (Vector3.Angle(player.go.transform.forward, dirToTarget) > player.gatheringComponent.viewAngle / 2)
            {
                player.gatheringComponent.nearbyCollectibles.Remove(c);
                player.eventComponent.OnHideUIElement(player.uiComponent.gatheringPrompt.gameObject);
                break;
            }
        }
    }

    private void DisplayLootUIElement(CollectibleComponent collectible)
    {
        string interactString = " to gather " + collectible.baseCollectible.name;
        player.uiComponent.gatheringPrompt.gatherTXT.text = interactString;
        player.eventComponent.OnDisplayUIElement(player.uiComponent.gatheringPrompt.gameObject, false);
    }
}

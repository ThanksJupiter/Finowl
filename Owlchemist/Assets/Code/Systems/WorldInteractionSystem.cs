using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class WorldInteractionSystem : BaseSystem
{
    public Filter[] filters;

    EnvironmentComponent environmentComponent;
    CartComponent transportableComponent;
    LightComponent lightComponent;
   // VillagerComponent villagerComponent;
    TravelComponent travelComponent;
    CollectibleComponent[] treeComponents;
    NavMeshComponent navMeshComponent;
    GOAPSystem goapSystem;
    private InteractableComponent cartInteractable;

    [Header("Settings")]
    float interactDistance = 2.5f;

    [Header("UI")]
    public Text interactTXT;
    public Text currentModeTXT;
    public GameObject bookGO;
    public GameObject mapGO;
    public GameObject craftGO;
    public GameObject toggleMapGO;
    public GameObject toggleBookGO;
    public GameObject playerGO;
    public Text lootTreeTXT;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(int id,
            GameObject go,
            InventoryComponent invC,
            InputComponent ic,
            InteractorComponent intC,
            MovementComponent movC
            )
        {
            this.id = go.GetInstanceID();

            gameObject = go;
            inventoryComponent = invC;
            inputComponent = ic;
            interactorComponent = intC;
            movementComponent = movC;
        }

        public int id;

        public GameObject gameObject;
        public InventoryComponent inventoryComponent;
        public InputComponent inputComponent;
        public InteractorComponent interactorComponent;
        public MovementComponent movementComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            InventoryComponent invC = objects[i].GetComponent<InventoryComponent>();
            InputComponent inpC = objects[i].GetComponent<InputComponent>();
            InteractorComponent intC = objects[i].GetComponent<InteractorComponent>();
            MovementComponent movC = objects[i].GetComponent<MovementComponent>();

            if (invC && inpC && intC && movC)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, invC, inpC, intC, movC));
            }
        }

        filters = tmpFilters.ToArray();

        environmentComponent = GetComponentInChildren<EnvironmentComponent>();
        transportableComponent = GetComponentInChildren<CartComponent>();
        cartInteractable = transportableComponent.GetComponent<InteractableComponent>();
        lightComponent = GetComponentInChildren<LightComponent>();
        //villagerComponent = GetComponentInChildren<VillagerComponent>();
        travelComponent = GetComponentInChildren<TravelComponent>();
        treeComponents = GetComponentsInChildren<CollectibleComponent>();
        goapSystem = GetComponent<GOAPSystem>();
        navMeshComponent = playerGO.GetComponent<NavMeshComponent>();
    }

    private bool isPlacingPlanks = false;
    private bool isCombat = false;
    private Vector3 plankPlaceLocation;
    private Vector3 actorLocation;

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            InventoryComponent invComp = filter.inventoryComponent;
            InputComponent inputComp = filter.inputComponent;
            InteractorComponent interactorComp = filter.interactorComponent;
            MovementComponent movComp = filter.movementComponent;

            // ----- logic -----
            CheckDistanceToInteractable(filter);
            FindVisibleTargets();

            // no longer used---------------------------
            InteractMode previous = interactorComp.interactMode;
            if (inputComp.GetKeyDown(KeyCode.Alpha1))
            {
                // move set none if previous was new to SetInteractMode
                interactorComp.SetInteractMode(InteractMode.PlacingPlanks);
                if (previous == InteractMode.PlacingPlanks)
                {
                    interactorComp.SetInteractMode(InteractMode.None);
                }

            } else if (inputComp.GetKeyDown(KeyCode.Alpha2))
            {
                interactorComp.SetInteractMode(InteractMode.Combat);
                if (previous == InteractMode.Combat)
                {
                    interactorComp.SetInteractMode(InteractMode.None);
                }
            }
            //--------------------------------------------------

            if (inputComp.GetKeyDown(KeyCode.E) || inputComp.GetButtonDown("Fire1"))
            {
                //AttemptWorldInteract(interactorComp, player.inventoryComponent, inputComp.worldInteractMask);
                //interactorComp.SetInteractMode(InteractMode.Object);

            } else if (inputComp.GetKeyDown(KeyCode.Escape))
            {
                interactorComp.interactMode = InteractMode.None;
                CartComponent tc = (CartComponent)interactorComp.currentInteractable;
                if (tc)
                {
                    tc.isTraveling = true;
                }
            }

            if (Vector3.Distance(filter.gameObject.transform.position, transportableComponent.transform.position) < 6f||
                Vector3.Distance(filter.gameObject.transform.position, travelComponent.transform.position) < 6f && lightComponent.IsLightEnabled())
            {
                goapSystem?.SetLightState(true);
            }
            else
            {
                goapSystem?.SetLightState(false);
            }

            if (Vector3.Distance(filter.gameObject.transform.position, transportableComponent.transform.position) < 3f)
            {
                if (inputComp.GetKeyDown(KeyCode.B) || inputComp.GetButtonDown("Fire3"))
                {
                    //bookGO.SetActive(!bookGO.activeSelf);
                    movComp.alive = false;
                }
                else
                {
                    movComp.alive = true;
                }

                if (inputComp.GetKeyDown(KeyCode.M) || inputComp.GetButtonDown("Fire2"))
                {
                    //mapGO.SetActive(!mapGO.activeSelf);
                }

                if (inputComp.GetKeyDown(KeyCode.E) || inputComp.GetButtonDown("Fire1"))
                {
                    //if (invComp.treeResources > 0)
                    //{
                    //    //craftingSystem?.CraftPotion1();
                    //    //craftGO.SetActive(!craftGO.activeSelf);
                    //}
                }
            }
            else
            {
                /*toggleMapGO.SetActive(false);
                toggleBookGO.SetActive(false);
                craftGO.SetActive(false);*/
            }

            isCombat = interactorComp.interactMode == InteractMode.Combat;
            actorLocation = interactorComp.gameObject.transform.position;
            if (interactorComp.interactMode == InteractMode.PlacingPlanks)
            {
                isPlacingPlanks = true;
                if (isPlacingPlanks)
                {
                    // Don't LogWarning this unity I'm sure you have your reasons but I don't like them.
                }
                // TODO remaing at last possible location if impossible position
                plankPlaceLocation = inputComp.GetMouseHoverTransformPosition(inputComp.objectPlaceMask);
                plankPlaceLocation.y = 0f;

                //if (inputComp.GetMouseButtonDown(0))
                //{
                //    PlacePlank(invComp);
                //}
            }
            else
            {
                isPlacingPlanks = false;
                if (inputComp.GetMouseButtonDown(0))
                {
                    inputComp.GetMouseWorldLocation(out RaycastHit hit);
                }
            }
        }
        if(Vector3.Distance(lightComponent.transform.position, transportableComponent.transform.position) < 7 && !lightComponent.IsLightEnabled()) //Magic number. Remove this 
        {
            //lightComponent.currentFuel = lightComponent.maxFuel;
        }
    }

    public void Travel(Transform t)
    {
        //navMeshComponent.surface.RemoveData();
        transportableComponent.gameObject.GetComponent<NavMeshAgent>().Warp(t.position + Vector3.up + Vector3.left * 5f);
        travelComponent.gameObject.GetComponent<NavMeshAgent>().Warp(t.position + Vector3.up);
/*        NavMeshSystem.UpdateNavMesh(navMeshComponent);*/
        //navMeshComponent.surface.BuildNavMesh();
        mapGO.SetActive(false);
/*        navSurfaceGO.SetActive(true);*/
    }

    private void CheckDistanceToInteractable(Filter filter)
    {
        Vector3 ownPos = filter.interactorComponent.transform.position;
        float dstToTransportable = Vector3.Distance(ownPos, transportableComponent.transform.position);
        if (dstToTransportable < interactDistance)
        {
            //DisplayInteractUIElement(cartInteractable);
        }
        else
        {
            //player.eventComponent.OnHideUIElement(player.uiComponent.interactPrompt.gameObject);
        }
    }

    private void DisplayInteractUIElement(InteractableComponent interactable)
    {
        string interactString = "hold [A]" + interactable.interactionString;
        player.uiComponent.interactPrompt.interactTXT.text = interactString;
        player.eventComponent.OnDisplayUIElement(player.uiComponent.interactPrompt.gameObject, false);
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(
            player.go.transform.position,
            player.interactorComponent.viewRadius,
            player.interactorComponent.targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            InteractableComponent interactable = targetsInViewRadius[i].GetComponent<InteractableComponent>();

            if (interactable == null) { return; }

            player.uiComponent.gatheringPrompt.SetPositionofText(targetsInViewRadius[0].transform.position);
            Vector3 dirToTarget = (interactable.transform.position - player.go.transform.position).normalized;
            if (Vector3.Angle(player.go.transform.forward, dirToTarget) < player.interactorComponent.viewAngle / 2)
            {
                if (!player.interactorComponent.nearbyInteractables.Contains(interactable))
                {
                    CollectibleComponent c = interactable as CollectibleComponent;
                    if (c)
                    {
                        if (!player.gatheringComponent.nearbyCollectibles.Contains(c))
                        {
                            player.gatheringComponent.nearbyCollectibles.Add(c);
                            player.eventComponent.OnDisplayGatherPrompt(c);
                        }
                    }

                    Light lc = interactable.GetComponentInChildren<Light>();
                    if (lc)
                    {
                        if (!player.lightComponent.nearbyLights.Contains(lc) && lc.enabled)
                        {
                            player.lightComponent.nearbyLights.Add(lc);
                        }
                    }

                    player.interactorComponent.nearbyInteractables.Add(interactable);
                    //DisplayInteractUIElement(c);
                }
            }
        }

        foreach (InteractableComponent interactable in player.interactorComponent.nearbyInteractables)
        {
            if (!interactable.gameObject.activeSelf)
            {
                player.interactorComponent.nearbyInteractables.Remove(interactable);
                player.eventComponent.OnHideUIElement(player.uiComponent.gatheringPrompt.gameObject);
                break;
            }

            float dst = Vector3.Distance(
                interactable.transform.position,
                player.go.transform.position);

            Vector3 dirToTarget = (interactable.transform.position - player.go.transform.position).normalized;

            if (dst > 4 || Vector3.Angle(player.go.transform.forward, dirToTarget) > player.interactorComponent.viewAngle / 2)
            {
                CollectibleComponent c = interactable as CollectibleComponent;
                if (c)
                {
                    if (player.gatheringComponent.nearbyCollectibles.Contains(c))
                    {
                        player.gatheringComponent.nearbyCollectibles.Remove(c);
                        player.eventComponent.OnHidePreviousGatherPrompt();
                    }
                }

                Light lc = interactable.GetComponentInChildren<Light>();
                if (lc)
                {
                    if (player.lightComponent.nearbyLights.Contains(lc))
                    {
                        player.lightComponent.nearbyLights.Remove(lc);
                    }
                }

                player.interactorComponent.nearbyInteractables.Remove(interactable);
                player.eventComponent.OnHideUIElement(player.uiComponent.gatheringPrompt.gameObject);
                break;
            }
        }
    }

    private void AttemptWorldInteract(InteractorComponent interactorComp, InventoryComponent inventoryComp, LayerMask mask)
    {
        Transform t = interactorComp.GetOwnerGO().transform;
        Vector3 position = t.position + t.forward;

        RaycastHit[] hits;
        hits = Physics.BoxCastAll(position, Vector3.one, t.forward, Quaternion.identity, 1f, mask);

        for (int i = 0; i < hits.Length; i++)
        {
            CartComponent tc = hits[i].transform.GetComponentInParent<CartComponent>();
            if (tc)
            {
                interactorComp.currentInteractable = tc;
                tc.isTraveling = false;
                interactorComp.SetInteractMode(InteractMode.Object);
            }

           // BaseResource treeComp = hits[i].transform.gameObject.GetComponentInParent<CollectibleComponent>().baseCollectible;
           // if (treeComp != null)
//             {
//                 GatherResource(treeComp, inventoryComp);
//             }
        }
    }

    private void InteractWithWorldObject(RaycastHit hit, InteractorComponent interactorComp, InventoryComponent inventoryComp)
    {
        switch (hit.transform.gameObject.GetComponentInParent<CollectibleComponent>().baseCollectible)
        {
            case BaseResource t:
            //    GatherResource(t, inventoryComp);
                break;

            default:
                break;
        }

        BaseResource resourceComp = hit.transform.gameObject.GetComponentInParent<CollectibleComponent>().baseCollectible;
        if (resourceComp != null)
        {
           // GatherResource(resourceComp, inventoryComp);
        }

        CartComponent transComp = hit.transform.GetComponent<CartComponent>();
        if (transComp != null)
        {
            interactorComp.interactMode = InteractMode.Object;
            interactorComp.isInteracting = true;
            interactorComp.currentInteractable = transComp;
            //if(player.go.GetComponent<InventoryComponent>().InventoryBag.Count > 0)
            //{
            //    transComp.gameObject.GetComponent<InventoryComponent>().TransferItemsFromTo(
            //        player.go.GetComponent<InventoryComponent>(), 
            //        transComp.gameObject.GetComponent<InventoryComponent>(), true);
            //}
        }
    }

    //private void GatherResource(BaseResource resource, InventoryComponent ic)
    //{
    //    //ic.AlterTreeResourceAmount(tc.Gather());
    //    //if (playerGO.GetComponent<Inventory>())
    //    player.inventoryComponent.AddtoInventroy(resource, player.inventoryComponent);
    //}

    //private void PlacePlank(InventoryComponent ic)
    //{
    //    if (ic.treeResources <= 0)
    //    {
    //        Debug.Log("not enough resources to place plank");
    //        return;
    //    }

    //    GameObject go = Instantiate(
    //        ic.planksPrefab,
    //        plankPlaceLocation + Vector3.up * 0.01f,
    //        Quaternion.identity
    //        );

    //    ic.AlterTreeResourceAmount(-1);
    //}

    //private void OnDrawGizmos()
    //{
    //    if (isPlacingPlanks && plankPlaceLocation != Vector3.zero)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireCube(plankPlaceLocation, new Vector3(1f, 0.3f, 1f));
    //    } else if (isCombat)
    //    {
    //        Gizmos.color = new Color(1f, 0f, 0f, .5f);
    //        Gizmos.DrawSphere(actorLocation + Vector3.up * 1.5f, .4f);
    //    }
    //}
}

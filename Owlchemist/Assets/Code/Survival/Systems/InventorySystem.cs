using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : BaseSystem
{
    public Filter[] filters;
    public GameObject tempUIfix;
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            InventoryComponent ic,
            InputComponent inc,
            InteractorComponent iac,
            CartComponent cart
            )
        {
            this.id = id;

            gameObject = go;
            inventoryComponent = ic;
            inputComponent = inc;
            interactorComponent = iac;
            cartComponent = cart;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public InventoryComponent inventoryComponent;
        public InputComponent inputComponent;
        public InteractorComponent interactorComponent;
        public CartComponent cartComponent;
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
            CartComponent cart = objects[i].GetComponent<CartComponent>();
            if (ic && iac && inc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic, inc, iac, cart));

            }
        }
        //  OnStart();
        //player.go.GetComponent<InventoryComponent>().invUI = tempUIfix.GetComponent<UseInventorySystem>();//*/GameObject.FindObjectOfType<UseInventorySystem>();

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
            CartComponent cartComp = filter.cartComponent;


            // ----- logic -----
            if (inputComp.GetKeyDown(KeyCode.E) || inputComp.GetButtonDown("Fire1"))
            {
                //if (invComp.treeResources > 0)
                //{
                //    //craftingSystem?.CraftPotion1();
                //    //craftGO.SetActive(!craftGO.activeSelf);
                //}
                AttemptWorldInteract(interactComp, player.inventoryComponent, inputComp.worldInteractMask);
                interactComp.SetInteractMode(InteractMode.Object);
            }
            if (inputComp.upDPad)
            {
                DropItem(0);
            }
            else if (inputComp.rightDPad)
            {
                DropItem(1);

            }
            else if (inputComp.downDPad)
            {
                DropItem(2);

            }
            else if (inputComp.leftDPad)
            {
                DropItem(3);

            }

        }
    }

    public void SetNewPositionandActivateObjectatDrop(Vector3 newPosition, GameObject droppedItem)
    {
        droppedItem.SetActive(true);
        if (droppedItem.GetComponent<CollectibleComponent>().TypeofCollectible != CollectibleComponent.CollectibleType.BATTLEPOTION)
        {
            droppedItem.transform.position = player.go.transform.position + player.go.transform.forward;//new Vector3(newPosition.x, newPosition.y, newPosition.z);
        }
        RaycastHit rayhit;
        Ray ray = new Ray(player.go.transform.position + player.go.transform.forward, Vector3.down);

        if (Physics.Raycast(ray, out rayhit, 1000, player.interactorComponent.groundMask))
        {
            if (droppedItem.GetComponent<CombatPotion>())
            {
                if (droppedItem.GetComponent<CombatPotion>().BattleType != CombatPotion.BattlePoition.QUEST)
                {
                    droppedItem.transform.position = player.go.transform.position;
                    droppedItem.transform.position = new Vector3(newPosition.x, rayhit.point.y /*+(droppedItem.GetComponent<MeshRenderer>().bounds.center.y / 2)*/, newPosition.z);
                }
            }
            else
            {
                droppedItem.transform.position = player.go.transform.position;
                droppedItem.transform.position = new Vector3(newPosition.x, rayhit.point.y /*+(droppedItem.GetComponent<MeshRenderer>().bounds.center.y / 2)*/, newPosition.z);
            }

        }
        if (droppedItem.GetComponent<CollectibleComponent>().TypeofCollectible == CollectibleComponent.CollectibleType.BATTLEPOTION)
        {
            droppedItem.GetComponent<CollectibleComponent>().isGatherable = false;
            if (droppedItem.GetComponent<CombatPotion>().BattleType == CombatPotion.BattlePoition.HEALTH)
            {
                player.go.GetComponent<HealthComponent>().RestoreGranularDamageOverTime(100, 6f, true);
            }
            if (droppedItem.GetComponent<CombatPotion>().BattleType == CombatPotion.BattlePoition.LIGHT)
            {
                player.go.GetComponent<HealthComponent>().RestoreGranularDamageOverTime(10, 3f, true);
            }
            if (droppedItem.GetComponent<CombatPotion>().BattleType == CombatPotion.BattlePoition.QUEST)
            {
                player.go.GetComponent<HealthComponent>().RestoreGranularDamageOverTime(10, 5f, true);
                if (Vector3.Distance(droppedItem.GetComponent<CombatPotion>().questObjective.transform.position, player.go.transform.position) < 5)
                {
                    droppedItem.GetComponent<CombatPotion>().questComplete = true;
                    player.go.GetComponent<PlayerEventComponent>().OnSisterCleansed();        
                }
                else
                {
                    droppedItem.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
                    RaycastHit rayhited;
                    Ray rayed = new Ray(player.go.transform.position + player.go.transform.forward, Vector3.down);
                    if (Physics.Raycast(rayed, out rayhited, 1000, player.interactorComponent.groundMask))
                    {
                        droppedItem.transform.position = player.go.transform.position;
                        droppedItem.transform.position = new Vector3(newPosition.x, rayhited.point.y, newPosition.z);

                    }
                }
            }
        }
    }
    private void DropItem(int index)
    {
        if (player.inventoryComponent.InventoryBag[index].itemType.baseCollectible != player.inventoryComponent.tempColl.itemType.baseCollectible)
        {
            for (int i = 0; i < GetComponent<GatheringSystem>().pickedUpItem.Count; i++)
            {
                if (GetComponent<GatheringSystem>().pickedUpItem[i].baseCollectible == player.inventoryComponent.InventoryBag[index].itemType.baseCollectible)
                {
                    if (GetComponent<GatheringSystem>().pickedUpItem[i].GetComponent<CombatPotion>())
                    {
                        if (!GetComponent<GatheringSystem>().pickedUpItem[i].GetComponent<CombatPotion>().isCurrentlyActivated)
                        {
                            SetNewPositionandActivateObjectatDrop(player.go.transform.position + player.go.transform.forward, GetComponent<GatheringSystem>().pickedUpItem[i].gameObject);//player.inventoryComponent.InventoryBag[index].itemType.gameObject);

                            if (!GetComponent<GatheringSystem>().pickedUpItem[i].GetComponent<CombatPotion>())
                            {
                                GetComponent<GatheringSystem>().pickedUpItem.RemoveAt(i);
                            }
                            player.inventoryComponent.invUI.RemoveFromInventoryUI(player.inventoryComponent.InventoryBag[index].itemType);
                            player.inventoryComponent.RemoveItemFromInventory(player.inventoryComponent.InventoryBag[index].itemType, 1);
                            player.animationComponent.OnPlayThrowAnimation();
                            break;
                        }
                    }
                    else
                    {
                        SetNewPositionandActivateObjectatDrop(player.go.transform.position + player.go.transform.forward, GetComponent<GatheringSystem>().pickedUpItem[i].gameObject);//player.inventoryComponent.InventoryBag[index].itemType.gameObject);

                        if (!GetComponent<GatheringSystem>().pickedUpItem[i].GetComponent<CombatPotion>())
                        {
                            GetComponent<GatheringSystem>().pickedUpItem.RemoveAt(i);
                        }
                        player.inventoryComponent.invUI.RemoveFromInventoryUI(player.inventoryComponent.InventoryBag[index].itemType);
                        player.inventoryComponent.RemoveItemFromInventory(player.inventoryComponent.InventoryBag[index].itemType, 1);
                        break;

                    }
                }

            }
        }
        player.uiComponent.groceryList.UpdateList();
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
                /* bool tempTest = false;
                 for (int y = 0; y < player.inventoryComponent.InventoryBag.Count; y++)
                 {
                     if (player.inventoryComponent.InventoryBag[y] != player.inventoryComponent.tempColl)
                     { 
                         tempTest = true;
                         break;
                     }
                 }

                 if(tempTest)
                     player.inventoryComponent.TransferItemsFromTo(player.inventoryComponent, tc.gameObject.GetComponent<InventoryComponent>(), true);*/
                break;
            }
        }
    }


    //public bool AddtoInventroy(CollectibleComponent item, InventoryComponent componentThatHoldsTheInventory, int resourceAmount)
    //{
    //    if (componentThatHoldsTheInventory.InventoryBag.Count <= componentThatHoldsTheInventory.MaxInventorySpace)
    //    {
    //        for (int i = 0; i < componentThatHoldsTheInventory.InventoryBag.Count; i++)
    //        {
    //            if (componentThatHoldsTheInventory.InventoryBag[i].itemType == item)
    //            {
    //                componentThatHoldsTheInventory.InventoryBag[i].AdjustAmount(resourceAmount);
    //                if (componentThatHoldsTheInventory.invUI)
    //                    componentThatHoldsTheInventory.invUI.UpdateInventory(item);
    //                return true;
    //            }
    //        }
    //        InventoryComponent.itemClass tempitem = new InventoryComponent.itemClass();
    //        //tempitem.item = item;
    //        tempitem.SetAmount(resourceAmount);
    //        if (componentThatHoldsTheInventory.invUI)
    //            componentThatHoldsTheInventory.invUI.UpdateInventory(item);
    //        tempitem.itemType = item;
    //        componentThatHoldsTheInventory.InventoryBag.Add(tempitem);
    //        return true;
    //    }
    //    else
    //        return false;


    //}
    //public void TransferItemsFromTo(InventoryComponent From, InventoryComponent To, bool DestroyOnTransfer = false)
    //{
    //    for (int i = 0; i <= From.InventoryBag.Count; i++)
    //    {
    //        if (i == From.InventoryBag.Count)
    //        {
    //            From.InventoryBag.Clear();
    //            break;
    //        }
    //        From.invUI.ResetInventory();
    //        AddtoInventroy(From.InventoryBag[i].itemType, To, From.InventoryBag[i].amountOfItem);
    //    }
    //}
    //public bool AddtoInventroy(BaseResource item, InventoryComponent componentThatHoldsTheInventory)
    //{
    //    if (componentThatHoldsTheInventory.InventoryBag.Count <= componentThatHoldsTheInventory.MaxInventorySpace)
    //    {
    //        for (int i = 0; i < componentThatHoldsTheInventory.InventoryBag.Count; i++)
    //        {
    //            if (componentThatHoldsTheInventory.InventoryBag[i].itemType == item)
    //            {
    //                componentThatHoldsTheInventory.InventoryBag[i].AdjustAmount(item.Quantity);
    //                return true;
    //            }
    //        }
    //        InventoryComponent.itemClass tempitem = new InventoryComponent.itemClass();
    //        //tempitem.item = item;
    //        tempitem.SetAmount(item.Quantity);
    //        if (componentThatHoldsTheInventory.invUI)
    //            componentThatHoldsTheInventory.invUI.UpdateInventory(item);
    //        tempitem.itemType = item;
    //        componentThatHoldsTheInventory.InventoryBag.Add(tempitem);
    //        return true;
    //    }
    //    else
    //        return false;


    //}
    //public void TransferItemsFromTo(List<InventoryComponent.itemClass> From, InventoryComponent To)
    //{
    //    for (int i = 0; i < From.Count; i++)
    //    {
    //        AddtoInventroy(From[i].itemType, To);
    //    }
    //}


}

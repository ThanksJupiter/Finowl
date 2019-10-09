using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryComponent : BaseComponent
{
    [System.Serializable]
    public class itemClass
    {
        //public Storable item;
        public int amountOfItem;
        public CollectibleComponent itemType;
        public void SetAmount(int Amount)
        {
            amountOfItem = Amount;
        }
        public void AdjustAmount(int Amount)
        {
            amountOfItem += Amount;
        }
    }

    public List<itemClass> InventoryBag = new List<itemClass>();
    public int MaxInventorySpace = 5;
    public UseInventorySystem invUI;

    ///////////////////////////////////////////////////////InventoryDropFix
    // private int currentSpace = 0;
    public itemClass tempColl;
    // public BaseResource tempRes;
    //////////////////////////////////////////////////////////////////////////
    private void Awake()
    {

        // tempColl.itemType = gameObject.AddComponent<CollectibleComponent>();
        //tempColl.itemType.baseCollectible = tempRes;
        for (int i = 0; i < MaxInventorySpace; i++)
        {
            InventoryBag.Add(tempColl);
        }
    }

    public void OutsideAwake(itemClass tmpCol)
    {
        for (int i = 0; i < MaxInventorySpace; i++)
        {
            InventoryBag.Add(tmpCol);
        }
    }

    public bool DoesItemExist(BaseResource itemToSearchFor)
    {
        for (int i = 0; i < InventoryBag.Count; i++)
        {
            //if (InventoryBag[i] != null)
            //{
            if (InventoryBag[i].itemType.baseCollectible.GetName() == itemToSearchFor.GetName())
            {
                return true;
            }
            //     }
        }
        return false;
    }
    public bool DoesThisManyItemExist(BaseResource itemToSearchFor, int AmountThatExist)
    {
        for (int i = 0; i < InventoryBag.Count; i++)
        {
            //if (InventoryBag[i] != null)
            //{
            if (InventoryBag[i] == tempColl)
                continue;
            if (InventoryBag[i].itemType.baseCollectible == itemToSearchFor && InventoryBag[i].amountOfItem >= AmountThatExist)
            {
                return true;
            }
            //   }

        }
        return false;
    }
    public bool AddtoInventroy(CollectibleComponent item, InventoryComponent componentThatHoldsTheInventory, int resourceAmount)
    {
        //if (componentThatHoldsTheInventory.InventoryBag.Count <= componentThatHoldsTheInventory.MaxInventorySpace)
        for (int i = 0; i < componentThatHoldsTheInventory.InventoryBag.Count; i++)
        {
            // componentThatHoldsTheInventory.InventoryBag[i].itemType = item;

            if (componentThatHoldsTheInventory.InventoryBag[i].itemType.baseCollectible == item.baseCollectible)
            {
                if (componentThatHoldsTheInventory.invUI)
                    componentThatHoldsTheInventory.invUI.UpdateInventory(item);
                componentThatHoldsTheInventory.InventoryBag[i].AdjustAmount(resourceAmount);
                return true;
            }
        }
        for (int i = 0; i < componentThatHoldsTheInventory.InventoryBag.Count; i++)
        {
            if (componentThatHoldsTheInventory.InventoryBag[i] == componentThatHoldsTheInventory.tempColl)
            {

                //for (int y = 0; y < componentThatHoldsTheInventory.InventoryBag.Count; y++)
                //{
                //    //if (componentThatHoldsTheInventory.InventoryBag[i] != null)
                //    //{
                //    if (componentThatHoldsTheInventory.InventoryBag[y].itemType == item)
                //    {
                //        if (componentThatHoldsTheInventory.invUI)
                //            componentThatHoldsTheInventory.invUI.UpdateInventory(item);
                //        componentThatHoldsTheInventory.InventoryBag[y].AdjustAmount(resourceAmount);
                //        return true;
                //    }
                //    // }
                //}
                InventoryComponent.itemClass tempitem = new InventoryComponent.itemClass();
                //tempitem.item = item;
                tempitem.SetAmount(resourceAmount);
                if (componentThatHoldsTheInventory.invUI)
                    componentThatHoldsTheInventory.invUI.UpdateInventory(item);
                tempitem.itemType = item;
                componentThatHoldsTheInventory.InventoryBag[i] = tempitem;
                return true;
            }

        }
        return false;


    }
    public void TransferItemsFromTo(InventoryComponent From, InventoryComponent To, bool DestroyOnTransfer = false)
    {
        for (int i = 0; i <= From.InventoryBag.Count; i++)
        {
            if (i == From.InventoryBag.Count)
            {
                //From.InventoryBag.Clear();
                for (int y = 0; y < From.InventoryBag.Count; y++)
                {

                    if (From.InventoryBag[y].itemType.TypeofCollectible == CollectibleComponent.CollectibleType.INGREDIENT || From.InventoryBag[y].itemType.TypeofCollectible == CollectibleComponent.CollectibleType.LETTER)
                    {
                        From.InventoryBag[y] = From.tempColl;
                    }
                }
                //From.invUI.ResetInventory();
                break;
            }
            if (From.InventoryBag[i] != From.tempColl && From.InventoryBag[i].itemType.TypeofCollectible != CollectibleComponent.CollectibleType.BATTLEPOTION)
            {

                AddtoInventroy(From.InventoryBag[i].itemType, To, From.InventoryBag[i].amountOfItem);
                for (int y = 0; y <= From.InventoryBag[i].amountOfItem; y++)
                {
                    From.invUI.RemoveFromInventoryUI(From.InventoryBag[i].itemType);
                }
            }

        }
    }
    public itemClass GetItemFromInventory(BaseResource itemToGet, int AmountToGet = 0, bool DestroyItems = false)
    {
        for (int i = 0; i < InventoryBag.Count; i++)
        {
            //if (InventoryBag[i] != null)
            //{
            if (InventoryBag[i].itemType.baseCollectible.GetName() == itemToGet.GetName())
            {

                if (DestroyItems == true && InventoryBag[i].amountOfItem >= AmountToGet)
                {
                    itemClass tempItem = InventoryBag[i];
                    if (InventoryBag[i].amountOfItem > AmountToGet)
                    {
                        InventoryBag[i].AdjustAmount(-AmountToGet);
                    }
                    else if (InventoryBag[i].amountOfItem == AmountToGet)
                    {
                        InventoryBag[i].SetAmount(0);
                        InventoryBag[i] = tempColl;
                    }

                    return tempItem;
                }
                else if (DestroyItems == false && AmountToGet > InventoryBag[i].amountOfItem)
                {
                    break;
                }
                else
                {
                    return InventoryBag[i];
                }
            }
            // }

        }

        itemClass empty = new itemClass();
        return empty;
    }
    public bool RemoveItemFromInventory(CollectibleComponent item, int HowManyToRemove)
    {
        if (DoesThisManyItemExist(item.baseCollectible, HowManyToRemove))
        {
            for (int i = 0; i < InventoryBag.Count; i++)
            {
                //if (InventoryBag[i] != null)
                //{
                if (InventoryBag[i].itemType.baseCollectible == item.baseCollectible)
                {
                    if (InventoryBag[i].amountOfItem == HowManyToRemove)
                    {
                        InventoryBag[i].AdjustAmount(-HowManyToRemove);
                        InventoryBag[i] = tempColl;
                    }
                    else if (InventoryBag[i].amountOfItem > HowManyToRemove)
                        InventoryBag[i].AdjustAmount(-HowManyToRemove);

                    break;
                }
                // }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void EmptyInventory()
    {
        for (int i = 0; i < InventoryBag.Count; i++)
        {
            if (InventoryBag[i] != tempColl && InventoryBag[i].itemType.TypeofCollectible != CollectibleComponent.CollectibleType.BATTLEPOTION)
            {
                InventoryBag[i].SetAmount(0);
                InventoryBag[i] = tempColl;
                if (invUI)
                {
                    invUI.RemoveFromInventoryUI(InventoryBag[i].itemType);
                }
            }
        }
    }
    public List<itemClass> GetInventory()
    {
        return InventoryBag;
    }
    /* public bool AddtoInventroy(CollectibleComponent item, InventoryComponent componentThatHoldsTheInventory, int resourceAmount)
     {
         //if (componentThatHoldsTheInventory.InventoryBag.Count <= componentThatHoldsTheInventory.MaxInventorySpace)
         Debug.Log(componentThatHoldsTheInventory.InventoryBag.Count);
         for (int i = 0; i < componentThatHoldsTheInventory.InventoryBag.Count; i++)
         {
            // componentThatHoldsTheInventory.InventoryBag[i].itemType = item;
             Debug.Log(componentThatHoldsTheInventory.InventoryBag[i].itemType + " <-> " + item);
             if(componentThatHoldsTheInventory.InventoryBag[i].itemType == item)
             {
                 Debug.Log("ASDASDASDASDASD");
                 if (componentThatHoldsTheInventory.invUI)
                     componentThatHoldsTheInventory.invUI.UpdateInventory(item);
                 componentThatHoldsTheInventory.InventoryBag[i].AdjustAmount(resourceAmount);
                 return true;
             }
         }
         for (int i = 0; i < componentThatHoldsTheInventory.InventoryBag.Count; i++)
         {
             if (InventoryBag[i] == tempColl)
             {


                 //for (int y = 0; y < componentThatHoldsTheInventory.InventoryBag.Count; y++)
                 //{
                 //    //if (componentThatHoldsTheInventory.InventoryBag[i] != null)
                 //    //{
                 //    if (componentThatHoldsTheInventory.InventoryBag[y].itemType == item)
                 //    {
                 //        if (componentThatHoldsTheInventory.invUI)
                 //            componentThatHoldsTheInventory.invUI.UpdateInventory(item);
                 //        componentThatHoldsTheInventory.InventoryBag[y].AdjustAmount(resourceAmount);
                 //        return true;
                 //    }
                 //    // }
                 //}
                 InventoryComponent.itemClass tempitem = new InventoryComponent.itemClass();
                 //tempitem.item = item;
                 tempitem.SetAmount(resourceAmount);
                 if (componentThatHoldsTheInventory.invUI)
                     componentThatHoldsTheInventory.invUI.UpdateInventory(item);
                 tempitem.itemType = item;
                 componentThatHoldsTheInventory.InventoryBag[i] = tempitem;
                 return true;
             }

         }
         return false;


     }
     public void TransferItemsFromTo(InventoryComponent From, InventoryComponent To, bool DestroyOnTransfer = false)
     {
         for (int i = 0; i <= From.InventoryBag.Count; i++)
         {
             Debug.Log(i);
             if (From.InventoryBag[i] != tempColl)
             {
                 Debug.Log("addingtoCartinv " + i);

                 AddtoInventroy(From.InventoryBag[i].itemType, To, From.InventoryBag[i].amountOfItem);
             }
             if (i == From.InventoryBag.Count)
             {
                 Debug.Log("I have reached the end " + i);

                 //From.InventoryBag.Clear();
                 for (int y = 0; y < From.InventoryBag.Count; y++)
                 {
                     From.InventoryBag[i] = tempColl;
                 }
                 From.invUI.ResetInventory();
                 break;
             }

         }
     }*/

}

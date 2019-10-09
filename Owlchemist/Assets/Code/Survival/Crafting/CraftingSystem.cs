using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : BaseSystem
{
    [HideInInspector]
    public Filter[] filters;
    public Button CraftThis;
    CartComponent cartComponent;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            InventoryComponent ic
            )
        {
            this.id = id;

            gameObject = go;
            inventoryComponent = ic;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public InventoryComponent inventoryComponent;
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

            if (ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic));
            }
        }

        filters = tmpFilters.ToArray();
        cartComponent = GetComponentInChildren<CartComponent>();
    }

    public bool craftPotion1 { get; set; }
    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            InventoryComponent inventoryComp = filter.inventoryComponent;


            // ----- logic -----

            //if (craftPotion1)
            //{
            //    if (inventoryComp.treeResources > 1)
            //    {
            //        Debug.Log("Hello");
            //    }
            //}
            /*if(Input.GetButtonDown("Fire4"))
            {
                Debug.Log("lädderlappen");
            }
            if (Input.GetButtonUp("Fire4"))
            {
                Debug.Log("lädderlappen");
            }*/
        }
    }

    [System.Serializable]
    public struct Ingredient
    {
        public BaseResource item;
        public int amount;
    }

    [System.Serializable]
    public class ItemRecipe
    {
        public Ingredient[] ingredients;
        public CollectibleComponent finalItem;

        public string Name() { return finalItem.baseCollectible.name; }
    }

    public List<ItemRecipe> recipeList = new List<ItemRecipe>();
    //private List<ItemRecipe> allRecipes = new List<ItemRecipe>();

    public List<ItemRecipe> GetRecipes()
    {
        return recipeList;
    }


    void CreateItem(CollectibleComponent finalItem)
    {
        CollectibleComponent newItem;
        newItem = finalItem;
        //Do thing with item, add to inventory? 
        //cartComponent.GetComponent<InventoryComponent>().AddtoInventroy(newItem,cartComponent.GetComponent<InventoryComponent>(), newItem.Quantity);
        if (finalItem.TypeofCollectible == CollectibleComponent.CollectibleType.BATTLEPOTION)
        {
            player.inventoryComponent.AddtoInventroy(newItem, player.inventoryComponent, newItem.Quantity);
        }
        else
        {
            cartComponent.GetComponent<InventoryComponent>().AddtoInventroy(newItem, cartComponent.GetComponent<InventoryComponent>(), newItem.Quantity);
        }
        //return newItem;
    }
    //BaseResource CreateItem(BaseResource finalItem)
    //{
    //    BaseResource newItem = finalItem;
    //    //Do thing with item, add to inventory? 
    //    cartComponent.GetComponent<InventoryComponent>().AddtoInventroy(newItem, cartComponent.GetComponent<InventoryComponent>());
    //    return newItem;
    //}

    public bool CanCraft(ItemRecipe currentRecipe)
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            if (recipeList[i].finalItem == currentRecipe.finalItem)
            {
                //return CraftFromRecipe(allRecipes[i]);  
                return true;
            }
        }
        return false;
    }
    public bool CraftGotMaterials(int currentRecipeIndex)
    {
        int TempCheck = 0;
        InventoryComponent pInventory = cartComponent.GetComponent<InventoryComponent>();

        List<CollectibleComponent> itemsInInventory = new List<CollectibleComponent>();

        for (int i = 0; i < recipeList[currentRecipeIndex].ingredients.Length; i++)
        {
            if (pInventory.DoesItemExist(recipeList[currentRecipeIndex].ingredients[i].item))
            {
                int inventoryAmount = pInventory.GetItemFromInventory(recipeList[currentRecipeIndex].ingredients[i].item).amountOfItem;

                if (recipeList[currentRecipeIndex].ingredients[i].amount <= inventoryAmount)
                {
                    TempCheck++;
                }

            }
        }

        if (TempCheck >= recipeList[currentRecipeIndex].ingredients.Length)
        {
            return true;
        }
        else
        {
            return false;
        }

        }
    public void CraftFromRecipe(int currentRecipeIndex)
    {
        int TempCheck = 0;
        InventoryComponent pInventory = cartComponent.GetComponent<InventoryComponent>();

        List<CollectibleComponent> itemsInInventory = new List<CollectibleComponent>();

        for (int i = 0; i < recipeList[currentRecipeIndex].ingredients.Length; i++)
        {
            if (pInventory.DoesItemExist(recipeList[currentRecipeIndex].ingredients[i].item))
            {
                int inventoryAmount = pInventory.GetItemFromInventory(recipeList[currentRecipeIndex].ingredients[i].item).amountOfItem;

                if (recipeList[currentRecipeIndex].ingredients[i].amount <= inventoryAmount)
                {
                    TempCheck++;
                }

            }
        }

        if (TempCheck >= recipeList[currentRecipeIndex].ingredients.Length)
        {
            for (int i = 0; i < recipeList[currentRecipeIndex].ingredients.Length; i++)
            {
                pInventory.GetItemFromInventory(recipeList[currentRecipeIndex].ingredients[i].item, recipeList[currentRecipeIndex].ingredients[i].amount, true);
            }
            CreateItem(recipeList[currentRecipeIndex].finalItem);
        }


    }
    //public BaseResource CraftFromRecipe(ItemRecipe currentRecipe)
    //{
    //    int TempCheck = 0;
    //    InventoryComponent pInventory = cartComponent.GetComponent<InventoryComponent>();

    //    List<BaseResource> itemsInInventory = new List<BaseResource>();

    //    for (int i = 0; i < currentRecipe.ingredients.Length; i++)
    //    {
    //        if (pInventory.DoesItemExist(currentRecipe.ingredients[i].item))
    //        {
    //            int inventoryAmount = pInventory.GetItemFromInventory(currentRecipe.ingredients[i].item).amountOfItem;

    //            if (currentRecipe.ingredients[i].amount <= inventoryAmount)
    //            {
    //                TempCheck++;
    //            }

    //        }
    //    }

    //    if (TempCheck >= currentRecipe.ingredients.Length)
    //    {
    //        for (int i = 0; i < currentRecipe.ingredients.Length; i++)
    //        {
    //            pInventory.GetItemFromInventory(currentRecipe.ingredients[i].item, currentRecipe.ingredients[i].amount, true);
    //        }
    //        return CreateItem(currentRecipe.finalItem);
    //    }
    //    else
    //    {
    //        return null;
    //    }

    //}
}

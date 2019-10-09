using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftPotion : MonoBehaviour
{
    public GameObject insidecartRef;
    public GameObject crafStation;
    public Canvas list;
    bool onList = false;
    public Text[] howmannyIndicators;
    public CollectibleComponent[] ingrediances;
    public Text yIndicatorOn;
    public Text yIndicatorOff;
    public Text pageTxT;
    public Text pageAmountTxT;
    public Image craftAnimationRef;
    public int myPotionNumb;
    List<int> tempNum = new List<int> { };
    bool doOnce = true;
    //public GameObject insideCartRef;
    //[Header("Potion")]
    //public BaseResource resource;
    //public int howmany;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {        
        if (Input.GetButtonDown("Fire4"))
        {
            tempNum.Clear();
            for (int i = 0; i < insidecartRef.GetComponent<CartInteraction>().worldRef.GetComponent<CraftingSystem>().recipeList[myPotionNumb].ingredients.Length; i++)
            {
                tempNum.Add(insidecartRef.GetComponent<CartInteraction>().worldRef.GetComponent<CraftingSystem>().recipeList[myPotionNumb].ingredients[i].amount);

            }
            if (onList)
            {               
                list.GetComponent<GroceryList>().RemoveFromList(ingrediances, tempNum.ToArray());
                onList = false;
                yIndicatorOn.gameObject.SetActive(false);
                yIndicatorOff.gameObject.SetActive(true);
            }
            else
            {
                list.GetComponent<GroceryList>().AddToList(ingrediances, tempNum.ToArray());
                onList = true;
                yIndicatorOn.gameObject.SetActive(true);
                yIndicatorOff.gameObject.SetActive(false);
            }
        }
        if (craftAnimationRef.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !craftAnimationRef.GetComponent<Animator>().IsInTransition(0))
        {
            craftAnimationRef.gameObject.SetActive(false);
        }
        UpdatePageNumber();
        UpdateUi();

    }
    public void UpdateUi()
    {
        
        for (int i = 0; i < howmannyIndicators.Length; i++)
        {
            //ingrediances[i]
            for (int y = 0; y < insidecartRef.GetComponent<CartInteraction>().cart.GetComponent<InventoryComponent>().InventoryBag.Count; y++)
            {
                if (ingrediances[i].baseCollectible == insidecartRef.GetComponent<CartInteraction>().cart.GetComponent<InventoryComponent>().InventoryBag[y].itemType.baseCollectible)
                {
                    howmannyIndicators[i].text = insidecartRef.GetComponent<CartInteraction>().cart.GetComponent<InventoryComponent>().InventoryBag[y].amountOfItem.ToString();
                    break;
                }
                else
                {
                    howmannyIndicators[i].text = "0";
                }
            }
        }
    }
    public void UpdatePageNumber()
    {
        pageTxT.text = (crafStation.GetComponent<UICartCanvas>().arrayTarget + 1).ToString();
        pageAmountTxT.text = ("/" + (crafStation.GetComponent<UICartCanvas>().menuButtonsListRefs.Count).ToString());
    }
    public void CraftNewPotion()
    {       
        //craftAnimationRef.GetComponent<Animator>().Play("CraftAnimation", 0);
        if (insidecartRef.GetComponent<CartInteraction>().worldRef.GetComponent<CraftingSystem>().CraftGotMaterials(myPotionNumb))
        {
            insidecartRef.GetComponent<CartInteraction>().worldRef.GetComponent<CraftingSystem>().CraftFromRecipe(myPotionNumb);
            craftAnimationRef.gameObject.SetActive(false);
            craftAnimationRef.gameObject.SetActive(true);
            insidecartRef.GetComponent<CartInteraction>().speaker.Craftsound();
            if (doOnce)
            {
                if (myPotionNumb == 0)
                {
                    if (list.GetComponent<GroceryList>().missonState < 3)
                    {
                        list.GetComponent<GroceryList>().NewMainQuest(3);
                    }
                }
                else if (myPotionNumb == 1)
                {
                    if (list.GetComponent<GroceryList>().missonState <6)
                    {
                        list.GetComponent<GroceryList>().NewMainQuest(6);
                    }
                }
                doOnce = false;
            }
        }
        else
        {
            // Play a can't craft sound
        }
    }
    //public void craft()
    //{
    //insideCartRef.GetComponent<CartInteraction>().CraftPotion(resource, howmany);
    //}
}

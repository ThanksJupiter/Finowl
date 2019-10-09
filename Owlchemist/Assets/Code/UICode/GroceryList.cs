using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GroceryList : MonoBehaviour
{
    public GameObject player;
    public GameObject cartRef;
    public Text[] list;
    public Image[] icons;
    public bool isButtonDown;
    public Text mainMissionTxT;
    public Sprite noIngerdiantIcon;
    public string[] mainMisionText;
    public int missonState = 0;
    public float fadeDelay = 1.2f;
    public float fadeTime = 0;
    public ingredientAmount[] allIngredienses;
    public Sprite[] allIngrediensesImages;
    public List<ingredientAmount> allIngrediensessOnList = new List<ingredientAmount>();
    public List<Sprite> thierimages = new List<Sprite>();


    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<CanvasGroup>().alpha = 0;
        UpdateList();
        NewMainQuest(0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isButtonDown)
        {
            fadeTime = 0;
            GetComponent<CanvasGroup>().alpha = 1;
        }
        else if (fadeTime == fadeDelay)
        {
            GetComponent<CanvasGroup>().alpha = Mathf.Clamp(GetComponent<CanvasGroup>().alpha - Time.deltaTime, 0, 1);
        }
        else
        {
            fadeTime = Mathf.Clamp(fadeTime + Time.deltaTime, 0, fadeDelay);
        }
        */
    }
    public void NewMainQuest(int newQuestNumber)
    {
        if(newQuestNumber < 0)
        {
            missonState++;
        }
        else
        {
            missonState = newQuestNumber;
        }

        mainMissionTxT.text = mainMisionText[missonState];
    }
    public void UpdateList()
    {
        allIngrediensessOnList.Clear();
        thierimages.Clear();
        for (int i = 0; i < allIngredienses.Length; i++)
        {
            int tempInt = 0;
            int tempInt2 = 0;
            //CartInvetory
            for (int y = 0; y < cartRef.GetComponent<InventoryComponent>().InventoryBag.Count; y++)
            {
                if (allIngredienses[i].ingredient.baseCollectible == cartRef.GetComponent<InventoryComponent>().InventoryBag[y].itemType.baseCollectible)
                {
                    tempInt = cartRef.GetComponent<InventoryComponent>().InventoryBag[y].amountOfItem;
                    break;   
                }
            }
            //playerInvetory
            for (int y = 0; y < player.GetComponent<InventoryComponent>().InventoryBag.Count; y++)
            {
                if (allIngredienses[i].ingredient.baseCollectible == player.GetComponent<InventoryComponent>().InventoryBag[y].itemType.baseCollectible)
                {
                    tempInt2 = player.GetComponent<InventoryComponent>().InventoryBag[y].amountOfItem;
                    break;
                }
            }
            if (Mathf.Clamp((allIngredienses[i].howMany - (tempInt + tempInt2)), 0,100000) > 0)
            {
                Debug.Log("cart amount " + tempInt + " : PlayerAmount " + tempInt2 + " : Whants " + allIngredienses[i].howMany);
                allIngrediensessOnList.Add(new ingredientAmount());
                allIngrediensessOnList[allIngrediensessOnList.Count - 1].ingredient = allIngredienses[i].ingredient;
                allIngrediensessOnList[allIngrediensessOnList.Count - 1].howMany = allIngredienses[i].howMany - (tempInt + tempInt2);
                thierimages.Add(allIngrediensesImages[i]);
            }
        }

        for (int i = 0; i < list.Length; i++)
        {
            if (allIngrediensessOnList.Count == 0)
            {
                list[i].text = "";
                icons[i].sprite = noIngerdiantIcon;
            }
            else if (allIngrediensessOnList.Count - 1 >= i)
            {
                list[i].text =  allIngrediensessOnList[i].howMany + " " + allIngrediensessOnList[i].ingredient.baseCollectible.GetName();
                icons[i].sprite = thierimages[i];
            }
            else
            {
                list[i].text = "";
                icons[i].sprite = noIngerdiantIcon;

            }
        }
        
    }
    public void RemoveFromList(CollectibleComponent[] ing, int[] howMany)
    {
        for (int i = 0; i < ing.Length; i++)
        {
            for (int y = 0; y < allIngredienses.Length; y++)
            {
                if (allIngredienses[y].ingredient.baseCollectible == ing[i].baseCollectible)
                {
                    allIngredienses[y].howMany -= howMany[i];
                    break;
                }
            }
        }
        
    }
    public void AddToList(CollectibleComponent[] ing, int[] howMany)
    {
        for (int i = 0; i < ing.Length; i++)
        {
            for (int y = 0; y < allIngredienses.Length; y++)
            {
                if (allIngredienses[y].ingredient.baseCollectible == ing[i].baseCollectible)
                {
                    allIngredienses[y].howMany += howMany[i];
                    break;
                }
            }
        }
        
        
    }
    [System.Serializable]
    public class ingredientAmount
    {
        public CollectibleComponent ingredient;
        public int howMany;
    }
}
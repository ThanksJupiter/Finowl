using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CartInteraction : MonoBehaviour
{
    CollectibleComponent[] worldPickups;
    public GameObject player;
    public GameObject[] interactives;
    public GameObject cart;
    public GameObject cartExitPoint;
    public GameObject worldRef;
    public GameObject glist;

    public InsideCartAudio speaker;

    public int target = -1;
    public GameObject targetTextCanvas;
    public Text targetText;
    public GameObject vCam;
    CinemachineVirtualCamera cameraRef;
    float lastiput;
    float pressdelay = 0.15f;
    public int dayCount = 1;
    public GameObject uiHolder;
    public CanvasGroupFader[] canvasGroupFaders;
    public List <GameObject> newRewards;
    public GameObject postProcessObject;
    bool everFirstEnter = true;
    //[Header("Friday Review Fix")]
    //public Text mushroomTXT;
    //public Text flowerTXT;

    // Start is called before the first frame update
    void Start()
    {
        cameraRef = vCam.GetComponent<CinemachineVirtualCamera>();
        UpdatePages();
        worldPickups = FindObjectsOfType<CollectibleComponent>();
        //player.GetComponent<GameManagerComponent>().OnDeath += PlayerDeath;
        for (int i = 0; i < interactives.Length; i++)
        {
            if (interactives[i].GetComponent<UICartCanvas>())
            {
                for (int y = 0; y < interactives[i].GetComponent<UICartCanvas>().menuButtonsList.Count; y++)
                {
                    if (interactives[i].GetComponent<UICartCanvas>().menuButtonsList[0].menu.GetComponent<TurnInPotion>())
                    {
                        interactives[i].GetComponent<UICartCanvas>().menuButtonsList[y].menu.GetComponent<TurnInPotion>().UpdatePageNumber();
                    }
                    else if (interactives[i].GetComponent<UICartCanvas>().menuButtonsList[0].menu.GetComponent<CraftPotion>())
                    {
                        interactives[i].GetComponent<UICartCanvas>().menuButtonsList[y].menu.GetComponent<CraftPotion>().UpdatePageNumber();
                    }
                    else
                    {
                        Debug.Log("GotHere");
                    }
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraRef.Priority == 100)
        {
            if (target == - 1)
            {
                EnterdCart();
            }
            if(interactives[target].GetComponent<UITarget>().closed)
            {
                if ((Input.GetAxis("Horizontal") >= 0.9f || Input.GetAxis("Horizontal") <= -0.9f) && lastiput >= pressdelay)
                {
                    MoveInCart(Mathf.RoundToInt(Input.GetAxis("Horizontal")));
                    lastiput = 0;
                }
                lastiput = Mathf.Clamp(lastiput + Time.deltaTime, 0, pressdelay);
                if(Input.GetButtonDown("Fire1"))
                {
                    interactives[target].GetComponent<UITarget>().MyFuction();
                }
            }
            else
            {
                if(Input.GetButtonDown("Fire2"))
                {
                    interactives[target].GetComponent<UITarget>().CloseUI();
                }
            }
        }
    }

    public void EnterCart()
    {
        postProcessObject.SetActive(false);
        player.GetComponent<GameManagerComponent>()?.OnStopGameTick();
        cameraRef.Priority = 100;       
        targetTextCanvas.SetActive(true);
        targetText.gameObject.SetActive(true);
        //uiHolder.SetActive(false);
        for (int i = 0; i < canvasGroupFaders.Length; i++)
        {
            canvasGroupFaders[i].Hide(true);
        }
        speaker.Doorsound();
        player.GetComponent<GameManagerComponent>().isInsideCart = true;
    }

    void EnterdCart()
    {
        target = 0;
        interactives[target].GetComponent<UITarget>().Selected();
        UpdateQuests();
        PlayerTargetText();
        //Hard Coded Main Mission Proggres
        if (everFirstEnter)
        {
            if (glist.GetComponent<GroceryList>().missonState < 2)
            {
                glist.GetComponent<GroceryList>().NewMainQuest(2);
            }
            everFirstEnter = false;
        }

    }
    public void MoveInCart (int newTarget)
    {
        interactives[target].GetComponent<UITarget>().UnSelected();
        //De Select Last Interactive
        target += newTarget;
        if(target == interactives.Length)
        {
            target -= interactives.Length;
        }
        else if(target < 0)
        {
            target += interactives.Length;
        }
        interactives[target].GetComponent<UITarget>().Selected();
        PlayerTargetText();
    }
    public void LeaveCart()
    {
        postProcessObject.SetActive(true);
        interactives[target].GetComponent<UITarget>().UnSelected();
        GameManagerComponent gmc = player.GetComponent<GameManagerComponent>();
        gmc?.OnStartGameTick();
        gmc.isInsideCart = false;
        cameraRef.Priority = 9;
        target = -1;
        targetTextCanvas.SetActive(false);
        targetText.gameObject.SetActive(false);
        //uiHolder.SetActive(true);
        for (int i = 0; i < canvasGroupFaders.Length; i++)
        {
            canvasGroupFaders[i].Display(true);
        }
        player.GetComponent<HealthComponent>().isTutorialNoTakeDamage = false;
        player.transform.position = cartExitPoint.transform.position;
        player.GetComponent<MovementComponent>().agent.Warp(cartExitPoint.transform.position);
        player.transform.rotation = cart.transform.rotation;
        
        glist.GetComponent<GroceryList>().UpdateList();
        player.GetComponent<MovementComponent>().alive = true;
        speaker.Doorsound();
        player.GetComponent<GameManagerComponent>().isInsideCart = false;
    }
/*
    public void UpdateCurrentResourcesTexts()
    {
        GatheringComponent gc = player.GetComponent<GatheringComponent>();
        //mushroomTXT.text = .ToString();
        //flowerTXT.text = gc.flowerAmount.ToString();
        //cart.GetComponent<InventoryComponent>().InventoryBag.
    }
    */
    public bool CraftPotion(CollectibleComponent finalQuestPotion, int amount)
    {
        GatheringComponent gc = player.GetComponent<GatheringComponent>();
        for (int i = 0; i < cart.GetComponent<InventoryComponent>().InventoryBag.Count; i++)
        {
        if(cart.GetComponent<InventoryComponent>().InventoryBag[i].itemType == finalQuestPotion) //player = cart
            {
                //Do stuff with quest = complete?
                if(cart.GetComponent<InventoryComponent>().RemoveItemFromInventory(finalQuestPotion, amount))
                {
                    //you turned in the quest!
                    gc.hasPurificationPotion = true;//ful bool, obselete, enbart för testing
                }
                else
                {
                    //You did't manage to complete the quest!
                }
            }

        }
        return false;
    }
    void UsePotion(BaseResource potion, int amount)
    {

    }
    void PlayerTargetText()
    {
        switch (target)
        {
            case 0:
                 targetText.text = "EXIT";
                break;
            case 1:
                targetText.text = "CRAFTING";
                break;
            case 2:
                targetText.text = "SLEEP";
                break;
            case 3:
                targetText.text = "QUESTS";
                break;
            case 4:
                targetText.text = "TRAVEL";
                break;
        }
        
    }
    public void DayTranition()
    {
        dayCount++;
        UpdatePages();
        //Refill tourch
        //Respawn Outside
    }
    void UpdatePages()
    {
        for (int i = 0; i < interactives.Length; i++)
        {
             if (interactives[i].GetComponent<UICartCanvas>())
             {
                    interactives[i].GetComponent<UICartCanvas>().ManagePages();
             }
        }
    }
    public void NewDayReset()
    {
        if (newRewards.Count > 0)
        {
            for (int i = 0; i < newRewards.Count; i++)
            {
                newRewards[i].SetActive(true);
            }
        }
        newRewards.Clear();
        for (int i = 0; i < worldPickups.Length; i++)
        {
            if(worldPickups[i].TypeofCollectible == CollectibleComponent.CollectibleType.INGREDIENT)
            { 
            worldPickups[i].ResetTransform();
            }
        }
        //Used For death
        cameraRef.Priority = 100;
    }
    public void PlayerDeath()
    {
        postProcessObject.SetActive(false);
        player.GetComponent<GameManagerComponent>()?.OnStopGameTick();
        target = 2;
        targetTextCanvas.SetActive(true);
        targetText.gameObject.SetActive(true);
        //uiHolder.SetActive(false);
        for (int i = 0; i < canvasGroupFaders.Length; i++)
        {
            canvasGroupFaders[i].Hide(true);
        }
        UpdateQuests();/*TENSIOUS*/
        interactives[target].GetComponent<UITarget>().Selected();
        PlayerTargetText();
        interactives[target].GetComponent<UITarget>().MyFuction();

        InventoryComponent ic = player.GetComponent<InventoryComponent>();
        InventoryComponent newIC = gameObject.AddComponent<InventoryComponent>();
        newIC.InventoryBag = new List<InventoryComponent.itemClass>();
        newIC.OutsideAwake(ic.tempColl);
        ic.TransferItemsFromTo(ic, newIC, true);
        Destroy(newIC);
        //player.GetComponent<InventoryComponent>().EmptyInventory();

    }
    void UpdateQuests()
    {
        for (int i = 0; i < interactives.Length; i++)
        {
            if (interactives[i].GetComponent<UICartCanvas>())
            {
                for (int y = 0; y < interactives[i].GetComponent<UICartCanvas>().menuButtonsList.Count; y++)
                {
                    if (interactives[i].GetComponent<UICartCanvas>().menuButtonsList[0].menu.GetComponent<TurnInPotion>())
                    {
                        interactives[i].GetComponent<UICartCanvas>().menuButtonsList[y].menu.GetComponent<TurnInPotion>().MainCompleted();
                    }

                }
            }
        }
    }
}

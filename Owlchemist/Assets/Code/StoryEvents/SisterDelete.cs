using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SisterDelete : MonoBehaviour
{

    [SerializeField] GameObject sisterBody;
    [SerializeField] GameObject sisterGravestone;
    [SerializeField] PlayerEventComponent eventComponent;
    public CartInteraction insideCart;
    bool ifSisterDead = false;
    bool decaing = false;
    float dealy = 3f;
   private void Awake()
    {
        eventComponent.OnSisterCleansed += DeleteSisterBody;
    }
    public void DeleteSisterBody()
    {

        if (ifSisterDead)
        {
            insideCart.player.GetComponent<PlayerEventComponent>().StartEnd();
        }
        else
        {
            sisterBody.gameObject.SetActive(false);
            sisterGravestone.gameObject.SetActive(true);
            decaing = true;
            insideCart.interactives[insideCart.interactives.Length - 1].GetComponent<UICartCanvas>().menuButtonsList[0].menu.GetComponent<TurnInPotion>().canAppear = true;
            insideCart.player.GetComponent<GameManagerComponent>()?.OnStopGameTick();
            insideCart.interactives[2].GetComponent<UICartSleep>().nextBitInQuest = true;
        }
    }
    private void Update()
    {
        if(decaing)
        {
            if (dealy <= 0)
            {               
                decaing = false;
                insideCart.player.GetComponent<InventoryComponent>().TransferItemsFromTo(insideCart.player.GetComponent<InventoryComponent>(), insideCart.cart.GetComponent<InventoryComponent>());
                insideCart.PlayerDeath();
                ifSisterDead = true;
                //Hard Coded Main Mission Proggres
                if (insideCart.glist.GetComponent<GroceryList>().missonState < 5)
                {
                    Debug.Log("GotHere");
                    insideCart.glist.GetComponent<GroceryList>().NewMainQuest(5);
                }
            }
            else
            {
                dealy -= Time.deltaTime;
            }
        }
    }
}

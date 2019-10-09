using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnInPotion : MonoBehaviour
{
    public GameObject insideCartRef;
    public GameObject completeQuestMark;
    public GameObject revard;
    public GameObject questStation;
    public GameObject unlocktQuest;
    public CombatPotion mainCompletion;
    [Header("Potion")]
    public CollectibleComponent potion;
    public int howmany;
    [Header("Maneging")]
    public int apperDay;
    public int maxDays;
    public bool completed;//Whant Idecation of Complete (visual and audio)
    public bool canAppear = true;
    public bool justApperd = true;
    public Text pageTxT;
    public Text pageAmountTxT;
    public Text DaysLeftTxT;
    bool completedquestAnimating;
    bool fadeOut = true;
    float fadeTime = 0;
    float fadeTimeGoal = 2; 

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        UpdatePageNumber();

        //questStation.GetComponent<UICartCanvas>().menuButtonsListRefs.Count;
        if (completedquestAnimating)
        {
            float tempAlpha = GetComponent<CanvasGroup>().alpha;
            if (fadeOut)
            {
                if (tempAlpha == 0)
                {
                    fadeTime += Time.deltaTime;
                    revard.SetActive(true);
                    revard.GetComponent<RevardCode>().ApperAnimation();

                    if (fadeTime >= fadeTimeGoal)
                    {
                        fadeOut = false;
                        questStation.GetComponent<UICartCanvas>().NewPage();
                        insideCartRef.GetComponent<CartInteraction>().NewDayReset();
                    }
                }
                else
                {
                    tempAlpha = Mathf.Clamp(tempAlpha - Time.deltaTime, 0f, 1f);
                }
            }
            else
            {
                if (tempAlpha == 1)
                {
                    completedquestAnimating = false;
                    questStation.GetComponent<UICartCanvas>().animating = false;
                }
                else
                {
                    tempAlpha = Mathf.Clamp(tempAlpha + Time.deltaTime, 0f, 1f);
                }

            }
            GetComponent<CanvasGroup>().alpha = tempAlpha;
        }
    }
    public void UseInPotion()
    {
        if (!completed)
        {
            if (insideCartRef.GetComponent<CartInteraction>().player.GetComponent<InventoryComponent>().RemoveItemFromInventory(potion, howmany))
            {
                if(unlocktQuest)
                {
                    unlocktQuest.GetComponent<TurnInPotion>().canAppear = true;
                }           
                if (revard)
                {
                    questStation.GetComponent<UICartCanvas>().animating = true;
                    completedquestAnimating = true;
                    fadeOut = true;
                }
                else
                {
                    Debug.Log("No Revard");
                }
                canAppear = false;
                completed = true;
                if (completeQuestMark)
                {
                    completeQuestMark.SetActive(true);
                }
            }
            else
            {
                Debug.Log("You dont have " + potion.ToString());
            }
        }
    }
    public bool Appear()
    {
        if(insideCartRef.GetComponent<CartInteraction>().dayCount >= apperDay && canAppear && (insideCartRef.GetComponent<CartInteraction>().dayCount - apperDay <= maxDays || maxDays == 0))
        {
            if (justApperd)
            {
                //implement a simple opening letter animation first time
                questStation.GetComponent<UICartCanvas>().newQuest = true;
                justApperd = false;
            }
            return true;
            
        }
        else
        {
            return false;
        }
    }
    public void UpdatePageNumber()
    {
        pageTxT.text = (questStation.GetComponent<UICartCanvas>().arrayTarget + 1).ToString();
        pageAmountTxT.text = ("/" + (questStation.GetComponent<UICartCanvas>().menuButtonsListRefs.Count).ToString());
        if (maxDays != 0)
        {
            DaysLeftTxT.text = (maxDays - (insideCartRef.GetComponent<CartInteraction>().dayCount - apperDay) + " Days Left");
        }
        else
        {
            DaysLeftTxT.text = "Main Objective";
        }
        
    }
    public void MainCompleted()
    {
        if (mainCompletion)
        {
            if (mainCompletion.questComplete)
            {
                if (unlocktQuest)
                {
                    unlocktQuest.GetComponent<TurnInPotion>().canAppear = true;
                }
                /*EnythingUsefull?
                if (revard)
                {
                    questStation.GetComponent<UICartCanvas>().animating = true;
                    completedquestAnimating = true;
                    fadeOut = true;
                }
                else
                {
                    Debug.Log("No Revard");
                }
                */
                canAppear = false;
                completed = true;
                if (completeQuestMark)
                {
                    completeQuestMark.SetActive(true);
                }
            }
        }
    }
}

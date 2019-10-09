using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UICartCanvas : UITarget
{
    public GameObject insideCartRef;
    public List<MenuButtonCombination> menuButtonsList;
    public List<MenuButtonCombination> menuButtonsListRefs;
    float lastiput;
    float pressdelay = 0.15f;
    
    public GameObject newQuestIndicator;
    public bool newQuest;
    public bool animating;
    //public MenuButtonCombination[] menuButtons = new MenuButtonCombination[] { };
    //public MenuButtonCombination[] menuButtonsRefs;
    public int arrayTarget = 0;
    bool doOnce = true;
    //float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        menuButtonsListRefs = menuButtonsList;
    }

    // Update is called once per frame
    void Update()
    {
        if (!closed && !animating)
        {
 /*           if (Input.GetButtonDown("MenyHorizontalMovement") && menuButtonsListRefs[arrayTarget].menu.gameObject.activeSelf)
            {
                MoveScreen(Mathf.RoundToInt(Input.GetAxis("MenyHorizontalMovement")));
            }*/
            if ((Input.GetAxis("Horizontal") >= 0.9f || Input.GetAxis("Horizontal") <= -0.9f) && lastiput >= pressdelay)
            {
                MoveScreen(Mathf.RoundToInt(Input.GetAxis("Horizontal")));
                lastiput = 0;
                if (menuButtonsListRefs.Count > 1)
                {
                    insideCartRef.GetComponent<CartInteraction>().speaker.Pagesound();
                }
            }
            lastiput = Mathf.Clamp(lastiput + Time.deltaTime, 0, pressdelay);
        }
    }

    public override void MyFuction()
    {

        closed = false;
        arrayTarget = 0;
        if (menuButtonsListRefs.Count == 0)
        {
            CloseUI();
        }
        else
        {
            menuButtonsListRefs[arrayTarget].menu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(menuButtonsListRefs[arrayTarget].button);
        }
        if (newQuestIndicator)
        {
            newQuestIndicator.SetActive(false);
        }

        
    }
    public override void CloseUI()
    {
        if (!animating)
        {
            closed = true;
            if (menuButtonsListRefs.Count != 0)
            {
                menuButtonsListRefs[arrayTarget].menu.gameObject.SetActive(false);
            }
            if (menuButtonsListRefs[arrayTarget].menu.GetComponent<CraftPotion>())
            {
                menuButtonsListRefs[arrayTarget].menu.GetComponent<CraftPotion>().craftAnimationRef.gameObject.SetActive(false);
            }
            arrayTarget = 0;
            //Hard Coded Main Mission Proggres
            if (menuButtonsListRefs.Count == 2)
            {
                if (doOnce)
                {
                    if (menuButtonsListRefs[0].menu.GetComponent<TurnInPotion>())
                    {           
                        if (insideCartRef.GetComponent<CartInteraction>().glist.GetComponent<GroceryList>())
                        {
                            if (insideCartRef.GetComponent<CartInteraction>().glist.GetComponent<GroceryList>().missonState < 5)
                            {
                                insideCartRef.GetComponent<CartInteraction>().glist.GetComponent<GroceryList>().NewMainQuest(5);
                            }
                            doOnce = false;
                        }
                        else
                        {
                            Debug.Log("What the actual!!");
                        }
                    }
                }
            }
        }
    }
    public void MoveScreen(int way)
    {
        if (menuButtonsListRefs[arrayTarget].menu.GetComponent<CraftPotion>())
        {
            menuButtonsListRefs[arrayTarget].menu.GetComponent<CraftPotion>().craftAnimationRef.gameObject.SetActive(false);
        }
        menuButtonsListRefs[arrayTarget].menu.gameObject.SetActive(false);
        //De Select Last Interactive
        arrayTarget += way;
        if (arrayTarget == menuButtonsListRefs.Count)
        {
            arrayTarget -= menuButtonsListRefs.Count;
        }
        else if (arrayTarget < 0)
        {
            arrayTarget += menuButtonsListRefs.Count;
        }
        menuButtonsListRefs[arrayTarget].menu.gameObject.SetActive(true);
        if (menuButtonsListRefs[arrayTarget].button)
        {
            EventSystem.current.SetSelectedGameObject(menuButtonsListRefs[arrayTarget].button);
        }
        
    }
    [System.Serializable]
    public struct MenuButtonCombination
    {
        public Canvas menu;
        public GameObject button;
    }
    public void ManagePages()
    {
        menuButtonsListRefs = new List<MenuButtonCombination>();
        
        for (int i = 0; i < menuButtonsList.Count; i++)
        {
            if (menuButtonsList[i].menu.GetComponent<TurnInPotion>())
            {
                if (menuButtonsList[i].menu.GetComponent<TurnInPotion>().Appear())
                {
                    
                    menuButtonsListRefs.Add(menuButtonsList[i]);
                }               
            }
            else
            {
                menuButtonsListRefs.Add(menuButtonsList[i]);
            }
        }
    }
    public void NewPage()
    {
        if (newQuest)
        {
            newQuest = false;
            newQuestIndicator.SetActive(true);
        }

    }
}

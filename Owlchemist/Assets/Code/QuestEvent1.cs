using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvent1 : MonoBehaviour
{
    public GameObject letter;
    public GameObject gList;
    bool done = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Hard Coded Main Mission Proggres
        if (done)
        {
            if(letter.activeSelf == false)
            {
                if (gList.GetComponent<GroceryList>().missonState <= 1)
                {
                     gList.GetComponent<GroceryList>().NewMainQuest(1);
                }
                done = false;                
            }
        }
    }
}

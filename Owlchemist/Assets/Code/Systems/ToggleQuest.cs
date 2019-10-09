using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleQuest : BaseSystem
{
    public Filter[] filters;
    public GameObject tempUIfix;
    public Canvas questlist;
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
           InputComponent inc
            )
        {
            this.id = id;

            gameObject = go;
            inputComponent = inc;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public InputComponent inputComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here

        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            InputComponent inc = objects[i].GetComponent<InputComponent>();
            if (inc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, inc));
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];
            InputComponent inputComp = filter.inputComponent;

            // ----- logic -----
            if (inputComp.GetKeyDown(KeyCode.E) || inputComp.GetButtonDown("Fire1"))
            {
             
            }
            if(Input.GetButtonDown("Fire4"))
            {
                questlist.GetComponent<GroceryList>().isButtonDown = true;
            }
            if(Input.GetButtonUp("Fire4"))
            {
                questlist.GetComponent<GroceryList>().isButtonDown = false;
            }
        }

    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        inputComponent.OnYButtonDown += ToggleUIStuff;
        inputComponent.OnYButtonUp += ToggleUIStuffOff;
    }
    private void ToggleUIStuff() //Pressed Y, only do this once 
    {
        //if (Input.GetButtonDown("Fire4"))
        //{
        //    questlist.GetComponent<GroceryList>().isButtonDown = true;
        //}
    }
    private void ToggleUIStuffOff() //Pressed Y, only do this once 
    {
       // if (Input.GetButtonUp("Fire4"))
       // {
       //     questlist.GetComponent<GroceryList>().isButtonDown = false;
       // }
    }
}


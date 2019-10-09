using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CollectibleComponent : InteractableComponent
{
    public BaseResource baseCollectible;
    public int Quantity = 1;
    public InteractableComponent interactable { get; private set; }
    public CollectibleType TypeofCollectible = CollectibleType.INGREDIENT;
    [HideInInspector] public Transform startTransform;

    [Header("Prototype Extras")]
    public bool isGatherable = true;
    public enum CollectibleType
    {
        INGREDIENT,
        BATTLEPOTION,
        UTILITY,
        LETTER
    }
    private void Awake()
    {
        startTransform = this.transform;
    
    }
    public void ResetTransform()
    {
        if(gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
        }
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
        transform.localScale = startTransform.localScale;
    }

    //public delegate void Idle();
    //public Idle OnBeingIdle;
    //public delegate void BeingHarvested();
    //public BeingHarvested OnBeingHarvested;
    //public delegate void Interrupted();
    //public Interrupted OnInterrupted;

    /*private void Awake()
    {
        interactable = GetComponent<InteractableComponent>();
        Assert.IsNotNull(interactable, this.ToString() + "does not have an [InteractableComponent]");
    }*/
}

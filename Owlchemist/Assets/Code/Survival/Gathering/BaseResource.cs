using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Resource")]
public class BaseResource : ScriptableObject
{
    public float interactionTime = 1.0f;
    public bool destroyObjectOnPickup = true;
    public Sprite GuiSprite;
    public string GetName()
    {
        return name;
    }

    public virtual bool PerformCollectibleEvent(PlayerFilter player)
    {
        Debug.Log("Calling base function on: " + name + " ,this doesn't have functionality");
        return false;
    }
}

using UnityEngine;

public abstract class BaseStoryEvent : ScriptableObject
{
    public abstract void TriggerStoryEvent(PlayerFilter playerFilter, GameObject obj);
    public virtual void TriggerStoryEvent(PlayerFilter playerfilter, GameObject obj, CanvasGroupFader auxFader) 
    {
        Debug.Log("[BaseStoryEvent] triggered: " + obj + " with aux fader on: " + auxFader.gameObject);
        auxFader.Display(false);
    }

    public abstract void TriggerStoryBegin(PlayerFilter playerFilter, GameObject obj);
    public abstract void TriggerStoryProgress(PlayerFilter playerFilter, GameObject obj);

    public abstract void TriggerStoryEnd(PlayerFilter playerFilter, GameObject obj);
    public virtual void TriggerStoryEnd(PlayerFilter playerfilter, GameObject obj, CanvasGroupFader auxFader)
    {
        Debug.Log("[BaseStoryEvent] triggered ending on: " + obj + " with aux fader on: " + auxFader.gameObject);
        auxFader.Display(false);
    }
}

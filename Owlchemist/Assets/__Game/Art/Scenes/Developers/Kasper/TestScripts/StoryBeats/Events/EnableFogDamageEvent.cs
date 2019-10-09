using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryEvents/EnableFogDamage")]
public class EnableFogDamageEvent : BaseStoryEvent
{
    public override void TriggerStoryEvent(PlayerFilter playerFilter, GameObject obj)
    {
        playerFilter.healthComponent.isTutorialNoTakeDamage = false;
    }

    public override void TriggerStoryEvent(PlayerFilter playerFilter, GameObject obj, CanvasGroupFader auxFader)
    {
        playerFilter.healthComponent.isTutorialNoTakeDamage = false;
        base.TriggerStoryEvent(playerFilter, obj, auxFader);
    }

    public override void TriggerStoryBegin(PlayerFilter playerFilter, GameObject obj)
    {
        
    }

    public override void TriggerStoryProgress(PlayerFilter playerFilter, GameObject obj)
    {
        
    }

    public override void TriggerStoryEnd(PlayerFilter playerFilter, GameObject obj)
    {
        
    }
}

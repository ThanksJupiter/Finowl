using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryEvents/EnableVFX")]
public class EnableVFXEvent : BaseStoryEvent
{
    public override void TriggerStoryEvent(PlayerFilter playerFilter, GameObject obj)
    {
        obj.GetComponent<ParticleSystem>()?.Play();
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

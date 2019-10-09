using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/IsInRange")]
public class IsInRangeCheck : BaseStoryProgressionCondition
{
    public float compareDistance = 4f;

    public bool torchCheck;

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        if (torchCheck)
        {
            float dst = Vector3.Distance(player.go.transform.position, player.conditionsComponent.firstInteractable.position);
            return dst < compareDistance;
        }

        return false;
    }   
}

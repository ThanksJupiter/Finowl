using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/Distance")]
public class StoryProgressionDistanceCheck : BaseStoryProgressionCondition
{
    public bool useBeatDistanceThreshold = true;
    public float triggerDistance;

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        float dst = Vector3.Distance(player.go.transform.position, currentBeat.transform.position);
        return dst < (useBeatDistanceThreshold ? currentBeat.firstTriggerThreshold : triggerDistance);
    }
}

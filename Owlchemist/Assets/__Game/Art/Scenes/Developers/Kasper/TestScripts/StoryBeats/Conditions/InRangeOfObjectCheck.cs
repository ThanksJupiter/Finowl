using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/IsInRangeOfObject")]
public class InRangeOfObjectCheck : BaseStoryProgressionCondition
{
    public float distance = 1f;

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        return true;
    }

    public override bool IsInRange(PlayerFilter player, Transform t)
    {
        return Vector3.Distance(player.go.transform.position, t.position) < distance;
    }
}

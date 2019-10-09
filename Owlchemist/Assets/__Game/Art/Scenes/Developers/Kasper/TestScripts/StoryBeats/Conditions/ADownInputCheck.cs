using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/ADown")]
public class ADownInputCheck : BaseStoryProgressionCondition
{
    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        return player.inputComponent.aButtonDown;
    }
}

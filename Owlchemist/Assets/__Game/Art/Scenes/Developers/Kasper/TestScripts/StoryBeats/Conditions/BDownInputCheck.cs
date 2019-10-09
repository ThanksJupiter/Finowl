using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/BDown")]
public class BDownInputCheck : BaseStoryProgressionCondition
{
    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        return player.inputComponent.bButtonDown;
    }
}

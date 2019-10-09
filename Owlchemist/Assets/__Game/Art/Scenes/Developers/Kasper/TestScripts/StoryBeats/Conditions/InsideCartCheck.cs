using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/InsideCart")]
public class InsideCartCheck : BaseStoryProgressionCondition
{
    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        return player.gameManagerComponent.isInsideCart;
    }
}

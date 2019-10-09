using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/AutoStart")]
public class AutoStartCheck : BaseStoryProgressionCondition
{
    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        return true;
    }
}

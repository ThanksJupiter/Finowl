using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/Input&Distance")]
public class StoryProgressionDistanceInputCheck : BaseStoryProgressionCondition
{
    public bool useBeatDistanceThreshold = true;
    public float triggerDistance;

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        float dst = Vector3.Distance(player.go.transform.position, currentBeat.transform.position);

        if (dst < (useBeatDistanceThreshold ? currentBeat.firstTriggerThreshold : triggerDistance))
        {
            if (player.inputComponent.aButtonDown)
            {
                return true;
            }
        }

        return false;
    }
}

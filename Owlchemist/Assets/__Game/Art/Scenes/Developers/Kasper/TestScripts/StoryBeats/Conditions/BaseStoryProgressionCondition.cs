using UnityEngine;

public abstract class BaseStoryProgressionCondition : ScriptableObject
{
    public abstract bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat);
    public virtual bool IsInRange(PlayerFilter player, Transform t)
    {
        return true;
    }
}

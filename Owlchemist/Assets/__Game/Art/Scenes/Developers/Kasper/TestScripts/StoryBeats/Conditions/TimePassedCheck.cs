using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/TimePassed")]
public class TimePassedCheck : BaseStoryProgressionCondition
{
    public float waitTime = 6f;

    private float currentTime = 0f;

    private bool hasReset = false;

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        currentTime = 0f;
    }

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        if (!hasReset)
        {
            Reset();
            hasReset = true;
        }

        currentTime += Time.deltaTime;


        bool returnVal = currentTime >= waitTime;
        hasReset = !returnVal;
        return returnVal;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/Moved And Time Passed")]
public class PlayerMovedAndTimePassed : BaseStoryProgressionCondition
{
    public float waitTime = 6f;

    private bool playerMoved = false;
    private float currentTime = 0f;

    private bool hasReset = false;

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        currentTime = 0f;
        playerMoved = false;
    }

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        if (!hasReset)
        {
            Reset();
            hasReset = true;
        }

        if (player.movementComponent.velocity.magnitude != 0)
        {
            playerMoved = true;
        }

        if (playerMoved)
        {
            currentTime += Time.deltaTime;
        }

        bool returnVal = playerMoved && currentTime >= waitTime;
        hasReset = !returnVal;
        return returnVal;
    }
}

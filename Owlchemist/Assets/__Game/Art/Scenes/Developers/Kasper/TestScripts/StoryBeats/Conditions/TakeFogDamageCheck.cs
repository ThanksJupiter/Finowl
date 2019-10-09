using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoryConditions/TakenFogDamage")]
public class TakeFogDamageCheck : BaseStoryProgressionCondition
{
    private bool hasTakenDamage = false;
    private bool subbed = false;

    private void Reset()
    {
        hasTakenDamage = false;
        subbed = false;
    }

    private void Awake()
    {
        Reset();
    }

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        if (!subbed)
        {
            player.healthComponent.OnPlayerDecreaseGranularHealth += SetHasTakenDamageTrue;
        }
        else
        {
            player.healthComponent.OnPlayerDecreaseGranularHealth -= SetHasTakenDamageTrue;
            Debug.Log("unsubbed");
        }

        return hasTakenDamage;
    }

    private void SetHasTakenDamageTrue()
    {
        hasTakenDamage = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("StoryConditions/WithinCartDistance"))]
public class WithinCartDistance : BaseStoryProgressionCondition
{
    public float distance = 4f;

    public override bool ConditionMet(PlayerFilter player, StoryBeatComponent currentBeat)
    {
        float dst = Vector3.Distance(player.go.transform.position, player.gameManagerComponent.cartInteraction.cart.transform.position);
        return dst < distance;
    }
}

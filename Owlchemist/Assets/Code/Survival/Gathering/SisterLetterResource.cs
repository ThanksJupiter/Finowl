using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/Sister's Letter")]
public class SisterLetterResource : BaseResource
{
    public override bool PerformCollectibleEvent(PlayerFilter player)
    {
        // display letter ui
        player.uiComponent.sisterLetter.OpenLetter(player);

        // return false to also pick up item
        return false;
    }
}

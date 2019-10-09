using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/RefillTorch")]
public class RefillTorchResource : BaseResource
{
    public override bool PerformCollectibleEvent(PlayerFilter player)
    {
        player.eventComponent.OnRefillOneTorch();
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/Cart")]
public class EnterCartResource : BaseResource
{
    public override bool PerformCollectibleEvent(PlayerFilter player)
    {
        bool tempTest = false;
        for (int y = 0; y < player.inventoryComponent.InventoryBag.Count; y++)
        {
            if (player.inventoryComponent.InventoryBag[y] != player.inventoryComponent.tempColl)
            {
                tempTest = true;
                break;
            }
        }

        if (tempTest)
        {
            GameObject cart = player.gameManagerComponent.cartInteraction.cart;
            player.inventoryComponent.TransferItemsFromTo(player.inventoryComponent, cart.GetComponent<InventoryComponent>(), true);
        }
        player.gameManagerComponent.isInsideCart = true;
        player.gameManagerComponent.cartInteraction.EnterCart();
        return true;
    }
}

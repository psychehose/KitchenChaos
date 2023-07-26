using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO cuttingKitchenObjectSO;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())  
        {
            if (player.HasKitchenObject())
            {
                // Player is carrying
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player is carrying
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                // Player가 is not carrying anythinh
            }
            else
            {
                // Player에 손에 들게 해야함
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            // There is Kitchen Object here
            kitchenObject.DestroySelf();
            
            // Cutting
            KitchenObject.SpawnKitchenObject(cuttingKitchenObjectSO, this);
        }
    }
}

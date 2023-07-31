using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject())  
        {
            if (player.HasKitchenObject())
            {
                // Player가 가지고 있음
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Nothing
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                // Player가 가지고 있으면 아무것도 해선 안됨
            }
            else
            {
                // Player에 손에 들게 해야함
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;


    public static event EventHandler OnAnyObjectPlacedHere;
    
    public virtual void InteractAlternate(Player player) {
        Debug.LogError("InteractAlternate.Interact();");
    }
    
    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact();");
    }

    // Implement: IKitchenObjectParent
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this,EventArgs.Empty);
        }
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}

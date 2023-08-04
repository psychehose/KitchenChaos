using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
        
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;


    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredient (일단 재료 개수가 맞는 지 체크

                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    // Cycling through all ingredient in the Recipe
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredient in the Plate
                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            // Ingredient Matches
                            ingredientFound = true;
                            break;
                        } 

                    }

                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the Correct Recipe
                    Debug.Log("Player delivered the Correct Recipe");
                    waitingRecipeSOList.RemoveAt(i);
                    
                    return;
                }
            }
        } 
        // No Match Found
        // // Player did not delivered the Correct Recipe
        Debug.Log("Player did not delivered the Correct Recipe");

    }
}

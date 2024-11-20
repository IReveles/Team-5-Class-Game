using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrewingSystem : MonoBehaviour
{
    public List<Recipe> recipes;          // List of all possible recipes
    public float brewingTime = 2f;        // Time to complete the brewing
    private bool isBrewing = false;

    public void StartBrewing(Recipe recipe)
    {
        if (isBrewing) return;

        if (CanBrew(recipe))
        {
            StartCoroutine(BrewingProcess(recipe));
        }
        else
        {
            Debug.Log("Missing required items!");
        }
    }

    private bool CanBrew(Recipe recipe)
    {
        // Check if all items in the recipe are in the inventory
        foreach (Item requiredItem in recipe.requiredItems)
        {
            var inventoryItem = InventoryManager.Instance.Items.Find(i => i.id == requiredItem.id);
            if (inventoryItem == null)
            {
                return false; // Item is missing or has insufficient quantity
            }
        }
        return true;
    }

    private IEnumerator BrewingProcess(Recipe recipe)
    {
        isBrewing = true;
        Debug.Log("Brewing started...");

        yield return new WaitForSeconds(brewingTime);

        // Consume required items
        foreach (Item requiredItem in recipe.requiredItems)
        {
            InventoryManager.Instance.Remove(requiredItem); // This removes one of each item
        }

        // Add the result to the inventory
        InventoryManager.Instance.Add(recipe.result);
        
        isBrewing = false;
        Debug.Log("Brewing complete: " + recipe.result.itemName);
    }
}
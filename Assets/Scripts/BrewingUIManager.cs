using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use this if you are using TextMeshPro for text.

public class BrewingUIManager : MonoBehaviour
{
    public GameObject brewingPanel; // Reference to the brewing panel (UI).
    public TextMeshProUGUI recipeRequirementsText; // Display the recipe requirements.
    public TextMeshProUGUI brewResultText; // Display brew result.
    public Button brewButton; // Brew button to trigger brewing.
    public Slider brewingProgressSlider; // Optional progress bar.

    public GameObject recipeItemPrefab; // Prefab for recipe items in the list.
    public Transform recipeListContent; // Content for the list of recipes.
    
    public BrewingSystem brewingSystem; // Reference to your BrewingSystem

    private Recipe selectedRecipe; // The currently selected recipe.

    private void Start()
    {
        brewButton.onClick.AddListener(OnBrewButtonClicked);
        brewingPanel.SetActive(false);
        GenerateRecipeList();
    }

    void GenerateRecipeList()
    {
        foreach (var recipe in brewingSystem.recipes) 
        {
            GameObject recipeItem = Instantiate(recipeItemPrefab, recipeListContent);
            TextMeshProUGUI recipeNameText = recipeItem.GetComponentInChildren<TextMeshProUGUI>();
            recipeNameText.text = recipe.result.itemName;
            recipeItem.GetComponent<Button>().onClick.AddListener(() => OnRecipeSelected(recipe));
        }
    }

    void OnRecipeSelected(Recipe recipe)
    {
        selectedRecipe = recipe;
        DisplayRecipeRequirements();
    }

    void DisplayRecipeRequirements()
    {
        if (selectedRecipe != null)
        {
            recipeRequirementsText.text = "Required ingredients:\n";
            foreach (var ingredient in selectedRecipe.requiredItems)
            {
                recipeRequirementsText.text += $"{ingredient.itemName} x1\n";  // Display 1 quantity for each
            }
        }
    }

    void OnBrewButtonClicked()
    {
        if (CanBrew())
        {
            BrewItem();
        }
        else
        {
            brewResultText.text = "Not enough ingredients!";
        }
    }

    bool CanBrew()
    {
        foreach (var ingredient in selectedRecipe.requiredItems)
        {
            if (!InventoryManager.Instance.HasItem(ingredient))  // Check if the item exists
            {
                return false;
            }
        }
        return true;
    }

    void BrewItem()
    {
        foreach (var ingredient in selectedRecipe.requiredItems)
        {
            InventoryManager.Instance.Remove(ingredient);  // Remove one of each item
        }

        brewResultText.text = $"Successfully brewed {selectedRecipe.result.itemName}!";
        InventoryManager.Instance.Add(selectedRecipe.result);
        UpdateProgress(100); // Update progress (optional).
    }

    void UpdateProgress(float value)
    {
        brewingProgressSlider.value = value;
    }
}

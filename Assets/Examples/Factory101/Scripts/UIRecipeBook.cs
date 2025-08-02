using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRecipeBook : MonoBehaviour
{
    [SerializeField] private Transform listContent;
    [SerializeField] private RecipeTracker recipeTracker;
    [SerializeField] private UIMcGibbleRecipeItem prefabRecipeItem;

    [Header("Recipe Info")]
    public TextMeshProUGUI resultIcon;
    public TextMeshProUGUI resultName;
    public TextMeshProUGUI resultInfo;
    
    [Header("Recipe One")]
    public TextMeshProUGUI oneIcon;
    public TextMeshProUGUI oneInfo;
    
    [Header("Recipe Two")]
    public TextMeshProUGUI twoIcon;
    public TextMeshProUGUI twoInfo;
    
    private readonly List<UIMcGibbleRecipeItem> spawnedItems = new();
    public void OnEnable()
    {
        ClearInfo();
        ClearSpawned();

        foreach (var recipe in recipeTracker.GetAllRecipes())
        {
            var listItem = Instantiate(prefabRecipeItem, listContent)
                .GetComponent<UIMcGibbleRecipeItem>();
            listItem.SetRecipe(recipe);
            listItem.OnSelectRecipe.AddListener(SetRecipeInfo);
            spawnedItems.Add(listItem);
        }
    }

    public void OnDisable()
    {
        ClearSpawned();
    }

    private void ClearSpawned()
    {
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            var item = spawnedItems[i];
            if (item != null)
                item.OnSelectRecipe.RemoveAllListeners();
                Destroy(item.gameObject);
        }
        spawnedItems.Clear();
    }

    public void SetRecipeInfo(Recipe recipe)
    {
        if (recipe == null || recipe.result == null) return;

        resultIcon.text = recipe.result.singleEmoji;
        resultName.text = recipe.result.name;

        // Show the short description, with a fallback
        string shortDesc = string.IsNullOrWhiteSpace(recipe.result.shortDescription)
            ? "No description available."
            : recipe.result.shortDescription;

        // Optionally append some extra info (remove if you only want the flavor text)
        resultInfo.text = $"{shortDesc}\nRarity: {recipe.result.normalizedRarity:P0}\nHeat Resistance: {recipe.result.normalizedHeatAffinity:P0}";

        oneIcon.text = recipe.inputOne.singleEmoji;
        oneInfo.text = recipe.inputOne.name;

        twoIcon.text = recipe.inputTwo.singleEmoji;
        twoInfo.text = recipe.inputTwo.name;
    }

    private void ClearInfo()
    {
        resultIcon.text = "";
        resultName.text = "";
        resultInfo.text = "";

        oneIcon.text = "";
        oneInfo.text = "";
        
        twoIcon.text = "";
        twoInfo.text = "";
    }
}

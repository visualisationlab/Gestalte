using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Examples.Factory101.Scripts;
using Newtonsoft.Json;
using UnityEngine;

public class RecipeTracker : MonoBehaviour
{
    public static RecipeTracker Instance { get; private set; }
    public OracleAgent oracleAgent;

    [TextArea (3,24)] public string recipeRequestPrompt;
    [TextArea (3,24)] public string componentsDescriptionPrompt;
    
    private Queue<RecipeResponse> responseQueue = new();
    public int uniqueCounter;

    public List<RecipeScriptableObject> predefinedRecipes;
    [SerializeField] private List<Recipe> recipeList = new();
    [SerializeField] private UINewRecipeMessage recipeMessage;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate McGibbleTracker detected. Destroying new instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        foreach (var recip in predefinedRecipes)
        {
            Add(recip.recipe);
        }
    }

    public void Add(Recipe recipe)
    {
        recipeList.Add(recipe);
        recipeMessage.ShowNewRecipe(recipe);
    }

    private string GetAllExistingResultEmojis()
    {
        return string.Concat(
            recipeList.Select(r => r.result.singleEmoji)
        );
    }
    
    public async void GetRecipe(McGibbleDescription one, McGibbleDescription two, Action<Recipe> callback)
    {
        var existing = recipeList.FirstOrDefault(to => 
            (to.inputOne.singleEmoji == one.singleEmoji && to.inputTwo.singleEmoji == two.singleEmoji) ||
            (to.inputOne.singleEmoji == two.singleEmoji && to.inputTwo.singleEmoji == one.singleEmoji));

        if (existing != null)
        {
            callback(existing);
            return;
        }
        
        //Else it doesnt exist yet and we need to ask Oracle to make one?
        var message = $"{componentsDescriptionPrompt} {one.singleEmoji} and {two.singleEmoji}.";
        // responseQueue.Enqueue(new RecipeResponse{recipe=new Recipe{inputOne = one, inputTwo = two}, callback=callback});
        // oracleAgent.SendMessage(message, OracleAgentReply);
        var systemMessage =
            $"{recipeRequestPrompt}. Follow this formatting in your response: {McGibbleDescription.Format()}. Absolutely Avoid using the following already existing emojis: {GetAllExistingResultEmojis()}";
        
        var response = await oracleAgent.SendMessageDirect(systemMessage, message);
        var recipe = OnResponseAddRecipe(new Recipe{inputOne = one, inputTwo = two}, response);
        callback.Invoke(recipe);
    }

    public void OracleAgentReply(string message)
    {
        var response = responseQueue.Dequeue();
        OnResponseAddRecipe(response.recipe, message);
        response.callback(response.recipe);
    }

    public Recipe OnResponseAddRecipe(Recipe partialRecipe, string message)
    {
        var json = JsonHelper.ExtractJson(message);
        
        string cleaned = Regex.Replace(
            json,
            @"^```json\s*|\s*```$",
            "",
            RegexOptions.Multiline
        ).Trim();
        
        McGibbleDescription resp = JsonConvert.DeserializeObject<McGibbleDescription>(cleaned);
        
        Recipe hasMatch = recipeList.FirstOrDefault(r => r.result.singleEmoji == resp.singleEmoji);
        //a recipe with this result already exists, we copy the result values over
        if (hasMatch != null)
        {
            partialRecipe.result = hasMatch.result;
        }
        else // a totally new one needs to be created
        {
            uniqueCounter++;
            var finalShortDesc = !string.IsNullOrWhiteSpace(resp.shortDescription)
                ? resp.shortDescription
                : $"A {resp.name} with rarity {resp.normalizedRarity:F2}.";

            // int newSalePrice = Mathf.CeilToInt(uniqueCounter * resp.normalizedRarity); //Fine tune to get increasing price
            partialRecipe.result = new McGibbleDescription
            {
                name = resp.name,
                singleEmoji = resp.singleEmoji,
                normalizedRarity = resp.normalizedRarity,
                normalizedHeatResistance = resp.normalizedHeatResistance,
                uniqueCreated = uniqueCounter,
                raritySalePrice = Mathf.FloorToInt(uniqueCounter * resp.normalizedRarity) + 1,
                shortDescription = finalShortDesc
            };
        }

        Add(partialRecipe);
        return partialRecipe;
    }

    public List<Recipe> GetAllRecipes()
    {
        return recipeList;
    }

}

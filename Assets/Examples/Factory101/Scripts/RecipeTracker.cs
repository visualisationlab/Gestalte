using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    private string GetAllExistingResultEmojis()
    {
        return string.Concat(
            recipeList.Select(r => r.result.singleEmoji)
        );
    }
    
    public void GetRecipe(McGibbleDescription one, McGibbleDescription two, Action<Recipe> callback)
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
        var message = $"{recipeRequestPrompt} {componentsDescriptionPrompt} {one.singleEmoji} and {two.singleEmoji}. Follow this formatting in your response: {McGibbleDescription.Format()}. Avoid using the following already existing emojis: {GetAllExistingResultEmojis()}";
        
        responseQueue.Enqueue(new RecipeResponse{recipe=new Recipe{inputOne = one, inputTwo = two}, callback=callback});
        oracleAgent.SendMessage(message, OracleAgentReply);
    }

    public void OracleAgentReply(string message)
    {
        var response = responseQueue.Dequeue();
        var json = JsonHelper.ExtractJson(message);
        McGibbleDescription resp = JsonConvert.DeserializeObject<McGibbleDescription>(json);
        
        Recipe hasMatch = recipeList.FirstOrDefault(r => r.result.singleEmoji == resp.singleEmoji);
        //a recipe with this result already exists, we copy the result values over
        if (hasMatch != null)
        {
            response.recipe.result = hasMatch.result;
        }
        else // a totally new one needs to be created
        {
            uniqueCounter++;
            // int newSalePrice = Mathf.CeilToInt(uniqueCounter * resp.normalizedRarity); //Fine tune to get increasing price
            response.recipe.result = new McGibbleDescription
            {
                name = resp.name,
                singleEmoji = resp.singleEmoji, 
                normalizedRarity = resp.normalizedRarity,
                normalizedHeatResistance = resp.normalizedHeatResistance,
                uniqueCreated = uniqueCounter
            };
        }
        
        recipeList.Add(response.recipe);
        response.callback(response.recipe);
    }

}

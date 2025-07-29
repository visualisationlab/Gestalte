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
    
    public class Recipe
    {
        public McGibbleDescription inputOne;
        public McGibbleDescription inputTwo;
        public McGibbleDescription result;
    }
    
    private List<Recipe> recipeList = new();
    
    public void Add(Recipe recipe)
    {
        recipeList.Add(recipe);
    }
    
    public void GetRecipe(McGibbleDescription one, McGibbleDescription two, Action<Recipe> callback)
    {
        var existing = recipeList.FirstOrDefault(to => 
            (to.inputOne.gibbleType == one.gibbleType && to.inputTwo.gibbleType == two.gibbleType) ||
            (to.inputOne.gibbleType == two.gibbleType && to.inputTwo.gibbleType == one.gibbleType));

        if (existing != null)
        {
            callback(existing);
            return;
        }
        
        //Else it doesnt exist yet and we need to ask Oracle to make one?
        var message = $"{recipeRequestPrompt} + {OracleRecipeResponse.Format()} + {componentsDescriptionPrompt} + {one.gibbleType} and {two.gibbleType}";
        
        responseQueue.Enqueue(new RecipeResponse{recipe=new Recipe{inputOne = one, inputTwo = two}, callback=callback});
        oracleAgent.SendMessage(message, OracleAgentReply);
    }

    public void OracleAgentReply(string message)
    {
        var response = responseQueue.Dequeue();
        var json = JsonHelper.ExtractJson(message);
        OracleRecipeResponse resp = JsonConvert.DeserializeObject<OracleRecipeResponse>(json);
        
        response.recipe.result = new McGibbleDescription { gibbleType = resp.emoji };
        recipeList.Add(response.recipe);
        response.callback(response.recipe);
    }

}

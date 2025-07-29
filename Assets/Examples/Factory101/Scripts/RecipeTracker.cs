using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeTracker : MonoBehaviour
{
    public static RecipeTracker Instance { get; private set; }

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
        public McGibble inputOne;
        public McGibble inputTwo;
        public McGibble result;
    }
    
    private List<Recipe> recipeList = new();
    
    public void Add(Recipe recipe)
    {
        recipeList.Add(recipe);
    }
    
    public Recipe RetrieveByInputs(McGibble one, McGibble two)
    {
        return recipeList.First(to =>
            (to.inputOne == one && to.inputTwo == two) ||
            (to.inputOne == two && to.inputTwo == one));
    }
    
}

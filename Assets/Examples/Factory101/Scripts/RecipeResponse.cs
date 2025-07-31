using System;

namespace Examples.Factory101.Scripts
{
    public class RecipeResponse
    {
        public Recipe recipe;
        public Action<Recipe> callback;
    }
}
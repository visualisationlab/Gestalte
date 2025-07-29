using System;

namespace Examples.Factory101.Scripts
{
    public class RecipeResponse
    {
        public RecipeTracker.Recipe recipe;
        public Action<RecipeTracker.Recipe> callback;
    }
}
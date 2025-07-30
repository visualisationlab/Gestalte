using UnityEngine;

namespace Examples.Factory101.Scripts
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "McGibble/Recipe")]
    public class RecipeScriptableObject:ScriptableObject
    {
        public Recipe recipe;
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIMcGibbleRecipeItem : MonoBehaviour,IPointerClickHandler
{
   [SerializeField] private TextMeshProUGUI icon;
   [SerializeField] private TextMeshProUGUI name;

   private Recipe recipe;
   public UnityEvent<Recipe> OnSelectRecipe;
   
   public void SetRecipe(Recipe newRecipe)
   {
      recipe = newRecipe;
      icon.text = recipe.result.singleEmoji;
      name.text = recipe.result.name;
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      OnSelectRecipe.Invoke(recipe);
   }
}

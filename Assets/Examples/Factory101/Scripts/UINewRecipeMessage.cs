using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINewRecipeMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] TextMeshProUGUI icon;
    [SerializeField] private string newRecipeMessage;
    [SerializeField] private Animator animation;
    
    public void ShowNewRecipe(Recipe recipe)
    {
        Debug.Log("New Recipe Message!");
        message.text = $"{newRecipeMessage}: {recipe.result.name}";
        icon.text = recipe.result.singleEmoji;
        animation.SetTrigger("ShowMessage");
        StartCoroutine(PlayChimeDelayed());
    }

    IEnumerator PlayChimeDelayed()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.Chime();
    } 
}

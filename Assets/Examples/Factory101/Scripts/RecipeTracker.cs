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

    [TextArea(3, 24)] public string recipeRequestPrompt;
    [TextArea(3, 24)] public string componentsDescriptionPrompt;

    private Queue<RecipeResponse> responseQueue = new();
    public int uniqueCounter;

    public List<RecipeScriptableObject> predefinedRecipes;
    [SerializeField] private List<Recipe> recipeList = new();
    [SerializeField] private UINewRecipeMessage recipeMessage;
    [Header("Fallback")]
    public McGibbleDescriptionFallbackList fallbackDescriptions; // drag your .asset here in inspector

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

        var message = $"{componentsDescriptionPrompt} {one.singleEmoji} and {two.singleEmoji}.";
        var systemMessage =
            $"{recipeRequestPrompt}. Follow this formatting in your response: {McGibbleDescription.Format()}. Absolutely Avoid using the following already existing emojis: {GetAllExistingResultEmojis()}";

        var response = await oracleAgent.SendMessageDirect(systemMessage, message);

        Recipe recipe = null;

        if (string.IsNullOrWhiteSpace(response))
        {
            Debug.LogWarning("[RecipeTracker] Empty/null response from OracleAgent. Falling back.");
            recipe = BuildFallbackRecipe(one, two);
            callback(recipe);
            return;
        }

        recipe = OnResponseAddRecipe(new Recipe { inputOne = one, inputTwo = two }, response);
        if (recipe == null || recipe.result == null)
        {
            Debug.LogWarning("[RecipeTracker] Parsed recipe invalid. Falling back.");
            recipe = BuildFallbackRecipe(one, two);
            callback(recipe);
            return;
        }

        callback(recipe);
    }

    public void OracleAgentReply(string message)
    {
        var response = responseQueue.Dequeue();
        OnResponseAddRecipe(response.recipe, message);
        response.callback(response.recipe);
    }

    public Recipe OnResponseAddRecipe(Recipe partialRecipe, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Debug.LogError("[RecipeTracker] Empty or null response from OracleAgent. Aborting recipe creation.");
            return partialRecipe;
        }

        string extractedJson = null;
        try
        {
            extractedJson = JsonHelper.ExtractJson(message);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RecipeTracker] Error extracting JSON: {ex}. Raw message: {message}");
        }

        if (string.IsNullOrWhiteSpace(extractedJson))
        {
            Debug.LogError($"[RecipeTracker] Could not extract valid JSON from response. Raw message: {message}");
            return partialRecipe;
        }

        string cleaned = Regex.Replace(
            extractedJson,
            @"^```json\s*|\s*```$",
            "",
            RegexOptions.Multiline
        ).Trim();

        McGibbleDescription resp = null;
        try
        {
            resp = JsonConvert.DeserializeObject<McGibbleDescription>(cleaned);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RecipeTracker] Failed to deserialize McGibbleDescription: {ex}. Cleaned JSON: {cleaned}");
            return partialRecipe;
        }

        if (resp == null)
        {
            Debug.LogError("[RecipeTracker] Deserialized McGibbleDescription was null.");
            return partialRecipe;
        }

        Recipe hasMatch = recipeList.FirstOrDefault(r => r.result.singleEmoji == resp.singleEmoji);
        if (hasMatch != null)
        {
            // backfill shortDescription if missing
            if (string.IsNullOrWhiteSpace(hasMatch.result.shortDescription) && !string.IsNullOrWhiteSpace(resp.shortDescription))
            {
                hasMatch.result.shortDescription = resp.shortDescription;
            }

            partialRecipe.result = hasMatch.result;
        }
        else
        {
            uniqueCounter++;
            var finalShortDesc = !string.IsNullOrWhiteSpace(resp.shortDescription)
                ? resp.shortDescription
                : $"A {resp.name} with rarity {resp.normalizedRarity:F2}.";

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

    private McGibbleDescription GetRandomFallbackDescriptionExcludingUsed()
    {
        // Gather emojis already present in existing recipe results
        var usedEmojis = new HashSet<string>(
            recipeList
                .Where(r => r.result != null && !string.IsNullOrWhiteSpace(r.result.singleEmoji))
                .Select(r => r.result.singleEmoji)
        );

        McGibbleDescription template = null;

        if (fallbackDescriptions != null && fallbackDescriptions.descriptions != null && fallbackDescriptions.descriptions.Count > 0)
        {
            // Filter out those with used emoji
            var candidates = fallbackDescriptions.descriptions
                .Where(f => !usedEmojis.Contains(f.singleEmoji))
                .ToArray();

            if (candidates.Length > 0)
            {
                template = candidates[UnityEngine.Random.Range(0, candidates.Length)];
            }
            else
            {
                // All fallback emojis are already used; fall back to full pool
                template = fallbackDescriptions.descriptions[UnityEngine.Random.Range(0, fallbackDescriptions.descriptions.Count)];
            }
        }

        if (template == null)
        {
            Debug.LogWarning("[RecipeTracker] No fallback descriptions assigned; using built-in default.");
            return new McGibbleDescription
            {
                name = "Fallback",
                singleEmoji = "❔",
                normalizedRarity = 0.1f,
                normalizedHeatResistance = 0.1f,
                shortDescription = "A default fallback gibble."
            };
        }

        Debug.Log($"[RecipeTracker] Using fallback description: {template.name} ({template.singleEmoji})");
        // Clone so we don't mutate the asset instance
        return new McGibbleDescription
        {
            name = template.name,
            singleEmoji = template.singleEmoji,
            normalizedRarity = template.normalizedRarity,
            normalizedHeatResistance = template.normalizedHeatResistance,
            shortDescription = template.shortDescription
            // uniqueCreated and raritySalePrice will be set later
        };
    }

    private Recipe BuildFallbackRecipe(McGibbleDescription inputOne, McGibbleDescription inputTwo)
    {
        var fallbackDesc = GetRandomFallbackDescriptionExcludingUsed();
        uniqueCounter++;
        fallbackDesc.uniqueCreated = uniqueCounter;
        fallbackDesc.raritySalePrice = Mathf.FloorToInt(uniqueCounter * fallbackDesc.normalizedRarity) + 1;

        var recipe = new Recipe
        {
            inputOne = inputOne,
            inputTwo = inputTwo,
            result = fallbackDesc
        };

        Add(recipe);
        return recipe;
    }
}

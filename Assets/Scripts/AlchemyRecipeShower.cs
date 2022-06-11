using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class AlchemyRecipeShower : MonoBehaviour
{

    private TMP_Text text => GetComponent<TMP_Text>();
    private void Awake()
    {
        text.text = "";
    }

    public void ShowIngredients(Dictionary<string, Ingredient> ingredients)
    {
        text.text = GetList(ingredients);
    }

    private string GetList(Dictionary<string, Ingredient> ingredients)
    {
        string recipe = "";
        foreach (Ingredient i in ingredients.Values)
        {
            recipe += i.ToString() + "\n";
        }
        return recipe;
    }
}

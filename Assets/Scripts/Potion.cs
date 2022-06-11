using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private Dictionary<string, Ingredient> ingredientList = new Dictionary<string, Ingredient>();

    public void SetPotion(Dictionary<string, Ingredient> ingredients)
    {
        this.ingredientList = ingredients;
    }

    public Dictionary<string, Ingredient> GetPotion()
    {
        return ingredientList;
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class CauldronLogger : BaseLogger
{
    //PATTERN
    // handtype, sourcetype, event_type, null, time, 
    //event interactable name, event interactable tag, event interactable id,
    // ingredient string
    public void LogAddIngredientEvent(XRBaseInteractable interactable, Ingredient ing)
    {
        string type = "D_cauldron";
        string eventType = "AddIngredient";

        float timeMillis = Time.time * 1000;
        string interactableInfo = "null,null,null";

        interactableInfo = interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();

        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "null" + "," + timeMillis;
        line += "," + interactableInfo;
        line += "," + ing.ToString();
        WriteLog(line, timeMillis);
    }

    //PATTERN (not using, gonna rely on ListUpdate with list being 'none'
    // handtype, sourcetype, event_type, null, time, 
    // previous ingredientlist string sep by |
    public void LogClearEvent(Dictionary<string, Ingredient> list)
    {

    }

    //PATTERN
    // handtype, sourcetype, event_type, null, time, 
    //  ingredientlist string sep by | ('none' if empty) (will be pre or post)
    public void LogListUpdate(Dictionary<string, Ingredient> list, string mod = "")
    {
        string type = "D_cauldron";
        string eventType = "UpdateList";
        if (!mod.Equals(""))
        {
            eventType += ":" + mod;
        }

        string recipe = GetList(list);
        recipe = recipe.Replace("\n", "|");

        float timeMillis = Time.time * 1000;
        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "null" + "," + timeMillis;
        line += "," + recipe;
        WriteLog(line, timeMillis);
    }

    private string GetList(Dictionary<string, Ingredient> ingredients)
    {
        string recipe = "";
        foreach (Ingredient i in ingredients.Values)
        {
            recipe += i.ToString() + "\n";
        }
        if (recipe.Equals(""))
            recipe = "none";
        return recipe;
    }
}

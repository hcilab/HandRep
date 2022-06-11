using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomBottleLogger : BaseLogger
{
    //PATTERN
    // handtype, sourcetype, event_type, fill, time, 
    //event interactable name, event interactable tag, event interactable id,
    //event interactor name, event interactor tag, event interactor id,
    // object props sep by |
    // recipe contents sep by |

    public void LogFillEvent(CustomPotionManager cpm)
    {
        string type = "D_custombottle";
        string eventType = "Fill";

        float timeMillis = Time.time * 1000;
        string interactorInfo = "null,null,null";
        string interactableInfo = "null,null,null";

        XRBaseInteractable interactable = cpm.GetComponent<XRBaseInteractable>(); // never null
        XRBaseInteractor interactor = interactable.isSelected ? interactable.selectingInteractor : null;

        if (interactable != null)
            interactableInfo = interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();

        if (interactor != null)
            interactorInfo = interactor.name + "," + interactor.tag + "," + interactor.GetInstanceID();
        string properties = "null";
        if (interactable.TryGetComponent<ObjectProperties>(out ObjectProperties props))
        {
            properties = props.ToString();
        }
        string contents = GetList(cpm.GetBottleRecipe()).Replace("\n", "|");

        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "fill" + "," + timeMillis;
        line += "," + interactableInfo;
        line += "," + interactorInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += "," + contents;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class CauldronBottleInteraction : MonoBehaviour
{
    [SerializeField] private AlchemyRecipe cauldronContents;
    [SerializeField] DialReceiver dr;
    public UnityEvent<Dictionary<string, Ingredient>> OnBottleFill = null;
    public void OnFillBottle(HoverEnterEventArgs args)
    {
        if(args.interactable.TryGetComponent<CustomPotionManager>(out CustomPotionManager cpm))
        {
            if (cpm.CheckCork() && cauldronContents.GetIngredients().Count > 0) 
            {
                // check it is off
                Dictionary<string, Ingredient> ingredients = cauldronContents.GetIngredients();
                if (dr != null)
                {
                    Ingredient temperature = new Ingredient("temperature", "Temperature", 0, dr.GetCurrentValue(), false, true);
                    ingredients.Add(temperature.key, temperature);
                }
                cpm.Fill(cauldronContents.GetIngredients());
                if(OnBottleFill != null)
                {
                    OnBottleFill.Invoke(cpm.GetBottleRecipe());
                }
                cauldronContents.FillBottle();
            }
        }
    }
}

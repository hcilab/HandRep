using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//[RequireComponent(typeof(CauldronLogger))]
public class AlchemyRecipe : MonoBehaviour
{
    public UnityEvent<Ingredient, Dictionary<string, Ingredient>> OnIngredientAdded = null;
    public UnityEvent OnCauldronEmpty = null;
    //public UnityEvent OnBottleFill = null;
    public UnityEvent<Dictionary<string, Ingredient>> OnIngredientsUpdate = null;

    private Dictionary<string, Ingredient> ingredientList = new Dictionary<string, Ingredient>();
    //private CauldronLogger clog;

    //public bool EnableLogging = false;

    private void Awake()
    {
        /*if(EnableLogging)
        {
            clog = GetComponent<CauldronLogger>();
        }*/
    }
    public void AddIngredient(Ingredient i)
    {
        /*if (clog)
        {
            clog.LogListUpdate(ingredientList, "pre");
        }*/
        if (ingredientList.TryGetValue(i.key, out Ingredient ix))
        {
            ix.AddAmounts(i);
        }
        else
        {
            this.ingredientList.Add(i.key, i);
        }
        if (OnIngredientsUpdate != null)
        {
            
            OnIngredientsUpdate.Invoke(ingredientList);
        }
        if(OnIngredientAdded != null)
        {
            OnIngredientAdded.Invoke(i, ingredientList);
        }
        /*if (clog)
        {
            clog.LogListUpdate(ingredientList, "post");
        }*/
    }

    public void AddIngredient(ObjectProperties properties)
    {
        Ingredient i = ConvertToIngredient(properties);
        this.AddIngredient(i);
    }

    public void AddIngredient(XRBaseInteractable interactable)
    {
        this.AddIngredient(interactable.GetComponent<ObjectProperties>());
        /*if(clog)
        {
            clog.LogAddIngredientEvent(interactable, ConvertToIngredient(interactable));
        }*/
    }

    public void AddIngredient(BaseInteractionEventArgs args)
    {
        this.AddIngredient(args.interactable);
    }

    public string GetList()
    {
        string recipe = "";
        foreach (Ingredient i in ingredientList.Values)
        {
            recipe += i.ToString() + "\n";
        }
        return recipe;
    }

    public Dictionary<string, Ingredient> GetIngredients()
    {
        return ingredientList;
    }

    public void ClearList()
    {
        /*if (clog)
        {
            clog.LogListUpdate(ingredientList, "pre");
        }*/
        ingredientList.Clear();
        /*if (clog)
        {
            clog.LogListUpdate(ingredientList, "post");
        }*/
        if (OnIngredientsUpdate != null)
        {
            OnIngredientsUpdate.Invoke(ingredientList);
        }
        if(OnCauldronEmpty != null)
        {
            OnCauldronEmpty.Invoke();
        }
    }

    public void FillBottle()
    {
        ingredientList.Clear();
        if (OnIngredientsUpdate != null)
        {
            OnIngredientsUpdate.Invoke(ingredientList);
        }
    }

    private Ingredient ConvertToIngredient(ObjectProperties properties)
    {
        List<ObjectProperty> props = properties.properties;
        string key = props.Find((p) => p.key.Equals("ingredient_type")).value;
        string name = props.Find((p) => p.key.Equals("clean_name")).value;
        float quantity = float.Parse(props.Find((p) => p.key.Equals("weight")).value);
        int count = 1;
        bool isSolid = props.Find((p) => p.key.Equals("isSolid")).value.Equals("true");
        if (key.Equals("bottle"))
        {
            key = props.Find((p) => p.key.Equals("liquid")).value + "_" + key;
        }
        Ingredient i = new Ingredient(key, name, count, quantity, isSolid);
        return i;
    }

    private Ingredient ConvertToIngredient(XRBaseInteractable interactable)
    {
        return ConvertToIngredient(interactable.GetComponent<ObjectProperties>());
    }
}

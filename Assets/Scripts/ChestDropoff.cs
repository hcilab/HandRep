using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class ChestDropoff : MonoBehaviour
{
    // TODO ADD LOGGER
    // TODO ADD RECIPE MATCHER LINKUP (need to make recipe system first)

    // for now, only going to support single item aka single bottle for this
    [SerializeField] private AlchemyRecipeTask tasks;
    private bool isClosed = true;
    private Dictionary<int, XRBaseInteractable> contents = new Dictionary<int, XRBaseInteractable>();
    public void AddObject(HoverEnterEventArgs args)
    {
        int id = args.interactable.GetInstanceID();
        if (args.interactable.TryGetComponent<XRInteractableDestroyer>(out XRInteractableDestroyer xrid))
        {
            xrid.NowSelected(null);
            xrid.enabled = false;
        }
        contents.Add(id, args.interactable);
    }

    public void RemoveObject(HoverExitEventArgs args)
    {
        int id = args.interactable.GetInstanceID();
        if(args.interactable.TryGetComponent<XRInteractableDestroyer>(out XRInteractableDestroyer xrid))
        {
            xrid.enabled = true;
            if (!args.interactable.isSelected)
                xrid.Unselected(null);
            else
                xrid.NowSelected(null);
        }
        contents.Remove(id);
    }

    public Ingredient[] GetRawIngredients()
    {
        Ingredient[] ingredients = { };
        foreach(XRBaseInteractable i in contents.Values)
        {
            ObjectProperties props = i.GetComponent<ObjectProperties>();
            if(!props.CompareProperty("ingredient_type", "bottle")) {

            }
        }
        return ingredients;
    }

    public Ingredient[][] GetBottles()
    {
        List<Ingredient[]> bottles = new List<Ingredient[]>();
        foreach (XRBaseInteractable i in contents.Values)
        {
            ObjectProperties props = i.GetComponent<ObjectProperties>();
            if (props.CompareProperty("ingredient_type", "bottle"))
            {
                bottles.Add(GetBottleIngredients(props.GetComponent<CustomPotionManager>()));
            }
        }
        return bottles.ToArray();
    }

    public Ingredient[] GetBottleIngredients(CustomPotionManager cpm)
    {
        if(cpm.CheckCork())
        {
            Debug.Log("No cork");
            // no cork means no good
            return null;
        }
        List<Ingredient> ingredients = new List<Ingredient>();
        
        foreach(Ingredient i in cpm.GetBottleRecipe().Values)
        {
            ingredients.Add(i);
        }
        return ingredients.ToArray();
    }

    public bool CheckIsClosed()
    {
        return isClosed;
    }

    public void SetClosed(bool val)
    {
        isClosed = val;
    }

    public void SubmitBottle()
    {
        if(!CheckIsClosed())
        {
            return;
        }
        if(contents == null || contents.Count == 0)
        {
            return;
        }
        Ingredient[] ins = null;
        int id = -1;
        foreach (XRBaseInteractable i in contents.Values)
        {
            ObjectProperties props = i.GetComponent<ObjectProperties>();
            if (props.CompareProperty("ingredient_type", "bottle"))
            {
                ins = GetBottleIngredients(props.GetComponent<CustomPotionManager>());
                id = props.GetComponent<XRBaseInteractable>().GetInstanceID();
                break;
            }
        }
        if(id != -1)
        {
            Destroy(contents[id].gameObject, 2f);
            contents.Remove(id);
        }
        if(ins == null)
        {
            Debug.Log("NULL INGREDIENTS");
        }
        else
        {
            /*foreach(Ingredient i in ins)
            {
                //Debug.Log(i.ToString());
            }*/
        }

        if(tasks)
        {
            tasks.SubmitPotion(ins);
        }
    }
}

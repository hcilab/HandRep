using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//[RequireComponent(typeof(CustomBottleLogger))]
public class CustomPotionManager : MonoBehaviour
{
    [SerializeField] GameObject contents;
    [SerializeField] Material liquidMaterial;
    [SerializeField] GameObject pourer;
    [SerializeField] private XRSocketInteractor corkSocket;
    [SerializeField] LiquidWobble wobbler;
    [SerializeField] AudioSource audioS;
    [SerializeField] AudioClip fillSound;
    public UnityEvent<Dictionary<string, Ingredient>> OnBottleFill = null;

    //CustomBottleLogger cblog => GetComponent<CustomBottleLogger>();

    private XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();
    private Dictionary<string, Ingredient> ingredientList = new Dictionary<string, Ingredient>();

    public void Fill(Dictionary<string, Ingredient> ingredientList)
    {
        if (this.ingredientList.Count == 0)
        {
            audioS.PlayOneShot(fillSound);
            interactable.GetComponent<ObjectProperties>().AddOrSet("liquid", "custom");
            interactable.GetComponent<ObjectProperties>().AddOrSet("liquid", "custom");
            interactable.GetComponent<ObjectProperties>().AddOrSet("clean_name", "Custom Potion");
            contents.GetComponent<MeshRenderer>().material = liquidMaterial;
            pourer.SetActive(true);
            this.ingredientList = new Dictionary<string, Ingredient>();
            foreach(Ingredient i in ingredientList.Values)
            {
                this.ingredientList.Add(i.key, i);
            }
            this.gameObject.tag = "CustomPotion";
            wobbler.enabled = true;
            if (OnBottleFill != null)
                OnBottleFill.Invoke(this.GetBottleRecipe());
            //cblog.LogFillEvent(this);
        }
    }

    public bool CheckCork()
    {
        if (!corkSocket)
        {
            return true;
        }
        if (corkSocket && corkSocket.selectTarget)
        {
            return false;
        }
        else if (corkSocket && !corkSocket.selectTarget)
        {
            return true;
        }
        return true;
    }
    public Dictionary<string, Ingredient> GetBottleRecipe()
    {
        return this.ingredientList;
    }
}

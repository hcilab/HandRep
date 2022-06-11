using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class EnchantObjectHelper : MonoBehaviour
{
    public UnityEvent OnEnchant = null;
    [SerializeField] private GameObject enchantedPrefab = null;
    //[SerializeField] private EnchanterLogger elog => GetComponent<EnchanterLogger>();

    public void EnchantObject(HoverEnterEventArgs args)
    {
        if(enchantedPrefab)
        {
            if(OnEnchant != null)
            {
                OnEnchant.Invoke();
            }
            //if(elog)
                //elog.LogEnchantEvent(args.interactable, "pre");
            Instantiate(enchantedPrefab,
                ((XRGrabInteractable)args.interactable).attachTransform.position,
                Quaternion.identity, args.interactable.transform);
            ChangeProperties(args.interactable);
            //if (elog)
                //elog.LogEnchantEvent(args.interactable, "post");
        }
    }

    private void ChangeProperties(XRBaseInteractable interactable)
    {
        if (interactable.GetComponent<ObjectProperties>().CompareProperty("enchanted", "false"))
        {
            interactable.GetComponent<ObjectProperties>().AddOrSet("enchantable", "false");
            interactable.GetComponent<ObjectProperties>().AddOrSet("enchanted", "true");
            interactable.GetComponent<ObjectProperties>().AddOrSet("clean_name", "Enchanted " +  
                interactable.GetComponent<ObjectProperties>().properties.Find( (p) =>
                    p.key == "clean_name").value);
            interactable.GetComponent<ObjectProperties>().AddOrSet("ingredient_type", "ench_" +
                interactable.GetComponent<ObjectProperties>().properties.Find((p) =>
                   p.key == "ingredient_type").value);
        }
    }
}

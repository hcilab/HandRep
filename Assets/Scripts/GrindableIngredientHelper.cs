using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ObjectProperties))]
[RequireComponent(typeof(XRGrabInteractable))]
//[RequireComponent(typeof(GrinderLogger))]
public class GrindableIngredientHelper : MonoBehaviour
{
    private XRGrabInteractable interactable => GetComponent<XRGrabInteractable>();
    private List<ObjectProperty> props => GetComponent<ObjectProperties>().properties;
    [SerializeField] private GameObject grindedPrefab = null;

    public bool IsTrainingTrialTask = false;
    //private GrinderLogger glog => GetComponent<GrinderLogger>();

    private void Awake()
    {
        bool isGrindable = false;
        foreach (ObjectProperty p in props)
        {
            if (p.key.Equals("grindable") && p.value.Equals("true"))
            {
                isGrindable = true;
                break;
            }
        }
        if(!isGrindable)
        {
            Destroy(this);
        }
    }

    public void Grind()
    {
        if(!grindedPrefab || !interactable.isSelected)
        {
            return;
        }
        //glog.LogGrindEvent(interactable, "old"); // equiv to stop but done
        GameObject spawn = Instantiate(grindedPrefab, transform.parent, true);
        LayerMask origMask = spawn.GetComponent<XRBaseInteractable>().interactionLayerMask;
        spawn.transform.position = interactable.selectingInteractor.attachTransform.transform.position;
        interactable.interactionManager.ForceSelect(interactable.selectingInteractor,
            spawn.GetComponent<XRBaseInteractable>());
        spawn.GetComponent<XRBaseInteractable>().interactionLayerMask = origMask;
        //glog.LogGrindEvent(spawn.GetComponent<XRBaseInteractable>(), "new");

        ObjectProperties props = GetComponent<ObjectProperties>();
        foreach(ObjectProperty p in props.properties)
        {
            string val = p.value;
            if(p.key.Equals("weight"))
            {
                val = (float.Parse(p.value) / 2).ToString("F2");
            }
            if(p.key.Equals("enchantable") || p.key.Equals("grindable"))
            {
                val = "false";
            }
            if(p.key.Equals("isDust"))
            {
                val = "true";
            }
            if(p.key.Equals("ingredient_type"))
            {
                val = "grnd_" + val.Replace("grind_", "");
            }
            if (p.key.Equals("clean_name"))
            {
                val = "Ground " + val.Replace("Ground ","");
            }
            spawn.GetComponent<ObjectProperties>().AddOrSet(p.key, val);

        }

        if (IsTrainingTrialTask)
        {
            T7_Manager t7m = FindObjectOfType<T7_Manager>();
            if (t7m != null)
            {
                t7m.GrindItem(spawn);
            }
        }
        if(this.gameObject)
            Destroy(this.gameObject);
    }

}

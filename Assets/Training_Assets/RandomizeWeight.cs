using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RandomizeWeight : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float RandomizePercentage = 0.10f;
    public void OnGrab(SelectExitEventArgs args)
    {

        GameObject go = args.interactable.gameObject;
        string weight = go.GetComponent<ObjectProperties>().GetProperty("weight");
        if (weight != null && !weight.Equals(""))
        {
            float newVal = float.Parse(weight);
            newVal = Random.Range(newVal - newVal * RandomizePercentage, newVal + newVal * RandomizePercentage);
            go.GetComponent<ObjectProperties>().AddOrSet("weight", newVal.ToString("F2"));
        }
            
    }
}

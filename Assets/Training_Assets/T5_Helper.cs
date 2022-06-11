using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T5_Helper : MonoBehaviour
{
    public void UpdateColourEvent(CubeColorController.CubeChangeArgs args)
    {
        T5_Manager t5m = FindObjectOfType<T5_Manager>();
        if(args.oldColour.Equals(CubeColorController.GemColour.Blank))
        {
            t5m.firstColour = args.newColour.ToString();
        }
        t5m.OnActivate();
    }
}

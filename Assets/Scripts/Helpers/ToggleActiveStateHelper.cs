using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveStateHelper : MonoBehaviour
{
    [SerializeField] GameObject obj;
    public void ToggleActive()
    {
        if(obj)
            obj.SetActive(!obj.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(XRGrabRotatable))]
public class FlameDialReceiver : MonoBehaviour, IDial
{
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 28f;
    [SerializeField] private float dialMax = 360f;
    [SerializeField] private string floatName;
    // Good value is 16f for flamerate;

    [SerializeField] public VisualEffect vfx;


    private void Start()
    {
        if (vfx && !floatName.Equals(null))
        {
            vfx.SetFloat(floatName, 0);
        }
    }

    public void DialUpdate(float value)
    {
        if(vfx && !floatName.Equals(null))
        {
            float percent = value / dialMax;
            float val = ((max - min) * percent) + min;
            vfx.SetFloat(floatName, val);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRQuantitySocket : XRPropertySocket
{
    [SerializeField] private GameObject containedObject = null;
    private GameObject lastObject = null;
    [SerializeField] private int count = 0;
    [SerializeField] private bool isInfinite = false;
    [SerializeField] private bool RandomizeWeight = false;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float RandomizePercentage = 0.10f;
    protected override void Awake()
    {
        base.Awake();
        if (isInfinite)
        {
            count = int.MaxValue;
        }

        /*if (this.startingSelectedInteractable)
        {
            containedObject = this.startingSelectedInteractable.gameObject;
        }
        else*/
        if (containedObject)
        {
            GameObject go = Instantiate(containedObject, this.attachTransform.position, this.attachTransform.rotation);
            if (RandomizeWeight)
            {
                string weight = go.GetComponent<ObjectProperties>().GetProperty("weight");
                if (weight != null && !weight.Equals(""))
                {
                    float newVal = float.Parse(weight);
                    newVal = Random.Range(newVal - newVal * RandomizePercentage, newVal + newVal * RandomizePercentage);
                    go.GetComponent<ObjectProperties>().AddOrSet("weight", newVal.ToString("F2"));
                }
            }
            interactionManager.ForceSelect(this, go.GetComponent<XRBaseInteractable>());
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectExited.AddListener(OnGrab);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectExited.RemoveListener(OnGrab);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        return;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        return;
    }

    public int GetCount()
    {
        if (isInfinite)
        {
            return int.MaxValue;
        }
        return count;
    }

    public bool HasObject()
    {
        return (containedObject != null); //this.selectTarget != null; 
    }

    public void AddCount(int num)
    {
        if (isInfinite)
        {
            return;
        }

        if (num <= 0)
        {
            return;
        }

        if (GetCount() == 0 && lastObject != null)
        {
            containedObject = lastObject;
            lastObject = null;
            GameObject go = Instantiate(containedObject, this.attachTransform.position, this.attachTransform.rotation);
            interactionManager.ForceSelect(this, go.GetComponent<XRBaseInteractable>());
        }

        count += num;
    }

    public bool SetObject(GameObject obj, int count = 1)
    {
        // only overwwrite when empty.
        // do NOT set last object. This will overrwrite the entire socket node.
        if (containedObject != null)
        {
            return false;
        }
        if (obj.TryGetComponent<XRBaseInteractable>(out XRBaseInteractable interactable) && (count > 0 || isInfinite))
        {
            this.count = isInfinite ? int.MaxValue : count;
            containedObject = obj;
            GameObject go = Instantiate(containedObject, this.attachTransform.position, this.attachTransform.rotation);
            interactionManager.ForceSelect(this, go.GetComponent<XRBaseInteractable>());
            return true;
        }
        return false;
    }

    private void OnGrab(SelectExitEventArgs args)
    {
        if (!this.socketActive)
        {
            return;
        }
        if ((isInfinite || GetCount() > 1) && this.selectTarget == null && containedObject != null)
        // 1 as it will spawn with the object first
        {
            GameObject go = Instantiate(containedObject, this.attachTransform.position, this.attachTransform.rotation);
            interactionManager.ForceSelect(this, go.GetComponent<XRBaseInteractable>());
            if(!isInfinite)
                count -= 1;
            if(RandomizeWeight)
            {
                string weight = go.GetComponent<ObjectProperties>().GetProperty("weight");
                if(weight != null && !weight.Equals(""))
                {
                    float newVal = float.Parse(weight);
                    newVal = Random.Range(newVal - newVal * RandomizePercentage, newVal + newVal * RandomizePercentage);
                    go.GetComponent<ObjectProperties>().AddOrSet("weight", newVal.ToString("F2"));
                }
            }
        }
        else if (GetCount() == 1 && containedObject != null && this.selectTarget == null)
        {
            if (!isInfinite)
                count -= 1;
        }

        if (GetCount() == 0)
        {
            lastObject = containedObject;
            containedObject = null;
        }
    }

    protected override void OnDestroy()
    {
        selectExited.RemoveListener(OnGrab);
        this.socketActive = false;
        if (this.selectTarget)
        {
            this.socketActive = false;
            Destroy(this.selectTarget.gameObject);
        }
        this.socketActive = false;
        base.OnDestroy();
    }

}

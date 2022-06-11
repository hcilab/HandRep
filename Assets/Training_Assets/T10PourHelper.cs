using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T10PourHelper : MonoBehaviour
{
    [SerializeField] GameObject contents;
    [SerializeField] GameObject pourer;
    public void OnPourStart()
    {
        //T10_Manager t10m = FindObjectOfType<T10_Manager>
        T10SummaryLog t10s = FindObjectOfType<T10SummaryLog>();
        if(t10s != null)
        {
            t10s.numTimesPoured += 1;
        }
    }

    public void DisablePouring()
    {
        contents.SetActive(false);
        pourer.SetActive(false);
    }

    public void DestroyOnLetGo(SelectExitEventArgs args)
    {
        if(args.interactor is XRDirectInteractor)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}

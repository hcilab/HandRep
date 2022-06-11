using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableDestroyHelper : MonoBehaviour
{

    public void DestroyMe(GameObject obj)
    {
        Destroy(obj);
    }

    public void DestroyMe(BaseInteractionEventArgs args)
    {
        DestroyMe(args.interactable.gameObject);
    }
}

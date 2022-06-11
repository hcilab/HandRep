using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class PourDetect : MonoBehaviour
{
    public int threshold = 46;
    public Transform origin = null;
    public GameObject pourStreamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    [SerializeField] private XRSocketInteractor corkSocket;

    // need to fix the direction angle thing as it sometimes just doesn't work
    // ^ may be fine
    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < threshold;
        pourCheck = pourCheck && CheckCork();

        if (currentStream && currentStream.CheckPoints() == 0)
        {
            isPouring = false;
        }
        if(isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring)
                StartPour();
            else 
                EndPour();
        }
    }

    private bool CheckCork()
    {
        if(!corkSocket)
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
    private void StartPour()
    {
        print("starting pour");
        if(!currentStream)
            currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        print("ending pour");
        currentStream.End();
        //currentStream = null;
    }

    private float CalculatePourAngle()
    {
        return transform.forward.z * Mathf.Rad2Deg;

    }

    private Stream CreateStream()
    {
        GameObject streamObj = Instantiate(pourStreamPrefab, origin.position,
            Quaternion.identity, transform);
        return streamObj.GetComponent<Stream>();
    }
}

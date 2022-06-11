using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BellMover : MonoBehaviour
{

    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;
    [SerializeField] private GameObject startBell;


    // Update is called once per frame
    void Update()
    {
        if(startBell != null)// && startBell.activeInHierarchy)
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            // Tell participant to drop anything/everything
            MoveBell(leftPos);
        }
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            // Tell participant to drop anything/everything
            MoveBell(rightPos);
        }
    }

    private void MoveBell(Transform pos)
    {
        if(startBell != null && pos != null)
        {
            startBell.transform.position = pos.position;
        }
    }


}

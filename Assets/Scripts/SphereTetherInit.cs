using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTetherInit : MonoBehaviour
{
    private LineRenderer lineRenderer => GetComponent<LineRenderer>();

    private void Awake()
    {
        lineRenderer.positionCount = 0;
    }
}

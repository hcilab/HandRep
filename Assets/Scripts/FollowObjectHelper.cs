using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class FollowObjectHelper : MonoBehaviour
{
    public Transform target;
    //private Rigidbody rb => GetComponent<Rigidbody>();

    public bool followPosition = true;
    public bool followRotation = true;
    public bool ignoreY = false;

    private void FixedUpdate()
    {
        if (followPosition)
        {
            Vector3 pos = target.position;
            if (ignoreY)
                pos.y = transform.position.y;

            transform.position = pos;
        }
        if (followRotation) transform.rotation = target.rotation;
        //rb.MovePosition(target.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
public class ConfigJointHelper : MonoBehaviour
{
    Rigidbody rb => GetComponent<Rigidbody>();
    // Start is called before the first frame update
    void Start()
    {
        rb.WakeUp();
    }

    // Update is called once per frame
    void Update()
    {
        rb.WakeUp();
    }
}

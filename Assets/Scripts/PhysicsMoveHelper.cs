using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsMoveHelper : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    public void MoveTo(Vector3 pos) => rb.MovePosition(pos);
}

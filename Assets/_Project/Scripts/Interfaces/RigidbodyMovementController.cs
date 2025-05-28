using UnityEngine;
using IAEngine.Interfaces;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovementController : MonoBehaviour, IMovementController
{
    public Rigidbody rb;

    public void Move(Vector3 direction, float speed)
    {
        if (rb == null) return;
        rb.MovePosition(rb.position + direction.normalized * speed * Time.deltaTime);
    }

    public void LookAt(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
        }
    }
}

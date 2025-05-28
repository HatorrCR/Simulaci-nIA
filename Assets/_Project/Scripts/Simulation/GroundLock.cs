using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundLock : MonoBehaviour
{
    public float fixedY = 0f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }
}

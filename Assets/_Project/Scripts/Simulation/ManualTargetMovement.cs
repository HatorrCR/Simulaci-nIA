using IAEngine.Movement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ManualTargetMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Vector3 lastDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

    }

    void FixedUpdate()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) h = 1f;
        if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) v = -1f;

        Vector3 move = new Vector3(h, 0, v).normalized * speed * Time.fixedDeltaTime;

        if (move != Vector3.zero)
        {
            rb.MovePosition(rb.position + move);
            lastDirection = move.normalized;
        }

        if (rb.velocity.magnitude > 0.01f)
        {
            var kinematic = GetComponent<Kinematic>();
            if (kinematic != null)
                kinematic.velocity = rb.velocity;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, lastDirection * 2f);
    }
}
using UnityEngine;

namespace IAEngine.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Kinematic : MonoBehaviour
    {
        public Vector3 velocity;
        public float rotation;
        public float maxSpeed = 5f;
        public float maxRotation = 180f;

        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
        }

        public void ApplySteering(SteeringOutput steering, float deltaTime)
        {
            if (steering == null) return;

            velocity += steering.linear * deltaTime;
            rotation += steering.angular * deltaTime;

            if (velocity.magnitude > maxSpeed)
                velocity = velocity.normalized * maxSpeed;

            if (Mathf.Abs(rotation) > maxRotation)
                rotation = Mathf.Sign(rotation) * maxRotation;

            rb.MovePosition(rb.position + velocity * deltaTime);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation * deltaTime, 0f));
        }
    }
}

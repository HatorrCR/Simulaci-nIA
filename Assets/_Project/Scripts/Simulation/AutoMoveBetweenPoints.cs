using UnityEngine;

namespace IAEngine.Movement
{
    public class AutoMoveBetween : MonoBehaviour
    {
        public Transform[] points;
        public float acceleration = 5f;
        public float maxSpeed = 10f;
        public float stopDistance = 0.1f;

        private int currentIndex = 0;
        private Vector3 velocity;

        void Update()
        {
            if (points == null || points.Length < 2) return;

            Transform target = points[currentIndex];
            Vector3 direction = (target.position - transform.position);
            Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);

            if (flatDirection.magnitude < stopDistance)
            {
                currentIndex = (currentIndex + 1) % points.Length;
                return;
            }

            Vector3 desiredVelocity = flatDirection.normalized * maxSpeed;
            Vector3 deltaVelocity = desiredVelocity - velocity;
            Vector3 accelerationStep = deltaVelocity.normalized * acceleration * Time.deltaTime;

            // Limit acceleration magnitude
            if (accelerationStep.magnitude > deltaVelocity.magnitude)
                accelerationStep = deltaVelocity;

            velocity += accelerationStep;

            transform.position += velocity * Time.deltaTime;
        }
    }
}

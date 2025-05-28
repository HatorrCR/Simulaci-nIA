using UnityEngine;

namespace IAEngine.Movement
{
    public class Arrive : MonoBehaviour
    {
        public Kinematic agent;
        public Transform target;
        public float maxSpeed = 5f;
        public float maxAcceleration = 10f;
        public float targetRadius = 0.5f;
        public float slowRadius = 2f;
        public float timeToTarget = 0.1f;

        public SteeringOutput GetSteering()
        {
            SteeringOutput steering = new SteeringOutput();

            Vector3 direction = target.position - agent.transform.position;
            float distance = direction.magnitude;

            if (distance < targetRadius)
                return null;

            float targetSpeed = (distance > slowRadius) ? maxSpeed : maxSpeed * (distance / slowRadius);

            Vector3 desiredVelocity = direction.normalized * targetSpeed;
            steering.linear = (desiredVelocity - agent.velocity) / timeToTarget;

            if (steering.linear.magnitude > maxAcceleration)
                steering.linear = steering.linear.normalized * maxAcceleration;

            steering.angular = 0f;
            return steering;
        }
    }
}

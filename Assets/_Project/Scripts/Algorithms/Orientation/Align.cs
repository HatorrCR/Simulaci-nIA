using UnityEngine;

namespace IAEngine.Movement
{
    public class Align : MonoBehaviour
    {
        public Kinematic agent;
        public Transform target;

        public float maxAngularAcceleration = 360f;
        public float maxRotation = 360f;
        public float targetRadius = 0.5f;
        public float slowRadius = 6f;
        public float timeToTarget = 0.05f;


        public SteeringOutput GetSteering()
        {
            SteeringOutput steering = new SteeringOutput();

            float rotation = Mathf.DeltaAngle(agent.transform.eulerAngles.y, target.eulerAngles.y);
            float rotationSize = Mathf.Abs(rotation);

            if (rotationSize < targetRadius)
                return null;

            float targetRotation = (rotationSize > slowRadius)
                ? maxRotation
                : maxRotation * rotationSize / slowRadius;

            targetRotation *= Mathf.Sign(rotation);

            steering.angular = (targetRotation - agent.rotation) / timeToTarget;

            if (Mathf.Abs(steering.angular) > maxAngularAcceleration)
                steering.angular = maxAngularAcceleration * Mathf.Sign(steering.angular);

            steering.linear = Vector3.zero;
            return steering;
        }
    }
}

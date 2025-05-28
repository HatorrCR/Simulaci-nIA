using UnityEngine;

namespace IAEngine.Movement
{
    public class Seek : MonoBehaviour
    {
        public Kinematic agent;
        public Transform target;
        public float maxAcceleration = 10f;

        public SteeringOutput GetSteering()
        {
            SteeringOutput steering = new SteeringOutput();

            Vector3 direction = target.position - agent.transform.position;
            direction.Normalize();
            steering.linear = direction * maxAcceleration;
            steering.angular = 0f;

            return steering;
        }
    }
}

using UnityEngine;

namespace IAEngine.Movement
{
    public class Evade : MonoBehaviour
    {
        public Kinematic agent;
        public Kinematic target;
        public float maxAcceleration = 10f;
        public float maxPrediction = 1f;

        public SteeringOutput GetSteering()
        {
            Vector3 direction = target.transform.position - agent.transform.position;
            float distance = direction.magnitude;

            float speed = agent.velocity.magnitude;
            float prediction = (speed <= distance / maxPrediction) ? maxPrediction : distance / speed;

            Vector3 predictedTarget = target.transform.position + target.velocity * prediction;

            SteeringOutput steering = new SteeringOutput();
            steering.linear = (agent.transform.position - predictedTarget).normalized * maxAcceleration;
            steering.angular = 0f;

            return steering;
        }
    }
}

using UnityEngine;

namespace IAEngine.Movement
{
    public class Wander : MonoBehaviour
    {
        public Kinematic agent;
        public float wanderOffset = 1.5f;
        public float wanderRadius = 4f;
        public float wanderRate = 45f; // grados por segundo
        public float maxAcceleration = 10f;

        private float wanderOrientation = 0f;

        private float RandomBinomial()
        {
            return Random.value - Random.value;
        }

        public SteeringOutput GetSteering()
        {
            wanderOrientation += RandomBinomial() * wanderRate;

            float agentOrientation = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float targetOrientation = wanderOrientation + agentOrientation;

            Vector3 orientationVector = new Vector3(Mathf.Sin(targetOrientation * Mathf.Deg2Rad), 0f, Mathf.Cos(targetOrientation * Mathf.Deg2Rad));

            Vector3 center = agent.transform.position;
            if (agent.velocity.magnitude > 0.1f)
                center += agent.velocity.normalized * wanderOffset;
            else
                center += agent.transform.forward * wanderOffset;

            Vector3 targetPosition = center + orientationVector * wanderRadius;

            SteeringOutput steering = new SteeringOutput();
            steering.linear = (targetPosition - agent.transform.position).normalized * maxAcceleration;
            steering.angular = 0f;

            return steering;
        }

        private void OnDrawGizmos()
        {
            if (agent == null) return;

            Vector3 center = agent.transform.position;
            if (agent.velocity.magnitude > 0.1f)
                center += agent.velocity.normalized * wanderOffset;
            else
                center += agent.transform.forward * wanderOffset;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, wanderRadius);

            float agentOrientation = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float targetOrientation = wanderOrientation + agentOrientation;
            Vector3 orientationVector = new Vector3(Mathf.Sin(targetOrientation * Mathf.Deg2Rad), 0f, Mathf.Cos(targetOrientation * Mathf.Deg2Rad));
            Vector3 target = center + orientationVector * wanderRadius;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.2f);
        }
    }
}
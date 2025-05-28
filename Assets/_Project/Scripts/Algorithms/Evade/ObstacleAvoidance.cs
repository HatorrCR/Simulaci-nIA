using UnityEngine;

namespace IAEngine.Movement
{
    public class ObstacleAvoidance : MonoBehaviour
    {
        public Kinematic agent;
        public Transform target;

        public float maxAcceleration = 4f;
        public float rayLength = 9f;
        public float sideStepDistance = 6f;
        public float tolerance = 0.5f;
        public LayerMask obstacleLayers;

        private Vector3 currentAvoidTarget;
        private bool avoiding = false;

        public SteeringOutput GetSteering()
        {
            if (agent == null || target == null)
                return null;

            Vector3 agentPos = agent.transform.position;
            Vector3 toTarget = (target.position - agentPos).normalized;

            if (Physics.Raycast(agentPos, toTarget, rayLength, obstacleLayers))
            {
                if (!avoiding)
                {
                    avoiding = true;

                    // Explora lados
                    Vector3 right = Quaternion.Euler(0, 45f, 0) * toTarget;
                    Vector3 left = Quaternion.Euler(0, -45f, 0) * toTarget;

                    Vector3 rightTest = agentPos + right * sideStepDistance;
                    Vector3 leftTest = agentPos + left * sideStepDistance;

                    bool rightClear = !Physics.Raycast(agentPos, right, rayLength, obstacleLayers);
                    bool leftClear = !Physics.Raycast(agentPos, left, rayLength, obstacleLayers);

                    if (rightClear && leftClear)
                        currentAvoidTarget = (Vector3.Distance(rightTest, target.position) < Vector3.Distance(leftTest, target.position)) ? rightTest : leftTest;
                    else if (rightClear)
                        currentAvoidTarget = rightTest;
                    else if (leftClear)
                        currentAvoidTarget = leftTest;
                    else
                        currentAvoidTarget = agentPos - toTarget * sideStepDistance;
                }

                Vector3 dir = (currentAvoidTarget - agentPos).normalized;
                return new SteeringOutput { linear = dir * maxAcceleration, angular = 0f };
            }

            if (avoiding)
            {
                if (!Physics.Raycast(agentPos, toTarget, rayLength, obstacleLayers))
                    avoiding = false;
                else
                    return new SteeringOutput { linear = (currentAvoidTarget - agentPos).normalized * maxAcceleration };
            }

            return new SteeringOutput { linear = toTarget * maxAcceleration, angular = 0f };
        }
    }
}

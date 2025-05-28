using UnityEngine;

namespace IAEngine.Movement
{
    public class CollisionAvoidance : MonoBehaviour
    {
        public Kinematic agent;
        public Transform target;

        public float maxAcceleration = 4f;
        public float rayLength = 10f;
        public float sideStepDistance = 6f;
        public float tolerance = 0.01f;
        public LayerMask detectionLayers;
        public string enemyTag = "Enemy";

        private Vector3 currentAvoidTarget;
        private bool avoiding = false;
        private bool triedLeft = false;
        private bool triedRight = false;
        private bool fallback = false;

        public SteeringOutput GetSteering()
        {
            if (agent == null || target == null)
                return null;

            Vector3 agentPos = agent.transform.position;
            Vector3 toTarget = (target.position - agentPos).normalized;

            RaycastHit hit;
            if (Physics.Raycast(agentPos, toTarget, out hit, rayLength, detectionLayers))
            {
                if (hit.collider.gameObject == agent.gameObject)
                    return null;

                if (!string.IsNullOrEmpty(enemyTag) && IsTagDefined(enemyTag) && hit.collider.CompareTag(enemyTag))
                {
                    if (!avoiding)
                    {
                        avoiding = true;
                        triedLeft = false;
                        triedRight = false;
                        fallback = false;
                    }

                    // Si el punto anterior sigue bloqueado, recalcular
                    if (Physics.Raycast(agentPos, (currentAvoidTarget - agentPos).normalized, rayLength, detectionLayers))
                    {
                        Vector3 right = Quaternion.Euler(0, 60f, 0) * toTarget;
                        Vector3 left = Quaternion.Euler(0, -60f, 0) * toTarget;

                        Vector3 rightTest = agentPos + right * sideStepDistance;
                        Vector3 leftTest = agentPos + left * sideStepDistance;

                        bool rightClear = !Physics.Raycast(agentPos, right, rayLength, detectionLayers);
                        bool leftClear = !Physics.Raycast(agentPos, left, rayLength, detectionLayers);

                        if (!triedRight && rightClear)
                        {
                            currentAvoidTarget = rightTest;
                            triedRight = true;
                        }
                        else if (!triedLeft && leftClear)
                        {
                            currentAvoidTarget = leftTest;
                            triedLeft = true;
                        }
                        else
                        {
                            fallback = true;
                        }
                    }

                    Vector3 direction;
                    if (fallback)
                    {
                        direction = -toTarget; // retroceder
                    }
                    else
                    {
                        direction = (currentAvoidTarget - agentPos).normalized;
                    }

                    return new SteeringOutput { linear = direction * maxAcceleration, angular = 0f };
                }
            }

            if (avoiding)
            {
                // salir del modo evasión si el camino está limpio
                if (!Physics.Raycast(agentPos, toTarget, rayLength, detectionLayers))
                {
                    avoiding = false;
                    triedLeft = triedRight = fallback = false;
                }
                else
                {
                    // continuar intentando evitar
                    Vector3 direction = (currentAvoidTarget - agentPos).normalized;
                    return new SteeringOutput { linear = direction * maxAcceleration, angular = 0f };
                }
            }

            return new SteeringOutput { linear = toTarget * maxAcceleration, angular = 0f };
        }

        private bool IsTagDefined(string tag)
        {
            try { GameObject.FindWithTag(tag); return true; }
            catch { return false; }
        }
    }
}

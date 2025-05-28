using UnityEngine;
using IAEngine.Shared;

namespace IAEngine.Communication
{
    public class AgentResponder : MonoBehaviour
    {
        [Header("Movimiento")]
        public float patrolSpeed = 3f;
        public float alertSpeed = 5f;
        public Transform[] patrolPoints;

        [Header("Comportamiento")]
        public bool respondToAlerts = true;
        public float waitAtAlertSeconds = 2f;

        private int patrolIndex = 0;
        private bool alertMode = false;
        private bool waiting = false;
        private float waitTimer = 0f;
        private Vector3 alertTarget;

        // Marca para evitar reaccionar varias veces a la misma alerta
        private Vector3 lastProcessedAlert = Vector3.positiveInfinity;

        void Update()
        {
            if (respondToAlerts &&
                Blackboard.Instance.HasKey("playerDetected") &&
                Blackboard.Instance.GetValue<bool>("playerDetected"))
            {
                Vector3 newAlert = Blackboard.Instance.GetValue<Vector3>("lastSeenPosition");

                // Solo reaccionamos si la alerta es nueva
                if (!alertMode && newAlert != lastProcessedAlert)
                {
                    alertTarget = newAlert;
                    alertMode = true;
                    waiting = false;
                    waitTimer = 0f;
                    lastProcessedAlert = newAlert;
                }
            }

            if (alertMode)
            {
                if (!waiting)
                {
                    MoveTowards(alertTarget, alertSpeed);

                    if (Vector3.Distance(transform.position, alertTarget) < 0.5f)
                    {
                        waiting = true;
                        waitTimer = waitAtAlertSeconds;
                    }
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                    if (waitTimer <= 0f)
                    {
                        alertMode = false;
                        waiting = false;
                    }
                }
            }
            else
            {
                Patrol();
            }
        }

        void Patrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return;

            Transform point = patrolPoints[patrolIndex];
            MoveTowards(point.position, patrolSpeed);

            if (Vector3.Distance(transform.position, point.position) < 0.5f)
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        void MoveTowards(Vector3 destination, float speed)
        {
            Vector3 dir = (destination - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
        }

        void OnDrawGizmos()
        {
            if (alertMode)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, alertTarget);
                Gizmos.DrawSphere(alertTarget, 0.2f);
            }
        }
    }
}

using UnityEngine;
using IAEngine.Shared;

namespace IAEngine.Communication
{
    public class AgentDetector : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;
        public float detectionRange = 8f;

        [Header("Movimiento")]
        public float patrolSpeed = 3f;
        public float alertSpeed = 5f;
        public Transform[] patrolPoints;

        [Header("Comportamiento")]
        public bool respondToPlayer = true;
        public float waitAtAlertSeconds = 2f;

        private int patrolIndex = 0;
        private bool alertMode = false;
        private bool waiting = false;
        private float waitTimer = 0f;
        private Vector3 lastAlertPosition;
        private Vector3 lastProcessedPosition = Vector3.positiveInfinity;

        void Update()
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance < detectionRange)
                {
                    Vector3 seenPosition = target.position;
                    Blackboard.Instance.SetValue("playerDetected", true);
                    Blackboard.Instance.SetValue("lastSeenPosition", seenPosition);

                    // Activar alerta solo si hay nueva posición y el detector está activo
                    if (respondToPlayer && seenPosition != lastProcessedPosition)
                    {
                        lastAlertPosition = seenPosition;
                        alertMode = true;
                        waiting = false;
                        waitTimer = 0f;
                        lastProcessedPosition = seenPosition;
                    }
                }
            }

            if (alertMode)
            {
                if (!waiting)
                {
                    MoveTowards(lastAlertPosition, alertSpeed);

                    if (Vector3.Distance(transform.position, lastAlertPosition) < 0.5f)
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

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}

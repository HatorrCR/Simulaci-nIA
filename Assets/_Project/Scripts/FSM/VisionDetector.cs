using UnityEngine;

namespace IAEngine.FSM
{
    public class VisionDetector : MonoBehaviour
    {
        public Transform target;
        public FSMController controller;
        public FSMState detectedState;

        [Header("Detección visual")]
        public float detectionRange = 10f;
        public float fieldOfView = 90f;
        public LayerMask obstacleMask;

        void Update()
        {
            if (target == null || controller == null || detectedState == null)
                return;

            Vector3 directionToTarget = target.position - transform.position;
            float distance = directionToTarget.magnitude;

            if (distance > detectionRange) return;

            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle < fieldOfView / 2f)
            {
                // comprobar si hay obstáculos
                if (!Physics.Raycast(transform.position, directionToTarget.normalized, distance, obstacleMask))
                {
                    controller.ChangeState(detectedState);
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Vector3 forward = transform.forward * detectionRange;
            Quaternion leftRayRotation = Quaternion.Euler(0, -fieldOfView / 2f, 0);
            Quaternion rightRayRotation = Quaternion.Euler(0, fieldOfView / 2f, 0);

            Vector3 leftRayDirection = leftRayRotation * forward;
            Vector3 rightRayDirection = rightRayRotation * forward;

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, leftRayDirection);
            Gizmos.DrawRay(transform.position, rightRayDirection);
        }
    }
}

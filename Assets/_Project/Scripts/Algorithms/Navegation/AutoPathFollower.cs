// Nuevo sistema de movimiento por Path independiente del AgentController
using UnityEngine;
using System.Collections.Generic;

namespace IAEngine.Movement
{
    public class AutoPathFollower : MonoBehaviour
    {
        public Path path;
        public float speed = 5f;
        public float waypointTolerance = 0.5f;
        public float waitTimeAtWaypoint = 0.5f;
        public bool loop = true;

        private int currentIndex = 0;
        private float waitTimer = 0f;
        private bool waiting = false;

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;

            if (path != null && path.waypoints.Count > 0)
            {
                currentIndex = path.GetClosestIndex(transform.position, 0);
            }
        }

        void FixedUpdate()
        {
            if (path == null || path.waypoints.Count == 0) return;

            Vector3 targetPos = path.waypoints[currentIndex].position;
            float distance = Vector3.Distance(transform.position, targetPos);

            if (distance <= waypointTolerance)
            {
                if (!waiting)
                {
                    waiting = true;
                    waitTimer = waitTimeAtWaypoint;
                    return;
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                    if (waitTimer <= 0f)
                    {
                        currentIndex = path.GetNextIndex(currentIndex);
                        waiting = false;
                    }
                    return;
                }
            }

            Vector3 direction = (targetPos - transform.position).normalized;
            Vector3 move = direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }
}

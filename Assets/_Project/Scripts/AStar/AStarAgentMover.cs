using System.Collections.Generic;
using UnityEngine;

namespace IAEngine.Pathfinding
{
    [RequireComponent(typeof(Rigidbody))]
    public class AStarAgentMover : MonoBehaviour
    {
        public Transform goal;
        public AStarPathfinder pathfinder;
        public float speed = 5f;
        public float stopDistance = 0.1f;
        public float repathInterval = 1f;
        public float turnSpeed = 10f;

        private List<Vector3> path;
        private int currentIndex = 0;
        private float repathTimer = 0f;
        private Vector3 lastGoalPos;

        void Start()
        {
            if (pathfinder != null && goal != null)
            {
                lastGoalPos = goal.position;
                path = pathfinder.FindPath(transform.position, goal.position);
                currentIndex = 0;
            }
        }

        void Update()
        {
            if (goal == null || pathfinder == null) return;

            repathTimer += Time.deltaTime;
            if (repathTimer >= repathInterval && Vector3.Distance(goal.position, lastGoalPos) > stopDistance)
            {
                path = pathfinder.FindPath(transform.position, goal.position);
                currentIndex = 0;
                lastGoalPos = goal.position;
                repathTimer = 0f;
            }

            if (path == null || path.Count == 0 || currentIndex >= path.Count)
                return;

            Vector3 targetPos = path[currentIndex];
            Vector3 direction = (targetPos - transform.position).normalized;

            // Rotación suave hacia el siguiente punto
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            }

            // Movimiento
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPos) < stopDistance)
                currentIndex++;
        }

        void OnDrawGizmos()
        {
            if (path == null) return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}

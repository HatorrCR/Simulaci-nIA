using System.Collections.Generic;
using UnityEngine;

namespace IAEngine.Movement
{
    public class Path : MonoBehaviour
    {
        public List<Transform> waypoints = new List<Transform>();
        public bool loop = true;

        public int GetClosestIndex(Vector3 position, int lastIndex)
        {
            int closest = lastIndex;
            float minDist = Vector3.Distance(position, waypoints[lastIndex].position);

            for (int i = 0; i < waypoints.Count; i++)
            {
                float dist = Vector3.Distance(position, waypoints[i].position);
                if (dist < minDist)
                {
                    closest = i;
                    minDist = dist;
                }
            }
            return closest;
        }

        public int GetNextIndex(int currentIndex)
        {
            if (waypoints.Count == 0) return 0;

            if (loop)
                return (currentIndex + 1) % waypoints.Count;
            else if (currentIndex < waypoints.Count - 1)
                return currentIndex + 1;
            else
                return currentIndex; // Stop at last point if not looping
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Count < 2)
                return;

            Gizmos.color = Color.magenta;
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                if (waypoints[i] != null && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    Gizmos.DrawSphere(waypoints[i].position, 0.2f);
                }
            }

            if (waypoints[waypoints.Count - 1] != null)
                Gizmos.DrawSphere(waypoints[waypoints.Count - 1].position, 0.2f);

            if (loop && waypoints[0] != null && waypoints[waypoints.Count - 1] != null)
                Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }
}

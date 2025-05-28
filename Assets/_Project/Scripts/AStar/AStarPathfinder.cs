using System.Collections.Generic;
using UnityEngine;

namespace IAEngine.Pathfinding
{
    public class AStarPathfinder : MonoBehaviour
    {
        public LayerMask obstacleMask;
        public float nodeSpacing = 1f;
        public Vector3 gridOrigin;
        public Vector2 gridSize;

        private List<AStarNode> openList = new();
        private HashSet<AStarNode> closedList = new();

        public List<Vector3> FindPath(Vector3 start, Vector3 goal)
        {
            openList.Clear();
            closedList.Clear();

            AStarNode startNode = new(start);
            AStarNode goalNode = new(goal);

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                openList.Sort((a, b) => a.fCost.CompareTo(b.fCost));
                AStarNode current = openList[0];

                if (Vector3.Distance(current.position, goal) < nodeSpacing)
                    return ReconstructPath(current);

                openList.Remove(current);
                closedList.Add(current);

                foreach (AStarNode neighbor in GetNeighbors(current, goal))
                {
                    if (closedList.Contains(neighbor)) continue;

                    float tentativeG = current.gCost + Vector3.Distance(current.position, neighbor.position);
                    bool inOpen = openList.Exists(n => Vector3.Distance(n.position, neighbor.position) < 0.01f);

                    if (!inOpen || tentativeG < neighbor.gCost)
                    {
                        neighbor.gCost = tentativeG;
                        neighbor.hCost = Vector3.Distance(neighbor.position, goal);
                        neighbor.parent = current;

                        if (!inOpen) openList.Add(neighbor);
                    }
                }
            }

            return null;
        }

        List<AStarNode> GetNeighbors(AStarNode node, Vector3 goal)
        {
            List<AStarNode> neighbors = new();
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

            foreach (var dir in directions)
            {
                Vector3 pos = node.position + dir * nodeSpacing;
                if (!Physics.CheckSphere(pos, nodeSpacing * 0.4f, obstacleMask))
                    neighbors.Add(new AStarNode(pos));
            }

            return neighbors;
        }

        List<Vector3> ReconstructPath(AStarNode endNode)
        {
            List<Vector3> path = new();
            AStarNode current = endNode;

            while (current != null)
            {
                path.Add(current.position);
                current = current.parent;
            }

            path.Reverse();
            return path;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for (float x = 0; x < gridSize.x; x += nodeSpacing)
                for (float z = 0; z < gridSize.y; z += nodeSpacing)
                {
                    Vector3 pos = gridOrigin + new Vector3(x, 0, z);
                    if (!Physics.CheckSphere(pos, nodeSpacing * 0.4f, obstacleMask))
                        Gizmos.DrawWireSphere(pos, 0.1f);
                }
        }
    }
}

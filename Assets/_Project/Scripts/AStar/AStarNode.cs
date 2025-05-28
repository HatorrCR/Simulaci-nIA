using UnityEngine;

namespace IAEngine.Pathfinding
{
    public class AStarNode
    {
        public Vector3 position;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        public AStarNode parent;

        public AStarNode(Vector3 pos)
        {
            position = pos;
            gCost = 0f;
            hCost = 0f;
            parent = null;
        }
    }
}

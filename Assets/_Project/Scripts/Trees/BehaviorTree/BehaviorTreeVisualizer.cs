using UnityEngine;
using IAEngine.BehaviorTree;
using System.Collections.Generic;

namespace IAEngine.BehaviorTree
{
    [RequireComponent(typeof(BehaviorTreeAgent))]
    public class BehaviorTreeVisualizer : MonoBehaviour
    {
        public float nodeSpacing = 2f;
        public float verticalSpacing = 1.5f;
        public Vector3 offset = new Vector3(2, 0, 0);

        private BehaviorTreeAgent agent;
        private Dictionary<BTNode, Vector3> nodePositions = new();
        private Dictionary<BTNode, BTStatus> nodeStates = new();

        void OnDrawGizmos()
        {
            agent = GetComponent<BehaviorTreeAgent>();
            if (agent == null || agent.GetRoot() == null) return;

            nodePositions.Clear();
            nodeStates.Clear();
            Vector3 startPos = transform.position + offset;
            SimulateTree(agent.GetRoot());
            DrawNode(agent.GetRoot(), startPos, 0);
        }

        void SimulateTree(BTNode node)
        {
            if (node == null) return;

            BTStatus result = node.Tick();
            nodeStates[node] = result;

            if (node is BTSequence seq)
                foreach (BTNode child in GetPrivateChildren(seq))
                    SimulateTree(child);

            if (node is BTSelector sel)
                foreach (BTNode child in GetPrivateChildren(sel))
                    SimulateTree(child);
        }

        void DrawNode(BTNode node, Vector3 position, int depth)
        {
            if (node == null) return;

            nodePositions[node] = position;

            BTStatus status = nodeStates.ContainsKey(node) ? nodeStates[node] : BTStatus.Failure;

            Gizmos.color = status switch
            {
                BTStatus.Success => Color.green,
                BTStatus.Failure => Color.red,
                BTStatus.Running => Color.yellow,
                _ => Color.gray
            };

            Gizmos.DrawSphere(position, 0.2f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(position + Vector3.up * 0.3f, node.GetType().Name);
#endif

            if (node is BTSequence seq)
                DrawChildren(seq, position, depth);
            else if (node is BTSelector sel)
                DrawChildren(sel, position, depth);
        }

        void DrawChildren(BTNode parent, Vector3 parentPos, int depth)
        {
            List<BTNode> children = new();

            if (parent is BTSequence seq)
                children = GetPrivateChildren(seq);
            else if (parent is BTSelector sel)
                children = GetPrivateChildren(sel);

            for (int i = 0; i < children.Count; i++)
            {
                Vector3 childPos = parentPos + new Vector3((i - children.Count / 2f) * nodeSpacing, -verticalSpacing, 0);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(parentPos, childPos);
                DrawNode(children[i], childPos, depth + 1);
            }
        }

        List<BTNode> GetPrivateChildren(object composite)
        {
            var field = composite.GetType().GetField("children", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(composite) as List<BTNode> ?? new List<BTNode>();
        }
    }
}

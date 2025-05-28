using UnityEngine;
using IAEngine.DecisionTree;

namespace IAEngine.DecisionTree
{
    public class DecisionTreeAgent : MonoBehaviour
    {
        public Transform target;
        public float detectionRange = 10f;
        public float speed = 3f;
        public Transform[] patrolPoints;

        private int patrolIndex = 0;
        private DecisionNode rootNode;

        void Start()
        {
            ActionNode patrol = new ActionNode { action = Patrol };
            ActionNode chase = new ActionNode { action = Chase };

            ConditionNode seeTarget = new ConditionNode
            {
                condition = () => target != null && Vector3.Distance(transform.position, target.position) < detectionRange,
                trueNode = chase,
                falseNode = patrol
            };

            rootNode = seeTarget;
        }

        void Update()
        {
            DecisionNode resultNode = rootNode.MakeDecision();
            if (resultNode is ActionNode actionNode)
            {
                actionNode.action?.Invoke(); // ¡Aquí sí se ejecuta la acción!
            }
        }

        void Patrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return;

            Transform point = patrolPoints[patrolIndex];
            MoveTowards(point.position);

            if (Vector3.Distance(transform.position, point.position) < 0.5f)
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        void Chase()
        {
            if (target == null) return;
            MoveTowards(target.position);
        }

        void MoveTowards(Vector3 destination)
        {
            Vector3 direction = (destination - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
        }

        void OnDrawGizmosSelected()
        {
            if (target == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}

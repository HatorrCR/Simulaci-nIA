// BehaviorTreeAgent.cs
using UnityEngine;
using IAEngine.BehaviorTree;
using System.Collections.Generic;

namespace IAEngine.BehaviorTree
{
    public class BehaviorTreeAgent : MonoBehaviour
    {
        public Transform target;
        public float detectionRange = 5f;
        public float speed = 3f;
        public Transform[] patrolPoints;

        private int patrolIndex = 0;
        private BTNode root;

        void Start()
        {
            BTCondition seesTarget = new BTCondition(() =>
                Vector3.Distance(transform.position, target.position) < detectionRange
            );

            BTAction chase = new BTAction(() => Chase());
            BTAction patrol = new BTAction(() => Patrol());

            BTSequence chaseSequence = new BTSequence(new List<BTNode> { seesTarget, chase });
            root = new BTSelector(new List<BTNode> { chaseSequence, patrol });
        }

        void Update()
        {
            root?.Tick();
        }

        private BTStatus Patrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return BTStatus.Failure;

            Transform point = patrolPoints[patrolIndex];
            MoveTowards(point.position);

            if (Vector3.Distance(transform.position, point.position) < 0.5f)
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

            return BTStatus.Running;
        }

        public BTNode GetRoot()
        {
            return root;
        }

        private BTStatus Chase()
        {
            if (target == null) return BTStatus.Failure;
            MoveTowards(target.position);
            return BTStatus.Running;
        }

        private void MoveTowards(Vector3 destination)
        {
            Vector3 dir = (destination - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
        }

        void OnDrawGizmosSelected()
        {
            if (target == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}

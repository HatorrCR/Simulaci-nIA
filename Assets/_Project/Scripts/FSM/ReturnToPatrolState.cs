using UnityEngine;

namespace IAEngine.FSM
{
    public class ReturnToPatrolState : FSMState
    {
        public Transform[] patrolPoints;
        public float speed = 3f;
        public float stopDistance = 0.2f;

        public FSMState nextState;

        private int closestIndex = 0;

        public override void OnEnter()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                Debug.LogWarning("[ReturnToPatrolState] No patrol points assigned.");
                return;
            }

            float minDistance = float.MaxValue;
            Vector3 currentPos = transform.position;

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                float distance = Vector3.Distance(currentPos, patrolPoints[i].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }
        }

        public override void OnUpdate()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
                return;

            Vector3 target = patrolPoints[closestIndex].position;
            Vector3 direction = target - transform.position;
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);

            transform.position += flatDir.normalized * speed * Time.deltaTime;

            if (flatDir.magnitude < stopDistance && nextState != null)
            {
                controller.ChangeState(nextState);
            }
        }

        public override void OnExit() { }
    }
}

using UnityEngine;

namespace IAEngine.FSM
{
    public class PatrolState : FSMState
    {
        public Transform[] patrolPoints;
        public float speed = 3f;
        public float stopDistance = 0.2f;

        private int currentIndex = 0;

        public override void OnEnter()
        {
            if (patrolPoints.Length == 0)
                Debug.LogWarning("[PatrolState] No patrol points assigned.");
        }

        public override void OnUpdate()
        {
            if (patrolPoints.Length == 0) return;

            Vector3 target = patrolPoints[currentIndex].position;
            Vector3 direction = (target - transform.position);
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
            transform.position += flatDir.normalized * speed * Time.deltaTime;

            if (flatDir.magnitude < stopDistance)
                currentIndex = (currentIndex + 1) % patrolPoints.Length;
        }

        public override void OnExit() { }
    }
}

using UnityEngine;

namespace IAEngine.FSM
{
    public class ChaseState : FSMState
    {
        public Transform target;
        public float speed = 4.5f;
        public float stopDistance = 0.2f;
        public float loseDistance = 10f;
        public FSMState fallbackState;

        public override void OnUpdate()
        {
            if (target == null)
            {
                if (fallbackState != null)
                    controller.ChangeState(fallbackState);
                return;
            }

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > loseDistance)
            {
                Debug.Log("[ChaseState] Objetivo fuera de alcance, cambiando a estado fallback.");
                if (fallbackState != null)
                    controller.ChangeState(fallbackState);
                return;
            }

            Vector3 direction = (target.position - transform.position);
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);

            if (flatDir.magnitude > stopDistance)
            {
                transform.position += flatDir.normalized * speed * Time.deltaTime;
            }
        }

        public override void OnEnter() { }
        public override void OnExit() { }
    }
}

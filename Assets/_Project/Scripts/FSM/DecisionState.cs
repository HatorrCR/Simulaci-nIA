using UnityEngine;

namespace IAEngine.FSM
{
    public class DecisionState : FSMState
    {
        public Transform target;
        public float detectionRange = 10f;
        public FSMState chaseState;
        public FSMState patrolState;

        public override void OnUpdate()
        {
            if (target == null || controller == null)
                return;

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < detectionRange && chaseState != null)
            {
                controller.ChangeState(chaseState);
            }
            else if (patrolState != null)
            {
                controller.ChangeState(patrolState);
            }
        }

        public override void OnEnter() { }
        public override void OnExit() { }
    }
}

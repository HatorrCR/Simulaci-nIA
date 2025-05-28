using UnityEngine;

namespace IAEngine.FSM
{
    public class IdleState : FSMState
    {
        public float idleDuration = 2f;
        private float timer = 0f;

        public FSMState nextState;

        public override void OnEnter()
        {
            timer = 0f;
            Debug.Log("[IdleState] Entrando al estado idle.");
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;
            Debug.Log($"[IdleState] Tiempo acumulado: {timer:F2}");

            if (timer >= idleDuration && nextState != null)
            {
                Debug.Log("[IdleState] Tiempo cumplido. Cambiando al siguiente estado.");
                controller.ChangeState(nextState);
            }
        }
        public override void OnExit() { }
    }
}

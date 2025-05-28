using UnityEngine;

namespace IAEngine.FSM
{
    public class FSMController : MonoBehaviour
    {
        private FSMState currentState;

        void Update()
        {
            if (currentState != null)
                currentState.OnUpdate();
        }

        public void ChangeState(FSMState newState)
        {
            if (currentState != null)
                currentState.OnExit();

            currentState = newState;

            if (currentState != null)
            {
                currentState.Init(this);
                currentState.OnEnter();
            }
        }

        public FSMState GetCurrentState() => currentState;
    }
}

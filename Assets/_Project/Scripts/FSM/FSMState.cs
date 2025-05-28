using UnityEngine;

namespace IAEngine.FSM
{
    public abstract class FSMState : MonoBehaviour
    {
        protected FSMController controller;

        public void Init(FSMController controller)
        {
            this.controller = controller;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public abstract void OnUpdate();
    }
}

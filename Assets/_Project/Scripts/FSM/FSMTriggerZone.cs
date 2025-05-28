using UnityEngine;

namespace IAEngine.FSM
{
    public class FSMTriggerZone : MonoBehaviour
    {
        public string triggeringTag = "Player";
        public FSMController controller;
        public FSMState targetState;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(triggeringTag) && controller != null && targetState != null)
            {
                Debug.Log($"[FSMTriggerZone] Triggered by {other.name}, switching to {targetState.name}");
                controller.ChangeState(targetState);
            }
        }
    }
}

using UnityEngine;

namespace IAEngine.Movement
{
    public class LookWhereYoureGoing : MonoBehaviour
    {
        public Kinematic agent;
        public float threshold = 0.02f;
        public float rotationSpeed = 360f;

        private Vector3 lastPosition;

        void Start()
        {
            if (agent == null)
                agent = GetComponent<Kinematic>();

            lastPosition = agent.transform.position;
        }

        void LateUpdate()
        {
            Vector3 currentPosition = agent.transform.position;
            Vector3 delta = currentPosition - lastPosition;
            lastPosition = currentPosition;

            Vector3 flatDelta = new Vector3(delta.x, 0f, delta.z);

            if (flatDelta.magnitude < threshold)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(flatDelta.normalized, Vector3.up);
            agent.transform.rotation = Quaternion.RotateTowards(
                agent.transform.rotation,
                desiredRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}

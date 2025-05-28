using UnityEngine;

namespace IAEngine.Movement
{
    public class VelocityMatching : MonoBehaviour
    {
        public Kinematic agent;
        public Kinematic target;
        public float maxAcceleration = 10f;
        public float velocityTolerance = 0.001f;
        public float blendTime = 0.001f;
        public float seekWeight = 1f;
        public float matchWeight = 1f;
        public float minSeparation = 0.01f;

        private Vector3 lastTargetVelocity;
        private bool locked = false;

        public SteeringOutput GetSteering()
        {
            if (agent == null || target == null)
                return null;

            SteeringOutput result = new SteeringOutput();

            Vector3 deltaVelocity = target.velocity - agent.velocity;
            Vector3 acceleration = (target.velocity - lastTargetVelocity) / Time.deltaTime;
            bool targetAccelerating = acceleration.magnitude > velocityTolerance;

            if (targetAccelerating)
                locked = false;

            Vector3 velocityMatch = Vector3.zero;
            if (!locked && deltaVelocity.magnitude > velocityTolerance)
            {
                velocityMatch = deltaVelocity / blendTime;
                if (velocityMatch.magnitude > maxAcceleration)
                    velocityMatch = velocityMatch.normalized * maxAcceleration;
            }
            else
            {
                locked = true;
            }

            Vector3 toTarget = target.transform.position - agent.transform.position;
            float distance = toTarget.magnitude;

            // Frenado progresivo al acercarse
            float slowDownFactor = Mathf.Clamp01(distance / minSeparation);

            Vector3 seekAccel = toTarget.normalized * maxAcceleration * slowDownFactor;

            result.linear = seekWeight * seekAccel + matchWeight * velocityMatch;
            result.angular = 0f;

            lastTargetVelocity = target.velocity;
            return result;
        }

        void OnDrawGizmos()
        {
            if (agent == null) return;
            Gizmos.color = locked ? Color.green : Color.red;
            Gizmos.DrawSphere(agent.transform.position + Vector3.up * 1.5f, 0.2f);
        }
    }
}

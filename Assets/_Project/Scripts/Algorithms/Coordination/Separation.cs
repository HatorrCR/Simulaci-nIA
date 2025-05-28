using UnityEngine;
using System.Collections.Generic;

namespace IAEngine.Movement
{
    public class Separation : MonoBehaviour
    {
        public Kinematic agent;
        public List<Kinematic> targets;
        public float threshold = 5f;
        public float decayCoef = 10f;
        public float maxAcceleration = 10f;

        public SteeringOutput GetSteering()
        {
            SteeringOutput steering = new SteeringOutput();
            steering.linear = Vector3.zero;

            foreach (var t in targets)
            {
                Vector3 direction = agent.transform.position - t.transform.position;
                float distance = direction.magnitude;

                if (distance < threshold)
                {
                    float strength = Mathf.Min(decayCoef / (distance * distance), maxAcceleration);
                    steering.linear += direction.normalized * strength;
                }
            }

            steering.angular = 0f;
            return steering;
        }
    }
}

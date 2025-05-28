using UnityEngine;

namespace IAEngine.Movement
{
    public class Pursue : MonoBehaviour
    {
        public Kinematic agent;
        public Kinematic target;
        public float maxPrediction = 1f;
        public float pursueRange = 30f;
        public float loseRange = 12f;
        public float maxAcceleration = 5f;

        private bool isPursuing = false;

        public SteeringOutput GetSteering()
        {
            if (agent == null || target == null)
                return null;

            float distance = Vector3.Distance(agent.transform.position, target.transform.position);

            // Activar persecución si entra en rango
            if (!isPursuing && distance < pursueRange)
                isPursuing = true;

            // Cancelar persecución si se aleja demasiado
            if (isPursuing && distance > loseRange)
                isPursuing = false;

            if (!isPursuing)
                return new SteeringOutput { linear = Vector3.zero, angular = 0 };

            // Predicción temporal
            Vector3 direction = target.transform.position - agent.transform.position;
            float speed = agent.velocity.magnitude;
            float prediction = (speed <= direction.magnitude / maxPrediction) ? maxPrediction : direction.magnitude / speed;

            Vector3 predictedTarget = target.transform.position + target.velocity * prediction;
            Vector3 linear = (predictedTarget - agent.transform.position).normalized * maxAcceleration;

            return new SteeringOutput
            {
                linear = linear,
                angular = 0
            };
        }
    }
}

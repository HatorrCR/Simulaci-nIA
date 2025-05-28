using UnityEngine;

namespace IAEngine.Movement
{
    public class Face : Align
    {
        public Transform faceTarget;

        public new SteeringOutput GetSteering()
        {
            Vector3 direction = faceTarget.position - agent.transform.position;

            if (direction.magnitude < 0.001f) return null;

            float desiredOrientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            target.eulerAngles = new Vector3(0, desiredOrientation, 0);

            return base.GetSteering();
        }
    }
}

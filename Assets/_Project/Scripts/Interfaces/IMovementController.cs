using UnityEngine;
using IAEngine.Interfaces;


namespace IAEngine.Interfaces
{
    public interface IMovementController
    {
        void Move(UnityEngine.Vector3 direction, float speed);
        void LookAt(UnityEngine.Vector3 target);
    }
}

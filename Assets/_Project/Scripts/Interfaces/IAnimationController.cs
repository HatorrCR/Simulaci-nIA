using UnityEngine;
using IAEngine.Interfaces;


namespace IAEngine.Interfaces
{
    public interface IAnimationController
    {
        void SetMovementSpeed(float value);
        void PlayAlert();
        void PlayIdle();
    }
}

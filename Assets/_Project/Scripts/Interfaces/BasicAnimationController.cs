using UnityEngine;
using IAEngine.Interfaces;

public class BasicAnimationController : MonoBehaviour, IAnimationController
{
    public Animator animator;

    public void SetMovementSpeed(float value)
    {
        if (animator) animator.SetFloat("Speed", value);
    }

    public void PlayAlert()
    {
        if (animator) animator.SetTrigger("Alert");
    }

    public void PlayIdle()
    {
        if (animator) animator.SetTrigger("Idle");
    }
}

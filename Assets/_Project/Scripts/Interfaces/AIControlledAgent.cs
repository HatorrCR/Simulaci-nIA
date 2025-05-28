using UnityEngine;
using IAEngine.Interfaces;

public class AIControlledAgent : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;

    private IMovementController movement;
    private IAnimationController animationCtrl;

    void Awake()
    {
        movement = GetComponent<IMovementController>();
        animationCtrl = GetComponent<IAnimationController>();
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        movement.Move(dir, speed);
        movement.LookAt(target.position);

        animationCtrl?.SetMovementSpeed(dir.magnitude);
    }
}

using UnityEngine;

public class SmoothMovement : MovementStrategy
{
    public SmoothMovement(Transform transform, float moveSpeed) : base(transform, moveSpeed) {}

    public override void Move(Vector3 direction)
    {
        targetPosition = transform.position + direction;
        IsMoving = true;
    }

    public override void Stop()
    {
        IsMoving = false;
    }

    public void Update()
    {
        if (!IsMoving) return;
        
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime / Vector3.Distance(transform.position, targetPosition)
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Stop();
        }
    }
}

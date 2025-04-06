using UnityEngine;

public abstract class MovementStrategy : IMovable
{
    protected Transform transform;
    protected float moveSpeed;
    protected Vector3 targetPosition;
    public bool IsMoving { get; protected set; }

    public MovementStrategy(Transform transform, float moveSpeed)
    {
        this.transform = transform;
        this.moveSpeed = moveSpeed;
    }

    public abstract void Move(Vector3 direction);
    public abstract void Stop();
}
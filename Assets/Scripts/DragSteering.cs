using UnityEngine;

// Keeps the last swipe direction while held, and reverses on the next small swipe.
public sealed class DragSteering
{
    private Vector2 samplePosition;
    public Vector2 Direction { get; private set; }

    public void Begin(Vector2 position)
    {
        samplePosition = position;
        Direction = Vector2.zero;
    }

    public Vector2 Move(Vector2 position, float threshold)
    {
        Vector2 delta = position - samplePosition;
        if (delta.sqrMagnitude >= threshold * threshold)
        {
            Direction = delta.normalized;
            samplePosition = position;
        }
        return Direction;
    }

    public void End()
    {
        Direction = Vector2.zero;
    }
}

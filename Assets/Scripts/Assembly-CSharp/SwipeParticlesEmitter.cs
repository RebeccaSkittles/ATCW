using UnityEngine;

public class SwipeParticlesEmitter : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public float baseSpeed = 4f;

    public float swipeVelocityScale = 0.001f;

    private void Start()
    {
        if (!particleSystem)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        if (particleSystem)
        {
            var emission = particleSystem.emission;
            emission.enabled = false;
        }
    }

    public void Emit(Vector3 heading, float swipeVelocity)
    {
        if (particleSystem)
        {
            particleSystem.transform.rotation = Quaternion.LookRotation(heading);
            
            var main = particleSystem.main;
            var startSpeed = main.startSpeed;
            startSpeed.constant = baseSpeed * swipeVelocityScale * swipeVelocity;
            main.startSpeed = startSpeed;

            particleSystem.Emit(1);
        }
    }

    public static Vector3 GetSwipeDirectionVector(FingerGestures.SwipeDirection direction)
    {
        switch (direction)
        {
            case FingerGestures.SwipeDirection.Up:
                return Vector3.up;
            case FingerGestures.SwipeDirection.UpperRightDiagonal:
                return 0.5f * (Vector3.up + Vector3.right);
            case FingerGestures.SwipeDirection.Right:
                return Vector3.right;
            case FingerGestures.SwipeDirection.LowerRightDiagonal:
                return 0.5f * (Vector3.down + Vector3.right);
            case FingerGestures.SwipeDirection.Down:
                return Vector3.down;
            case FingerGestures.SwipeDirection.LowerLeftDiagonal:
                return 0.5f * (Vector3.down + Vector3.left);
            case FingerGestures.SwipeDirection.Left:
                return Vector3.left;
            case FingerGestures.SwipeDirection.UpperLeftDiagonal:
                return 0.5f * (Vector3.up + Vector3.left);
            default:
                return Vector3.zero;
        }
    }

    public void Emit(FingerGestures.SwipeDirection direction, float swipeVelocity)
    {
        Vector3 swipeDirectionVector = GetSwipeDirectionVector(direction);
        Emit(swipeDirectionVector, swipeVelocity);
    }
}

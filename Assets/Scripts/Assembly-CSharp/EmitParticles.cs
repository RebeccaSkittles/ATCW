using UnityEngine;

public class EmitParticles : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public Transform left;
    public Transform right;
    public Transform up;
    public Transform down;

    private void Start()
    {
        // If particleSystem is not assigned, try to get it from the current GameObject
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
    }

    public void Emit()
    {
        if (particleSystem != null)
        {
            particleSystem.Emit(1);
        }
    }

    public void Emit(Vector3 dir)
    {
        Emit(Quaternion.LookRotation(dir));
    }

    public void Emit(Quaternion rot)
    {
        if (particleSystem != null)
        {
            particleSystem.transform.rotation = rot;
            Emit();
        }
    }

    public void EmitLeft()
    {
        Emit(left.rotation);
    }

    public void EmitRight()
    {
        Emit(right.rotation);
    }

    public void EmitUp()
    {
        Emit(up.rotation);
    }

    public void EmitDown()
    {
        Emit(down.rotation);
    }
}

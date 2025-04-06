using UnityEngine;

public class AtomStabilityChecker : MonoBehaviour
{
    [Header("Stability Settings")]
    public bool isStable = false;  // Peut �tre modifi� par un autre syst�me

    [Tooltip("Dur�e pendant laquelle la stabilit� est maintenue apr�s activation.")]
    public float stabilityDuration = 10f;

    private float stabilityTimer = 0f;

    void Update()
    {
        if (isStable)
        {
            stabilityTimer -= Time.deltaTime;
            if (stabilityTimer <= 0f)
            {
                isStable = false;
            }
        }
    }
    public bool AreAtomsStable()
    {
        return isStable;
    }

    public void ActivateStability()
    {
        isStable = true;
        stabilityTimer = stabilityDuration;
    }

    public bool IsStable()
    {
        return isStable;
    }
}
